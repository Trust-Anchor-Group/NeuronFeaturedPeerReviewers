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

## Installable Package

The `TAG.Identity.FeaturedPeerReviewers` project has been made into a package that can be downloaded and installed on any 
[TAG Neuron](https://lab.tagroot.io/Documentation/Index.md).
To create a package, that can be distributed or installed, you begin by creating a *manifest file*. The
`TAG.Identity.FeaturedPeerReviewers` project has a manifest file called `TAG.Identity.FeaturedPeerReviewers.manifest`. It defines the
assemblies and content files included in the package. You then use the `Waher.Utility.Install` and `Waher.Utility.Sign` command-line
tools in the [IoT Gateway](https://github.com/PeterWaher/IoTGateway) repository, to create a package file and cryptographically
sign it for secure distribution across the Neuron network.

The Featured Peer-Reviewers service is published as a package on TAG Neurons. If your neuron is connected to this network, you can 
install the package using the following information:

| Package information                                                                                                              ||
|:-----------------|:---------------------------------------------------------------------------------------------------------------|
| Package          | `TAG.Identity.FeaturedPeerReviewers.package`                                                                   |
| Installation key | TBD                                                                                                            |
| More Information | TBD                                                                                                            |

## Building, Compiling & Debugging

The repository assumes you have the [IoT Gateway](https://github.com/PeterWaher/IoTGateway) repository cloned in a folder called
`C:\My Projects\IoT Gateway`, and that this repository is placed in `C:\My Projects\NeuronFeaturedPeerReviewers`. You can place the
repositories in different folders, but you need to update the build events accordingly. To run the application, you select the
`TAG.Identity.FeaturedPeerReviewers` project as your startup project. It will execute the console version of the
[IoT Gateway](https://github.com/PeterWaher/IoTGateway), and make sure the compiled files of the `NeuronFeaturedPeerReviewers` 
solution is run with it.

### Configuring service

You configure the service via the browser, by navigating to the `/FeaturedPeerReviewers/Settings.md` resource. There you will be
able to access the list of approved peer reviewers, as well as the peer reviewers that have applied to become featured reviewers.

A peer reviewer can apply to become a featured peer reviewer. Such an application can be registered navigating to the
`/FeaturedPeerReviewers/Apply.md` resource on the Neuron(R). To remove an application, or a featured peer reviewer, the
operator can either go to the `/FeaturedPeerReviewers/Settings.md` resource, or the user go to the 
`/FeaturedPeerReviewers/SettingRemove.md` resource.

### Gateway.config

To simplify development, once the project is cloned, add a `FileFolder` reference
to your repository folder in your [gateway.config file](https://lab.tagroot.io/Documentation/IoTGateway/GatewayConfig.md). 
This allows you to test and run your changes to Markdown and Javascript immediately, 
without having to synchronize the folder contents with an external 
host, or recompile or go through the trouble of generating a distributable software 
package just for testing purposes. Changes you make in .NET can be applied in runtime
if you the *Hot Reload* permits, otherwise you need to recompile and re-run the
application again.

Example of how to point a web folder to your project folder:

```
<FileFolders>
  <FileFolder webFolder="/FeaturedPeerReviewers" folderPath="C:\My Projects\NeuronFeaturedPeerReviewers\TAG.Identity.FeaturedPeerReviewers\Root\FeaturedPeerReviewers"/>
</FileFolders>
```

**Note**: Once file folder reference is added, you need to restart the IoT Gateway service for the change to take effect.

**Note 2**:  Once the gateway is restarted, the source for the files is in the new location. Any changes you make in the corresponding
`ProgramData` subfolder will have no effect on what you see via the browser.

**Note 3**: This file folder is only necessary on your developer machine, to give you real-time updates as you edit the files in your
developer folder. It is not necessary in a production environment, as the files are copied into the correct folders when the package 
is installed.
