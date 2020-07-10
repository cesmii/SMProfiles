using Opc.Ua;
using Opc.Ua.Export;
using Opc.Ua.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nodeset2json
{
    public class NodesetServer : StandardServer
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="nodesetFilePath"></param>
        public NodesetServer(string nodesetFilePath)
        {
            NodesetFilePath = nodesetFilePath ?? throw new ArgumentNullException(nameof(nodesetFilePath));
        }


        #region Public Properties
        public string NodesetFilePath { get; set; }
        public UANodeSet NodeSet { get; set; }
        public NodesetNodeManager NodeManager {get;set;}
        #endregion

        #region Overridden Methods

        ///// <summary>
        ///// Creates the node managers for the server.
        ///// </summary>
        ///// <remarks>
        ///// This method allows the sub-class create any additional node managers which it uses. The SDK
        ///// always creates a CoreNodeManager which handles the built-in nodes defined by the specification.
        ///// Any additional NodeManagers are expected to handle application specific nodes.
        ///// Applications with small address spaces do not need to create their own NodeManagers and can add any
        ///// application specific nodes to the CoreNodeManager. Applications should use custom NodeManagers when
        ///// the structure of the address space is stored in another system or when the address space is too large
        ///// to keep in memory.
        ///// </remarks>
        protected override MasterNodeManager CreateMasterNodeManager(IServerInternal server, ApplicationConfiguration configuration)
        {
            var nsm = new NodesetNodeManager(server, configuration, NodesetFilePath);
            List<INodeManager> nodeManagers = new List<INodeManager> { nsm };

            //update the public property
            NodeManager = nsm;

            // create master node manager.
            return new MasterNodeManager(server, configuration, null, nodeManagers.ToArray());
        }

        /// <summary>
        /// Loads the non-configurable properties for the application.
        /// </summary>
        /// <remarks>
        /// These properties are exposed by the server but cannot be changed by administrators.
        /// </remarks>
        protected override ServerProperties LoadServerProperties()
        {
            ServerProperties properties = new ServerProperties
            {
                ManufacturerName = "Beeond",
                ProductName = "Nodeset2Json",
                ProductUri = "https://www.beeond.net/Nodeset2Json",
                SoftwareVersion = Utils.GetAssemblySoftwareVersion(),
                BuildNumber = Utils.GetAssemblyBuildNumber(),
                BuildDate = Utils.GetAssemblyTimestamp()
            };

            return properties;
        }

        #endregion
    }
}
