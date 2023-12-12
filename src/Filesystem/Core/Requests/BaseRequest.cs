namespace LSymds.Filesystem;

/// <summary>
/// Base request containing functionality that all Baseline.Filesystem requests must implement.
/// </summary>
public abstract class BaseRequest<T> where T : BaseRequest<T>, new()
{
    /// <summary>
    /// Clones the current instance into a new instance ready for the paths to be updated. Only the references for
    /// paths are modified (no values), so there is no need to clone any properties.
    /// </summary>
    /// <returns></returns>
    internal abstract T ShallowClone();

    /// <summary>
    /// Clones the current instance and then combines the current instance's paths with a root path.
    /// </summary>
    /// <param name="rootPath">The root path to combine the current paths with.</param>
    /// <returns>A clone of the current instance with its paths combined with a root path.</returns>
    internal abstract T CloneAndCombinePathsWithRootPath(PathRepresentation rootPath);
}
