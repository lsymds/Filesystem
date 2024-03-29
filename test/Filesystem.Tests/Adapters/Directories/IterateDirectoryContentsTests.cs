using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LSymds.Filesystem.Tests.Adapters.Directories;

public class IterateDirectoryContentsTests : BaseIntegrationTest
{
    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Iterates_The_Contents_Of_A_Simple_Directory(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var contents = new List<PathRepresentation>();

        await TestAdapter.CreateFileAndWriteTextAsync(
            "iterate-simple/file.txt".AsFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            "iterate-simple/another-file.txt".AsFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            "iterate-simple/.config".AsFilesystemPath()
        );

        // Act.
        await DirectoryManager.IterateContentsAsync(
            new IterateDirectoryContentsRequest
            {
                DirectoryPath = "iterate-simple/".AsFilesystemPath(),
                Action = paths =>
                {
                    contents.Add(paths);
                    return Task.FromResult(true);
                }
            }
        );

        // Assert.
        contents.Should().HaveCount(4);
        contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "iterate-simple" && x.FinalPathPartIsADirectory
            );
        contents.Should().ContainSingle(x => x.NormalisedPath == "iterate-simple/file.txt");
        contents.Should().ContainSingle(x => x.NormalisedPath == "iterate-simple/another-file.txt");
        contents.Should().ContainSingle(x => x.NormalisedPath == "iterate-simple/.config");
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Lists_The_Contents_Of_A_More_Complex_Directory(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var contents = new List<PathRepresentation>();

        await TestAdapter.CreateFileAndWriteTextAsync(
            "iterate-complex/a/file.txt".AsFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            "iterate-complex/a/another-file.txt".AsFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            "iterate-complex/a/b/.config".AsFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            "iterate-complex/a/b/c/.keep".AsFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            "iterate-complex/a/c/d/e/f/g/.keep".AsFilesystemPath()
        );

        // Act.
        await DirectoryManager.IterateContentsAsync(
            new IterateDirectoryContentsRequest
            {
                DirectoryPath = "iterate-complex/a/".AsFilesystemPath(),
                Action = paths =>
                {
                    contents.Add(paths);
                    return Task.FromResult(true);
                }
            }
        );

        // Assert.
        contents.Should().HaveCount(13);
        contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "iterate-complex/a" && x.FinalPathPartIsADirectory
            );
        contents.Should().ContainSingle(x => x.NormalisedPath == "iterate-complex/a/file.txt");
        contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "iterate-complex/a/another-file.txt");
        contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "iterate-complex/a/b" && x.FinalPathPartIsADirectory
            );
        contents.Should().ContainSingle(x => x.NormalisedPath == "iterate-complex/a/b/.config");
        contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "iterate-complex/a/b/c" && x.FinalPathPartIsADirectory
            );
        contents.Should().ContainSingle(x => x.NormalisedPath == "iterate-complex/a/b/c/.keep");
        contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "iterate-complex/a/c" && x.FinalPathPartIsADirectory
            );
        contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "iterate-complex/a/c/d" && x.FinalPathPartIsADirectory
            );
        contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "iterate-complex/a/c/d/e" && x.FinalPathPartIsADirectory
            );
        contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "iterate-complex/a/c/d/e/f" && x.FinalPathPartIsADirectory
            );
        contents
            .Should()
            .ContainSingle(
                x =>
                    x.NormalisedPath == "iterate-complex/a/c/d/e/f/g" && x.FinalPathPartIsADirectory
            );
        contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "iterate-complex/a/c/d/e/f/g/.keep");
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Lists_The_Contents_Of_A_Directory_With_A_Large_Number_Of_Files_In(
        Adapter adapter
    )
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var contents = new List<PathRepresentation>();

        for (int i = 0; i < 1000; i++)
        {
            await TestAdapter.CreateFileAndWriteTextAsync(
                $"iterate-large/{i}.txt".AsFilesystemPath()
            );
        }

        // Act.
        await DirectoryManager.IterateContentsAsync(
            new IterateDirectoryContentsRequest
            {
                DirectoryPath = "iterate-large/".AsFilesystemPath(),
                Action = p =>
                {
                    contents.Add(p);
                    return Task.FromResult(true);
                }
            }
        );

        // Assert.
        for (var i = 0; i < 1000; i++)
        {
            // ReSharper disable once AccessToModifiedClosure
            contents.Should().ContainSingle(x => x.OriginalPath == $"iterate-large/{i}.txt");
        }
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Lists_The_Contents_Of_A_Simple_Directory_With_A_Root_Path(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        var contents = new List<PathRepresentation>();

        await TestAdapter.CreateFileAndWriteTextAsync("simple/file.txt".AsFilesystemPath());
        await TestAdapter.CreateFileAndWriteTextAsync(
            "simple/another-file.txt".AsFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync("simple/.config".AsFilesystemPath());

        // Act.
        await DirectoryManager.IterateContentsAsync(
            new IterateDirectoryContentsRequest
            {
                DirectoryPath = "simple/".AsFilesystemPath(),
                Action = p =>
                {
                    contents.Add(p);
                    return Task.FromResult(true);
                }
            }
        );

        // Assert.
        contents.Should().HaveCount(4);
        contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "simple" && x.FinalPathPartIsADirectory);
        contents.Should().ContainSingle(x => x.NormalisedPath == "simple/file.txt");
        contents.Should().ContainSingle(x => x.NormalisedPath == "simple/another-file.txt");
        contents.Should().ContainSingle(x => x.NormalisedPath == "simple/.config");
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Lists_The_Contents_Of_A_Complex_Directory_Within_A_Root_Path(
        Adapter adapter
    )
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        var contents = new List<PathRepresentation>();

        await TestAdapter.CreateFileAndWriteTextAsync("a/file.txt".AsFilesystemPath());
        await TestAdapter.CreateFileAndWriteTextAsync(
            "a/another-file.txt".AsFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync("a/b/.config".AsFilesystemPath());
        await TestAdapter.CreateFileAndWriteTextAsync("a/b/c/.keep".AsFilesystemPath());
        await TestAdapter.CreateFileAndWriteTextAsync(
            "a/c/d/e/f/g/.keep".AsFilesystemPath()
        );

        // Act.
        await DirectoryManager.IterateContentsAsync(
            new IterateDirectoryContentsRequest
            {
                DirectoryPath = "a/".AsFilesystemPath(),
                Action = p =>
                {
                    contents.Add(p);
                    return Task.FromResult(true);
                }
            }
        );

        // Assert.
        contents.Should().HaveCount(13);
        contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a" && x.FinalPathPartIsADirectory);
        contents.Should().ContainSingle(x => x.NormalisedPath == "a/file.txt");
        contents.Should().ContainSingle(x => x.NormalisedPath == "a/another-file.txt");
        contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/b" && x.FinalPathPartIsADirectory);
        contents.Should().ContainSingle(x => x.NormalisedPath == "a/b/.config");
        contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/b/c" && x.FinalPathPartIsADirectory);
        contents.Should().ContainSingle(x => x.NormalisedPath == "a/b/c/.keep");
        contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/c" && x.FinalPathPartIsADirectory);
        contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/c/d" && x.FinalPathPartIsADirectory);
        contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/c/d/e" && x.FinalPathPartIsADirectory);
        contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/c/d/e/f" && x.FinalPathPartIsADirectory);
        contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/c/d/e/f/g" && x.FinalPathPartIsADirectory);
        contents.Should().ContainSingle(x => x.NormalisedPath == "a/c/d/e/f/g/.keep");
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Can_Exit_An_Iteration_Early(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        var count = 0;

        await TestAdapter.CreateFileAndWriteTextAsync("a/file.txt".AsFilesystemPath());
        await TestAdapter.CreateFileAndWriteTextAsync(
            "a/another-file.txt".AsFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            "a/third-file.txt".AsFilesystemPath()
        );

        // Act.
        await DirectoryManager.IterateContentsAsync(
            new IterateDirectoryContentsRequest
            {
                DirectoryPath = "a/".AsFilesystemPath(),
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
