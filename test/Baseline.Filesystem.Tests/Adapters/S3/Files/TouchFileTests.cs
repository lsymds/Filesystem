using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Files;

public class TouchFileTests : BaseIntegrationTest
{
    [Fact]
    public async Task It_Successfully_Touches_A_File_In_S3()
    {
        // Arrange.
        var path = RandomFilePathRepresentation();

        // Act.
        await CreateFileAndWriteTextAsync(path);

        // Assert.
        await ExpectFileToExistAsync(path);
        (await ReadFileAsStringAsync(path)).Should().BeEmpty();
    }

    [Fact]
    public async Task It_Successfully_Touches_A_File_In_S3_Under_A_Root_Path()
    {
        // Arrange.
        ConfigureTestAsync(true);

        var path = RandomFilePathRepresentation();

        // Act.
        await CreateFileAndWriteTextAsync(path);

        // Assert.
        await ExpectFileToExistAsync(path);
        (await ReadFileAsStringAsync(path)).Should().BeEmpty();
    }
}
