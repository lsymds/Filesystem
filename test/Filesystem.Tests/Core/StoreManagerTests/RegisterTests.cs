using System;
using FluentAssertions;
using LSymds.Filesystem.Tests.Core.Fixtures;
using Xunit;

namespace LSymds.Filesystem.Tests.Core.StoreManagerTests;

public class RegisterTests : BaseStoreManagerTest
{
    [Fact]
    public void It_Registers_A_Store_With_A_Normalised_Name()
    {
        // Arrange.
        StoreManager.Register(
            new StoreRegistration
            {
                Adapter = new SuccessfulOutcomeAdapter(),
                Name = "my-NAMIng-ConVentION"
            }
        );

        // Act.
        var result = StoreManager.Get("my-naming-convention");

        // Assert.
        result.Should().NotBeNull();
    }

    [Fact]
    public void It_Throws_An_Exception_When_A_Store_Is_Already_Registered_With_That_Name()
    {
        // Arrange.
        StoreManager.Register(
            new StoreRegistration
            {
                Adapter = new SuccessfulOutcomeAdapter(),
                Name = "my-NAMIng-ConVentION"
            }
        );

        // Act.
        Action func = () =>
            StoreManager.Register(
                new StoreRegistration
                {
                    Adapter = new SuccessfulOutcomeAdapter(),
                    Name = "my-naming-convention"
                }
            );

        // Assert.
        func.Should().ThrowExactly<StoreAlreadyRegisteredException>();
    }

    [Fact]
    public void It_Registers_With_A_Default_Store_Name_If_One_Is_Not_Specified()
    {
        // Arrange.
        StoreManager.Register(
            new StoreRegistration { Adapter = new SuccessfulOutcomeAdapter() }
        );

        // Act.
        var result = StoreManager.Get("default");

        // Assert.
        result.Should().NotBeNull();
    }

    [Fact]
    public void It_Throws_An_Exception_When_The_Root_Path_For_A_Store_Is_Not_Obviously_A_Directory()
    {
        // Act.
        Action sut = () =>
            StoreManager.Register(
                new StoreRegistration
                {
                    Adapter = new SuccessfulOutcomeAdapter(),
                    RootPath = "not/an/obvious/directory".AsFilesystemPath()
                }
            );

        // Assert.
        sut.Should().ThrowExactly<PathIsNotObviouslyADirectoryException>();
    }
}
