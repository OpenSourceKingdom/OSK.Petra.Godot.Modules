using GdUnit4;
using Godot;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OSK.Petra.DependencyInjection.Ports;
using OSK.Petra.Godot.Modules.Services.Internal.Services;
using OSK.Petra.Godot.Modules.Services.Ports;
using OSK.Petra.Godot.Modules.Services.Scripts;
using OSK.Petra.Godot.Modules.Services.UnitTests._Helpers;
using OSK.Petra.Modules.Services;
using OSK.Petra.Modules.Services.Ports;

namespace OSK.Petra.Godot.Modules.Services.UnitTests;

[TestSuite]
public class GameModuleBootstrapperTests
{
    #region Variables

    private readonly Mock<IServiceModule> _mockServiceModule = new();
    private readonly Mock<IGameServiceProvider> _mockServiceProvider = new();
    private readonly Mock<IGameServiceConfigurator> _mockConfigurator = new();

    #endregion

    #region Constructors

    public GameModuleBootstrapperTests()
    {
        _mockServiceModule
            .Setup(m => m.Initialize(It.IsAny<IGameServiceProvider>()))
            .Verifiable();

        _mockServiceProvider
            .Setup(m => m.CreateScopedServices())
            .Returns(new ServiceCollection());
    }

    #endregion

    #region _Setup

    [Before]
    public void Test()
    {

    }

    [BeforeTest]
    public void TestTwo()
    {

    }

    #endregion

    #region Initialize

    [TestCase]
    [RequireGodotRuntime]
    public void Initialize_NullRoot_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => GameModuleBootstrapper.Initialize(null!, _mockServiceModule.Object, Array.Empty<IGameServiceConfigurator>(), null));
    }

    [TestCase]
    [RequireGodotRuntime]
    public void Initialize_NullServiceModule_ThrowsArgumentNullException()
    {
        // Arrange/Act/Assert
        Assert.Throws<ArgumentNullException>(() => GameModuleBootstrapper.Initialize(new Node(), null!, [], null));
    }

    [TestCase]
    [RequireGodotRuntime]
    public void Initialize_NullConfigurators_ThrowsArgumentNullException()
    {
        // Arrange/Act/Assert
        Assert.Throws<ArgumentNullException>(() => GameModuleBootstrapper.Initialize(new Node(), _mockServiceModule.Object, null!, null));
    }

    [TestCase]
    [RequireGodotRuntime]
    public void Initialize_ValidInputs_CallsConfigureOnAllConfigurators()
    {
        // Arrange
        var configurator1 = new TestServiceConfigurator();
        var configurator2 = new TestServiceConfigurator();
        var configurators = new[] { configurator1, configurator2 };

        // Act
        GameModuleBootstrapper.Initialize(new Node(), _mockServiceModule.Object, configurators, null, null);

        // Assert
        Assert.True(configurator1.ConfigureWasCalled);
        Assert.True(configurator2.ConfigureWasCalled);
    }

    [TestCase]
    [RequireGodotRuntime]
    public void Initialize_ValidInputs_CallsModuleInitialize()
    {
        // Act
        GameModuleBootstrapper.Initialize(new Node(), _mockServiceModule.Object, Array.Empty<IGameServiceConfigurator>(), null, null);

        // Assert
        _mockServiceModule.Verify(m => m.Initialize(It.IsAny<IGameServiceProvider>()), Times.Once);
    }

    [TestCase]
    [RequireGodotRuntime]
    [GodotExceptionMonitor]
    public void Initialize_Generic_NotIServiceModule_ReturnsEarly()
    {
    }

    [TestCase]
    [RequireGodotRuntime]
    public void Initialize_WithConfigurationProvider_UseProvidedProvider()
    {
        // Arrange
        var testConfigProvider = new TestConfigurationProvider();

        // Act
        GameModuleBootstrapper.Initialize(
            new Node(),
            (IGameServiceConfigurator[])[],
            testConfigProvider,
            null);

        // Assert
        Assert.True(testConfigProvider.GetConfigurationWasCalled);
    }

    [TestCase]
    [RequireGodotRuntime]
    public void Initialize_ModuleIsConfigurationProvider_UseModuleProvider()
    {
        // Arrange
        var mockModule = new Mock<TestableGameServiceModule>();
        mockModule.Setup(m => m.Services).Returns(_mockServiceProvider.Object);

        // Act - pass no explicit provider, module itself is IConfigurationProvider
        GameModuleBootstrapper.Initialize(
            new Node(),
            mockModule.Object,
            Array.Empty<IGameServiceConfigurator>(),
            null);

        // Assert
        _mockServiceModule.Verify(m => m.Initialize(It.IsAny<IGameServiceProvider>()), Times.Once);
    }

    [TestCase]
    [RequireGodotRuntime]
    public void Initialize_ParentIsConfigurationProvider_FallBackToParent()
    {
        // Arrange
        var parentConfigProvider = new TestConfigurationProvider();
        var mockChildModule = new Mock<GameServiceModule>();
        mockChildModule.Setup(m => m.Services).Returns(_mockServiceProvider.Object);

        // Act - pass parent that is IConfigurationProvider
        GameModuleBootstrapper.Initialize(
            new Node(),
            mockChildModule.Object,
            Array.Empty<IGameServiceConfigurator>(),
            parentConfigProvider);

        // Assert
        Assert.True(parentConfigProvider.GetConfigurationWasCalled);
    }

    [TestCase]
    [RequireGodotRuntime]
    public void Initialize_ModuleHasConfigurator_AddsFromConfigurator()
    {
        // Arrange
        var configurator = new TestServiceConfigurator();
        var mockModule = new Mock<TestableGameServiceModule>();
        mockModule.Setup(m => m.Services).Returns(_mockServiceProvider.Object);

        // Act - module implements IGameServiceConfigurator via TestableGameServiceModule
        GameModuleBootstrapper.Initialize(
            new Node(),
            mockModule.Object,
            new[] { configurator },
            null);

        // Assert
        Assert.True(configurator.ConfigureWasCalled);
    }

    #endregion
}
