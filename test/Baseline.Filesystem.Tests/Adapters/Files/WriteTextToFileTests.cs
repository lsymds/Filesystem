using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.Files;

public class WriteTextToFileTests : BaseIntegrationTest
{
    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Successfully_Writes_A_Simple_File_To_S3(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var path = TestUtilities.RandomFilePathRepresentation();

        // Act.
        await FileManager.WriteTextAsync(
            new WriteTextToFileRequest
            {
                ContentType = "text/plain",
                FilePath = path,
                TextToWrite = "it-successfully-writes-simple-file-to-s3"
            }
        );

        // Assert.
        await ExpectFileToExistAsync(path);
        (await TestAdapter.ReadFileAsStringAsync(path))
            .Should()
            .Be("it-successfully-writes-simple-file-to-s3");
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Successfully_Writes_A_Simple_File_Under_A_Root_Path_To_S3(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        var path = TestUtilities.RandomFilePathRepresentation();

        // Act.
        await FileManager.WriteTextAsync(
            new WriteTextToFileRequest
            {
                ContentType = "text/plain",
                FilePath = path,
                TextToWrite = "it-successfully-writes-simple-file-to-s3"
            }
        );

        // Assert.
        await ExpectFileToExistAsync(path);
        (await TestAdapter.ReadFileAsStringAsync(path))
            .Should()
            .Be("it-successfully-writes-simple-file-to-s3");
    }
}
