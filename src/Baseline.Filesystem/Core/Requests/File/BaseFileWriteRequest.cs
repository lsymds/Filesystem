namespace Baseline.Filesystem;

/// <summary>
/// A base class used for write requests. 
/// </summary>
public abstract class BaseFileWriteRequest<T> : BaseSingleFileRequest<T> where T : BaseFileWriteRequest<T>, new()
{
    /// <summary>
    /// Gets or sets whether a <see cref="FileAlreadyExistsException"/> should be thrown if an attempt is made to
    /// write to a path that already exists.
    /// </summary>
    public bool ThrowOnExistingFile { get; set; } = true;
}
