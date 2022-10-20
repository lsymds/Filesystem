using System;
using System.Threading.Tasks;
using Amazon.S3.Model;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Directories;

public class DeleteDirectoryTests : BaseS3AdapterIntegrationTest
{
    [Fact]
    public async Task It_Deletes_A_Simple_Directory()
    {
        // Arrange.
        var firstFile = "simples/file.txt".AsBaselineFilesystemPath();
        var secondFile = "simples/nother-file.txt".AsBaselineFilesystemPath();

        await CreateFileAndWriteTextAsync(firstFile);
        await CreateFileAndWriteTextAsync(secondFile);

        // Act.
        await DirectoryManager.DeleteAsync(
            new DeleteDirectoryRequest { DirectoryPath = "simples/".AsBaselineFilesystemPath() }
        );

        // Assert.
        await ExpectDirectoryNotToExistAsync("simples".AsBaselineFilesystemPath());
    }

    [Fact]
    public async Task It_Deletes_A_Complex_Nested_Directory_Structure()
    {
        // Arrange.
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
            await CreateFileAndWriteTextAsync(file.AsBaselineFilesystemPath());
        }

        // Act.
        await DirectoryManager.DeleteAsync(
            new DeleteDirectoryRequest
            {
                DirectoryPath = "prefixed/".AsBaselineFilesystemPath()
            }
        );

        // Assert.
        await ExpectDirectoryNotToExistAsync("prefixed".AsBaselineFilesystemPath());
    }

    [Fact]
    public async Task It_Deletes_A_Directory_Structure_With_A_Large_Number_Of_Paths_In()
    {
        // Arrange.
        for (int i = 0; i < 1001; i++)
        {
            await CreateFileAndWriteTextAsync($"prefixed/{i}.txt".AsBaselineFilesystemPath());
        }

        // Act.
        await DirectoryManager.DeleteAsync(
            new DeleteDirectoryRequest
            {
                DirectoryPath = "prefixed/".AsBaselineFilesystemPath()
            }
        );

        // Assert.
        var objectsLeftWithPrefix = await S3Client.ListObjectsAsync(
            new ListObjectsRequest { BucketName = GeneratedBucketName, Prefix = "prefixed/" }
        );
        objectsLeftWithPrefix.S3Objects.Should().BeEmpty();
    }

    [Fact]
    public async Task It_Throws_An_Exception_If_The_Directory_To_Delete_Does_Not_Exist()
    {
        // Act.
        Func<Task> func = async () =>
            await DirectoryManager.DeleteAsync(
                new DeleteDirectoryRequest
                {
                    DirectoryPath = RandomDirectoryPathRepresentation()
                }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<DirectoryNotFoundException>();
    }

    [Fact]
    public async Task It_Deletes_A_Directory_Under_A_Root_Path()
    {
        // Arrange.
        ReconfigureManagerInstances(true);

        var firstFile = "simples-root/file.txt".AsBaselineFilesystemPath();
        var secondFile = "simples-root/nother-file.txt".AsBaselineFilesystemPath();

        await CreateFileAndWriteTextAsync(firstFile);
        await CreateFileAndWriteTextAsync(secondFile);

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
