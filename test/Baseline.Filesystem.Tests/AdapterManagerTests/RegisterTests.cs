using System;
using FluentAssertions;
using Baseline.Filesystem.Tests.Fixtures;
using Xunit;

namespace Baseline.Filesystem.Tests.AdapterManagerTests
{
    public class RegisterTests : BaseAdapterManagerTest
    {
        [Fact]
        public void It_Registers_An_Adapter_With_A_Normalised_Name()
        {
            // Arrange.
            AdapterManager.Register(new AdapterRegistration
            {
                Adapter = new SuccessfulOutcomeAdapter(),
                Name = "my-NAMIng-ConVentION"
            });
            
            // Act.
            var result = AdapterManager.Get("my-naming-convention");
            
            // Assert.
            result.Should().NotBeNull();
        }

        [Fact]
        public void It_Throws_An_Exception_When_An_Adapter_Is_Already_Registered_With_That_Name()
        {
            // Arrange.
            AdapterManager.Register(new AdapterRegistration
            {
                Adapter = new SuccessfulOutcomeAdapter(),
                Name = "my-NAMIng-ConVentION"
            });
            
            // Act.
            Action func = () => AdapterManager.Register(new AdapterRegistration
            {
                Adapter = new SuccessfulOutcomeAdapter(),
                Name = "my-naming-convention"
            });
            
            // Assert.
            func.Should().ThrowExactly<AdapterAlreadyRegisteredException>();
        }

        [Fact]
        public void It_Registers_With_A_Default_Adapter_Name_If_One_Is_Not_Specified()
        {
            // Arrange.
            AdapterManager.Register(new AdapterRegistration { Adapter = new SuccessfulOutcomeAdapter() });
            
            // Act.
            var result = AdapterManager.Get("default");
            
            // Assert.
            result.Should().NotBeNull();
        }

        [Fact]
        public void It_Throws_An_Exception_When_The_Root_Path_For_An_Adapter_Is_Not_Obviously_A_Directory()
        {
            // Act.
            Action sut = () => AdapterManager.Register(new AdapterRegistration
            {
                Adapter = new SuccessfulOutcomeAdapter(),
                RootPath = "not/an/obvious/directory".AsBaselineFilesystemPath()
            });
            
            // Assert.
            sut.Should().ThrowExactly<PathIsNotObviouslyADirectoryException>();
        }
    }
}
