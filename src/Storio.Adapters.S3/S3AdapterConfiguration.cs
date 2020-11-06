using Amazon.S3;

namespace Storio.Adapters.S3
{
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
        
        /// <summary>
        /// Gets or sets an optional root path which is always combined with a consuming application's path. This is a
        /// great feature to use when you want to keep normalised URLs in your application but also want to store content
        /// in different subfolders in the configured S3 bucket.
        ///
        /// For example:
        ///     - I have a bucket named my-bucket that I store all of my company's information in.
        ///     - The adapter using this configuration object is going to be storing invoices for a particular run time
        ///       environment.
        ///     - Instead of littering my code with /invoices/dev in the file paths, I can set the root path of the
        ///       configuration to be /invoices/{environment} (defined at runtime) instead.
        ///     - Then, when performing invoice related file actions, I can reference the 'invoices' adapter registration
        ///       and store them anyway I want, without worrying about non-invoice related file paths.
        /// </summary>
        public string RootPath { get; set; }
    }
}
