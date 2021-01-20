# Smart Manufacturing Profiles

## Concepts

SM Profiles™ have multiple uses throughout a smart manufacturing stack, but the simplest conceptual explanation is that they start their life as a Type definition.

Specifically, CESMII has chosen a JSON serialization of an OPC UA Information Model Type definition as our starting point. While the SM Profile contains more parts than just the Type definition, you can start creating SM Profiles now, by using tools like Beeond's [OPC UA Information Model eXcelerator (UMX Pro)](https://beeond.net/opc-ua-information-model-excelerator-umx-pro-download/).

CESMII's first goal with SM Profiles is to see a standarization of Type definitions that can be exchanged between industry members. As an example, an OEM sells a piece of equipment and includes a SM Profile that describes the characteristics of the equipment and the run-time data available within it. This SM Profile, or Type definition, can be used to build information systems, as features for a machine learning algorithm, or to communicate energy consumption information to potential customers.

So far, SM Profiles are 100% in-line with the capabilities of an OPC UA Information Model -- but don't fully realise CESMII's vision. CESMII and the OPC Foundation have formed a joint working group with a goal to make Information Models redistributable, via a Cloud Library, to allow the hydration of pre-created information models in a remote server.

### Part 1

In Part 1 of the SM Profile effort, we add information to allow distribution of SM Profiles from a global Marketplace. This information will allow us to "crowd source" SM Profiles from a variety of contributors who publish Type definitions, in whole or in part, for manfacturing processes and equipment based on their expertise. Such a publisher could be the OEM, or it could be an process expert, system integrator, or developer that has identified commonality in a range of applications.

We'll allow SM Profile creators to extend existing SM Profiles, either at design-time, through Class inheritance, or at run-time, through polymorphism, by publishing SM Profiles in fragments, that get assembled on-the-fly.

Part 1 of the SM Profile effort is aligned with OPC UA Information Models ([Part 5 of the UA Specification](https://reference.opcfoundation.org/v104/Core/docs/Part5/)). Examples of these Information Models can be found here:
[https://github.com/OPCFoundation/UA-Nodeset](https://github.com/OPCFoundation/UA-Nodeset)

### Part 2

In Part 2 of the SM Profile effort, we add information to a Type definition that specifies how to bind an instance of the Type to particular protocol.

As an example, the abstract Type definition includes a property for the current die temperature in the machine. The actual implementation of the machine, however, may very depending on vendor or generation, so the SM Profile binding extensions can be seperately published that indicate how to bind the temperature value to a tag in a PLC or value from a sensor.

### Other Parts

As the membership collaborates with standards bodies, we've identified some additional capabilities we want to include in SM Profiles, that will be added over time. Some of the ideas currently being discussed include:
- Ontologies for attributes and measurement units
- Combinations of SM Profiles and Ontologies (Libraries) for rapid system implementation
- Implementation of event members for Workflow integration
- SM Profiles for Supply Chain transactions

Where ever possible, CESMII will be adopting existing standards, extending them via approved methods, and publishing open specifications for industry application and derivation.

## Application

As an open specification, SM Profiles™ can be implemented by any platform. However, CESMII and its members are developing an implementation that can be used for reference, rapid innovation, and production use of the SM Profile capabilities. This Smart Manufacturing Innovation Platform™ (or SMIP™) includes a secure API that allows developers to build against SM Profile instances as an interface contract -- meaning an app can be written once, and re-deployed against any platform that implements the SM Profile. [Read more about the GraphQL API here](https://github.com/cesmii/API).

Although its not strictly necessary, to fully realize the vision of the SM Profile, it needs to be applied at multiple levels. The CESMII SM Innovation Platform articulates three levels (other platforms may approach these problems differently).

### Edge

At the Edge, the SM Platform uses a connector that acts as a proxy for connected assets that cannot (or should not) directly communicate with an information system.

The SM Edge™ applies the SM Profile via protocol specific adapters to determine 1) what data to collect 2) how to extract it from the asset

### Platform Core

At the Core, the SMIP aggregates data from 1 or more SM Edge connectors and enforces the SM Profile definition into strongly-typed instances. Data can be stored in the SMIP Core, with the in-cluster PostGresQL database, or streamed to an external data store as objects. Objects in the SMIP Core™ are stored in Graph relationship to each other, to facilitate analysis that requires more than just a hierarchical object organization (although hierarchies, like S95, are one supported set of relationships.)

The SMIP Core is also responsible for surfacing the API. Using GraphQL, the API allows developers to discover the Type of each Object, and queries Objects and their relationships at run time.

### Apps

Applications developed against the GraphQL API are fully portable between SMIP instances and to other platforms that implement the API and SM Profiles.
