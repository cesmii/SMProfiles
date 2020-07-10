using Opc.Ua;
using Opc.Ua.Export;
using Opc.Ua.Server;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace nodeset2json
{
    public class NodesetNodeManager : CustomNodeManager2
    {
        private string nodesetFilePath;

        #region Constructors

        /// <summary>
        /// Overriden constructor.
        /// </summary>
        /// <param name="server">the server object </param>
        /// <param name="configuration">the configuration settings</param>
        /// <param name="nodesetfile">the nodeset file path</param>
        public NodesetNodeManager(IServerInternal server, ApplicationConfiguration configuration)
            : base(server, configuration, Namespaces.NodesetServerApplication)
        {
            SystemContext.NodeIdFactory = this;
        }

        /// <summary>
        /// Overriden constructor takes the nodeset xml file.
        /// </summary>
        /// <param name="server">the server object </param>
        /// <param name="configuration">the configuration settings</param>
        /// <param name="nodesetfile">the nodeset file path</param>
        public NodesetNodeManager(IServerInternal server, ApplicationConfiguration configuration, string nodesetfile)
            : base(server, configuration, Namespaces.NodesetServerApplication)
        {
            SystemContext.NodeIdFactory = this;
            nodesetFilePath = nodesetfile;
        }

        #endregion


        #region Overriden Methods

        /// <summary>
        /// This is where the nodes are added to the server by loading the nodeset file
        /// </summary>
        public override void CreateAddressSpace(IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            lock (Lock)
            {
                IList<IReference> references = null;

                if (!externalReferences.TryGetValue(ObjectIds.ObjectsFolder, out references))
                {
                    externalReferences[ObjectIds.ObjectsFolder] = references = new List<IReference>();
                }

                ImportXml(externalReferences, nodesetFilePath);
            }
        }

        #endregion


        #region Public Properties
        string NodesetFilePath { get => nodesetFilePath; set => nodesetFilePath = value; }

        public UANodeSet NodeSet { get; set; }

        #endregion

        #region Private Methods
        /// <summary>
        /// Import NodeSets from xml
        /// </summary>
        /// <param name="path">String to path of XML</param>
        public void ImportXml(IDictionary<NodeId, IList<IReference>> externalReferences, string resourcepath)
        {
            NodeStateCollection predefinedNodes = new NodeStateCollection();

            Stream stream = new FileStream(resourcepath, FileMode.Open);
            NodeSet = Opc.Ua.Export.UANodeSet.Read(stream);

            SystemContext.NamespaceUris.Append(NodeSet.NamespaceUris.ToString());
            NodeSet.Import(SystemContext, predefinedNodes);

        }

        #endregion
    }
}
