using Microsoft.Extensions.Configuration;
using OSK.Petra.DependencyInjection.Ports;
using OSK.Petra.Godot.Modules.Services.Scripts;
using OSK.Petra.Modules;
using OSK.Petra.Modules.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OSK.Petra.Godot.Modules.Services.UnitTests._Helpers;

public partial class TestGameModuleServiceConfigurator : GameServiceModule, ITestModuleServiceConfigurator
{
    public bool ConfigurationWasCalled { get; private set;  }

    public override ModuleName ModuleName => "Abc";

    public IConfiguration GetConfiguration()
    {
        ConfigurationWasCalled = true;
        return new EmptyConfigurationProvider().GetConfiguration();
    }
}
