using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LSymds.Filesystem.Tests.Adapters.Files;

public class WriteStreamToFileTests : BaseIntegrationTest
{
    private readonly PathRepresentation _filePath = TestUtilities.RandomFilePathRepresentation();
    private readonly Stream _stream = new MemoryStream(
        Encoding.UTF8.GetBytes("hello stream my old friend")
    );

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Successfully_Writes_A_Stream_To_A_File(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        await ExpectFileNotToExistAsync(_filePath);

        // Act.
        await FileManager.WriteStreamAsync(
            new WriteStreamToFileRequest
            {
                FilePath = _filePath,
                ContentType = "application/json",
                Stream = _stream
            }
        );

        // Assert.
        var fileContents = await TestAdapter.ReadFileAsStringAsync(_filePath);
        fileContents.Should().Be("hello stream my old friend");
        _stream.CanRead.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Successfully_Restarts_The_Stream_If_It_Has_Already_Been_Read(
        Adapter adapter
    )
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        _stream.Seek(5, SeekOrigin.Begin);

        await ExpectFileNotToExistAsync(_filePath);

        // Act.
        await FileManager.WriteStreamAsync(
            new WriteStreamToFileRequest
            {
                FilePath = _filePath,
                ContentType = "application/json",
                Stream = _stream
            }
        );

        // Assert.
        var fileContents = await TestAdapter.ReadFileAsStringAsync(_filePath);
        fileContents.Should().Be("hello stream my old friend");
        _stream.CanRead.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Successfully_Writes_A_Stream_Under_A_Root_Path(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        await ExpectFileNotToExistAsync(_filePath);

        // Act.
        await FileManager.WriteStreamAsync(
            new WriteStreamToFileRequest
            {
                FilePath = _filePath,
                ContentType = "application/json",
                Stream = _stream
            }
        );

        // Assert.
        var fileContents = await TestAdapter.ReadFileAsStringAsync(_filePath);
        fileContents.Should().Be("hello stream my old friend");
        _stream.CanRead.Should().BeTrue();
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Can_Write_And_Then_Read_A_Non_Text_Based_File(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        await using var fileStream = File.OpenRead("TestFiles/Pug.jpg");
        
        // Act.
        await FileManager.WriteStreamAsync(new WriteStreamToFileRequest
        {
            FilePath = _filePath,
            ContentType = "image/jpeg",
            Stream = fileStream
        });
        
        // Assert.
        using var md5 = MD5.Create();
        var file = await FileManager.ReadAsStreamAsync(new ReadFileAsStreamRequest { FilePath = _filePath });
        var hash = BitConverter.ToString(await md5.ComputeHashAsync(file.FileContents)).Replace("-", "").ToLower();
        hash.Should().Be("24b6db48289db83887a01c86caa4fd04");
    }
}
