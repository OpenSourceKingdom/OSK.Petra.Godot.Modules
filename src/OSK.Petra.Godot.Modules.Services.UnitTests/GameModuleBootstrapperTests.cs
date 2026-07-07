using GdUnit4;
using Godot;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OSK.Petra.DependencyInjection.Ports;
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

    #endregion

    #region Constructors

    public GameModuleBootstrapperTests()
    {
        _mockServiceModule
            .Setup(m => m.Initialize(It.IsAny<IGameServiceProvider>()))
            .Verifiable();

        _mockServiceProvider
            .Setup(m => m.GetServiceDescriptors())
            .Returns([]);
    }

    #endregion

    #region Initialize

    [TestCase]
    [RequireGodotRuntime]
    public void Initialize_NullRoot_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => GameModuleBootstrapper.Initialize(null!, Mock.Of<IServiceModule>(), [], null));
    }

    [TestCase]
    [RequireGodotRuntime]
    public void Initialize_NullServiceModule_ThrowsArgumentNullException()
    {
        // Arrange
        var node = new Node();

        // Act/Assert
        Assert.Throws<ArgumentNullException>(() => GameModuleBootstrapper.Initialize(node, null!, [], null));

        node.Free();
    }

    [TestCase]
    [RequireGodotRuntime]
    public void Initialize_NullConfigurators_ThrowsArgumentNullException()
    {
        // Arrange
        var node = new Node();

        // Act/Assert
        Assert.Throws<ArgumentNullException>(() => GameModuleBootstrapper.Initialize(node, Mock.Of<IServiceModule>(), null!, null));

        node.Free();
    }

    [TestCase]
    [RequireGodotRuntime]
    public void Initialize_ValidInputs_CallsConfigureOnAllConfigurators()
    {
        // Arrange
        var node = new Node();
        var configurator1 = new TestServiceConfigurator();
        var configurator2 = new TestServiceConfigurator();
        var configurators = new[] { configurator1, configurator2 };

        // Act
        GameModuleBootstrapper.Initialize(node, _mockServiceModule.Object, configurators, null, null);

        // Assert
        Assert.True(configurator1.ConfigureWasCalled);
        Assert.True(configurator2.ConfigureWasCalled);

        _mockServiceModule.Verify(m => m.Initialize(It.IsAny<IGameServiceProvider>()), Times.Once);

        node.Free();
    }

    [TestCase]
    [RequireGodotRuntime]
    [GodotExceptionMonitor]
    public void Initialize_Generic_NotIServiceModule_ReturnsEarly()
    {
        // Arrange
        var nonServiceModule = new NonServiceModuleNode();

        var mockParentModule = new Mock<IServiceModule>();
        var mockConfigurationProvider = new Mock<IModuleConfigurationProvider>();
        var mockConfigurator = new Mock<IGameServiceConfigurator>();

        // Act
        GameModuleBootstrapper.Initialize(nonServiceModule, mockParentModule.Object);
        GameModuleBootstrapper.Initialize(nonServiceModule, mockConfigurationProvider.Object);
        GameModuleBootstrapper.Initialize(nonServiceModule, [mockConfigurator.Object], null);

        // Assert

        // nothing should have triggered since it bailed early
        mockParentModule.VerifyGet(m => m.Services, Times.Never);
        mockConfigurationProvider.Verify(m => m.GetConfiguration(), Times.Never);
        mockConfigurator.Verify(m => m.Configure(It.IsAny<IGameModuleServiceBuilder>()), Times.Never);

        nonServiceModule.Free();
    }

    [TestCase]
    [RequireGodotRuntime]
    public void Initialize_WithConfigurationProvider_UseProvidedProvider()
    {
        // Arrange
        var node = new TestableGameServiceModule();
        var testConfigProvider = new TestConfigurationProvider();

        // Act
        GameModuleBootstrapper.Initialize(node, [], testConfigProvider, null);

        // Assert
        Assert.True(testConfigProvider.GetConfigurationWasCalled);

        _mockServiceModule.Verify(m => m.Initialize(It.IsAny<IGameServiceProvider>()), Times.Once);
        
        node.Free();
    }

    [TestCase]
    [RequireGodotRuntime]
    public void Initialize_ModuleIsConfigurationProvider_UseModuleProvider()
    {
        // Arrange
        var node = new Node();
        var mockModule = new Mock<IServiceModule>();
        mockModule.SetupGet(m => m.Services)
            .Returns(_mockServiceProvider.Object);

        // Act
        GameModuleBootstrapper.Initialize(node, mockModule.Object, [], null);

        // Assert
        node.Free();
    }

    [TestCase]
    [RequireGodotRuntime]
    public void Initialize_ParentIsConfigurationProvider_FallBackToParent()
    {
        // Arrange
        var node = new Node();
        var parentConfigProvider = new TestConfigurationProvider();
        var mockChildModule = new Mock<IServiceModule>();
        mockChildModule.Setup(m => m.Services)
            .Returns(_mockServiceProvider.Object);

        // Act
        GameModuleBootstrapper.Initialize(node, mockChildModule.Object, [], parentConfigProvider);

        // Assert
        Assert.True(parentConfigProvider.GetConfigurationWasCalled);
        node.Free();
    }

    [TestCase]
    [RequireGodotRuntime]
    public void Initialize_ModuleHasConfigurator_AddsFromConfigurator()
    {
        // Arrange
        var node = new Node();
        var configurator = new TestServiceConfigurator();
        var mockModule = new Mock<IServiceModule>();
        mockModule.SetupGet(m => m.Services)
            .Returns(_mockServiceProvider.Object);

        // Act
        GameModuleBootstrapper.Initialize(node, mockModule.Object, [ configurator ], null);

        // Assert
        Assert.True(configurator.ConfigureWasCalled);
        node.Free();
    }

    #endregion
}
