using Microsoft.Extensions.Configuration;
using OSK.Petra.Modules.Services.Ports;

namespace OSK.Petra.Godot.Modules.Services.UnitTests._Helpers;

public class TestConfigurationProvider : IModuleConfigurationProvider
{
    public bool GetConfigurationWasCalled { get; private set; }

    public IConfiguration GetConfiguration()
    {
        GetConfigurationWasCalled = true;
        return new ConfigurationBuilder().Build();
    }
}
