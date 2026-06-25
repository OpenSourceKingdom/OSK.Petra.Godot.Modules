using Microsoft.Extensions.DependencyInjection;
using OSK.Petra.DependencyInjection.Ports;
using System;

namespace OSK.Petra.Godot.Modules.Services.Internal.Services;

internal class EmptyGameServiceProvider : IGameServiceProvider
{
    public IServiceCollection CreateScopedServices()
        => new ServiceCollection();

    public object? GetService(Type serviceType)
        => null;
}
