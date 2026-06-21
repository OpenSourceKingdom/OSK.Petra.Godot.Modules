using Godot;
using OSK.Petra.DependencyInjection.Ports;
using System;
using OSK.Petra.Godot.Modules.Services.Ports;
using OSK.Petra.Modules.Services;
using OSK.Petra.Modules.Services.Ports;
using OSK.Extensions.Petra.Godot.DependencyInjection;

namespace OSK.Petra.Godot.Modules.Services.Internal.Services;


internal class GameServiceBuilder: ModuleServiceBuilder, IGameModuleServiceBuilder
{
    #region Variables

    private readonly Node _rootNode;

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a <see cref="GameServiceBuilder"/> using the given <see cref="Node"/> as the root for initialization and an <see cref="EmptyConfigurationProvider"/> and an optional <see cref="IGameServiceProvider"/> as an initializer
    /// </summary>
    /// <param name="rootNode">The node treated as the root during scene initialization</param>
    /// <param name="serviceProvider">An initializing service provider</param>
    public GameServiceBuilder(Node rootNode, IGameServiceProvider? serviceProvider = null)
        : base(serviceProvider)
    {
        if (rootNode is null)
        {
            throw new ArgumentNullException(nameof(rootNode));
        }

        _rootNode = rootNode;
    }

    /// <summary>
    /// Creates a <see cref="GameServiceBuilder"/> using the given <see cref="Node"/> as the root for initialization and an <see cref="EmptyConfigurationProvider"/> and an optional <see cref="IGameServiceProvider"/> as an initializer
    /// </summary>
    /// <param name="rootNode">The node treated as the root during scene initialization</param>
    /// <param name="configurationProvider">The configuration provider to use to retrieve the app configuration with the scene initialization</param>
    /// <param name="serviceProvider">An initializing service provider</param>
    public GameServiceBuilder(Node rootNode, IConfigurationProvider configurationProvider, IGameServiceProvider? serviceProvider = null)
        : base(configurationProvider, serviceProvider)
    {
        if (rootNode is null)
        {
            throw new ArgumentNullException(nameof(rootNode));
        }

        _rootNode = rootNode;
    }

    #endregion

    #region IGameServiceBuilder

    /// <inheritdoc/>
    public IGameModuleServiceBuilder AddNode<TNode>()
        where TNode : Node
    {
        Services.AddSingletonNode<TNode>(_rootNode);
        return this;
    }

    /// <inheritdoc/>
    public IGameModuleServiceBuilder AddNode<TInterface, TNode>()
        where TInterface : class
        where TNode : Node, TInterface
    {
        Services.AddSingletonNode<TInterface, TNode>(_rootNode);
        return this;
    }

    #endregion
}