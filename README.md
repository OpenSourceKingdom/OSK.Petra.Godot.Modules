# OSK.Petra.Godot.Modules

Defines a simple Godot module for the Petra module system, which is represented by a node in a given scene.

# OSK.Petra.Godot.Modules.Services

Defines a Godot module for the Petra module system that is able to utilize standard .NET DI practices for setting up and initializing a given module within a scene.

## Usage

The only expectations required on consumers comes from the Service modules library, as this library provides several helpers and builders to initialize a service module within a Godot scene.

Users can initialize a Godot service module either by providing their own `IGameServiceProvider` to the Godot service module or utilizing the library provided `GameModuleBootstrapper` that will
handle building and maintaining the required game service builder and triggering any post actions required for a module. The bootstrapper provides various methods to help with initializing a module and
can be called from anywhere to do so.

For conceptual purposes, a game module should be thought of as the root scene node. There may be parent or sibling modules within a scene, but a module is typically its own isolated unit of services and
related entities.