namespace Baseline.Filesystem
{
    /// <summary>
    /// Configuration class containing all of the properties and information required to register an adapter.
    /// </summary>
    public class AdapterRegistration
    {
        /// <summary>
        /// Gets or sets the adapter instance to be registered.
        /// </summary>
        public IAdapter Adapter { get; set; }
        
        /// <summary>
        /// Gets or sets the name of the adapter.
        /// </summary>
        public string Name { get; set; } = "default";
        
        /// <summary>
        /// <para>
        /// Gets or sets an optional root path which is always combined with a path specified in any call to any methods
        /// within an adapter. This is a great feature to use when you want to keep normalised URLs in your application
        /// but also want to store content in different places within the adapter's data store.
        /// </para>
        /// <para>
        /// For example:
        /// <list type="bullet">
        ///     <item>
        ///         <description>I have an S3 bucket named my-bucket that I store all of my company's information in.</description>
        ///     </item>
        ///     <item>
        ///         <description>The adapter using this configuration object is going to be storing invoices.</description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             Instead of littering my code with /invoices/dev in the file paths, I can set the root path of the
        ///             configuration to be /invoices/ instead.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             Then, when performing invoice related file actions, I can reference the 'invoices' adapter registration
        ///             and store them anyway I want, without worrying about where the invoices are stored within the adapter's
        ///             data store.
        ///         </description>
        ///     </item>
        ///     <item>
        ///         <description>
        ///             If, in the future, I decide I want to use a physical file system, I can use the file system adapter,
        ///             configure its root path accordingly, and not have to change anything within my application.
        ///         </description>
        ///     </item> 
        /// </list>
        /// </para>
        /// </summary>
        public PathRepresentation RootPath { get; set; }
    }
}
