using layoutlovers.Configuration;
using Microsoft.Extensions.Configuration;

namespace layoutlovers.Amazon
{
    public class AmazonS3Configuration : IAmazonS3Configuration
    {
        private readonly IConfigurationRoot _appConfiguration;
        public string BucketName => _appConfiguration["Aws:S3:BucketName"];
        public string AwsAccessKeyId => _appConfiguration["Aws:S3:AwsAccessKeyId"];
        public string AwsSecretAccessKey => _appConfiguration["Aws:S3:AwsSecretAccessKey"];
        public string Region => _appConfiguration["Aws:S3:Region"];
        public AmazonS3Configuration(IAppConfigurationAccessor configurationAccessor)
        {
            _appConfiguration = configurationAccessor.Configuration;
        }
    }
}
