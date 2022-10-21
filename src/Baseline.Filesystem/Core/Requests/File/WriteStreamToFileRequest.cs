using System.IO;

namespace Baseline.Filesystem;

/// <summary>
/// A request containing information required in order to write a stream to a file.
/// </summary>
public class WriteStreamToFileRequest : BaseSingleFileRequest<WriteStreamToFileRequest>
{
    /// <summary>
    /// Gets or sets the content (mime) type to set the file as. A jpeg file, for example, would be image/jpeg.
    /// If this is not provided, we'll attempt to infer it from the file's extension. Some adapter's don't support
    /// setting alternative MIME types - but they won't throw errors if you do.
    /// </summary>
    public string ContentType { get; set; }

    /// <summary>
    /// Gets or sets the stream that will be written to a file. It is your responsibility as a consumer of this
    /// library to correctly close the stream.
    /// </summary>
    public Stream Stream { get; set; }

    /// <inheritdoc />
    internal override WriteStreamToFileRequest ShallowClone()
    {
        var cloned = base.ShallowClone();
        cloned.ContentType = ContentType;
        cloned.Stream = Stream;
        return cloned;
    }
}
