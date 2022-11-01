using System;
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
        throw new System.NotImplementedException();
    }

    public ValueTask<bool> FileExistsAsync(PathRepresentation path)
    {
        throw new System.NotImplementedException();
    }

    public ValueTask<bool> DirectoryExistsAsync(PathRepresentation path)
    {
        throw new System.NotImplementedException();
    }

    public ValueTask<string> ReadFileAsStringAsync(PathRepresentation path)
    {
        throw new System.NotImplementedException();
    }

    public ValueTask<IReadOnlyCollection<string>> TextThatShouldBeInPublicUrlForPathAsync(
        PathRepresentation path
    )
    {
        throw new System.NotImplementedException();
    }

    private void CreateParentDirectoryForPathIfNotExists(PathRepresentation path)
    {
        var parentDirectory = path.GetPathTree().ToList()[^2];
        Directory.CreateDirectory(parentDirectory.NormalisedPath);
    }
}
