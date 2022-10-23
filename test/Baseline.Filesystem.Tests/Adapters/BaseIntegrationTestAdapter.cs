using System.IO;

namespace Baseline.Filesystem.Tests.Adapters;

public abstract class BaseIntegrationTestAdapter
{
    protected PathRepresentation? RootPath;

    protected BaseIntegrationTestAdapter(PathRepresentation rootPath)
    {
        RootPath = rootPath;
    }

    protected PathRepresentation CombineRootPathWith(PathRepresentation path)
    {
        return RootPath == null ? path : new PathCombinationBuilder(RootPath, path).Build();
    }
}
