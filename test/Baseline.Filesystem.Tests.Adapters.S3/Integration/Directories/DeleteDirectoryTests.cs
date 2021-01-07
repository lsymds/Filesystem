using System;
using System.Threading.Tasks;
using Amazon.S3.Model;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Directories
{
    public class DeleteDirectoryTests : BaseS3AdapterIntegrationTest
    {
        [Fact]
        public async Task It_Deletes_A_Simple_Directory()
        {
            var firstFile = "a/file.txt".AsBaselineFilesystemPath();
            var secondFile = "a/nother-file.txt".AsBaselineFilesystemPath();
            
            await CreateFileAndWriteTextAsync(firstFile);
            await CreateFileAndWriteTextAsync(secondFile);

            await DirectoryManager.DeleteAsync(new DeleteDirectoryRequest
            {
                DirectoryPath = "a/".AsBaselineFilesystemPath()
            });

            (await FileExistsAsync(firstFile)).Should().BeFalse();
            (await FileExistsAsync(secondFile)).Should().BeFalse();
        }

        [Fact]
        public async Task It_Deletes_A_Complex_Nested_Directory_Structure()
        {
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

            await DirectoryManager.DeleteAsync(new DeleteDirectoryRequest
            {
                DirectoryPath = "prefixed/".AsBaselineFilesystemPath()
            });

            foreach (var file in files)
            {
                (await FileExistsAsync(file.AsBaselineFilesystemPath())).Should().BeFalse();
            }
        }

        [Fact]
        public async Task It_Deletes_A_Directory_Structure_With_A_Large_Number_Of_Paths_In()
        {
            for (int i = 0; i < 1001; i++)
            {
                await CreateFileAndWriteTextAsync($"prefixed/{i}.txt".AsBaselineFilesystemPath());
            }

            await DirectoryManager.DeleteAsync(new DeleteDirectoryRequest
            {
                DirectoryPath = "prefixed/".AsBaselineFilesystemPath()
            });

            var objectsLeftWithPrefix = await S3Client.ListObjectsAsync(new ListObjectsRequest
            {
                BucketName = GeneratedBucketName,
                Prefix = "prefixed/"
            });
            objectsLeftWithPrefix.S3Objects.Should().BeEmpty();
        }

        [Fact]
        public async Task It_Throws_An_Exception_If_The_Directory_To_Delete_Does_Not_Exist()
        {
            Func<Task> func = async () => await DirectoryManager.DeleteAsync(new DeleteDirectoryRequest
            {
                DirectoryPath = RandomDirectoryPath()
            });
            await func.Should().ThrowExactlyAsync<DirectoryNotFoundException>();
        }
    }
}
