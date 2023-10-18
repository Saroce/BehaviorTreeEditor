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
        
        [NonSerialized]
        public NodeState TreeState = NodeState.Inactive;
        
        /// <summary>
        /// deltaTime为服务端Tick一次的时间间隔，或者帧同步的帧间隔时间，单位为ms
        /// </summary>
        /// <param name="deltaTime"></param>
        /// <returns></returns>
        public NodeState Update(int deltaTime) {
            if (TreeNodeData.EntryNode != null && TreeState is NodeState.Inactive or NodeState.Running) {
                TreeState = TreeNodeData.EntryNode.Update(deltaTime);
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


