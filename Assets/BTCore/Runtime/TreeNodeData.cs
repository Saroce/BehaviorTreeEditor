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
        private readonly List<BTNode> Nodes = new List<BTNode>();
        
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
        
        public List<BTNode> GetNodes() => Nodes;

        public BTNode GetNodeByGuid(string guid) {
            return _guid2Nodes.ContainsKey(guid) ? _guid2Nodes[guid] : null;
        }
        
        [OnDeserialized]
        private void OnAfterDeserialize(StreamingContext context) {
            Nodes.ForEach(node => { _guid2Nodes.Add(node.Guid, node); });
        }
    }
}