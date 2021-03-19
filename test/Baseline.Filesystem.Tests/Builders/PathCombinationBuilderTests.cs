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

            path.Should().BeEquivalentTo(originalPath, x => x.Excluding(y => y.GetPathTree));
        }

        [Fact]
        public void It_Combines_Many_Paths()
        {
            var firstPath = new PathRepresentationBuilder("C:/users/foo/bar").Build();
            var secondPath = new PathRepresentationBuilder("another/simple/directory/structure").Build();
            var thirdPath = new PathRepresentationBuilder("image.jpeg").Build();
            
            var combinedPath = new PathCombinationBuilder(firstPath, secondPath, thirdPath).Build();
            combinedPath.FinalPathPart.Should().Be("image.jpeg");
            combinedPath.FinalPathPartIsObviouslyADirectory.Should().BeFalse();
            combinedPath.OriginalPath.Should().Be("C:/users/foo/bar/another/simple/directory/structure/image.jpeg");
            combinedPath.NormalisedPath.Should().Be("C:/users/foo/bar/another/simple/directory/structure/image.jpeg");
        }

        [Fact]
        public void It_Combines_Directories_And_Configures_Them_Correctly()
        {
            var combinedPath = new PathCombinationBuilder(
                "a".AsBaselineFilesystemPath(), 
                "b/c/d".AsBaselineFilesystemPath(), 
                "e/f/g/".AsBaselineFilesystemPath()
            ).Build();

            combinedPath.FinalPathPart.Should().Be("g");
            combinedPath.FinalPathPartIsObviouslyADirectory.Should().BeTrue();
            combinedPath.OriginalPath.Should().Be("a/b/c/d/e/f/g/");
            combinedPath.NormalisedPath.Should().Be("a/b/c/d/e/f/g");
        }
    }
}
