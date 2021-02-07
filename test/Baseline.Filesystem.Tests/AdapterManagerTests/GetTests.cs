using System;
using FluentAssertions;
using Baseline.Filesystem.Tests.Fixtures;
using Xunit;

namespace Baseline.Filesystem.Tests.AdapterManagerTests
{
    public class GetTests : BaseAdapterManagerTest
    {
        [Fact]
        public void It_Throws_An_Exception_When_An_Attempt_Is_Made_To_Get_An_Unregistered_Adapter()
        {
            Action func = () => AdapterManager.Get("i-am-not-registered");
            func.Should().ThrowExactly<AdapterNotFoundException>();
        }

        [Fact]
        public void It_Returns_The_Adapter_When_An_Adapter_With_The_Exact_Name_Has_Been_Registered()
        {
            AdapterManager.Register(new AdapterRegistration 
            {
                    Adapter = new SuccessfulOutcomeAdapter(), 
                    Name = "mySpecificNaming-CONVENTION"
            });
            var adapter = AdapterManager.Get("mySpecificNaming-CONVENTION");
            adapter.Should().NotBeNull();
        }

        [Fact]
        public void It_Returns_The_Adapter_When_An_Adapter_With_A_Normalised_Version_Of_The_Name_Has_Been_Registered()
        {
            AdapterManager.Register(new AdapterRegistration
            {
                Adapter = new SuccessfulOutcomeAdapter(),
                Name = "my-specific-naming-convention"
            });
            var adapter = AdapterManager.Get("MY-SpeCifIc-NAMING-convention");
            adapter.Should().NotBeNull();
        }
    }
}
