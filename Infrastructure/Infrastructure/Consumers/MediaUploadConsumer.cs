using Application.Abstraction.Storage;
using Application.Events;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Infrastructure.Consumers
{
    public class MediaUploadConsumer : IConsumer<MediaUploadRequestedEvent>
    {
        private readonly IStorageService _storageService;
        private readonly ILogger<MediaUploadConsumer> _logger;

        public MediaUploadConsumer(IStorageService storageService, ILogger<MediaUploadConsumer> logger)
        {
            _storageService = storageService;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<MediaUploadRequestedEvent> context)
        {
            var msg = context.Message;
            _logger.LogInformation("[Media] Dosya yükleme (Arka Plan) başladı: {FileName}", msg.OriginalFileName);

            try
            {
                using (var fileStream = new FileStream(msg.TempFilePath, FileMode.Open, FileAccess.Read))
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(msg.OriginalFileName)}";
                    
                    if (!string.IsNullOrWhiteSpace(msg.FolderName))
                    {
                        fileName = $"{msg.FolderName}/{fileName}";
                    }

                    var uploadedUrl = await _storageService.UploadAsync(fileStream, fileName, msg.ContentType);
                    
                    _logger.LogInformation("[Media] Başarılı! Dosya URL: {Url}", uploadedUrl);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Media] Dosya yüklenirken hata oluştu! Dosya: {FileName}", msg.OriginalFileName);
                throw; 
            }
            finally
            {
                if (File.Exists(msg.TempFilePath))
                {
                    File.Delete(msg.TempFilePath);
                }
            }
        }
    }
}
