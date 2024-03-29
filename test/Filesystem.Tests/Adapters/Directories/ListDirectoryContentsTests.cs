using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LSymds.Filesystem.Tests.Adapters.Directories;

public class ListDirectoryContentsTests : BaseIntegrationTest
{
    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Lists_The_Contents_Of_A_Simple_Directory(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        await TestAdapter.CreateFileAndWriteTextAsync(
            "simple-list/file.txt".AsFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            "simple-list/another-file.txt".AsFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            "simple-list/.config".AsFilesystemPath()
        );

        // Act.
        var contents = await DirectoryManager.ListContentsAsync(
            new ListDirectoryContentsRequest
            {
                DirectoryPath = "simple-list/".AsFilesystemPath()
            }
        );

        // Assert.
        contents.Contents.Should().HaveCount(4);
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "simple-list" && x.FinalPathPartIsADirectory);
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "simple-list/file.txt");
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "simple-list/another-file.txt");
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "simple-list/.config");
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Lists_The_Contents_Of_A_More_Complex_Directory(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        await TestAdapter.CreateFileAndWriteTextAsync(
            "list-complex/a/file.txt".AsFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            "list-complex/a/another-file.txt".AsFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            "list-complex/a/b/.config".AsFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            "list-complex/a/b/c/.keep".AsFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync(
            "list-complex/a/c/d/e/f/g/.keep".AsFilesystemPath()
        );

        // Act.
        var contents = await DirectoryManager.ListContentsAsync(
            new ListDirectoryContentsRequest
            {
                DirectoryPath = "list-complex/a/".AsFilesystemPath()
            }
        );

        // Assert.
        contents.Contents.Should().HaveCount(13);
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "list-complex/a" && x.FinalPathPartIsADirectory
            );
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "list-complex/a/file.txt");
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "list-complex/a/another-file.txt");
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "list-complex/a/b" && x.FinalPathPartIsADirectory
            );
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "list-complex/a/b/.config");
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "list-complex/a/b/c" && x.FinalPathPartIsADirectory
            );
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "list-complex/a/b/c/.keep");
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "list-complex/a/c" && x.FinalPathPartIsADirectory
            );
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "list-complex/a/c/d" && x.FinalPathPartIsADirectory
            );
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "list-complex/a/c/d/e" && x.FinalPathPartIsADirectory
            );
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "list-complex/a/c/d/e/f" && x.FinalPathPartIsADirectory
            );
        contents.Contents
            .Should()
            .ContainSingle(
                x => x.NormalisedPath == "list-complex/a/c/d/e/f/g" && x.FinalPathPartIsADirectory
            );
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "list-complex/a/c/d/e/f/g/.keep");
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
                $"a-dir/{i}.txt".AsFilesystemPath()
            );
        }

        // Act.
        var contents = await DirectoryManager.ListContentsAsync(
            new ListDirectoryContentsRequest { DirectoryPath = "a-dir/".AsFilesystemPath() }
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

        await TestAdapter.CreateFileAndWriteTextAsync("simple/file.txt".AsFilesystemPath());
        await TestAdapter.CreateFileAndWriteTextAsync(
            "simple/another-file.txt".AsFilesystemPath()
        );
        await TestAdapter.CreateFileAndWriteTextAsync("simple/.config".AsFilesystemPath());

        // Act.
        var contents = await DirectoryManager.ListContentsAsync(
            new ListDirectoryContentsRequest
            {
                DirectoryPath = "simple/".AsFilesystemPath()
            }
        );

        // Assert.
        contents.Contents.Should().HaveCount(4);
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "simple" && x.FinalPathPartIsADirectory);
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
        var contents = await DirectoryManager.ListContentsAsync(
            new ListDirectoryContentsRequest { DirectoryPath = "a/".AsFilesystemPath() }
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
            .ContainSingle(x => x.NormalisedPath == "a/b/c" && x.FinalPathPartIsADirectory);
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/b/c/.keep");
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/c" && x.FinalPathPartIsADirectory);
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/c/d" && x.FinalPathPartIsADirectory);
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/c/d/e" && x.FinalPathPartIsADirectory);
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/c/d/e/f" && x.FinalPathPartIsADirectory);
        contents.Contents
            .Should()
            .ContainSingle(x => x.NormalisedPath == "a/c/d/e/f/g" && x.FinalPathPartIsADirectory);
        contents.Contents.Should().ContainSingle(x => x.NormalisedPath == "a/c/d/e/f/g/.keep");
    }
}
