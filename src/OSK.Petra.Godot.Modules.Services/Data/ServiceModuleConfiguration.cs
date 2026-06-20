using Godot;
using OSK.Petra.Godot.Modules.Services.Scripts;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSK.Petra.Godot.Modules.Services.Data;

[GlobalClass]
[Tool]
public partial class ServiceModuleConfiguration: Resource
{
    #region Variables

    [Export]
    public GameServiceModule Module { get; set; }

    [Export]
    public GameModuleServiceConfigurator[] Configurators { get; set; } = [];

    #endregion
}
