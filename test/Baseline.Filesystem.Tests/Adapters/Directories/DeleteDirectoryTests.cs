using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.Directories;

public class DeleteDirectoryTests : BaseIntegrationTest
{
    [Theory]
    [InlineData(Adapter.S3)]
    public async Task It_Deletes_A_Simple_Directory(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var firstFile = "simples/file.txt".AsBaselineFilesystemPath();
        var secondFile = "simples/nother-file.txt".AsBaselineFilesystemPath();

        await TestAdapter.CreateFileAndWriteTextAsync(firstFile);
        await TestAdapter.CreateFileAndWriteTextAsync(secondFile);

        // Act.
        await DirectoryManager.DeleteAsync(
            new DeleteDirectoryRequest { DirectoryPath = "simples/".AsBaselineFilesystemPath() }
        );

        // Assert.
        await ExpectDirectoryNotToExistAsync("simples".AsBaselineFilesystemPath());
    }

    [Theory]
    [InlineData(Adapter.S3)]
    public async Task It_Deletes_A_Complex_Nested_Directory_Structure(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var files = new[]
        {
            "prefixed/file.txt",
            "prefixed/nother-file.txt",
            "prefixed/b/c.txt",
            "prefixed/b/c/d/foo.txt",
            "prefixed/c/d/e/f/g/another.txt",
            "prefixed/v/d/e/f/g/d/x/z/s/e/d/h/t/d/c/s/a.txt",
            "prefixed/v/d/e/f/g/d/x/z/s/e/d/h/t/d/c/a.txt"
        };

        foreach (var file in files)
        {
            await TestAdapter.CreateFileAndWriteTextAsync(
                file.AsBaselineFilesystemPath()
            );
        }

        // Act.
        await DirectoryManager.DeleteAsync(
            new DeleteDirectoryRequest { DirectoryPath = "prefixed/".AsBaselineFilesystemPath() }
        );

        // Assert.
        await ExpectDirectoryNotToExistAsync("prefixed".AsBaselineFilesystemPath());
    }

    [Theory]
    [InlineData(Adapter.S3)]
    public async Task It_Deletes_A_Directory_Structure_With_A_Large_Number_Of_Paths_In(
        Adapter adapter
    )
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        for (int i = 0; i < 1001; i++)
        {
            await TestAdapter.CreateFileAndWriteTextAsync(
                $"prefixed/{i}.txt".AsBaselineFilesystemPath()
            );
        }

        // Act.
        await DirectoryManager.DeleteAsync(
            new DeleteDirectoryRequest { DirectoryPath = "prefixed/".AsBaselineFilesystemPath() }
        );

        // Assert.
        var has = await TestAdapter.HasFilesOrDirectoriesUnderPathAsync(
            "prefixed/".AsBaselineFilesystemPath()
        );
        has.Should().BeFalse();
    }

    [Theory]
    [InlineData(Adapter.S3)]
    public async Task It_Throws_An_Exception_If_The_Directory_To_Delete_Does_Not_Exist(
        Adapter adapter
    )
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        // Act.
        Func<Task> func = async () =>
            await DirectoryManager.DeleteAsync(
                new DeleteDirectoryRequest
                {
                    DirectoryPath = TestUtilities.RandomDirectoryPathRepresentation()
                }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<DirectoryNotFoundException>();
    }

    [Theory]
    [InlineData(Adapter.S3)]
    public async Task It_Deletes_A_Directory_Under_A_Root_Path(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        var firstFile = "simples-root/file.txt".AsBaselineFilesystemPath();
        var secondFile = "simples-root/nother-file.txt".AsBaselineFilesystemPath();

        await TestAdapter.CreateFileAndWriteTextAsync(firstFile);
        await TestAdapter.CreateFileAndWriteTextAsync(secondFile);

        // Act.
        await DirectoryManager.DeleteAsync(
            new DeleteDirectoryRequest
            {
                DirectoryPath = "simples-root/".AsBaselineFilesystemPath()
            }
        );

        // Assert.
        await ExpectFileNotToExistAsync(firstFile);
        await ExpectFileNotToExistAsync(secondFile);
    }
}
