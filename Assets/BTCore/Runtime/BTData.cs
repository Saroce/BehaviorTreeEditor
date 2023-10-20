//------------------------------------------------------------
//        File:  BTData.cs
//       Brief:  BTData
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-01
//============================================================

using System;
using System.Runtime.Serialization;
using BTCore.Runtime.Blackboards;
using BTCore.Runtime.Composites;
using BTCore.Runtime.Decorators;

namespace BTCore.Runtime
{
    public class BTData
    {
        public TreeNodeData TreeNodeData = new TreeNodeData();
        public Blackboard Blackboard = new Blackboard();
        public BTSettings Settings = new BTSettings();
        
        [NonSerialized]
        public NodeState TreeState = NodeState.Inactive;
        
        public NodeState Update() {
            if (TreeNodeData.EntryNode == null) {
                return NodeState.Failure;
            }
            
            if (TreeState is NodeState.Inactive or NodeState.Running) {
                TreeState = TreeNodeData.EntryNode.Update();
            }
            else if (Settings.RestartWhenComplete) {
                TreeNodeData.EntryNode.Abort();
                TreeState = NodeState.Inactive;
            }

            return TreeState;
        }

        [OnDeserialized]
        private void OnAfterDeserialize(StreamingContext context) {
            TreeNodeData.GetNodes().ForEach(node => {
                node.OnInit(Blackboard);
            });
        }

        /// <summary>
        /// 反序列化后重建树的节点之间的连接关系
        /// </summary>
        public void RebuildTree() {
            var treeNodes = TreeNodeData.GetNodes();
            treeNodes.ForEach(RebindChild);
        }
        
        private void RebindChild(BTNode node) {
            var childrenGuids = node.GetChildrenGuids();
            foreach (var guid in childrenGuids) {
                if (node is EntryNode entryNode) {
                    entryNode.SetChild(TreeNodeData.GetNodeByGuid(guid));
                    break;
                }
                if (node is Decorator decorator) {
                    decorator.SetChild(TreeNodeData.GetNodeByGuid(guid));
                    break;
                }
                if (node is Composite composite) {
                    composite.AddChild(TreeNodeData.GetNodeByGuid(guid));
                }
            }
        }
    }
}


