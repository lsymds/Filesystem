using FluentAssertions;
using Xunit;

namespace LSymds.Filesystem.Tests.Core.Builders;

public class PathCombinationBuilderTests
{
    [Fact]
    public void It_Combines_Paths_When_Only_One_Is_Specified()
    {
        // Arrange.
        var originalPath = new PathRepresentationBuilder("users/foo/bar").Build();

        // Act.
        var path = new PathCombinationBuilder(originalPath).Build();

        // Assert.
        path.Should().BeEquivalentTo(originalPath, x => x.Excluding(y => y.GetPathTree));
    }

    [Fact]
    public void It_Combines_Many_Paths()
    {
        // Arrange.
        var firstPath = new PathRepresentationBuilder("C:/users/foo/bar").Build();
        var secondPath = new PathRepresentationBuilder(
            "another/simple/directory/structure"
        ).Build();
        var thirdPath = new PathRepresentationBuilder("image.jpeg").Build();

        // Act.
        var combinedPath = new PathCombinationBuilder(firstPath, secondPath, thirdPath).Build();

        // Assert.
        combinedPath.FinalPathPart.Should().Be("image.jpeg");
        combinedPath.FinalPathPartIsADirectory.Should().BeFalse();
        combinedPath.OriginalPath
            .Should()
            .Be("C:/users/foo/bar/another/simple/directory/structure/image.jpeg");
        combinedPath.NormalisedPath
            .Should()
            .Be("C:/users/foo/bar/another/simple/directory/structure/image.jpeg");
    }

    [Fact]
    public void It_Combines_Directories_And_Configures_Them_Correctly()
    {
        // Act.
        var combinedPath = new PathCombinationBuilder(
            "a".AsFilesystemPath(),
            "b/c/d".AsFilesystemPath(),
            "e/f/g/".AsFilesystemPath()
        ).Build();

        // Assert.
        combinedPath.FinalPathPart.Should().Be("g");
        combinedPath.FinalPathPartIsADirectory.Should().BeTrue();
        combinedPath.OriginalPath.Should().Be("a/b/c/d/e/f/g/");
        combinedPath.NormalisedPath.Should().Be("a/b/c/d/e/f/g");
    }
}
