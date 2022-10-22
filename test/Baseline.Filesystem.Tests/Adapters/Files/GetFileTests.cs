using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.Files;

public class GetFileTests : BaseIntegrationTest
{
    [Theory]
    [InlineData(Adapter.S3)]
    public async Task It_Returns_Null_If_File_Does_Not_Exist(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        // Act.
        var response = await FileManager.GetAsync(
            new GetFileRequest { FilePath = TestUtilities.RandomFilePathRepresentation() }
        );

        // Assert.
        response.Should().BeNull();
    }

    [Theory]
    [InlineData(Adapter.S3)]
    public async Task It_Returns_The_File_If_It_Does_Exist(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var path = TestUtilities.RandomFilePathRepresentation();

        await TestAdapter.CreateFileAndWriteTextAsync(path);

        // Act.
        var response = await FileManager.GetAsync(new GetFileRequest { FilePath = path });

        // Assert.
        response.File.Path.Should().Be(path);
    }

    [Theory]
    [InlineData(Adapter.S3)]
    public async Task It_Retrieves_A_File_Under_A_Root_Path(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        var path = TestUtilities.RandomFilePathRepresentation();

        await TestAdapter.CreateFileAndWriteTextAsync(path);

        // Act.
        var response = await FileManager.GetAsync(new GetFileRequest { FilePath = path });

        // Assert.
        response.File.Path.Should().BeEquivalentTo(path, x => x.Excluding(y => y.GetPathTree));
    }
}
