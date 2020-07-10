using Opc.Ua;
using Opc.Ua.Export;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nodeset2json.Serializer
{
    public class RecursiveNode : UANode
    {
        private IList<RecursiveNode> childrenNodes;
        private IList<RecursiveNode> typeDefinitionNodes;

        public IList<RecursiveNode> ChildrenNodes 
        { 
            get => childrenNodes; 
            set => childrenNodes = value; 
        }
        public IList<RecursiveNode> TypeDefinition 
        { 
            get => typeDefinitionNodes; 
            set => typeDefinitionNodes = value; 
        }

        public String DataTypeId { get; set; }
        public String DataTypeName { get; set; }


        public RecursiveNode() : base()
        {
            childrenNodes = new List<RecursiveNode>();
            typeDefinitionNodes = new List<RecursiveNode>();
        }
        public RecursiveNode(UANodeSet nodeset) : base()
        {
            childrenNodes = new List<RecursiveNode>();
            typeDefinitionNodes = new List<RecursiveNode>();
           

            foreach (var item in nodeset.Items)
            {
                try
                {
                    //search for Root
                    if (item.NodeId == "i=84")
                    {

                        this.AccessRestrictions = item.AccessRestrictions;
                        this.BrowseName = item.BrowseName;
                        this.Category = item.Category;
                        this.Description = item.Description;
                        this.DisplayName = item.DisplayName;
                        this.Documentation = item.Documentation;
                        this.Extensions = item.Extensions;
                        this.NodeId = item.NodeId;
                        this.References = item.References;
                        this.ReleaseStatus = item.ReleaseStatus;
                        this.RolePermissions = item.RolePermissions;
                        this.SymbolicName = item.SymbolicName;
                        this.UserWriteMask = item.UserWriteMask;
                        this.WriteMask = item.WriteMask;

                        
                    }

                }
                catch (Exception e)
                {

                    throw e;
                }

            }
            AddTypes(this, nodeset);
            AddChildren(this, nodeset);

        }

        private void AddChildren(RecursiveNode node, UANodeSet nodeset)
        {

            foreach (var item in nodeset.Items)
            {

                if (item.NodeId.Contains("ns=")
                    || item.NodeId == "i=85" //Objects
                    || item.NodeId == "i=86" //Types
                    || item.NodeId == "i=90" //DataTypes
                    || item.NodeId == "i=88" //ObjectTypes
                    || item.NodeId == "i=91" //ReferenceTypes
                    || item.NodeId == "i=89" //VariableTypes
                    || item.NodeId == "i=58" //BaseObjectType
                    || item.NodeId == "i=24" //BaseDataType
                    || item.NodeId == "i=62" //BaseVariableType
                    || item.NodeId == "i=63" //BaseDataVariableType
                    || item.NodeId == "i=68" //PropertyType
                )
                {

                    foreach (var reference in item.References)
                    {
                        RecursiveNode child = new RecursiveNode();


                        // i=47 is HasComponent
                        if (
                            (reference.ReferenceType == "i=47" //HasComponent
                            || reference.ReferenceType == "i=35" 
                            || reference.ReferenceType == "i=45"
                            || reference.ReferenceType == "i=46") //HasProperty
                            && reference.IsForward == false 
                            && reference.Value == node.NodeId)
                        {


                            child.AccessRestrictions = item.AccessRestrictions;
                            child.BrowseName = item.BrowseName;
                            child.Category = item.Category;
                            child.Description = item.Description;
                            child.DisplayName = item.DisplayName;
                            child.Documentation = item.Documentation;
                            child.Extensions = item.Extensions;
                            child.NodeId = item.NodeId;
                            child.References = item.References;
                            child.ReleaseStatus = item.ReleaseStatus;
                            child.RolePermissions = item.RolePermissions;
                            child.SymbolicName = item.SymbolicName;
                            child.UserWriteMask = item.UserWriteMask;
                            child.WriteMask = item.WriteMask;


                            //need to knw if the item is of UAVariable type
                            try
                            {
                                var v = new UAVariable();
                                v = (UAVariable)item;
                                child.DataTypeId = v.DataType;
                                child.DataTypeName = nodeset.Items.SingleOrDefault(i => i.NodeId == child.DataTypeId).BrowseName;
                            }
                            catch (Exception)
                            {
                                
                            }                           


                            node.childrenNodes.Add(child);
                            AddTypes(child, nodeset);
                            AddChildren(child, nodeset);
                        }



                    }
                }
                
            }

        }


        private void AddTypes(RecursiveNode node, UANodeSet nodeset)
        {
            node.typeDefinitionNodes = new List<RecursiveNode>();

            foreach (var reference in node.References)
            {
                if (reference.ReferenceType == "i=40"
               && reference.IsForward == true) //HasTypeDefinition is NOT an Inverse reference
                {
                    var _item = nodeset.Items.SingleOrDefault(n => n.NodeId == reference.Value);

                    if (_item != null)
                    {
                        var typeDefnode = new RecursiveNode();

                        typeDefnode.NodeId = reference.Value;
                        typeDefnode.AccessRestrictions = _item.AccessRestrictions;
                        typeDefnode.BrowseName = _item.BrowseName;
                        typeDefnode.Category = _item.Category;
                        typeDefnode.Description = _item.Description;
                        typeDefnode.DisplayName = _item.DisplayName;
                        typeDefnode.Documentation = _item.Documentation;
                        typeDefnode.Extensions = _item.Extensions;
                        typeDefnode.References = _item.References;
                        typeDefnode.ReleaseStatus = _item.ReleaseStatus;
                        typeDefnode.RolePermissions = _item.RolePermissions;
                        typeDefnode.SymbolicName = _item.SymbolicName;
                        typeDefnode.UserWriteMask = _item.UserWriteMask;
                        typeDefnode.WriteMask = _item.WriteMask;

                        node.typeDefinitionNodes.Add(typeDefnode);
                    }

                };
            }

            
        }
        

     }
}
