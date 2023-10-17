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
    }
}


