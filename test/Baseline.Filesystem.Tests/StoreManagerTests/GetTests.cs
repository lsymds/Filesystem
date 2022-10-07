using System;
using Baseline.Filesystem.Tests.Fixtures;
using FluentAssertions;
using Xunit;

namespace Baseline.Filesystem.Tests.StoreManagerTests
{
    public class GetTests : BaseStoreManagerTest
    {
        [Fact]
        public void It_Throws_An_Exception_When_An_Attempt_Is_Made_To_Get_An_Unregistered_Store()
        {
            // Act.
            Action func = () => StoreManager.Get("i-am-not-registered");

            // Assert.
            func.Should().ThrowExactly<StoreNotFoundException>();
        }

        [Fact]
        public void It_Returns_The_Store_When_A_Store_With_The_Exact_Name_Has_Been_Registered()
        {
            // Arrange.
            StoreManager.Register(
                new StoreRegistration
                {
                    Adapter = new SuccessfulOutcomeAdapter(),
                    Name = "mySpecificNaming-CONVENTION"
                }
            );

            // Act.
            var store = StoreManager.Get("mySpecificNaming-CONVENTION");

            // Assert.
            store.Should().NotBeNull();
        }

        [Fact]
        public void It_Returns_The_Store_When_A_Store_With_A_Normalised_Version_Of_The_Name_Has_Been_Registered()
        {
            // Arrange.
            StoreManager.Register(
                new StoreRegistration
                {
                    Adapter = new SuccessfulOutcomeAdapter(),
                    Name = "my-specific-naming-convention"
                }
            );

            // Act.
            var store = StoreManager.Get("MY-SpeCifIc-NAMING-convention");

            // Assert.
            store.Should().NotBeNull();
        }
    }
}
