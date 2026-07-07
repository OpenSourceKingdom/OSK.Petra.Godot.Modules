using Microsoft.Extensions.DependencyInjection;
using OSK.Petra.DependencyInjection.Ports;
using System;
using System.Collections.Generic;

namespace OSK.Petra.Godot.Modules.Services.Internal.Services;

internal class EmptyGameServiceProvider : IGameServiceProvider
{
    public object? GetService(Type serviceType)
        => null;

    public IReadOnlyCollection<ServiceDescriptor> GetServiceDescriptors()
        => [];
}
