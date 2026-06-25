using Godot;
using OSK.Petra.DependencyInjection;
using OSK.Petra.DependencyInjection.Ports;
using System;
using OSK.Petra.Modules.Services;
using OSK.Petra.Godot.Modules.Scripts;
using OSK.Petra.Modules;
using OSK.Petra.Godot.Modules.Services.Internal.Services;

namespace OSK.Petra.Godot.Modules.Services.Scripts;

/// <summary>
/// A base service module for Godot. Implementations are only required to provide a <see cref="ModuleName"/>, but overriding the initialization logic should be achievable
/// </summary>
[GlobalClass]
public abstract partial class GameServiceModule: GameModule, IServiceModule
{
    #region IGameServiceModule

    /// <inheritdoc/>
    public IGameServiceProvider Services { get; protected set; } = new EmptyGameServiceProvider();

    /// <inheritdoc/>
    public void Initialize(IGameServiceProvider serviceProvider)
    {
        if (serviceProvider is null)
        {
            throw new ArgumentNullException(nameof(serviceProvider));
        }

        Services = serviceProvider;

        InitializeNode(this, Services);
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

    #endregion
}
