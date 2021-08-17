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
        public string ThumbnailImages => "https://{0}.s3.{1}.amazonaws.com/{2}/Thumbnail images/{3}";
        public string FileTypes => "https://{0}.s3.{1}.amazonaws.com/{2}/File types/{3}";
        public AmazonS3Configuration(IAppConfigurationAccessor configurationAccessor)
        {
            _appConfiguration = configurationAccessor.Configuration;
        }
    }
}
