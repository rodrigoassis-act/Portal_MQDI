using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using ONS.PortalMQDI.Services.Interfaces;
using ONS.PortalMQDI.Shared.Settings;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Services
{
    public class AwsService : IAwsService
    {
        #region AwsService
        private IAmazonS3 _s3Client;
        private string _bucketName;

        private readonly IOptions<AwsSettings> _awsSettings;
        public AwsService(IOptions<AwsSettings> awsSettings)
        {
            _awsSettings = awsSettings;

            var creds = new BasicAWSCredentials(
                _awsSettings.Value.AccessKey,
                _awsSettings.Value.SecretKey
           );

            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(_awsSettings.Value.Region)
            };

            _s3Client = new AmazonS3Client(creds, config);
            _bucketName = _awsSettings.Value.BucketName;
        }
        #endregion

        public async Task<string> UploadFileAsync(string folder, byte[] file, string nameFile, CancellationToken cancellationToken)
        {
            try
            {
                string key = $"{folder}/{nameFile}";

                var existeFile = await GetFileContentAsync(key, cancellationToken);

                if (existeFile != null)
                {
                    await RemoveFileAsync(key, cancellationToken);
                }

                await AddFileAsync(key, file, cancellationToken);

                return key;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                throw new Exception($"AWS Service: {errorMessage}");
            }
        }

        public async Task<string> RenameFolderAsync(string oldFolderKey, string folder, CancellationToken cancellationToken)
        {
            try
            {
                string newFolderKey = $"{folder}/";

                var listObjectsRequest = new ListObjectsV2Request
                {
                    BucketName = _bucketName,
                    Prefix = oldFolderKey + "/"
                };

                var listObjectsResponse = await _s3Client.ListObjectsV2Async(listObjectsRequest);

                foreach (var obj in listObjectsResponse.S3Objects)
                {
                    string newKey = $"{newFolderKey}{obj.Key.Substring(obj.Key.IndexOf('/') + 1)}";
                    await CopyFileAsync(obj.Key, newKey, cancellationToken);

                    await _s3Client.DeleteObjectAsync(_bucketName, obj.Key);
                }

                return newFolderKey;
            }
            catch (Exception ex)
            {
                string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                throw new Exception($"AWS Service: {errorMessage}");
            }
        }

        public async Task<byte[]> DownloadAsync(string keyFile, CancellationToken cancellationToken)
        {
            try
            {
                return await GetFileContentAsync(keyFile, cancellationToken);
            }
            catch (AmazonS3Exception ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    throw new Exception($"AWS Service: {errorMessage}");
                }
            }
        }

        public async Task<bool> RemoverFileAsync(string keyFile, CancellationToken cancellationToken)
        {
            try
            {
                return await RemoveFileAsync(keyFile, cancellationToken);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                throw new Exception($"AWS Service: {errorMessage}");
            }
        }

        #region Auxiliares

        private async Task<byte[]> GetFileContentAsync(string fileId, CancellationToken cancellationToken)
        {
            try
            {
                var getObjectRequest = new Amazon.S3.Model.GetObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileId
                };

                var response = await _s3Client.GetObjectAsync(getObjectRequest, cancellationToken);

                using (var memoryStream = new System.IO.MemoryStream())
                {
                    await response.ResponseStream.CopyToAsync(memoryStream);
                    return memoryStream.ToArray();
                }
            }
            catch 
            {
                return null;
            }
        }
        private async Task CopyFileAsync(string oldKey, string newKey, CancellationToken cancellationToken)
        {
            var copyObjectRequest = new CopyObjectRequest
            {
                SourceBucket = _bucketName,
                SourceKey = oldKey,
                DestinationBucket = _bucketName,
                DestinationKey = newKey
            };

            await _s3Client.CopyObjectAsync(copyObjectRequest, cancellationToken);
        }
        private async Task<string> AddFileAsync(string key, byte[] fileContent, CancellationToken cancellationToken)
        {
            var putObjectRequest = new Amazon.S3.Model.PutObjectRequest
            {
                BucketName = _bucketName,
                Key = key,
                InputStream = new MemoryStream(fileContent),
                ContentType = "application/octet-stream"
            };

            await _s3Client.PutObjectAsync(putObjectRequest, cancellationToken);
            return key;
        }
        private async Task<bool> RemoveFileAsync(string fileId, CancellationToken cancellationToken)
        {
            var deleteObjectRequest = new Amazon.S3.Model.DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = fileId
            };

            var response = await _s3Client.DeleteObjectAsync(deleteObjectRequest, cancellationToken);

            return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
        }

        #endregion
    }
}
