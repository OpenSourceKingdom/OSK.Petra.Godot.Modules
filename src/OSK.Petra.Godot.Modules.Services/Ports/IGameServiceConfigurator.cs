using OSK.Hexagonal.MetaData;
using OSK.Petra.Modules.Services.Ports;

namespace OSK.Petra.Godot.Modules.Services.Ports;

/// <summary>
/// A service configurator meamt to configure a <see cref="IGameModuleServiceBuilder"/>
/// </summary>
[HexagonalIntegration(HexagonalIntegrationType.IntegrationOptional, HexagonalIntegrationType.ConsumerOptional)]
public interface IGameServiceConfigurator: IModuleServiceConfigurator<IGameModuleServiceBuilder>
{
}
