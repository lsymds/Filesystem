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
            AdapterManager.Register(new SuccessfulOutcomeAdapter(), "my-NAMIng-ConVentION");
            var result = AdapterManager.Get("my-naming-convention");
            result.Should().NotBeNull();
        }

        [Fact]
        public void It_Throws_An_Exception_When_An_Adapter_Is_Already_Registered_With_That_Name()
        {
            AdapterManager.Register(new SuccessfulOutcomeAdapter(), "my-NAMING-convENTION");
            Action func = () => AdapterManager.Register(new SuccessfulOutcomeAdapter(), "my-naming-convention");
            func.Should().ThrowExactly<AdapterAlreadyRegisteredException>();
        }

        [Fact]
        public void It_Registers_With_A_Default_Adapter_Name_If_One_Is_Not_Specified()
        {
            AdapterManager.Register(new SuccessfulOutcomeAdapter());
            var result = AdapterManager.Get("default");
            result.Should().NotBeNull();
        }
    }
}
