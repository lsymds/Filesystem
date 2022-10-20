using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Files;

public class DeleteFileTests : BaseS3AdapterIntegrationTest
{
    [Fact]
    public async Task It_Throws_An_Exception_If_File_Does_Not_Exist()
    {
        // Act.
        Func<Task> func = async () =>
            await FileManager.DeleteAsync(
                new DeleteFileRequest { FilePath = RandomFilePathRepresentation() }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<FileNotFoundException>();
    }

    [Fact]
    public async Task It_Deletes_A_File()
    {
        // Arrange.
        var filePath = RandomFilePathRepresentation();

        await CreateFileAndWriteTextAsync(filePath);

        await ExpectFileToExistAsync(filePath);

        // Act.
        await FileManager.DeleteAsync(new DeleteFileRequest { FilePath = filePath });

        // Assert.
        await ExpectFileNotToExistAsync(filePath);
    }

    [Fact]
    public async Task It_Deletes_A_File_With_A_Root_Path()
    {
        // Arrange.
        ReconfigureManagerInstances(true);

        var filePath = RandomFilePathRepresentation();

        await CreateFileAndWriteTextAsync(filePath);

        await ExpectFileToExistAsync(filePath);

        // Act.
        await FileManager.DeleteAsync(new DeleteFileRequest { FilePath = filePath });

        // Assert.
        await ExpectFileNotToExistAsync(filePath);
    }
}
