using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Directories
{
    public class ListDirectoryContentsTests : BaseS3AdapterIntegrationTest
    {
        private readonly PathRepresentation _directoryPath = RandomDirectoryPathRepresentation();

        [Fact]
        public async Task It_Lists_The_Contents_Of_The_Root_Directory()
        {
            
        }
        
        [Fact]
        public async Task It_Lists_The_Contents_Of_A_Simple_Directory()
        {
            await CreateFileAndWriteTextAsync("simple/file.txt".AsBaselineFilesystemPath());
            await CreateFileAndWriteTextAsync("simple/another-file.txt".AsBaselineFilesystemPath());
            await CreateFileAndWriteTextAsync("simple/.config".AsBaselineFilesystemPath());

            var contents = await DirectoryManager.ListContentsAsync(new ListDirectoryContentsRequest
            {
                DirectoryPath = "simple/".AsBaselineFilesystemPath()
            });

            contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "simple" && x.FinalPathPartIsObviouslyADirectory);
            contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "simple/file.txt");
            contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "simple/another-file.txt");
            contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "simple/.config");
        }

        [Fact]
        public async Task It_Lists_The_Contents_Of_A_More_Complex_Directory()
        {
            await CreateFileAndWriteTextAsync("a/file.txt".AsBaselineFilesystemPath());
            await CreateFileAndWriteTextAsync("a/another-file.txt".AsBaselineFilesystemPath());
            await CreateFileAndWriteTextAsync("a/b/.config".AsBaselineFilesystemPath());
            await CreateFileAndWriteTextAsync("a/b/c/.keep".AsBaselineFilesystemPath());
            await CreateFileAndWriteTextAsync("a/c/d/e/f/g/.keep".AsBaselineFilesystemPath());

            var contents = await DirectoryManager.ListContentsAsync(new ListDirectoryContentsRequest
            {
                DirectoryPath = "a/".AsBaselineFilesystemPath()
            });

            contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a" && x.FinalPathPartIsObviouslyADirectory);
            contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/file.txt");
            contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/another-file.txt");
            contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/b" && x.FinalPathPartIsObviouslyADirectory);
            contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/b/.config");
            contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/b/c" && x.FinalPathPartIsObviouslyADirectory);
            contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/b/c/.keep");
            contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/c" && x.FinalPathPartIsObviouslyADirectory);
            contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/c/d" && x.FinalPathPartIsObviouslyADirectory);
            contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/c/d/e" && x.FinalPathPartIsObviouslyADirectory);
            contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/c/d/e/f" && x.FinalPathPartIsObviouslyADirectory);
            contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/c/d/e/f/g" && x.FinalPathPartIsObviouslyADirectory);
            contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/b/c/e/f/g/.keep");
        }
        
        [Fact]
        public async Task It_Lists_The_Contents_Of_A_Directory_With_A_Large_Number_Of_Files_In()
        {
            
        }

        [Fact]
        public async Task It_Lists_The_Contents_Of_A_Simple_Directory_With_A_Root_Path()
        {
            
        }

        [Fact]
        public async Task It_Lists_The_Contents_Of_A_Complex_Directory_Within_A_Root_Path()
        {
            
        }
    }
}
