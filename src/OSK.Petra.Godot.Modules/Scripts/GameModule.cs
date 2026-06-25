using Godot;
using OSK.Petra.Modules;

namespace OSK.Petra.Godot.Modules.Scripts;

/// <summary>
/// Represents a Godot module that is defined by a root node
/// </summary>
[GlobalClass]
public abstract partial class GameModule : Node, IModule
{
    #region IGameModule

    /// <inheritdoc/>
    public abstract ModuleName ModuleName { get; }

    #endregion
}
