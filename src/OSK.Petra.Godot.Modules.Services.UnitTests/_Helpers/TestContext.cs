using Godot;
using Microsoft.Extensions.Configuration;
using Moq;
using OSK.Petra.DependencyInjection.Ports;
using OSK.Petra.Godot.Modules.Services.Ports;
using OSK.Petra.Modules.Services;
using IConfigurationProvider = OSK.Petra.Modules.Services.Ports.IConfigurationProvider;

namespace OSK.Petra.Godot.Modules.Services.UnitTests._Helpers;

public class TestServiceConfigurator : IGameServiceConfigurator
{
    public bool ConfigureWasCalled { get; private set; }
    public IGameModuleServiceBuilder? LastBuilder { get; private set; }

    public void Configure(IGameModuleServiceBuilder builder)
    {
        ConfigureWasCalled = true;
        LastBuilder = builder;
    }
}

public class TestConfigurationProvider : IConfigurationProvider
{
    public bool GetConfigurationWasCalled { get; private set; }

    public IConfiguration GetConfiguration()
    {
        GetConfigurationWasCalled = true;
        return new ConfigurationBuilder().Build();
    }
}
