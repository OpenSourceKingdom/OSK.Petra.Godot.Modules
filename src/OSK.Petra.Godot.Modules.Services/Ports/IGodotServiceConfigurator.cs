using Godot;
using OSK.Hexagonal.MetaData;
using OSK.Petra.Modules.Bootstrapper.Ports;
using System;
using System.Collections.Generic;
using System.Text;
using OSK.Petra.Godot.Modules.Services.Ports;

namespace OSK.Petra.Godot.Modules.Services.Ports;

/// <summary>
/// A game service configurator for the Godot game engine
/// </summary>
[HexagonalIntegration(HexagonalIntegrationType.LibraryProvided)]
public interface IGodotServiceConfigurator: IModuleServiceBuilder
{
    /// <summary>
    /// Registers a <see cref="Node"/> object to the dependency container
    /// </summary>
    /// <typeparam name="TNode">The type of node being registered</typeparam>
    /// <returns>The configurator for chaining</returns>
    IGodotServiceConfigurator AddNode<TNode>()
        where TNode : Node;

    /// <summary>
    /// Registers a <see cref="Node"/> object to the dependency container that is associated to a given interface
    /// </summary>
    /// <typeparam name="TInterface">The interface service type</typeparam>
    /// <typeparam name="TNode">The implementation service type</typeparam>
    /// <returns>The configurator for chaining</returns>
    IGodotServiceConfigurator AddNode<TInterface, TNode>()
        where TInterface : class
        where TNode : Node, TInterface;
}
