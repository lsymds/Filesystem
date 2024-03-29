using FluentAssertions;
using Xunit;

namespace LSymds.Filesystem.Tests.Core.Extensions;

public class StringExtensionsTests
{
    [Fact]
    public void It_Converts_A_String_Path_Into_A_Filesystem_Path()
    {
        // Arrange.
        const string originalPath = "/users/foo/bar/xyz/Documents/my-DOCUmenT.xslx";

        // Act.
        var pathRepresentation = originalPath.AsFilesystemPath();

        // Assert.
        pathRepresentation.FinalPathPart.Should().Be("my-DOCUmenT.xslx");
        pathRepresentation.OriginalPath.Should().Be(originalPath);
        pathRepresentation
            .NormalisedPath
            .Should()
            .Be("users/foo/bar/xyz/Documents/my-DOCUmenT.xslx");
    }
}
