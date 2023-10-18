//------------------------------------------------------------
//        File:  NodeData.cs
//       Brief:  NodeData
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-17
//============================================================

using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace BTCore.Runtime
{
    public class TreeNodeData
    {
        [JsonIgnore]
        public EntryNode EntryNode;
        [JsonProperty]
        private List<BTNode> Nodes = new List<BTNode>();
        
        // key == Guid.NewGuid().ToString, 暂时不对key做判断，应该不会存在相同的key
        private readonly Dictionary<string, BTNode> _guid2Nodes = new Dictionary<string, BTNode>();
        
        public void AddNode(BTNode node) {
            Nodes.Add(node);
            _guid2Nodes.Add(node.Guid, node);
        }

        public void RemoveNode(BTNode node) {
            if (!_guid2Nodes.ContainsKey(node.Guid)) {
                return;
            }

            var toRemove = _guid2Nodes[node.Guid];
            Nodes.Remove(toRemove);
            _guid2Nodes.Remove(node.Guid);
        }

        /// <summary>
        /// 节点替换，主要用于外部节点处理
        /// </summary>
        /// <param name="index">位于Nodes列表中的索引</param>
        /// <param name="newNode">新的外部节点</param>
        public void ReplaceNode(int index, BTNode newNode) {
            if (index < 0 || index >= Nodes.Count) {
                return;
            }
            
            Nodes[index] = newNode;
            _guid2Nodes[newNode.Guid] = newNode;
        }
        
        public List<BTNode> GetNodes() => Nodes;

        public BTNode GetNodeByGuid(string guid) {
            return _guid2Nodes.ContainsKey(guid) ? _guid2Nodes[guid] : null;
        }
        
        [OnDeserialized]
        private void OnAfterDeserialize(StreamingContext context) {
            Nodes.ForEach(node => {
                if (node is EntryNode entryNode) {
                    EntryNode = entryNode;
                }
                _guid2Nodes.Add(node.Guid, node);
            });
            
        }
    }
}