using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Abstraction.Storage
{
    public interface IStorageService
    {

        Task<string> UploadAsync(Stream fileStream, string fileName, string contentType);
        Task DeleteAsync(string fileKey);
    }
}
