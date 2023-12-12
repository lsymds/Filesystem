using System;
using System.Linq;

namespace LSymds.Filesystem.Tests;

public static class TestUtilities
{
    private static readonly Random Random = new();

    public static string RandomDirectoryPath(bool includeBlank = false)
    {
        var directories = new[]
        {
            includeBlank ? "" : RandomString(),
            $"{RandomString(12)}/{RandomString(4)}",
            $"{RandomString(4)}/{RandomString(6)}/{RandomString(3)}/{RandomString()}",
            $"{RandomString(6)}/{RandomString(3)}",
            RandomString(12),
            RandomString(18)
        };
        var randomDirectory = directories[Random.Next(directories.Length)];

        return string.IsNullOrWhiteSpace(randomDirectory) ? randomDirectory : $"{randomDirectory}/";
    }

    public static PathRepresentation RandomDirectoryPathRepresentation()
    {
        return RandomDirectoryPath().AsFilesystemPath();
    }

    public static PathRepresentation RandomFilePathRepresentation()
    {
        var extensions = new[] { "txt", "jpg", "pdf", ".config.json" };
        var fileNames = new[]
        {
            $".{RandomString()}",
            $".{RandomString()}.config",
            $"{RandomString()}.{extensions[Random.Next(extensions.Length)]}"
        };

        return $"{RandomDirectoryPath(true)}/{fileNames[Random.Next(fileNames.Length)]}".AsFilesystemPath();
    }

    public static PathRepresentation RandomFilePathRepresentationWithPrefix(string prefix)
    {
        return $"{prefix}/{RandomFilePathRepresentation().OriginalPath}".AsFilesystemPath();
    }

    public static string RandomString(int length = 8)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        return new string(
            Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray()
        );
    }
}
