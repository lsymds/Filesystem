using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.Files;

public class TouchFileTests : BaseIntegrationTest
{
    [Theory]
    [InlineData(Adapter.S3)]
    public async Task It_Successfully_Touches_A_File_In_S3(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var path = TestUtilities.RandomFilePathRepresentation();

        // Act.
        await FileManager.TouchAsync(new TouchFileRequest { FilePath = path });

        // Assert.
        await ExpectFileToExistAsync(path);
        (await TestAdapter.ReadFileAsStringAsync(path)).Should().BeEmpty();
    }

    [Theory]
    [InlineData(Adapter.S3)]
    public async Task It_Successfully_Touches_A_File_In_S3_Under_A_Root_Path(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        var path = TestUtilities.RandomFilePathRepresentation();

        // Act.
        await FileManager.TouchAsync(new TouchFileRequest { FilePath = path });

        // Assert.
        await ExpectFileToExistAsync(path);
        (await TestAdapter.ReadFileAsStringAsync(path)).Should().BeEmpty();
    }
}
