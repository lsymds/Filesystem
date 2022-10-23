using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baseline.Filesystem.Tests.Adapters;

public class MemoryIntegrationTestAdapter : BaseIntegrationTestAdapter, IIntegrationTestAdapter
{
    private readonly MemoryFilesystem _memoryFilesystem = new();

    public MemoryIntegrationTestAdapter(PathRepresentation rootPath = null) : base(rootPath) { }

    public ValueTask DisposeAsync()
    {
        throw new System.NotImplementedException();
    }

    public ValueTask<IAdapter> BootstrapAsync()
    {
        return ValueTask.FromResult(
            new MemoryAdapter(
                new MemoryAdapterConfiguration
                {
                    MemoryFilesystem = _memoryFilesystem,
                    PublicUrlToReturn = "https://i.imgur.com/0HLw1x4.mp4"
                }
            ) as IAdapter
        );
    }

    public ValueTask CreateFileAndWriteTextAsync(PathRepresentation path, string contents = "")
    {
        var workingPath = CombineRootPathWith(path);

        _memoryFilesystem
            .GetOrCreateParentDirectoryOf(workingPath)
            .Files.Add(
                workingPath,
                new MemoryFileRepresentation(Content: contents, ContentType: "text/plain")
            );

        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> HasFilesOrDirectoriesUnderPathAsync(PathRepresentation path)
    {
        var workingPath = CombineRootPathWith(path);

        var directory = _memoryFilesystem.GetOrCreateDirectory(workingPath);
        return ValueTask.FromResult(directory.Files.Any() || directory.ChildDirectories.Any());
    }

    public ValueTask<bool> FileExistsAsync(PathRepresentation path)
    {
        var workingPath = CombineRootPathWith(path);
        return ValueTask.FromResult(
            _memoryFilesystem
                .GetOrCreateParentDirectoryOf(workingPath)
                .Files.ContainsKey(workingPath)
        );
    }

    public ValueTask<bool> DirectoryExistsAsync(PathRepresentation path)
    {
        var workingPath = CombineRootPathWith(path);

        return ValueTask.FromResult(_memoryFilesystem.DirectoryExists(workingPath));
    }

    public ValueTask<string> ReadFileAsStringAsync(PathRepresentation path)
    {
        var workingPath = CombineRootPathWith(path);

        var parentDirectory = _memoryFilesystem.GetOrCreateParentDirectoryOf(workingPath);
        return ValueTask.FromResult(parentDirectory.Files[workingPath].Content);
    }

    public ValueTask<IReadOnlyCollection<string>> TextThatShouldBeInPublicUrlForPathAsync(
        PathRepresentation path
    )
    {
        return ValueTask.FromResult(
            new[] { "https://i.imgur.com/0HLw1x4.mp4" } as IReadOnlyCollection<string>
        );
    }
}
