using Godot;
using OSK.Extensions.Petra.Godot;
using OSK.Petra.DependencyInjection.Ports;
using OSK.Petra.Godot.Modules.Services.Data;
using OSK.Petra.Godot.Modules.Services.Internal.Services;
using OSK.Petra.Modules.Bootstrapper;
using OSK.Petra.Modules.Bootstrapper.Ports;
using OSK.Petra.Modules.Models;
using OSK.Petra.Modules.Services.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OSK.Petra.Godot.Modules.Services.Scripts;

[GlobalClass]
[Tool]
public partial class ModuleBootstrapper: Node
{
    #region Variables

    [Export]
    private ServiceModuleConfiguration[] _moduleConfigurations;

    [Export]
    private GameModuleServiceConfigurator[] _globalConfigurators;

    #endregion

    #region Godot Overrides

    public override void _Ready()
    {
        ArgumentNullException.ThrowIfNull(_root);

        var initalizingModules = _modules.Cast<IServiceModule>() ?? _root.FindNodes<IServiceModule>();
        var configurators = _configurators ?? _root.FindNodes<IModuleServiceConfigurator>();
        Initialize(_root, initalizingModules, configurators);
    }

    #endregion

    #region Static

    public static void Initialize<TNode>(TNode module, IServiceModule? parentModule = null)
        where TNode : Node, IServiceModule, IModuleServiceConfigurator
        => Initialize(module, module, [module], null, parentModule);

    public static void Initialize<TNode>(TNode module, IConfigurationProvider configurationProvider, IServiceModule? parentModule = null)
        where TNode: Node, IServiceModule, IModuleServiceConfigurator
        => Initialize(module, module, [module], configurationProvider, parentModule);

    public static void Initialize<TNode>(TNode module, IEnumerable<IModuleServiceConfigurator> configurators, IServiceModule? parentModule = null)
        where TNode : Node, IServiceModule
        => Initialize(module, module, configurators, null, parentModule);
    
    public static void Initialize<TNode>(TNode module, IEnumerable<IModuleServiceConfigurator> configurators, IConfigurationProvider configurationProvider, IServiceModule? parentModule = null)
        where TNode : Node, IServiceModule
        => Initialize(module, module, configurators, configurationProvider, parentModule);

    public static void Initialize(Node root, IServiceModule serviceModule, IEnumerable<IModuleServiceConfigurator> configurators, IConfigurationProvider? configurationProvider,
        IServiceModule? parentModule = null)
    {
        ArgumentNullException.ThrowIfNull(root);
        ArgumentNullException.ThrowIfNull(serviceModule);
        ArgumentNullException.ThrowIfNull(configurators);

        configurators ??= [];

        var serviceBuilder = new GodotServiceConfigurator(root, configurationProvider ?? new EmptyConfigurationProvider(), parentModule?.Services);
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
