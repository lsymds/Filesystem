using FluentAssertions;
using Xunit;

namespace LSymds.Filesystem.Tests.Core.RepresentationTests;

public class PathRepresentationTests
{
    [Theory]
    [InlineData("a/b/", "")]
    [InlineData("a/b/.config", "")]
    [InlineData("a/b/file.xml", ".xml")]
    [InlineData("a/b/file.config.json", ".json")]
    [InlineData("a/b/file.cOnFIG", ".config")]
    public void It_Correctly_Retrieves_The_Files_Extension_If_There_Is_One(
        string path,
        string outcome
    )
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
    [InlineData("a/b/.xml/config.xml", "config")]
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
    [InlineData(null, "a/b/c/", false)]
    [InlineData("a/b/c/", null, false)]
    [InlineData("a/b/c/", "a/b/C/", false)]
    [InlineData("a/b/c/", "a/b/c", false)]
    [InlineData("a/b/c/a.config", "a/b/c/a.config.example", false)]
    [InlineData("a/b/", "a/b/", true)]
    [InlineData("a/b/c/a.xml", "a/b/c/a.xml", true)]
    public void It_Correctly_Compares_Two_Path_Representations(
        string left,
        string right,
        bool outcome
    )
    {
        // Arrange.
        var leftRepresentation = left?.AsBaselineFilesystemPath();
        var rightRepresentation = right?.AsBaselineFilesystemPath();

        // Act.
        var equal = leftRepresentation == rightRepresentation;

        // Assert.
        equal.Should().Be(outcome);
    }

    [Theory]
    [InlineData("a/b", "a/b", true)]
    [InlineData("a/b/", "a/b/", true)]
    [InlineData("a/b/c/", "a/b/", true)]
    [InlineData("a/b/c", "a", false)]
    [InlineData("a/b/c/", "a/b/c", false)]
    [InlineData("a/b/c/", null, false)]
    [InlineData("a/b/c/", "a/b/c/d", false)]
    public void It_Correct_Identifies_If_A_Path_Starts_With_Another_Path(
        string left,
        string right,
        bool outcome
    )
    {
        // Arrange.
        var leftRepresentation = left.AsBaselineFilesystemPath();
        var rightRepresentation = right?.AsBaselineFilesystemPath();

        // Act.
        var startsWith = leftRepresentation.StartsWith(rightRepresentation);

        // Assert.
        startsWith.Should().Be(outcome);
    }
}
