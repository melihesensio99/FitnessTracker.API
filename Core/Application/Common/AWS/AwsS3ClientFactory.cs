using Amazon;
using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.AWS
{
    public static class AwsS3ClientFactory
    {
        public static IAmazonS3 Create(AWSOptions options)
        {
            return new AmazonS3Client(
                options.AccessKey,
                options.SecretKey,
                RegionEndpoint.GetBySystemName(options.Region));
        }
    }
}
