using Amazon.S3;
using Amazon.S3.Model;
using Application.Abstraction.Storage;
using Application.Common.AWS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Storage.AWS
{
    public class AwsS3StorageService : IStorageService
    {
        private readonly IAmazonS3 _s3;
        private readonly AWSOptions _options;

        public AwsS3StorageService(
            IAmazonS3 s3,
            AWSOptions options)
        {
            _s3 = s3;
            _options = options;
        }

        public async Task<string> UploadAsync(
            Stream stream,
            string fileName,
            string contentType)
        {
            await _s3.PutObjectAsync(new PutObjectRequest
            {
                BucketName = _options.BucketName,
                Key = fileName,
                InputStream = stream,
                ContentType = contentType
            });

            return $"https://{_options.BucketName}.s3.{_options.Region}.amazonaws.com/{fileName}";
        }

        public async Task DeleteAsync(string key)
        {
            await _s3.DeleteObjectAsync(_options.BucketName, key);
        }
    }
}
