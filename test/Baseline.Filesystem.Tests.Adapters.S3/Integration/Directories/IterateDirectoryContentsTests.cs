using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Directories;

public class IterateDirectoryContentsTests : BaseS3AdapterIntegrationTest
{
    [Fact]
    public async Task It_Iterates_The_Contents_Of_A_Simple_Directory()
    {
        // Arrange.
        var files = new List<PathRepresentation>();

        await CreateFileAndWriteTextAsync("simple/file.txt".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("simple/another-file.txt".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("simple/.config".AsBaselineFilesystemPath());

        // Act.
        await DirectoryManager.IterateContentsAsync(
            new IterateDirectoryContentsRequest
            {
                DirectoryPath = "simple/".AsBaselineFilesystemPath(),
                Action = paths =>
                {
                    files.Add(paths);
                    return Task.FromResult(true);
                }
            }
        );

        // Assert.
        files.Should().HaveCount(4);
        files
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "simple" && x.FinalPathPartIsObviouslyADirectory
            );
        files.Should().ContainSingle(x => x.NormalisedPath == "simple/file.txt");
        files.Should().ContainSingle(x => x.NormalisedPath == "simple/another-file.txt");
        files.Should().ContainSingle(x => x.NormalisedPath == "simple/.config");
    }

    [Fact]
    public async Task It_Lists_The_Contents_Of_A_More_Complex_Directory()
    {
        // Arrange.
        var files = new List<PathRepresentation>();

        await CreateFileAndWriteTextAsync("a/file.txt".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("a/another-file.txt".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("a/b/.config".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("a/b/c/.keep".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("a/c/d/e/f/g/.keep".AsBaselineFilesystemPath());

        // Act.
        await DirectoryManager.IterateContentsAsync(
            new IterateDirectoryContentsRequest
            {
                DirectoryPath = "a/".AsBaselineFilesystemPath(),
                Action = paths =>
                {
                    files.Add(paths);
                    return Task.FromResult(true);
                }
            }
        );

        // Assert.
        files.Should().HaveCount(13);
        files
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a" && x.FinalPathPartIsObviouslyADirectory
            );
        files.Should().ContainSingle(x => x.NormalisedPath == "a/file.txt");
        files.Should().ContainSingle(x => x.NormalisedPath == "a/another-file.txt");
        files
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/b" && x.FinalPathPartIsObviouslyADirectory
            );
        files.Should().ContainSingle(x => x.NormalisedPath == "a/b/.config");
        files
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/b/c" && x.FinalPathPartIsObviouslyADirectory
            );
        files.Should().ContainSingle(x => x.NormalisedPath == "a/b/c/.keep");
        files
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c" && x.FinalPathPartIsObviouslyADirectory
            );
        files
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d" && x.FinalPathPartIsObviouslyADirectory
            );
        files
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d/e" && x.FinalPathPartIsObviouslyADirectory
            );
        files
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d/e/f" && x.FinalPathPartIsObviouslyADirectory
            );
        files
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d/e/f/g" && x.FinalPathPartIsObviouslyADirectory
            );
        files.Should().ContainSingle(x => x.NormalisedPath == "a/c/d/e/f/g/.keep");
    }

    [Fact]
    public async Task It_Lists_The_Contents_Of_A_Directory_With_A_Large_Number_Of_Files_In()
    {
        // Arrange.
        var files = new List<PathRepresentation>();

        for (int i = 0; i < 1000; i++)
        {
            await CreateFileAndWriteTextAsync($"a-dir/{i}.txt".AsBaselineFilesystemPath());
        }

        // Act.
        await DirectoryManager.IterateContentsAsync(
            new IterateDirectoryContentsRequest
            {
                DirectoryPath = "a-dir/".AsBaselineFilesystemPath(),
                Action = p =>
                {
                    files.Add(p);
                    return Task.FromResult(true);
                }
            }
        );

        // Assert.
        for (var i = 0; i < 1000; i++)
        {
            files.Should().ContainSingle(x => x.OriginalPath == $"a-dir/{i}.txt");
        }
    }

    [Fact]
    public async Task It_Lists_The_Contents_Of_A_Simple_Directory_With_A_Root_Path()
    {
        // Arrange.
        ReconfigureManagerInstances(true);

        var files = new List<PathRepresentation>();

        await CreateFileAndWriteTextAsync("simple/file.txt".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("simple/another-file.txt".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("simple/.config".AsBaselineFilesystemPath());

        // Act.
        await DirectoryManager.IterateContentsAsync(
            new IterateDirectoryContentsRequest
            {
                DirectoryPath = "simple/".AsBaselineFilesystemPath(),
                Action = p =>
                {
                    files.Add(p);
                    return Task.FromResult(true);
                }
            }
        );

        // Assert.
        files.Should().HaveCount(4);
        files
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "simple" && x.FinalPathPartIsObviouslyADirectory
            );
        files.Should().ContainSingle(x => x.NormalisedPath == "simple/file.txt");
        files.Should().ContainSingle(x => x.NormalisedPath == "simple/another-file.txt");
        files.Should().ContainSingle(x => x.NormalisedPath == "simple/.config");
    }

    [Fact]
    public async Task It_Lists_The_Contents_Of_A_Complex_Directory_Within_A_Root_Path()
    {
        // Arrange.
        ReconfigureManagerInstances(true);

        var files = new List<PathRepresentation>();

        await CreateFileAndWriteTextAsync("a/file.txt".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("a/another-file.txt".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("a/b/.config".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("a/b/c/.keep".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("a/c/d/e/f/g/.keep".AsBaselineFilesystemPath());

        // Act.
        await DirectoryManager.IterateContentsAsync(
            new IterateDirectoryContentsRequest
            {
                DirectoryPath = "a/".AsBaselineFilesystemPath(),
                Action = p =>
                {
                    files.Add(p);
                    return Task.FromResult(true);
                }
            }
        );

        // Assert.
        files.Should().HaveCount(13);
        files
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a" && x.FinalPathPartIsObviouslyADirectory
            );
        files.Should().ContainSingle(x => x.NormalisedPath == "a/file.txt");
        files.Should().ContainSingle(x => x.NormalisedPath == "a/another-file.txt");
        files
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/b" && x.FinalPathPartIsObviouslyADirectory
            );
        files.Should().ContainSingle(x => x.NormalisedPath == "a/b/.config");
        files
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/b/c" && x.FinalPathPartIsObviouslyADirectory
            );
        files.Should().ContainSingle(x => x.NormalisedPath == "a/b/c/.keep");
        files
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c" && x.FinalPathPartIsObviouslyADirectory
            );
        files
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d" && x.FinalPathPartIsObviouslyADirectory
            );
        files
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d/e" && x.FinalPathPartIsObviouslyADirectory
            );
        files
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d/e/f" && x.FinalPathPartIsObviouslyADirectory
            );
        files
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d/e/f/g" && x.FinalPathPartIsObviouslyADirectory
            );
        files.Should().ContainSingle(x => x.NormalisedPath == "a/c/d/e/f/g/.keep");
    }

    [Fact]
    public async Task It_Can_Exit_An_Iteration_Early()
    {
        // Arrange.
        ReconfigureManagerInstances(true);

        var count = 0;

        await CreateFileAndWriteTextAsync("a/file.txt".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("a/another-file.txt".AsBaselineFilesystemPath());
        await CreateFileAndWriteTextAsync("a/third-file.txt".AsBaselineFilesystemPath());

        // Act.
        await DirectoryManager.IterateContentsAsync(
            new IterateDirectoryContentsRequest
            {
                DirectoryPath = "a/".AsBaselineFilesystemPath(),
                Action = p =>
                {
                    count += 1;

                    if (count == 2)
                    {
                        return Task.FromResult(false);
                    }

                    return Task.FromResult(true);
                }
            }
        );

        // Assert.
        count.Should().Be(2);
    }
}
