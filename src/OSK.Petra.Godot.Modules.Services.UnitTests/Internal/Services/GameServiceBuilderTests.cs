using Godot;
using Moq;
using OSK.Petra.DependencyInjection.Ports;
using OSK.Petra.Godot.Modules.Services.Internal.Services;
using GdUnit4;
using TestNode = OSK.Petra.Godot.Modules.Services.UnitTests._Helpers.TestNode;
using OSK.Petra.Godot.Modules.Services.UnitTests._Helpers;

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
        var rootNode = new Node();
        var childNode = new TestNode();

        rootNode.AddChild(childNode);

        var mockServiceProvider = new Mock<IGameServiceProvider>();
        mockServiceProvider.Setup(s => s.GetServiceDescriptors())
            .Returns([]);
        var builder = new GameServiceBuilder(rootNode, mockServiceProvider.Object);

        // Act
        builder.AddNode<TestNode>();

        // Assert
        Assert.Single(builder.Services);

        rootNode.Free();
    }

    [TestCase]
    [RequireGodotRuntime]
    public void AddNode_TInterface_TNode_RegistersSingletonNodeWithInterface()
    {
        // Arrange
        var rootNode = new Node();
        var childNode = new TestNode();

        rootNode.AddChild(childNode);

        var mockServiceProvider = new Mock<IGameServiceProvider>();
        mockServiceProvider.Setup(s => s.GetServiceDescriptors())
            .Returns([]);
        var builder = new GameServiceBuilder(rootNode, mockServiceProvider.Object);

        // Act
        builder.AddNode<ITestInterface, TestNode>();

        // Assert
        Assert.Single(builder.Services);

        rootNode.Free();
    }

    [TestCase]
    [RequireGodotRuntime]
    public void AddNode_ReturnsSameBuilderForChaining()
    {
        // Arrange
        var rootNode = new Node();
        var childNode = new TestNode();

        rootNode.AddChild(childNode);

        var mockServiceProvider = new Mock<IGameServiceProvider>();
        mockServiceProvider.Setup(s => s.GetServiceDescriptors())
            .Returns([]);
        var builder = new GameServiceBuilder(rootNode, mockServiceProvider.Object);

        // Act
        var result = builder.AddNode<TestNode>();

        // Assert
        Assert.Same(builder, result);
        rootNode.Free();
    }

    [TestCase]
    [RequireGodotRuntime]
    public void AddNode_TInterface_TNode_ReturnsSameBuilderForChaining()
    {
        // Arrange
        var rootNode = new Node();
        var childNode = new TestNode();

        rootNode.AddChild(childNode);

        var mockServiceProvider = new Mock<IGameServiceProvider>();
        mockServiceProvider.Setup(s => s.GetServiceDescriptors())
            .Returns([]);
        var builder = new GameServiceBuilder(rootNode, mockServiceProvider.Object);

        // Act
        var result = builder.AddNode<ITestInterface, TestNode>();

        // Assert
        Assert.Same(builder, result);
        rootNode.Free();
    }

    #endregion
}
