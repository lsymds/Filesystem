using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Baseline.Filesystem.Tests.Adapters;

public class LocalIntegrationTestAdapter : BaseIntegrationTestAdapter, IIntegrationTestAdapter
{
    public LocalIntegrationTestAdapter(PathRepresentation rootPath = null)
        : base(
            rootPath == null
                ? $"{Guid.NewGuid().ToString().Replace("-", "")}/".AsBaselineFilesystemPath()
                : $"{rootPath.NormalisedPath}/{Guid.NewGuid().ToString().Replace("-", "")}/".AsBaselineFilesystemPath()
        ) { }

    public ValueTask DisposeAsync()
    {
        Directory.Delete(RootPath.NormalisedPath);
        return ValueTask.CompletedTask;
    }

    public ValueTask<IAdapter> BootstrapAsync()
    {
        Directory.CreateDirectory(RootPath.NormalisedPath);
        return ValueTask.FromResult(new LocalAdapter() as IAdapter);
    }

    public async ValueTask CreateFileAndWriteTextAsync(
        PathRepresentation path,
        string contents = ""
    )
    {
        await File.WriteAllTextAsync(CombineRootPathWith(path).NormalisedPath, contents);
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
}
