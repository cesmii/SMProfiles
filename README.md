# Smart Manufacturing Profiles

## Updates

Since initial publication in 2020, the SM Profile concept has taken on a life of its own -- it now lives well beyond this repo, in the form of more than 100 companion specifications, dozens of CESMII member contributions, hardware, software, and multiple libraries that cascade from an authoritative root, through crowd sourcing, and even private libraries of internal Namespaces. While the concepts haven't changed, and this sparse documentation may still be useful, there's much more to learn in ongoing conversation with practioners. Some of those call the effort to standardize information models a "Unified Namespace" -- such nomenclature is welcome in the conversation. Ultimately, the ability to know and share a contract (template, schema, Class, Namespace) for a set of data -- independent of an instance of that data -- remains the ideal we all need to work toward!

You can see some of the implementations of Profiles, or tools using Profiles at [CESMII's Marketplace](https://marketplace.cesmii.net), or read on to learn more about the concept and approach.

## Concepts

SM Profiles™ have multiple uses throughout a smart manufacturing stack, but the simplest conceptual explanation is that they start their life as a Type definition (the building blocks of a Namespace).

Specifically, CESMII has chosen an OPC UA Information Model Type definition as our starting point. While the SM Profile contains more parts than just the Type definition, you can start creating SM Profiles now, by using tools like CESMII's [Profile Designer](https://profiledesigner.cesmii.net) or Siemen's [Siome](https://support.industry.siemens.com/cs/document/109755133/siemens-opc-ua-modeling-editor-(siome)?lc=en-WW&dti=0).

CESMII's first goal with SM Profiles is to see a standarization of Type definitions that can be exchanged between industry members -- independent of communication protocol or information infrastructure. As an example, an OEM sells a piece of equipment and includes a SM Profile that describes the characteristics of the equipment and the run-time data available within it. This SM Profile, or Type definition, can be used to build information systems, as features for a machine learning algorithm, or to communicate energy consumption information to potential customers.

So far, SM Profiles are 100% in-line with the capabilities of an OPC UA Information Model -- but in its current state, these don't fully realise CESMII's vision. As a step in that direction, CESMII and the OPC Foundation run a joint working group that make Information Models redistributable, via a free and open source [Cloud Library](https://github.com/cesmii/CESMII-CloudLibrary), to allow the programmatic retrieval information models in an information system.

### Part 1

In Part 1 of the SM Profile effort, we add information to allow distribution of SM Profiles from a global Marketplace. This information will allow us to "crowd source" SM Profiles from a variety of contributors who publish Type definitions, in whole or in part, for manfacturing processes and equipment based on their expertise. Such a publisher could be the OEM, or it could be an process expert, system integrator, or developer that has identified commonality in a range of applications.

We'll allow SM Profile creators to extend existing SM Profiles, either at design-time, through Class inheritance, or at run-time, through polymorphism, by publishing SM Profiles in fragments, that get assembled on-the-fly.

Part 1 of the SM Profile effort is aligned with OPC UA Information Models ([Part 5 of the UA Specification](https://reference.opcfoundation.org/v104/Core/docs/Part5/)). Examples of these Information Models can be found on the [CESMII Marketplace](https://marketplace.cesmii.net), the [OPC Cloud Library](https://github.com/OPCFoundation/UA-CloudLibrary), or on [GitHub](https://github.com/OPCFoundation/UA-Nodeset).

### Part 2

In Part 2 of the SM Profile effort, we add information to a Type definition that specifies how to bind an instance of the Type to particular protocol. There is a one-to-many relationship between a Part 1 Profile and Part 2 Profiles for various implementation-specific bindings.

As an example, the abstract Type definition includes a property for the current die temperature in the machine. The actual implementation of the machine, however, may very depending on vendor or generation, so the SM Profile binding extensions can be seperately published that indicate how to bind the temperature value to a tag in a PLC or value from a sensor.

As of this writing, a candidate format for Part 2 is the [W3C Web Of Things](https://www.w3.org/WoT/) specification, which provides a JSON document format that can be used for binding a variety of protocols to an Information Model. An implementation of this approach can be found [here](https://github.com/OPCFoundation/UA-EdgeTranslator)

### Other Parts

As the membership collaborates with standards bodies, we've identified some additional capabilities we want to include in SM Profiles, that will be added over time. Some of the ideas currently being discussed include:
- Ontologies for attributes and measurement units
- An extension that provides for programmability and logic execution
- Combinations of SM Profiles and Ontologies (Libraries) for rapid system implementation
- Implementation of event members for Workflow integration
- SM Profiles for Supply Chain transactions

Where ever possible, CESMII will be adopting existing standards, extending them via approved methods, and publishing open specifications for industry application and derivation.

## Application

As an open specification, SM Profiles™ can be implemented by any platform. However, CESMII and its members are developing an implementation that can be used for reference, rapid innovation, and production use of the SM Profile capabilities. A Smart Manufacturing Innovation Platform™ (or SMIP™) provides a secure, open API that allows developers to build against SM Profile instances as an interface contract -- meaning an app can be written once, and re-deployed against any platform that implements the SM Profile. [Read more about the API here](https://github.com/cesmii/API).

Although its not strictly necessary, to fully realize the vision of the SM Profile, it needs to be applied at multiple levels. The CESMII SM Innovation Platform define articulates three levels (other platforms may approach these problems differently).

### Edge

At the Edge, a SM Platform uses a connector that acts as a proxy for connected assets that cannot (or should not) directly communicate with an information system.

An Edge or Gateway can apply the SM Profile via protocol specific adapters to determine 1) what data to collect 2) how to extract it from the asset.

### Platform Core

At the Core, a SM Platform aggregates data from 1 or more SM Edge connectors and enforces the SM Profile definition into strongly-typed instances. Objects in a Platform Core are stored in Graph relationship to each other, to facilitate analysis that requires more than just a hierarchical object organization (although hierarchies, like S95, are one supported set of relationships.)

A SMIP Core is also responsible for surfacing the API. The API allows developers to discover the Type of each Object, and queries Objects and their relationships at run time.

### Apps

Applications developed against the Open API are fully portable between SMIP instances and to other platforms that implement the API and SM Profiles.
