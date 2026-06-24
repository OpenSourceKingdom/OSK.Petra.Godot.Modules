using OSK.Petra.Godot.Modules.Services.Ports;

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
