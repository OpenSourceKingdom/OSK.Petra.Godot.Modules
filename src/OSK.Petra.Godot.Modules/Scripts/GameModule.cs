using Godot;
using TowerDefenseLabs.Libraries.OSK.Godot.AssetManagement.Scripts;

namespace OSK.Petra.Godot.Modules.Scripts;

public abstract partial class GameModule : Node, IModule
{
    #region IGameModule

    public abstract ModuleName ModuleName { get; }

    #endregion
}
