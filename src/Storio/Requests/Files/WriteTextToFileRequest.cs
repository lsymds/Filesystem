namespace Storio
{
    /// <summary>
    /// Request used to write text to a particular file path.
    /// </summary>
    public class WriteTextToFileRequest : BaseSingleFileRequest
    {
        /// <summary>
        /// Gets or sets the text to write to the file defined by the <see cref="FilePath" /> property.
        ///
        /// NOTE: This property cannot be null (or it'll fail validation) but it can be empty or whitespace.
        /// </summary>
        public string TextToWrite { get; set; }
    }
}
