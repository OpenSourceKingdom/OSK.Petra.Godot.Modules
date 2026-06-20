using Godot;
using OSK.Extensions.Petra.Godot.DependencyInjection;
using OSK.Petra.DependencyInjection.Ports;
using OSK.Petra.Modules.Bootstrapper;
using OSK.Petra.Modules.Bootstrapper.Ports;
using System;
using System.Collections.Generic;
using System.Text;
using OSK.Petra.Godot.Modules.Services.Ports;

namespace OSK.Petra.Godot.Modules.Services.Internal.Services;


public partial class GodotServiceConfigurator: ModuleServiceBuilder, IGodotServiceConfigurator
{
    #region Variables

    private readonly Node _rootNode;

    #endregion

    #region Constructors

    /// <summary>
    /// Creates a <see cref="GodotServiceConfigurator"/> using the given <see cref="Node"/> as the root for initialization and an <see cref="EmptyConfigurationProvider"/> and an optional <see cref="IGameServiceProvider"/> as an initializer
    /// </summary>
    /// <param name="rootNode">The node treated as the root during scene initialization</param>
    /// <param name="serviceProvider">An initializing service provider</param>
    public GodotServiceConfigurator(Node rootNode, IGameServiceProvider? serviceProvider = null)
        : base(serviceProvider)
    {
        if (rootNode is null)
        {
            throw new ArgumentNullException(nameof(rootNode));
        }

        _rootNode = rootNode;
    }

    /// <summary>
    /// Creates a <see cref="GodotServiceConfigurator"/> using the given <see cref="Node"/> as the root for initialization and an <see cref="EmptyConfigurationProvider"/> and an optional <see cref="IGameServiceProvider"/> as an initializer
    /// </summary>
    /// <param name="rootNode">The node treated as the root during scene initialization</param>
    /// <param name="configurationProvider">The configuration provider to use to retrieve the app configuration with the scene initialization</param>
    /// <param name="serviceProvider">An initializing service provider</param>
    public GodotServiceConfigurator(Node rootNode, IConfigurationProvider configurationProvider, IGameServiceProvider? serviceProvider = null)
        : base(configurationProvider, serviceProvider)
    {
        if (rootNode is null)
        {
            throw new ArgumentNullException(nameof(rootNode));
        }

        _rootNode = rootNode;
    }

    #endregion

    #region IGodotServiceConfigurator

    /// <inheritdoc/>
    public IGodotServiceConfigurator AddNode<TNode>()
        where TNode : Node
    {
        Services.AddSingletonNode<TNode>(_rootNode);
        return this;
    }

    /// <inheritdoc/>
    public IGodotServiceConfigurator AddNode<TInterface, TNode>()
        where TInterface : class
        where TNode : Node, TInterface
    {
        Services.AddSingletonNode<TInterface, TNode>(_rootNode);
        return this;
    }

    #endregion
}