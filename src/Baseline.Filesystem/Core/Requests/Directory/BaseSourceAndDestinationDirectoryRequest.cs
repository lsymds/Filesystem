namespace Baseline.Filesystem;

/// <summary>
/// Base class containing all properties for a source and destination based directory request.
/// </summary>
public abstract class BaseSourceAndDestinationDirectoryRequest<T> : BaseRequest<T>
    where T : BaseSourceAndDestinationDirectoryRequest<T>, new()
{
    /// <summary>
    /// Gets or sets the source directory path.
    /// </summary>
    public PathRepresentation SourceDirectoryPath { get; set; }

    /// <summary>
    /// Gets or sets the destination directory path.
    /// </summary>
    public PathRepresentation DestinationDirectoryPath { get; set; }

    /// <summary>
    /// Combines the paths belonging to this request with a root path, if the root path is not null.
    /// </summary>
    /// <param name="rootPath">The root path to combine the current paths with.</param>
    /// <returns>
    /// A cloned version of the current class, with the paths combined with the root path if applicable.
    /// </returns>
    internal override T CloneAndCombinePathsWithRootPath(PathRepresentation rootPath)
    {
        if (rootPath == null)
        {
            return (T)this;
        }

        var cloned = ShallowClone();
        cloned.SourceDirectoryPath = cloned.SourceDirectoryPath.CombineWithBase(rootPath);
        cloned.DestinationDirectoryPath = cloned.DestinationDirectoryPath.CombineWithBase(
            rootPath
        );
        return cloned;
    }

    /// <summary>
    /// Clones the current instance for path updates. Some properties do not need to be cloned (for example path
    /// representations) as they're never modified.
    /// </summary>
    /// <returns>A clone of the current instance.</returns>
    internal override T ShallowClone()
    {
        return new T
        {
            SourceDirectoryPath = SourceDirectoryPath,
            DestinationDirectoryPath = DestinationDirectoryPath
        };
    }
}