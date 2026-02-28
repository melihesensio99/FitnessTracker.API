using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Application.Abstraction.Storage;
using Application.Abstraction.Services;
using Application.Events;

namespace Application.Helpers
{
    public static class MediaHelper
    {
        /// <summary>
        /// Gelen IFormFile dosyasını AWS S3'e yükler ve URL'sini döner.
        /// </summary>
        /// <param name="file">Yüklenecek dosya nesnesi</param>
        /// <param name="storageService">IStorageService örneği (dependency injection ile alınır)</param>
        /// <returns>AWS S3 üzerindeki dosyanın public URL'si</returns>
        public static async Task<string?> UploadMediaAsync(IFormFile? file, IStorageService storageService, string? folder = null)
        {
            if (file == null || file.Length == 0)
                return null;

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

           
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

            if (!string.IsNullOrWhiteSpace(folder))
            {
                fileName = $"{folder}/{fileName}";
            }

            
            var url = await storageService.UploadAsync(memoryStream, fileName, file.ContentType);

            return url;
        }

        public static async Task EnqueueMediaUploadAsync(IFormFile? file, IEventBus eventBus, string? folder = null)
        {
            if (file == null || file.Length == 0)
                return;

            var localStoreFolder = Path.Combine(Directory.GetCurrentDirectory(), "QueueStorage");
            if (!Directory.Exists(localStoreFolder))
            {
                Directory.CreateDirectory(localStoreFolder);
            }

            var tempFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var tempPath = Path.Combine(localStoreFolder, tempFileName);

            using (var stream = new FileStream(tempPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            await eventBus.PublishAsync(new MediaUploadRequestedEvent 
            {
                TempFilePath = tempPath,
                FolderName = folder,
                ContentType = file.ContentType,
                OriginalFileName = file.FileName
            });
        }

  
        public static async Task<bool> DeleteMediaAsync(string? url, IStorageService storageService)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            try
            {
                var uri = new Uri(url);
                var key = uri.AbsolutePath.TrimStart('/');
                
               
                key = Uri.UnescapeDataString(key);

                await storageService.DeleteAsync(key);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
