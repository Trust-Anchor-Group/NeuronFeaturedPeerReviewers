# NeuronFeaturedPeerReviewers

Service module for Neurons, allowing the operator to feature pre-approved peer reviewers that can facilitate onboarding of 
TAG ID users in their respective areas.

## Projects

The solution contains the following C# projects:

| Project                              | Framework         | Description |
|:-------------------------------------|:------------------|:------------|
| `TAG.Identity.FeaturedPeerReviewers` | .NET Standard 2.0 | Service module for the [TAG Neuron](https://lab.tagroot.io/Documentation/Index.md), permitting configuration of featured peer-reviewers, to simplify onboarding of TAG ID users. |

## Nugets

The following nugets external are used. They faciliate common programming tasks, and
enables the libraries to be hosted on an [IoT Gateway](https://github.com/PeterWaher/IoTGateway).
This includes hosting the bridge on the [TAG Neuron](https://lab.tagroot.io/Documentation/Index.md).
They can also be used standalone.

| Nuget                                                                                              | Description |
|:---------------------------------------------------------------------------------------------------|:------------|
| [Paiwise](https://www.nuget.org/packages/Paiwise)                                                  | Contains services for integration of financial services into Neurons. |
| [Waher.Events](https://www.nuget.org/packages/Waher.Events/)                                       | An extensible architecture for event logging in the application. |
| [Waher.IoTGateway](https://www.nuget.org/packages/Waher.IoTGateway/)                               | Contains the [IoT Gateway](https://github.com/PeterWaher/IoTGateway) hosting environment. |
| [Waher.Persistence](https://www.nuget.org/packages/Waher.Persistence/)                             | Abstraction layer for object databases. |
| [Waher.Runtime.Inventory](https://www.nuget.org/packages/Waher.Runtime.Inventory/)                 | Maintains an inventory of type definitions in the runtime environment, and permits easy instantiation of suitable classes, and inversion of control (IoC). |
| [Waher.Runtime.Threading](https://www.nuget.org/packages/Waher.Runtime.Threading/)                 | Provides tools for managing objects in a multi-user, multi-threaded environment. |
