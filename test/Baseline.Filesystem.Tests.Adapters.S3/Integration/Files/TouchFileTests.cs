using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.Adapters.S3.Integration.Files
{
    public class TouchFileTests : BaseS3AdapterIntegrationTest
    {
        [Fact]
        public async Task It_Successfully_Touches_A_File_In_S3()
        {
            var path = RandomFilePathRepresentation();

            await CreateFileAndWriteTextAsync(path);

            await ExpectFileToExistAsync(path);
            (await ReadFileAsStringAsync(path)).Should().BeEmpty();
        }

        [Fact]
        public async Task It_Successfully_Touches_A_File_In_S3_Under_A_Root_Path()
        {
            ReconfigureManagerInstances(true);
            
            var path = RandomFilePathRepresentation();

            await CreateFileAndWriteTextAsync(path);

            await ExpectFileToExistAsync(path);
            (await ReadFileAsStringAsync(path)).Should().BeEmpty();
        }
    }
}
