﻿using System;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace LSymds.Filesystem.Tests.Adapters.Files;

public class DeleteFileTests : BaseIntegrationTest
{
    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Throws_An_Exception_If_File_Does_Not_Exist(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        // Act.
        Func<Task> func = async () =>
            await FileManager.DeleteAsync(
                new DeleteFileRequest { FilePath = TestUtilities.RandomFilePathRepresentation() }
            );

        // Assert.
        await func.Should().ThrowExactlyAsync<FileNotFoundException>();
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Deletes_A_File(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter);

        var filePath = TestUtilities.RandomFilePathRepresentation();

        await TestAdapter.CreateFileAndWriteTextAsync(filePath);

        await ExpectFileToExistAsync(filePath);

        // Act.
        await FileManager.DeleteAsync(new DeleteFileRequest { FilePath = filePath });

        // Assert.
        await ExpectFileNotToExistAsync(filePath);
    }

    [Theory]
    [ClassData(typeof(RunOnAllProvidersConfiguration))]
    public async Task It_Deletes_A_File_With_A_Root_Path(Adapter adapter)
    {
        // Arrange.
        await ConfigureTestAsync(adapter, true);

        var filePath = TestUtilities.RandomFilePathRepresentation();

        await TestAdapter.CreateFileAndWriteTextAsync(filePath);

        await ExpectFileToExistAsync(filePath);

        // Act.
        await FileManager.DeleteAsync(new DeleteFileRequest { FilePath = filePath });

        // Assert.
        await ExpectFileNotToExistAsync(filePath);
    }
}
