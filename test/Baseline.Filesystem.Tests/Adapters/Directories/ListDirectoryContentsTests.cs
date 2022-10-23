using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.Directories;

public class ListDirectoryContentsTests : BaseIntegrationTest
{
    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Lists_The_Contents_Of_A_Simple_Directory(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        await TestAdapter.CreateFileAndWriteTextAsync("simple/file.txt".AsBaselineFilesystemPath());
        await TestAdapter.CreateFileAndWriteTextAsync(
            "simple/another-file.txt".AsBaselineFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync("simple/.config".AsBaselineFilesystemPath());

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
                x => x.NormalisedPath == "simple" && x.FinalPathPartIsADirectory
            );
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "simple/file.txt");
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "simple/another-file.txt");
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "simple/.config");
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Lists_The_Contents_Of_A_More_Complex_Directory(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

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
        var contents = await DirectoryManager.ListContentsAsync(
            new ListDirectoryContentsRequest { DirectoryPath = "a/".AsBaselineFilesystemPath() }
        );

        // Assert.
        contents.Contents.Should().HaveCount(13);
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a" && x.FinalPathPartIsADirectory);
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/file.txt");
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/another-file.txt");
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/b" && x.FinalPathPartIsADirectory);
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/b/.config");
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/b/c" && x.FinalPathPartIsADirectory
            );
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/b/c/.keep");
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/c" && x.FinalPathPartIsADirectory);
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d" && x.FinalPathPartIsADirectory
            );
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d/e" && x.FinalPathPartIsADirectory
            );
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d/e/f" && x.FinalPathPartIsADirectory
            );
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d/e/f/g" && x.FinalPathPartIsADirectory
            );
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/c/d/e/f/g/.keep");
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Lists_The_Contents_Of_A_Directory_With_A_Large_Number_Of_Files_In(
        Adapter adapter
    )
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        for (int i = 0; i < 1000; i++)
        {
            await TestAdapter.CreateFileAndWriteTextAsync(
                $"a-dir/{i}.txt".AsBaselineFilesystemPath()
            );
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

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Lists_The_Contents_Of_A_Simple_Directory_With_A_Root_Path(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        await TestAdapter.CreateFileAndWriteTextAsync("simple/file.txt".AsBaselineFilesystemPath());
        await TestAdapter.CreateFileAndWriteTextAsync(
            "simple/another-file.txt".AsBaselineFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync("simple/.config".AsBaselineFilesystemPath());

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
                x => x.NormalisedPath == "simple" && x.FinalPathPartIsADirectory
            );
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "simple/file.txt");
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "simple/another-file.txt");
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "simple/.config");
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Lists_The_Contents_Of_A_Complex_Directory_Within_A_Root_Path(
        Adapter adapter
    )
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

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
        var contents = await DirectoryManager.ListContentsAsync(
            new ListDirectoryContentsRequest { DirectoryPath = "a/".AsBaselineFilesystemPath() }
        );

        // Assert.
        contents.Contents.Should().HaveCount(13);
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a" && x.FinalPathPartIsADirectory);
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/file.txt");
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/another-file.txt");
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/b" && x.FinalPathPartIsADirectory);
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/b/.config");
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/b/c" && x.FinalPathPartIsADirectory
            );
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/b/c/.keep");
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/c" && x.FinalPathPartIsADirectory);
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d" && x.FinalPathPartIsADirectory
            );
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d/e" && x.FinalPathPartIsADirectory
            );
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d/e/f" && x.FinalPathPartIsADirectory
            );
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "a/c/d/e/f/g" && x.FinalPathPartIsADirectory
            );
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/c/d/e/f/g/.keep");
    }
}
