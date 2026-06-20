using Godot;
using OSK.Petra.DependencyInjection;
using OSK.Petra.DependencyInjection.Ports;
using OSK.Petra.Godot.Configuration;
using OSK.Petra.Modules.Models;
using System;
using System.Collections.Generic;
using System.Text;
using TowerDefenseLabs.Libraries.OSK.Godot.AssetManagement.Scripts;
using OSK.Petra.Godot.Modules.Services.Ports;

namespace OSK.Petra.Godot.Modules.Services.Scripts;

public abstract partial class GameServiceModule: GodotModule, IServiceModule
{
    #region IGameServiceModule

    /// <inheritdoc/>
    public IGameServiceProvider Services { get; private set; }

    /// <inheritdoc/>
    public void Initialize(IGameServiceProvider serviceProvider)
    {
        if (serviceProvider is null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        Services = serviceProvider;
    }

    #endregion

    #region Helpers

    private void InitializeNode(Node node, IServiceProvider serviceProvider)
    {
        foreach (var child in node.GetChildren())
        {
            InitializeNode(child, serviceProvider);
        }

        DependencyInjector.InjectDependencies(serviceProvider, node);
    }

    protected abstract void Configure(IGodotServiceConfigurator configurator);

    #endregion
}
