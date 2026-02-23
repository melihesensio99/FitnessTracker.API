using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Common.AWS
{
    public class AWSOptions
    {
        public string AccessKey { get; set; } = default!;
        public string SecretKey { get; set; } = default!;
        public string Region { get; set; } = default!;
        public string BucketName { get; set; } = default!;
    }
}
