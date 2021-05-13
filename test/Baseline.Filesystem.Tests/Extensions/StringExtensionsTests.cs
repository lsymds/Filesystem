using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public void It_Converts_A_String_Path_Into_A_BaselineFilesystem_Path()
        {
            // Arrange.
            const string originalPath = "/users/foo/bar/xyz/Documents/my-DOCUmenT.xslx";
            
            // Act.
            var pathRepresentation = originalPath.AsBaselineFilesystemPath();
            
            // Assert.
            pathRepresentation.FinalPathPart.Should().Be("my-DOCUmenT.xslx");
            pathRepresentation.OriginalPath.Should().Be(originalPath);
            pathRepresentation.NormalisedPath.Should().Be("users/foo/bar/xyz/Documents/my-DOCUmenT.xslx");
        }
    }
}
