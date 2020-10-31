namespace Storio
{
    /// <summary>
    /// Request used to write text to a particular file path.
    /// </summary>
    public class WriteTextToFileRequest : BaseSingleFileRequest
    {
        /// <summary>
        /// Gets or sets the content (mime) type to set the file as. A jpeg file, for example, would be image/jpeg.
        /// If this is not provided, we'll attempt to infer it from the file's extension. Some adapter's don't support
        /// setting alternative MIME types - but they won't throw errors if you do.
        /// </summary>
        public string ContentType { get; set; }
        
        /// <summary>
        /// Gets or sets the text to write to the file defined by the <see cref="FilePath" /> property.
        ///
        /// NOTE: This property cannot be null (or it'll fail validation) but it can be empty or whitespace.
        /// </summary>
        public string TextToWrite { get; set; }
    }
}
