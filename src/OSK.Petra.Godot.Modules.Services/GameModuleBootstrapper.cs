using Godot;
using OSK.Petra.Godot.Modules.Services.Internal.Services;
using OSK.Petra.Godot.Modules.Services.Options;
using OSK.Petra.Godot.Modules.Services.Ports;
using OSK.Petra.Godot.Modules.Services.Scripts;
using OSK.Petra.Modules.Services;
using OSK.Petra.Modules.Services.Ports;
using System;

namespace OSK.Petra.Godot.Modules.Services;

/// <summary>
/// A bootstrapper that configures a <see cref="GameServiceModule"/> and its services for running
/// </summary>
public static class GameModuleBootstrapper
{
    #region Static

    /// <summary>
    /// Initializes a <see cref="Node"/>, if it is an <see cref="IServiceModule"/>, otherwise it will be ignored. Default configuration is used for the <see cref="BootstrapperOptions"/>.
    /// </summary>
    /// <param name="root">The root node for the scene</param>
    public static void Initialize(Node root)
    {
        if (root is not IServiceModule serviceModule)
        {
            return;
        }

        Initialize(root, serviceModule);
    }

    /// <summary>
    /// Initializes a <see cref="Node"/>, if it is an <see cref="IServiceModule"/>, otherwise it will be ignored. Custom configuration is applied for the <see cref="BootstrapperOptions"/>.
    /// </summary>
    /// <param name="root">The root node for the scene</param>
    /// <param name="configurator">The options configurator</param>
    public static void Initialize(Node root, Action<BootstrapperOptions> configurator)
    {
        if (root is not IServiceModule serviceModule)
        {
            return;
        }

        Initialize(root, serviceModule, configurator);
    }

    /// <summary>
    /// Initializes the provided <see cref="IServiceModule"/> using the <see cref="Node"/> as the root. Default configuration is applied for the <see cref="BootstrapperOptions"/>.
    /// </summary>
    /// <param name="root">The root node for the scene</param>
    /// <param name="serviceModule">The service module to intialize</param>
    public static void Initialize(Node root, IServiceModule serviceModule)
        => Initialize(root, serviceModule, _ => { });

    /// <summary>
    /// Initializes the provided <see cref="IServiceModule"/> using the <see cref="Node"/> as the root. Custom configuration is applied for the <see cref="BootstrapperOptions"/>.
    /// </summary>
    /// <param name="root">The root node for the scene</param>
    /// <param name="serviceModule">The service module to intialize</param>
    /// <param name="configurator">The options configurator</param>
    public static void Initialize(Node root, IServiceModule serviceModule, Action<BootstrapperOptions> configurator)
    {
        ArgumentNullException.ThrowIfNull(root);
        ArgumentNullException.ThrowIfNull(serviceModule);
        ArgumentNullException.ThrowIfNull(configurator);

        var options = new BootstrapperOptions();
        configurator(options);
        var configurationProvider = GetConfigurationProviderOrDefault(serviceModule, options);

        var serviceBuilder = options.ParentModule is null
            ? new GameServiceBuilder(root, configurationProvider)
            : new GameServiceBuilder(root, configurationProvider, options.ParentModule.Services, options.UseParentProviderAsPrimary);
        foreach (var serviceConfigurator in options.ServiceConfigurators)
        {
            serviceConfigurator.Configure(serviceBuilder);
        }

        if (serviceModule is IGameServiceConfigurator moduleConfigurator)
        {
            moduleConfigurator.Configure(serviceBuilder);
        }

        var services = serviceBuilder.BuildServiceProvider();
        serviceModule.Initialize(services);

        foreach (var postInitializationAction in serviceBuilder.PostInitializationActions)
        {
            postInitializationAction(services);
        }
    }

    #endregion

    #region Helpers

    private static IModuleConfigurationProvider GetConfigurationProviderOrDefault(IServiceModule serviceModule, BootstrapperOptions options)
    {
        if (options.ConfigurationProvider is not null)
        {
            return options.ConfigurationProvider;
        }

        var moduleConfigurationProvider = serviceModule is IModuleConfigurationProvider configurationProvider
            ? configurationProvider
            : options.ParentModule as IModuleConfigurationProvider;
        return moduleConfigurationProvider ?? new EmptyConfigurationProvider();
    }

    #endregion
}
