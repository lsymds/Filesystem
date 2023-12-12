using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LSymds.Filesystem.Internal.Extensions;

/// <summary>
/// Extension methods related to the removal of root paths from <see cref="PathRepresentation"/> instances.
/// </summary>
internal static class RootPathRemovalExtensions
{
    /// <summary>
    /// Remove a root path from a single object with a path and return it with that root path removed. If no root
    /// path is present for the adapter, the original object is returned.
    /// </summary>
    /// <param name="obj">The object that has a path that might contain the root path.</param>
    /// <param name="pathGetter">A lambda used to retrieve the value to modify.</param>
    /// <param name="pathToNewResponseMapper">A lambda used to set the value that was modified.</param>
    /// <param name="rootPath">The root path of the adapter (if present).</param>
    internal static async Task<TResponse> RemoveRootPathsAsync<TResponse>(
        this Task<TResponse> obj,
        Func<TResponse, PathRepresentation> pathGetter,
        Func<TResponse, PathRepresentation, TResponse> pathToNewResponseMapper,
        PathRepresentation rootPath
    )
    {
        var objResult = await obj;

        if (rootPath == null)
        {
            return objResult;
        }

        var withPathRemoved = pathGetter(objResult).RemoveRootPath(rootPath);

        return pathToNewResponseMapper(objResult, withPathRemoved);
    }

    /// <summary>
    /// Removes the root path from a collection of objects that may have a root path.
    /// </summary>
    /// <param name="obj">The entity that contains root paths that need removing.</param>
    /// <param name="getter">A selector to use to retrieve the path that needs modifying.</param>
    /// <param name="mapper">A function used to modify the path of an entity.</param>
    /// <param name="rootPath">The root path of the adapter (if present).</param>
    internal static async Task<TResponse> RemoveRootPathsAsync<TResponse>(
        this Task<TResponse> obj,
        Func<TResponse, IEnumerable<PathRepresentation>> getter,
        Func<TResponse, IEnumerable<PathRepresentation>, TResponse> mapper,
        PathRepresentation rootPath
    )
    {
        var objResult = await obj;

        if (rootPath == null)
        {
            return objResult;
        }

        var withPathRemoved = getter(objResult).RemoveRootPath(rootPath);

        return mapper(objResult, withPathRemoved);
    }
}
