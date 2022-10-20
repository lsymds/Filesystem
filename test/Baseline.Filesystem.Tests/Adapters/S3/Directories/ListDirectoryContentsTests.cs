using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Directories;

public class ListDirectoryContentsTests : BaseS3AdapterIntegrationTest
{
    [Fact]
    public async Task It_Lists_The_Contents_Of_A_Simple_Directory()
    {
        // Arrange.
        await CreateFileAndWriteTextAsync("simple/file.txt".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("simple/another-file.txt".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("simple/.config".AsBaselineFilesystemPath());

        // Act.
        var contents = await DirectoryManager.ListContentsAsync(
            new ListDirectoryContentsRequest
            {
                DirectoryPath = "simple/".AsBaselineFilesystemPath()
            }
        );

        // Assert.
        contents.Contents.Should().HaveCount(4);
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "simple" && x.FinalPathPartIsObviouslyADirectory
            );
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "simple/file.txt");
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "simple/another-file.txt");
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "simple/.config");
    }

    [Fact]
    public async Task It_Lists_The_Contents_Of_A_More_Complex_Directory()
    {
        // Arrange.
        await CreateFileAndWriteTextAsync("a/file.txt".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("a/another-file.txt".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("a/b/.config".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("a/b/c/.keep".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("a/c/d/e/f/g/.keep".AsBaselineFilesystemPath());

        // Act.
        var contents = await DirectoryManager.ListContentsAsync(
            new ListDirectoryContentsRequest { DirectoryPath = "a/".AsBaselineFilesystemPath() }
        );

        // Assert.
        contents.Contents.Should().HaveCount(13);
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a" && x.FinalPathPartIsObviouslyADirectory);
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/file.txt");
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/another-file.txt");
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/b" && x.FinalPathPartIsObviouslyADirectory);
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/b/.config");
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/b/c" && x.FinalPathPartIsObviouslyADirectory
            );
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/b/c/.keep");
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/c" && x.FinalPathPartIsObviouslyADirectory);
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d" && x.FinalPathPartIsObviouslyADirectory
            );
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d/e" && x.FinalPathPartIsObviouslyADirectory
            );
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d/e/f" && x.FinalPathPartIsObviouslyADirectory
            );
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d/e/f/g" && x.FinalPathPartIsObviouslyADirectory
            );
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/c/d/e/f/g/.keep");
    }

    [Fact]
    public async Task It_Lists_The_Contents_Of_A_Directory_With_A_Large_Number_Of_Files_In()
    {
        // Arrange.
        for (int i = 0; i < 1000; i++)
        {
            await CreateFileAndWriteTextAsync($"a-dir/{i}.txt".AsBaselineFilesystemPath());
        }

        // Act.
        var contents = await DirectoryManager.ListContentsAsync(
            new ListDirectoryContentsRequest { DirectoryPath = "a-dir/".AsBaselineFilesystemPath() }
        );

        // Assert.
        for (var i = 0; i < 1000; i++)
        {
            // ReSharper disable once AccessToModifiedClosure
            contents.Contents.Should().ContainSingle(x => x.OriginalPath == $"a-dir/{i}.txt");
        }
    }

    [Fact]
    public async Task It_Lists_The_Contents_Of_A_Simple_Directory_With_A_Root_Path()
    {
        // Arrange.
        ReconfigureManagerInstances(true);
        await CreateFileAndWriteTextAsync("simple/file.txt".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("simple/another-file.txt".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("simple/.config".AsBaselineFilesystemPath());

        // Act.
        var contents = await DirectoryManager.ListContentsAsync(
            new ListDirectoryContentsRequest
            {
                DirectoryPath = "simple/".AsBaselineFilesystemPath()
            }
        );

        // Assert.
        contents.Contents.Should().HaveCount(4);
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "simple" && x.FinalPathPartIsObviouslyADirectory
            );
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "simple/file.txt");
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "simple/another-file.txt");
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "simple/.config");
    }

    [Fact]
    public async Task It_Lists_The_Contents_Of_A_Complex_Directory_Within_A_Root_Path()
    {
        // Arrange.
        ReconfigureManagerInstances(true);
        await CreateFileAndWriteTextAsync("a/file.txt".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("a/another-file.txt".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("a/b/.config".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("a/b/c/.keep".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("a/c/d/e/f/g/.keep".AsBaselineFilesystemPath());

        // Act.
        var contents = await DirectoryManager.ListContentsAsync(
            new ListDirectoryContentsRequest { DirectoryPath = "a/".AsBaselineFilesystemPath() }
        );

        // Assert.
        contents.Contents.Should().HaveCount(13);
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a" && x.FinalPathPartIsObviouslyADirectory);
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/file.txt");
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/another-file.txt");
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/b" && x.FinalPathPartIsObviouslyADirectory);
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/b/.config");
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/b/c" && x.FinalPathPartIsObviouslyADirectory
            );
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/b/c/.keep");
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/c" && x.FinalPathPartIsObviouslyADirectory);
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d" && x.FinalPathPartIsObviouslyADirectory
            );
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d/e" && x.FinalPathPartIsObviouslyADirectory
            );
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d/e/f" && x.FinalPathPartIsObviouslyADirectory
            );
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d/e/f/g" && x.FinalPathPartIsObviouslyADirectory
            );
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/c/d/e/f/g/.keep");
    }
}
