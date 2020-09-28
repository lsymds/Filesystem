using FluentAssertions;
using Xunit;

namespace Storio.Tests.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public void It_Converts_A_String_Path_Into_A_Storio_Path()
        {
            const string originalPath = "/users/foo/bar/xyz/Documents/my-DOCUmenT.xslx";
            
            var pathRepresentation = originalPath.AsStorioPath();
            pathRepresentation.DirectoryLevels.Should().Be(5);
            pathRepresentation.DirectoryPath.Should().Be("users/foo/bar/xyz/Documents");
            pathRepresentation.DirectoryTree.Should().BeEquivalentTo(
                "users",
                "users/foo",
                "users/foo/bar",
                "users/foo/bar/xyz",
                "users/foo/bar/xyz/Documents"
            );
            pathRepresentation.Extension.Should().Be("xslx");
            pathRepresentation.FinalPathPart.Should().Be("my-DOCUmenT.xslx");
            pathRepresentation.OriginalPath.Should().Be(originalPath);
            pathRepresentation.NormalisedPath.Should().Be("users/foo/bar/xyz/Documents/my-DOCUmenT.xslx");
        }
    }
}
