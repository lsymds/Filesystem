using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baseline.Filesystem.Internal.Extensions
{
    internal static class RootPathRemovalExtensions
    {
        /// <summary>
        /// Remove a root path from a single object with a path and return it with that root path removed. If no root
        /// path is present for the adapter, the original object is returned.
        /// </summary>
        /// <param name="obj">The object that has a path that might contain the root path.</param>
        /// <param name="getter">A lambda used to retrieve the value to modify.</param>
        /// <param name="setter">A lambda used to set the value that was modified.</param>
        internal static async Task<TResponse> RemoveRootPathsAsync<TResponse>(
            this Task<TResponse> obj, 
            Func<TResponse, PathRepresentation> getter, 
            Action<TResponse, PathRepresentation> setter,
            PathRepresentation rootPath
        )
        {
            var objResult = await obj;
            
            if (rootPath == null)
            {
                return objResult;
            }
            
            var withPathRemoved = getter(objResult).RemoveRootPath(rootPath);
            setter(objResult, withPathRemoved);

            return objResult;
        }

        /// <summary>
        /// Removes the root path from a collection of objects that may have a root path.
        /// </summary>
        /// <param name="obj">The entity that contains root paths that need removing.</param>
        /// <param name="getter">A selector to use to retrieve the path that needs modifying.</param>
        /// <param name="setter">A function used to modify the path of an entity.</param>
        internal static async Task<TResponse> RemoveRootPathsAsync<TResponse>(
            this Task<TResponse> obj, 
            Func<TResponse, IEnumerable<PathRepresentation>> getter, 
            Action<TResponse, IEnumerable<PathRepresentation>> setter,
            PathRepresentation rootPath
        )
        {
            var objResult = await obj;
            
            if (rootPath == null)
            {
                return objResult;
            }
            
            var withPathRemoved = getter(objResult).RemoveRootPath(rootPath);
            setter(objResult, withPathRemoved);

            return objResult;
        }
    }
}

