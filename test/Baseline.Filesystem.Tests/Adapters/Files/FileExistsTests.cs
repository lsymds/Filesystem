using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Files;

public class FileExistsTests : BaseIntegrationTest
{
    [Theory]
    [InlineData(Adapter.S3)]
    public async Task It_Returns_True_When_A_File_Does_Exist(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var path = TestUtilities.RandomFilePathRepresentation();

        await TestAdapter.CreateFileAndWriteTextAsync(path);

        // Act.
        var response = await FileManager.ExistsAsync(new FileExistsRequest { FilePath = path });

        // Assert.
        response.FileExists.Should().BeTrue();
    }

    [Theory]
    [InlineData(Adapter.S3)]
    public async Task It_Returns_False_When_A_File_Does_Not_Exist(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        // Act.
        var response = await FileManager.ExistsAsync(
            new FileExistsRequest { FilePath = TestUtilities.RandomFilePathRepresentation() }
        );

        // Assert.
        response.FileExists.Should().BeFalse();
    }

    [Theory]
    [InlineData(Adapter.S3)]
    public async Task It_Returns_True_When_A_File_Exists_Under_A_Root_Path(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        var path = TestUtilities.RandomFilePathRepresentation();

        await TestAdapter.CreateFileAndWriteTextAsync(path);

        // Act.
        var response = await FileManager.ExistsAsync(new FileExistsRequest { FilePath = path });

        // Assert.
        response.FileExists.Should().BeTrue();
    }
}
