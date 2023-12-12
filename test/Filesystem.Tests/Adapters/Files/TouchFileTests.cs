using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LSymds.Filesystem.Tests.Adapters.Files;

public class TouchFileTests : BaseIntegrationTest
{
    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Successfully_Touches_A_File(Adapter adapter)
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
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Successfully_Touches_A_File_Under_A_Root_Path(Adapter adapter)
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
