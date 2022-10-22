using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.Directories;

public class IterateDirectoryContentsTests : BaseIntegrationTest
{
    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Iterates_The_Contents_Of_A_Simple_Directory(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var files = new List<PathRepresentation>();

        await TestAdapter.CreateFileAndWriteTextAsync("simple/file.txt".AsBaselineFilesystemPath());
        await TestAdapter.CreateFileAndWriteTextAsync(
            "simple/another-file.txt".AsBaselineFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync("simple/.config".AsBaselineFilesystemPath());

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

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Lists_The_Contents_Of_A_More_Complex_Directory(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var files = new List<PathRepresentation>();

        await TestAdapter.CreateFileAndWriteTextAsync("a/file.txt".AsBaselineFilesystemPath());
        await TestAdapter.CreateFileAndWriteTextAsync(
            "a/another-file.txt".AsBaselineFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync("a/b/.config".AsBaselineFilesystemPath());
        await TestAdapter.CreateFileAndWriteTextAsync("a/b/c/.keep".AsBaselineFilesystemPath());
        await TestAdapter.CreateFileAndWriteTextAsync(
            "a/c/d/e/f/g/.keep".AsBaselineFilesystemPath()
        );

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
            .ContainSingle(x => x.NormalisedPath == "a" && x.FinalPathPartIsObviouslyADirectory);
        files.Should().ContainSingle(x => x.NormalisedPath == "a/file.txt");
        files.Should().ContainSingle(x => x.NormalisedPath == "a/another-file.txt");
        files
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/b" && x.FinalPathPartIsObviouslyADirectory);
        files.Should().ContainSingle(x => x.NormalisedPath == "a/b/.config");
        files
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/b/c" && x.FinalPathPartIsObviouslyADirectory
            );
        files.Should().ContainSingle(x => x.NormalisedPath == "a/b/c/.keep");
        files
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/c" && x.FinalPathPartIsObviouslyADirectory);
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

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Lists_The_Contents_Of_A_Directory_With_A_Large_Number_Of_Files_In(
        Adapter adapter
    )
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var files = new List<PathRepresentation>();

        for (int i = 0; i < 1000; i++)
        {
            await TestAdapter.CreateFileAndWriteTextAsync(
                $"a-dir/{i}.txt".AsBaselineFilesystemPath()
            );
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
            // ReSharper disable once AccessToModifiedClosure
            files.Should().ContainSingle(x => x.OriginalPath == $"a-dir/{i}.txt");
        }
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Lists_The_Contents_Of_A_Simple_Directory_With_A_Root_Path(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        var files = new List<PathRepresentation>();

        await TestAdapter.CreateFileAndWriteTextAsync("simple/file.txt".AsBaselineFilesystemPath());
        await TestAdapter.CreateFileAndWriteTextAsync(
            "simple/another-file.txt".AsBaselineFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync("simple/.config".AsBaselineFilesystemPath());

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

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Lists_The_Contents_Of_A_Complex_Directory_Within_A_Root_Path(
        Adapter adapter
    )
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        var files = new List<PathRepresentation>();

        await TestAdapter.CreateFileAndWriteTextAsync("a/file.txt".AsBaselineFilesystemPath());
        await TestAdapter.CreateFileAndWriteTextAsync(
            "a/another-file.txt".AsBaselineFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync("a/b/.config".AsBaselineFilesystemPath());
        await TestAdapter.CreateFileAndWriteTextAsync("a/b/c/.keep".AsBaselineFilesystemPath());
        await TestAdapter.CreateFileAndWriteTextAsync(
            "a/c/d/e/f/g/.keep".AsBaselineFilesystemPath()
        );

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
            .ContainSingle(x => x.NormalisedPath == "a" && x.FinalPathPartIsObviouslyADirectory);
        files.Should().ContainSingle(x => x.NormalisedPath == "a/file.txt");
        files.Should().ContainSingle(x => x.NormalisedPath == "a/another-file.txt");
        files
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/b" && x.FinalPathPartIsObviouslyADirectory);
        files.Should().ContainSingle(x => x.NormalisedPath == "a/b/.config");
        files
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/b/c" && x.FinalPathPartIsObviouslyADirectory
            );
        files.Should().ContainSingle(x => x.NormalisedPath == "a/b/c/.keep");
        files
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/c" && x.FinalPathPartIsObviouslyADirectory);
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

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Can_Exit_An_Iteration_Early(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        var count = 0;

        await TestAdapter.CreateFileAndWriteTextAsync("a/file.txt".AsBaselineFilesystemPath());
        await TestAdapter.CreateFileAndWriteTextAsync(
            "a/another-file.txt".AsBaselineFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            "a/third-file.txt".AsBaselineFilesystemPath()
        );

        // Act.
        await DirectoryManager.IterateContentsAsync(
            new IterateDirectoryContentsRequest
            {
                DirectoryPath = "a/".AsBaselineFilesystemPath(),
                Action = _ =>
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
