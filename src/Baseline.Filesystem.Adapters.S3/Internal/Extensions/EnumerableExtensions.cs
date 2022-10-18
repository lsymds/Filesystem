using System.Collections.Generic;
using System.Linq;

namespace Baseline.Filesystem.Adapters.S3.Internal.Extensions;

/// <summary>
/// Extensions for enumerables.
/// </summary>
internal static class EnumerableExtensions
{
    /// <summary>
    /// Chunks an enumerable by a specified amount.
    /// </summary>
    /// <param name="source">The source enumerable to chunk.</param>
    /// <param name="chunkBy">The size to chunk the source enumerable into.</param>
    /// <returns>An enumerable yielding chunked enumerables of the configured size.</returns>
    public static IEnumerable<IEnumerable<T>> ChunkBy<T>(this IEnumerable<T> source, int chunkBy)
    {
        var sourceEnumerated = source.ToList();

        while (sourceEnumerated.Any())
        {
            yield return sourceEnumerated.Take(chunkBy);
            sourceEnumerated = sourceEnumerated.Skip(chunkBy).ToList();
        }
    }
}
