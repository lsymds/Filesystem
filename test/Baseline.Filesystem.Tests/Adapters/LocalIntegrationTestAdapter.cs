using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Baseline.Filesystem.Tests.Adapters;

public class LocalIntegrationTestAdapter : BaseIntegrationTestAdapter, IIntegrationTestAdapter
{
    public LocalIntegrationTestAdapter(PathRepresentation rootPath = null) : base(rootPath) { }

    public ValueTask DisposeAsync()
    {
        if (RootPath != null)
        {
            Directory.Delete(RootPath.NormalisedPath);
        }

        return ValueTask.CompletedTask;
    }

    public ValueTask<IAdapter> BootstrapAsync()
    {
        if (RootPath != null)
        {
            Directory.CreateDirectory(RootPath.NormalisedPath);
        }

        return ValueTask.FromResult(new LocalAdapter() as IAdapter);
    }

    public async ValueTask CreateFileAndWriteTextAsync(
        PathRepresentation path,
        string contents = ""
    )
    {
        var workingPath = CombineRootPathWith(path);

        CreateParentDirectoryForPathIfNotExists(workingPath);

        await File.WriteAllTextAsync(workingPath.NormalisedPath, contents);
    }

    public ValueTask<bool> HasFilesOrDirectoriesUnderPathAsync(PathRepresentation path)
    {
        var workingPath = CombineRootPathWith(path);

        return ValueTask.FromResult(
            Directory.Exists(workingPath.NormalisedPath)
                && (
                    Directory.EnumerateDirectories(workingPath.NormalisedPath).Any()
                    || Directory.EnumerateFiles(workingPath.NormalisedPath).Any()
                )
        );
    }

    public ValueTask<bool> FileExistsAsync(PathRepresentation path)
    {
        return ValueTask.FromResult(File.Exists(CombineRootPathWith(path).NormalisedPath));
    }

    public ValueTask<bool> DirectoryExistsAsync(PathRepresentation path)
    {
        return ValueTask.FromResult(Directory.Exists(CombineRootPathWith(path).NormalisedPath));
    }

    public async ValueTask<string> ReadFileAsStringAsync(PathRepresentation path)
    {
        return await File.ReadAllTextAsync(CombineRootPathWith(path).NormalisedPath);
    }

    public ValueTask<IReadOnlyCollection<string>> TextThatShouldBeInPublicUrlForPathAsync(
        PathRepresentation path
    )
    {
        return ValueTask.FromResult(new[] { "" } as IReadOnlyCollection<string>);
    }

    private static void CreateParentDirectoryForPathIfNotExists(PathRepresentation path)
    {
        var pathTree = path.GetPathTree().ToList();

        // If only the file is in the path, then the directory it is in MUST exist.
        if (pathTree.Count == 1)
        {
            return;
        }

        var parentDirectory = pathTree[^2];
        Directory.CreateDirectory(parentDirectory.NormalisedPath);
    }
}
