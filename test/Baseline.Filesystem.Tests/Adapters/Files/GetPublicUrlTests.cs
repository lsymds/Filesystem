using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.Files;

public class GetPublicUrlTests : BaseIntegrationTest
{
    [Theory]
    [InlineData(Adapter.S3)]
    public async Task It_Throws_An_Exception_If_The_File_Does_Not_Exist(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        // Act.
        Func<Task> act = async () =>
            await FileManager.GetPublicUrlAsync(
                new GetFilePublicUrlRequest
                {
                    FilePath = TestUtilities.RandomFilePathRepresentation()
                }
            );

        // Assert.
        await act.Should().ThrowExactlyAsync<FileNotFoundException>();
    }

    [Theory]
    [InlineData(Adapter.S3)]
    public async Task It_Returns_The_Public_Url_If_The_File_Exists(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var path = TestUtilities.RandomFilePathRepresentationWithPrefix("abc");

        await TestAdapter.CreateFileAndWriteTextAsync(path);

        // Act.
        var response = await FileManager.GetPublicUrlAsync(
            new GetFilePublicUrlRequest { FilePath = path, Expiry = DateTime.Today.AddDays(1) }
        );

        // Assert.
        response.Expiry.Should().Be(DateTime.Today.AddDays(1));
        await ExpectPublicUrlContainsTextFromAdapter(response.Url, path);
    }

    [Theory]
    [InlineData(Adapter.S3)]
    public async Task It_Defaults_The_Expiry_Date(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var path = TestUtilities.RandomFilePathRepresentationWithPrefix("abc");

        await TestAdapter.CreateFileAndWriteTextAsync(path);

        // Act.
        var response = await FileManager.GetPublicUrlAsync(
            new GetFilePublicUrlRequest { FilePath = path }
        );

        // Assert.
        response.Expiry.Should().Be(DateTime.Today.AddDays(1));
    }

    [Theory]
    [InlineData(Adapter.S3)]
    public async Task It_Retrieves_A_Public_Url_For_A_File_Under_A_Root_Path(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        var path = TestUtilities.RandomFilePathRepresentationWithPrefix("abc");

        await TestAdapter.CreateFileAndWriteTextAsync(path);

        // Act.
        var response = await FileManager.GetPublicUrlAsync(
            new GetFilePublicUrlRequest { FilePath = path, Expiry = DateTime.Today.AddDays(1) }
        );

        // Assert.
        response.Expiry.Should().Be(DateTime.Today.AddDays(1));
        await ExpectPublicUrlContainsTextFromAdapter(response.Url, path);
    }
}
