using System;
using FluentAssertions;
using Xunit;

namespace Storio.Tests.Builders
{
    public class PathRepresentationBuilderTests
    {
        [Theory]
        [InlineData("*")]
        [InlineData("\"")]
        [InlineData("\\")]
        [InlineData("[")]
        [InlineData("]")]
        [InlineData(":")]
        [InlineData(";")]
        [InlineData("|")]
        [InlineData(",")]
        [InlineData("<")]
        [InlineData(">")]
        [InlineData("'")]
        [InlineData("$")]
        [InlineData("£")]
        [InlineData("%")]
        [InlineData("^")]
        [InlineData("(")]
        [InlineData(")")]
        [InlineData("+")]
        [InlineData("=")]
        [InlineData("!")]
        public void It_Throws_An_Exception_If_Path_Contains_An_Invalid_Character(string character)
        {
            Action func = () => 
                new PathRepresentationBuilder($"/home/users/Foo/Documents/a-{character}-folder/file.jpg")
                    .Build();

            func.Should().ThrowExactly<PathContainsInvalidCharacterException>();
        }

        [Theory]
        [InlineData("~/Documents/my-file.jpg")]
        [InlineData("../../files/")]
        [InlineData("./debug/files/output-file.xml")]
        public void It_Throws_An_Exception_If_The_Path_Is_A_Relative_Path(string path)
        {
            Action func = () => new PathRepresentationBuilder(path).Build();
            func.Should().ThrowExactly<PathIsRelativeException>();
        }

        [Fact]
        public void It_Successfully_Builds_A_Simple_File_Path()
        {
            const string simplePath = "/my-file.jpg";
            
            var builtRepresentation = new PathRepresentationBuilder(simplePath).Build();
            builtRepresentation.DirectoryLevels.Should().Be(0);
            builtRepresentation.DirectoryPath.Should().BeNullOrWhiteSpace();
            builtRepresentation.DirectoryTree.Should().BeEmpty();
            builtRepresentation.Extension.Should().Be("jpg");
            builtRepresentation.FinalPathPart.Should().Be("my-file.jpg");
            builtRepresentation.OriginalPath.Should().Be(simplePath);
            builtRepresentation.NormalisedPath.Should().Be("my-file.jpg");
        }

        [Fact]
        public void It_Successfully_Builds_A_More_Complex_File_Path()
        {
            const string moreComplexPath = "users/Documents/Projects/ProjectA/TechnicalSpecs/MainTechnicalSpec.docx";
            
            var builtRepresentation = new PathRepresentationBuilder(moreComplexPath).Build();
            builtRepresentation.DirectoryLevels.Should().Be(5);
            builtRepresentation.DirectoryPath.Should().Be("users/Documents/Projects/ProjectA/TechnicalSpecs/");
            builtRepresentation.DirectoryTree.Should().BeEquivalentTo(
                "users/",
                "users/Documents/",
                "users/Documents/Projects/",
                "users/Documents/Projects/ProjectA/",
                "users/Documents/Projects/ProjectA/TechnicalSpecs/"
            );
            builtRepresentation.Extension.Should().Be("docx");
            builtRepresentation.FinalPathPart.Should().Be("MainTechnicalSpec.docx");
            builtRepresentation.OriginalPath.Should().Be(moreComplexPath);
            builtRepresentation.NormalisedPath.Should().Be(moreComplexPath);
        }
    }
}
