using Amazon.S3;

namespace LSymds.Filesystem;

/// <summary>
/// Configuration options for the S3 adapter.
/// </summary>
public class S3AdapterConfiguration
{
    /// <summary>
    /// Gets or sets the S3 client to use in the adapter.
    /// </summary>
    public IAmazonS3 S3Client { get; set; }

    /// <summary>
    /// Gets or sets the bucket name used in the S3 adapter's actions.
    /// </summary>
    public string BucketName { get; set; }
}
