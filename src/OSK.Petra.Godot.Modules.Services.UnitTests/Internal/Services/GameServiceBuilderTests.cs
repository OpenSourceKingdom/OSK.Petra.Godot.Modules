using Godot;
using GdUnit4.Api;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using OSK.Petra.DependencyInjection.Ports;
using OSK.Petra.Godot.Modules.Services.Internal.Services;
using OSK.Petra.Godot.Modules.Services.Ports;
using OSK.Petra.Godot.Modules.Services.UnitTests._Helpers;
using OSK.Petra.Modules.Services;
using GdUnit4;
using TestNode = OSK.Petra.Godot.Modules.Services.UnitTests._Helpers.TestNode;

namespace OSK.Petra.Godot.Modules.Services.UnitTests.Internal.Services;

[TestSuite]
public class GameServiceBuilderTests
{
    #region Constructors

    [TestCase]
    public void Constructor_NullRootNode_ThrowsArgumentNullException()
    {
        // Arrange/Act/Assert
        Assert.Throws<ArgumentNullException>(() => new GameServiceBuilder(null!));
    }

    [TestCase]
    [RequireGodotRuntime]
    public void Constructor_ValidInputs_SetsInternalState()
    {
        // Arrange
        var node = new Node();

        // Act
        var builder = new GameServiceBuilder(node);

        // Assert
        Assert.NotNull(builder.Services);

        node.Free();
    }

    #endregion

    #region AddNode

    [TestCase]
    [RequireGodotRuntime]
    public void AddNode_TNode_RegistersSingletonNode()
    {
        // Arrange
        var node = new Node();

        var mockServiceProvider = new Mock<IGameServiceProvider>();
        mockServiceProvider.Setup(s => s.CreateScopedServices())
            .Returns(new ServiceCollection());
        var builder = new GameServiceBuilder(node, mockServiceProvider.Object);

        var serviceCollection = new ServiceCollection();
        mockServiceProvider.Setup(s => s.CreateScopedServices())
            .Returns(serviceCollection);

        // Act
        builder.AddNode<Node>();

        // Assert


        node.Free();
    }

    [TestCase]
    [RequireGodotRuntime]
    public void AddNode_TInterface_TNode_RegistersSingletonNodeWithInterface()
    {
        // Arrange
        var mockServiceProvider = new Mock<IGameServiceProvider>();
        mockServiceProvider.Setup(s => s.CreateScopedServices())
            .Returns(new ServiceCollection());
        var builder = new GameServiceBuilder(new Node(), mockServiceProvider.Object);

        // Act
        builder.AddNode<ITestInterface, TestNode>();

        // Assert
    }

    [TestCase]
    [RequireGodotRuntime]
    public void AddNode_ReturnsSameBuilderForChaining()
    {
        // Arrange
        var mockServiceProvider = new Mock<IGameServiceProvider>();
        mockServiceProvider.Setup(s => s.CreateScopedServices())
            .Returns(new ServiceCollection());
        var builder = new GameServiceBuilder(new Node(), mockServiceProvider.Object);

        // Act
        var result = builder.AddNode<TestNode>();

        // Assert
        Assert.Same(builder, result);
    }

    [TestCase]
    [RequireGodotRuntime]
    public void AddNode_TInterface_TNode_ReturnsSameBuilderForChaining()
    {
        // Arrange
        var mockServiceProvider = new Mock<IGameServiceProvider>();
        mockServiceProvider.Setup(s => s.CreateScopedServices())
            .Returns(new ServiceCollection());
        var builder = new GameServiceBuilder(new Node(), mockServiceProvider.Object);

        // Act
        var result = builder.AddNode<ITestInterface, TestNode>();

        // Assert
        Assert.Same(builder, result);
    }

    #endregion
}

public interface ITestInterface
{
}
