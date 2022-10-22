using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baseline.Filesystem.Tests.Adapters;

public class MemoryIntegrationTestAdapter : IIntegrationTestAdapter
{
    private readonly MemoryFilesystem _memoryFilesystem = new();

    public ValueTask DisposeAsync()
    {
        throw new System.NotImplementedException();
    }

    public ValueTask<IAdapter> BootstrapAsync()
    {
        return ValueTask.FromResult(
            new MemoryAdapter(
                new MemoryAdapterConfiguration { MemoryFilesystem = _memoryFilesystem }
            ) as IAdapter
        );
    }

    public ValueTask CreateFileAndWriteTextAsync(PathRepresentation path, string contents = "")
    {
        throw new System.NotImplementedException();
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
        return ValueTask.FromResult(_memoryFilesystem.DirectoryExists(path));
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
