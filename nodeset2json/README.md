# Introduction 
TODO: Give a short introduction of your project. Let this section explain the objectives or the motivation behind this project. 

---

# Getting Started
TODO: Guide users through getting your code up and running on their own system. In this section you can talk about:
1.	Installation process
2.	Software dependencies
3.	Latest releases
4.	API references

---

# Build and Test
TODO: Describe and show how to build your code and run the tests. 

---

#Design Notes
## Creating the NodeSet Classes ##

I need to model POCO entities to reqresent the NodeSet required types (see Randy's document OPC 10000-6 - UA Specification Part 6 - Mappings Draft 1.05.16.docx ).

In general, we need a few classes that handle all the data coming from an XML NodeSet.

```mermaid
graph TD;
    NodeSet---BaseNode;
    BaseNode-->InstanceNode;
    InstanceNode---ObjectNode;
    InstanceNode---VariableNode;
    InstanceNode---MethodNode;
    InstanceNode---ViewNode;
    BaseNode-->TypeNode;
    TypeNode---ObjectTypeNode;
    TypeNode---VariableTypeNode;
    TypeNode---DataTypeNode;
    TypeNode---ReferenceTypeNode
```

