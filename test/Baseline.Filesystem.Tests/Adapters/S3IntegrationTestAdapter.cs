using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;

namespace Baseline.Filesystem.Tests.Adapters;

public class S3IntegrationTestAdapter : BaseIntegrationTestAdapter, IIntegrationTestAdapter
{
    private readonly string _rootPath;
    private readonly string _generatedBucketName = Guid.NewGuid().ToString();
    private readonly IAmazonS3 _s3Client;

    public S3IntegrationTestAdapter(string rootPath)
    {
        _rootPath = rootPath;
        _s3Client = new AmazonS3Client(
            new BasicAWSCredentials("abc", "def"),
            new AmazonS3Config
            {
                AuthenticationRegion = "eu-west-1",
                ServiceURL = "http://localhost:4566",
                ForcePathStyle = true,
            }
        );
    }

    public async ValueTask DisposeAsync()
    {
        await _s3Client.DeleteBucketAsync(_generatedBucketName);
    }

    public async ValueTask<IAdapter> BootstrapAsync()
    {
        await _s3Client.PutBucketAsync(
            new PutBucketRequest
            {
                BucketRegion = S3Region.EUWest1,
                BucketName = _generatedBucketName
            }
        );

        return new S3Adapter(
            new S3AdapterConfiguration { BucketName = _generatedBucketName, S3Client = _s3Client }
        );
    }

    /// <inheritdoc />
    public async ValueTask CreateFileAndWriteTextAsync(
        PathRepresentation path,
        string contents = ""
    )
    {
        if (await FileExistsAsync(path))
        {
            throw new Exception("File already exists!");
        }

        await _s3Client.PutObjectAsync(
            new PutObjectRequest
            {
                BucketName = _generatedBucketName,
                Key = CombinePathWithRootPath(path),
                ContentBody = contents
            }
        );
    }

    /// <inheritdoc />
    public async ValueTask<bool> HasFilesOrDirectoriesUnderPathAsync(PathRepresentation path)
    {
        var objects = await _s3Client.ListObjectsAsync(
            new ListObjectsRequest { BucketName = _generatedBucketName, Prefix = "prefixed/" }
        );

        return objects.S3Objects.Any();
    }

    /// <inheritdoc />
    public async ValueTask<string> ReadFileAsStringAsync(PathRepresentation path)
    {
        var file = await _s3Client.GetObjectAsync(
            new GetObjectRequest
            {
                BucketName = _generatedBucketName,
                Key = CombinePathWithRootPath(path)
            }
        );

        return await new StreamReader(file.ResponseStream).ReadToEndAsync();
    }

    /// <inheritdoc />
    public ValueTask<IReadOnlyCollection<string>> TextThatShouldBeInPublicUrlForPathAsync(
        PathRepresentation path
    )
    {
        return ValueTask.FromResult(
            new List<string>
            {
                $"https://localhost:4566/{_generatedBucketName}/{CombinePathWithRootPath(path)}",
                "X-Amz-Expires",
                "X-Amz-Algorithm",
                "X-Amz-SignedHeaders"
            } as IReadOnlyCollection<string>
        );
    }

    /// <inheritdoc />
    public async ValueTask<bool> FileExistsAsync(PathRepresentation path)
    {
        try
        {
            var file = await _s3Client.GetObjectAsync(
                new GetObjectRequest
                {
                    BucketName = _generatedBucketName,
                    Key = CombinePathWithRootPath(path)
                }
            );
            return file.HttpStatusCode == HttpStatusCode.OK;
        }
        catch (AmazonS3Exception e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }

            throw;
        }
    }

    /// <inheritdoc />
    public async ValueTask<bool> DirectoryExistsAsync(PathRepresentation path)
    {
        var objects = await _s3Client.ListObjectsAsync(
            new ListObjectsRequest
            {
                BucketName = _generatedBucketName,
                Prefix = CombinePathWithRootPath(path)
            }
        );
        return objects != null && objects.S3Objects.Any();
    }

    private string CombinePathWithRootPath(PathRepresentation path)
    {
        return $"{(_rootPath ?? string.Empty)}{path.NormalisedPath}";
    }
}
