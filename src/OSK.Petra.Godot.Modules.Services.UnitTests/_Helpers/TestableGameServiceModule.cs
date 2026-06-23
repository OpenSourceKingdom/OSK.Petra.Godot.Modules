using Godot;
using OSK.Petra.DependencyInjection.Ports;
using OSK.Petra.Godot.Modules.Services.Scripts;
using OSK.Petra.Modules;
using OSK.Petra.Modules.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace OSK.Petra.Godot.Modules.Services.UnitTests._Helpers;

public partial class TestableGameServiceModule : GameServiceModule
{
    public override ModuleName ModuleName => "Test";
}
