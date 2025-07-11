using System.Threading;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Services.Interfaces
{
    public interface IAwsService
    {
        Task<string> UploadFileAsync(string folder, byte[] file, string nameFile, CancellationToken cancellationToken);
        Task<string> RenameFolderAsync(string oldFolderKey, string folder, CancellationToken cancellationToken);
        Task<byte[]> DownloadAsync(string keyFile, CancellationToken cancellationToken);
        Task<bool> RemoverFileAsync(string keyFile, CancellationToken cancellationToken);
    }
}
