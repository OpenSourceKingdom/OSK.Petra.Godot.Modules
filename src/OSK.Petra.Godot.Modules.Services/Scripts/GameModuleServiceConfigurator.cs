using Godot;
using OSK.Petra.Modules.Bootstrapper.Ports;
using OSK.Petra.Modules.Services.Ports;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSK.Petra.Godot.Modules.Services.Scripts;

[GlobalClass]
public abstract partial class GameModuleServiceConfigurator : Node, IModuleServiceConfigurator
{
    #region IModuleServiceConfigurator

    /// <inheritdoc/>
    public abstract void Configure(IModuleServiceBuilder serviceBuilder);

    #endregion
}
