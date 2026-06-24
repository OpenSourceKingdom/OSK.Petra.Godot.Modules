using Godot;
using OSK.Petra.Godot.Modules.Services.Internal.Services;
using OSK.Petra.Godot.Modules.Services.Ports;
using OSK.Petra.Godot.Modules.Services.Scripts;
using OSK.Petra.Modules.Services;
using OSK.Petra.Modules.Services.Ports;
using System;
using System.Collections.Generic;

namespace OSK.Petra.Godot.Modules.Services;

/// <summary>
/// A bootstrapper that configures a <see cref="GameServiceModule"/> and its services for running
/// </summary>
public static class GameModuleBootstrapper
{
    #region Static

    /// <summary>
    /// Initializes the provided <see cref="Node"/> module.
    /// </summary>
    /// <typeparam name="TNode">The type of node being initialized</typeparam>
    /// <param name="module">The module to initialize</param>
    /// <param name="parentModule">The parent module, if one exists</param>
    /// <remarks>
    /// 💡Notes:
    /// <list type="bullet">
    /// <item>If the provided node is not of type <see cref="IServiceModule"/>, then this method will do nothing</item>
    /// <item>The method will attempt to determine if the provided module is an <see cref="IGameServiceConfigurator"/> and use it if possible.</item>
    /// <item>The method will attempt to determine if the provided module is an <see cref="IModuleConfigurationProvider"/> and use it if possible, falling back to the parent if the module is not and it can be used.</item>
    /// </list>
    /// </remarks>
    public static void Initialize<TNode>(TNode module, IServiceModule? parentModule = null)
        where TNode : Node
    {
        if (module is not IServiceModule serviceModule)
        {
            return;
        }

        IGameServiceConfigurator[] configurators = module is IGameServiceConfigurator configurator
            ? [configurator]
            : [];

        var configurationProvider = module is IModuleConfigurationProvider moduleConfigurationProvider
            ? moduleConfigurationProvider
            : parentModule is IModuleConfigurationProvider parentConfigurationProvider 
                ? parentConfigurationProvider
                : null;

        Initialize(module, serviceModule, configurators, configurationProvider, parentModule);
    }

    /// <summary>
    /// Initializes the provided <see cref="Node"/> module.
    /// </summary>
    /// <typeparam name="TNode">The type of node being initialized</typeparam>
    /// <param name="module">The module to initialize</param>
    /// <param name="configurationProvider">The specific configuration provider to use</param>
    /// <param name="parentModule">The parent module, if one exists</param>
    /// <remarks>
    /// 💡Notes:
    /// <list type="bullet">
    /// <item>If the provided node is not of type <see cref="IServiceModule"/>, then this method will do nothing</item>
    /// <item>The method will attempt to determine if the provided module is an <see cref="IGameServiceConfigurator"/> and use it if possible.</item>
    /// </list>
    /// </remarks>
    public static void Initialize<TNode>(TNode module, IModuleConfigurationProvider configurationProvider, IServiceModule? parentModule = null)
        where TNode: Node
    {
        if (module is not IServiceModule serviceModule)
        {
            return;
        }

        IGameServiceConfigurator[] configurators = module is IGameServiceConfigurator configurator
            ? [configurator]
            : [];

        Initialize(module, serviceModule, configurators, configurationProvider, parentModule);
    }

    /// <summary>
    /// Initializes the provided <see cref="Node"/> module.
    /// </summary>
    /// <typeparam name="TNode">The type of node being initialized</typeparam>
    /// <param name="module">The module to initialize</param>
    /// <param name="configurators">The specific configurators to use with the initialization of the module</param>
    /// <param name="parentModule">The parent module, if one exists</param>
    /// <remarks>
    /// 💡Notes:
    /// <list type="bullet">
    /// <item>If the provided node is not of type <see cref="IServiceModule"/>, then this method will do nothing</item>
    /// <item>The method will attempt to determine if the provided module is an <see cref="IModuleConfigurationProvider"/> and use it if possible, falling back to the parent if the module is not and it can be used.</item>
    /// </list>
    /// </remarks>
    public static void Initialize<TNode>(TNode module, IEnumerable<IGameServiceConfigurator> configurators, IServiceModule? parentModule = null)
        where TNode : Node
    {
        if (module is not IServiceModule serviceModule)
        {
            return;
        }

        var configurationProvider = module is IModuleConfigurationProvider moduleConfigurationProvider
            ? moduleConfigurationProvider
            : parentModule is IModuleConfigurationProvider parentConfigurationProvider
                ? parentConfigurationProvider
                : null;

        Initialize(module, serviceModule, configurators, configurationProvider, parentModule);
    }

    /// <summary>
    /// Initializes the provided <see cref="Node"/> module.
    /// </summary>
    /// <typeparam name="TNode">The type of node being initialized</typeparam>
    /// <param name="module">The module to initialize</param>
    /// <param name="configurators">The specific configurators to use with the initialization of the module</param>
    /// <param name="parentModule">The parent module, if one exists</param>
    /// <remarks>
    /// 💡Notes:
    /// <list type="bullet">
    /// <item>If the provided node is not of type <see cref="IServiceModule"/>, then this method will do nothing</item>
    /// <item>The method will attempt to determine if the provided module is an <see cref="IGameServiceConfigurator"/> and use it if possible.</item>
    /// <param name="configurationProvider">The specific configuration provider to use</param>
    /// </list>
    /// </remarks>
    public static void Initialize<TNode>(TNode module, IEnumerable<IGameServiceConfigurator> configurators, IModuleConfigurationProvider configurationProvider, IServiceModule? parentModule = null)
        where TNode : Node
    {
        if (module is not IServiceModule serviceModule)
        {
            return;
        }

        Initialize(module, serviceModule, configurators, configurationProvider, parentModule);
    }

    /// <summary>
    /// Initializes the provided <see cref="Node"/> module.
    /// </summary>
    /// <param name="root">The node that is used to scan for required node dependencies</param>
    /// <param name="serviceModule">The module to initialize</param>
    /// <param name="configurators">The specific configurators to use with the initialization of the module</param>
    /// <param name="configurationProvider">The configuration provider to get the application configuration with</param>
    /// <param name="parentModule">The parent module, if one exists</param>
    public static void Initialize(Node root, IServiceModule serviceModule, IEnumerable<IGameServiceConfigurator> configurators, IModuleConfigurationProvider? configurationProvider,
        IServiceModule? parentModule = null)
    {
        ArgumentNullException.ThrowIfNull(root);
        ArgumentNullException.ThrowIfNull(serviceModule);
        ArgumentNullException.ThrowIfNull(configurators);

        configurators ??= [];

        var serviceBuilder = new GameServiceBuilder(root, configurationProvider ?? new EmptyConfigurationProvider(), parentModule?.Services);
        foreach (var configurator in configurators)
        {
            configurator.Configure(serviceBuilder);
        }

        var services = serviceBuilder.BuildServiceProvider();
        serviceModule.Initialize(services);

        foreach (var postInitializationAction in serviceBuilder.PostInitializationActions)
        {
            postInitializationAction(services);
        }
    }

    #endregion
}
