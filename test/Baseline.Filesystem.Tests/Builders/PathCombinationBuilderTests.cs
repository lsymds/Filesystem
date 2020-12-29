using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Builders
{
    public class PathCombinationBuilderTests
    {
        [Fact]
        public void It_Combines_Paths_When_Only_One_Is_Specified()
        {
            var originalPath = new PathRepresentationBuilder("users/foo/bar").Build();
            var path = new PathCombinationBuilder(originalPath).Build();

            path.Should().BeEquivalentTo(originalPath);
        }

        [Fact]
        public void It_Combines_Many_Paths()
        {
            var firstPath = new PathRepresentationBuilder("C:/users/foo/bar").Build();
            var secondPath = new PathRepresentationBuilder("another/simple/directory/structure").Build();
            var thirdPath = new PathRepresentationBuilder("image.jpeg").Build();
            
            var combinedPath = new PathCombinationBuilder(firstPath, secondPath, thirdPath).Build();
            combinedPath.DirectoryLevels.Should().Be(8);
            combinedPath.DirectoryPath.Should().Be("C:/users/foo/bar/another/simple/directory/structure");
            combinedPath.DirectoryTree.Should().BeEquivalentTo(
                "C:",
                "C:/users",
                "C:/users/foo",
                "C:/users/foo/bar",
                "C:/users/foo/bar/another",
                "C:/users/foo/bar/another/simple",
                "C:/users/foo/bar/another/simple/directory",
                "C:/users/foo/bar/another/simple/directory/structure"
            );
            combinedPath.Extension.Should().Be("jpeg");
            combinedPath.FinalPathPart.Should().Be("image.jpeg");
            combinedPath.FinalPathPartIsObviouslyADirectory.Should().BeFalse();
            combinedPath.OriginalPath.Should().Be("C:/users/foo/bar/another/simple/directory/structure/image.jpeg");
            combinedPath.NormalisedPath.Should().Be("C:/users/foo/bar/another/simple/directory/structure/image.jpeg");
        }
    }
}
