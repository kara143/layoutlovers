using Abp.Dependency;

namespace layoutlovers.Amazon
{
    public interface IAmazonS3Configuration : ITransientDependency
    {
        string BucketName { get; }
        string AwsAccessKeyId { get; }
        string AwsSecretAccessKey { get; }
    }
}
