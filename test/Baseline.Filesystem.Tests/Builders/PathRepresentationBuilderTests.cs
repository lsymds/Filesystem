using System;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Builders
{
    public class PathRepresentationBuilderTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("        ")]
        public void It_Throws_An_Exception_When_A_Blank_Path_Is_Provided(string path)
        {
            Action nullCheck = () => new PathRepresentationBuilder(null).Build();
            nullCheck.Should().ThrowExactly<PathIsBlankException>();
            
            Action paramCheck = () => path.AsBaselineFilesystemPath();
            paramCheck.Should().ThrowExactly<PathIsBlankException>();
        }
        
        [Theory]
        [InlineData("*")]
        [InlineData("\"")]
        [InlineData("\\")]
        [InlineData("[")]
        [InlineData("]")]
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
        public void It_Builds_A_Simple_File_Path()
        {
            const string simplePath = "/my-file.jpg";
            
            var builtRepresentation = new PathRepresentationBuilder(simplePath).Build();
            builtRepresentation.FinalPathPart.Should().Be("my-file.jpg");
            builtRepresentation.FinalPathPartIsObviouslyADirectory.Should().BeFalse();
            builtRepresentation.OriginalPath.Should().Be(simplePath);
            builtRepresentation.NormalisedPath.Should().Be("my-file.jpg");
        }

        [Fact]
        public void It_Builds_A_More_Complex_File_Path()
        {
            const string moreComplexPath = "users/Documents/Projects/ProjectA/TechnicalSpecs/MainTechnicalSpec.docx";
            
            var builtRepresentation = new PathRepresentationBuilder(moreComplexPath).Build();
            builtRepresentation.FinalPathPart.Should().Be("MainTechnicalSpec.docx");
            builtRepresentation.FinalPathPartIsObviouslyADirectory.Should().BeFalse();
            builtRepresentation.OriginalPath.Should().Be(moreComplexPath);
            builtRepresentation.NormalisedPath.Should().Be(moreComplexPath);
        }

        [Fact]
        public void It_Builds_A_Root_Directory()
        {
            var builtRepresentation = new PathRepresentationBuilder("/users").Build();
            builtRepresentation.FinalPathPart.Should().Be("users");
            builtRepresentation.FinalPathPartIsObviouslyADirectory.Should().BeFalse();
            builtRepresentation.OriginalPath.Should().Be("/users");
            builtRepresentation.NormalisedPath.Should().Be("users");
        }

        [Fact]
        public void It_Builds_A_More_Complex_Directory_Structure()
        {
            const string moreComplexDirectoryStructure = "/var/www/hosted/wp-files/content/themes/my-theme/storage/";
            
            var builtRepresentation = new PathRepresentationBuilder(moreComplexDirectoryStructure).Build();
            builtRepresentation.FinalPathPart.Should().Be("storage");
            builtRepresentation.FinalPathPartIsObviouslyADirectory.Should().BeTrue();
            builtRepresentation.OriginalPath.Should().Be(moreComplexDirectoryStructure);
            builtRepresentation.NormalisedPath.Should().Be("var/www/hosted/wp-files/content/themes/my-theme/storage");
        }

        [Fact]
        public void It_Builds_A_File_Path_In_A_Hidden_Folder()
        {
            const string hiddenFilePath = "/users/foo/.ssh/id_rsa";
            
            var builtRepresentation = new PathRepresentationBuilder(hiddenFilePath).Build();
            builtRepresentation.FinalPathPart.Should().Be("id_rsa");
            builtRepresentation.FinalPathPartIsObviouslyADirectory.Should().BeFalse();
            builtRepresentation.OriginalPath.Should().Be(hiddenFilePath);
            builtRepresentation.NormalisedPath.Should().Be("users/foo/.ssh/id_rsa");
        }

        [Fact]
        public void It_Builds_A_Hidden_File_Not_In_A_Directory()
        {
            var builtRepresentation = new PathRepresentationBuilder(".npmrc").Build();
            builtRepresentation.FinalPathPart.Should().Be(".npmrc");
            builtRepresentation.FinalPathPartIsObviouslyADirectory.Should().BeFalse();
            builtRepresentation.OriginalPath.Should().Be(".npmrc");
            builtRepresentation.NormalisedPath.Should().Be(".npmrc");
        }

        [Fact]
        public void It_Builds_A_Hidden_File_In_A_Directory_With_An_Extension()
        {
            const string hiddenFileInDirectoryWithExtension = "/users/FOO/configuration/.tailwind.config";
            
            var builtRepresentation = new PathRepresentationBuilder(hiddenFileInDirectoryWithExtension).Build();
            builtRepresentation.FinalPathPart.Should().Be(".tailwind.config");
            builtRepresentation.FinalPathPartIsObviouslyADirectory.Should().BeFalse();
            builtRepresentation.OriginalPath.Should().Be(hiddenFileInDirectoryWithExtension);
            builtRepresentation.NormalisedPath.Should().Be("users/FOO/configuration/.tailwind.config");
        }
    }
}
