using Abp.Dependency;
using System;

namespace layoutlovers.Amazon
{
    public interface IAmazonS3Configuration : ITransientDependency
    {
        string BucketName { get; }
        string AwsAccessKeyId { get; }
        string AwsSecretAccessKey { get; }
        string ThumbnailImages { get; }
        string FileTypes { get; }

        string GetPreviewUrl(Guid layoutProductId, string s3FileName);
    }
}
