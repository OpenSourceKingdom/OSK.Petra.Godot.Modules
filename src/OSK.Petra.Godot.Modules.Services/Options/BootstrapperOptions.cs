using OSK.Petra.DependencyInjection.Ports;
using OSK.Petra.Godot.Modules.Services.Ports;
using OSK.Petra.Modules.Services;
using OSK.Petra.Modules.Services.Ports;
using System.Collections.Generic;

namespace OSK.Petra.Godot.Modules.Services.Options;

/// <summary>
/// A set of options that helps the <see cref="GameModuleBootstrapper"/> configure an <see cref="IServiceModule"/>
/// </summary>
public class BootstrapperOptions
{
    #region Variables

    internal List<IGameServiceConfigurator> ServiceConfigurators { get; private set; } = [];

    internal IServiceModule? ParentModule { get; private set; }

    internal bool UseParentProviderAsPrimary { get; private set; }

    internal IModuleConfigurationProvider? ConfigurationProvider { get; private set; }

    #endregion

    #region Api

    /// <summary>
    /// Sets the parent module for the bootstrapper to use
    /// </summary>
    /// <param name="serviceModule">The parent module</param>
    /// <param name="useParentProviderAsPrimary">Whether the parent module's services should be considered the primary dependency resolver</param>
    /// <returns>The options for chaining</returns>
    public BootstrapperOptions WithParent(IServiceModule serviceModule, bool useParentProviderAsPrimary = false)
    {
        ParentModule = serviceModule;
        UseParentProviderAsPrimary = useParentProviderAsPrimary;

        return this;
    }

    /// <summary>
    /// Sets the configuration provider for the bootstrapper to use
    /// </summary>
    /// <param name="provider">The configuration provider to use</param>
    /// <returns>The options for chaining</returns>
    public BootstrapperOptions WithConfigurationProvider(IModuleConfigurationProvider provider)
    {
        ConfigurationProvider = provider;

        return this;
    }

    /// <summary>
    /// Adds service configurators for the bootstrapper to use configuring the service provider
    /// </summary>
    /// <param name="serviceConfigurators">The service configurators</param>
    /// <returns>The options for chaining</returns>
    public BootstrapperOptions WithServiceConfigurators(params IGameServiceConfigurator[] serviceConfigurators)
    {
        ServiceConfigurators.AddRange(serviceConfigurators);

        return this;
    }

    #endregion
}
