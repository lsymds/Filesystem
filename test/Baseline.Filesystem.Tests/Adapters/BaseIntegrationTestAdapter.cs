namespace Baseline.Filesystem.Tests.Adapters;

public abstract class BaseIntegrationTestAdapter
{
    private readonly PathRepresentation _rootPath;

    protected BaseIntegrationTestAdapter(PathRepresentation rootPath)
    {
        _rootPath = rootPath;
    }

    protected PathRepresentation CombineRootPathWith(PathRepresentation path)
    {
        return _rootPath == null ? path : new PathCombinationBuilder(_rootPath, path).Build();
    }
}
