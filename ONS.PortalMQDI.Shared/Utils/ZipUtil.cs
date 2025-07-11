using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace ONS.PortalMQDI.Shared.Utils
{
    public static class ZipUtil
    {
        public static async Task<byte[]> CreateZipFromFilesAsync(Dictionary<string, byte[]> files)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var file in files)
                    {
                        var zipEntry = zipArchive.CreateEntry(file.Key);

                        using (var zipStream = zipEntry.Open())
                        {
                            using (var ms = new MemoryStream(file.Value))
                            {
                                await ms.CopyToAsync(zipStream);
                            }
                        }
                    }
                }
                return memoryStream.ToArray();
            }
        }
    }
}
