namespace Baseline.Filesystem.Configuration
{
    /// <summary>
    /// Base adapter configuration which defines common functionality across all adapter configuration objects.
    /// </summary>
    public class BaseAdapterConfiguration
    {
        /// <summary>
        /// Gets or sets an optional root path which is always combined with a path specified in any call to any methods
        /// within an adapter. This is a great feature to use when you want to keep normalised URLs in your application
        /// but also want to store content in different places within the adapter's data store.
        ///
        /// For example:
        ///     - I have an S3 bucket named my-bucket that I store all of my company's information in.
        ///     - The adapter using this configuration object is going to be storing invoices.
        ///     - Instead of littering my code with /invoices/dev in the file paths, I can set the root path of the
        ///       configuration to be /invoices/ instead.
        ///     - Then, when performing invoice related file actions, I can reference the 'invoices' adapter registration
        ///       and store them anyway I want, without worrying about where the invoices are stored within the adapter's
        ///       data store.
        ///     - If, in the future, I decide I want to use a physical file system, I can use the file system adapter,
        ///       configure its root path accordingly, and not have to change anything within my application. 
        /// </summary>
        public string RootPath { get; set; }
    }
}
