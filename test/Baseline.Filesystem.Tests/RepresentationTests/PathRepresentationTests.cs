using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.RepresentationTests
{
    public class PathRepresentationTests
    {
        [Theory]
        [InlineData("a/b/", "")]
        [InlineData("a/b/.config", "")]
        [InlineData("a/b/file.xml", ".xml")]
        [InlineData("a/b/file.config.json", ".json")]
        [InlineData("a/b/file.cOnFIG", ".config")]
        public void It_Correctly_Retrieves_The_Files_Extension_If_There_Is_One(string path, string outcome)
        {
            // Arrange.
            var pathRepresentation = path.AsBaselineFilesystemPath();

            // Act.
            var extension = pathRepresentation.Extension;

            // Assert.
            extension.Should().Be(outcome);
        }

        [Theory]
        [InlineData("a/b/", "")]
        [InlineData("a/b/file", "file")]
        [InlineData("a/b/file.xml", "file")]
        [InlineData("a/b/file.config.json", "file.config")]
        public void It_Correctly_Retrieves_The_File_Name_If_There_Is_One(string path, string outcome)
        {
            // Arrange.
            var pathRepresentation = path.AsBaselineFilesystemPath();

            // Act.
            var fileName = pathRepresentation.FileNameWithoutExtension;

            // Assert.
            fileName.Should().Be(outcome);
        }

        [Theory]
        [InlineData("a/b/c/", "a/b/C/", false)]
        public void It_Correctly_Compares_Two_Path_Representations(string left, string right, bool invert)
        {
            // Arrange.
            
            // Act.
            
            // Assert.
        }
    }
}
