//------------------------------------------------------------
//        File:  AIService.cs
//       Brief:  AIService
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-18
//============================================================

using System;
using BTCore.Runtime;
using BTCore.Runtime.Externals;
using Newtonsoft.Json;

namespace Examples.ExternalNode
{
    public interface IAIService
    {
        /// <summary>
        /// Crate AIAgent
        /// </summary>
        /// <param name="strategy">对应BT资源json配置数据</param>
        /// <returns></returns>
        IAIAgent CreateAIAgent(string strategy);
    }
    
    public class AIService : IAIService
    {
        private readonly ExternalNodeFactory nodeFactory = new ExternalNodeFactory(); 
        
        public AIService() {
            // 注意：typeName要与编辑器上ExternalNode节点上的TypeName对应上，才能映射替换为对应的外部类型
            // 不想手动的话，也可以用特性来做映射关系，先这么处理吧0.0
            nodeFactory.AddNodeType("MyAction", typeof(MyAction));
            nodeFactory.AddNodeType("MyCondition", typeof(MyCondition));
        }

        private IExternalNode CreateExternalNode(string typeName) => nodeFactory.CrateNode(typeName);
        
        public IAIAgent CreateAIAgent(string strategy) {
            var btData = (BTData) null;
            try {
                // 对于含有外部节点配置的BT数据，先替换外部节点，再重建树的连接关系
                btData = JsonConvert.DeserializeObject<BTData>(strategy, BTDef.SerializerSettingsAuto);
                ReplaceWithExternalNodes(btData);
                btData?.RebuildTree();
            }
            catch (Exception ex) {
                BTLogger.Error($"BT data deserialize failed! \nex: {ex}");
                return null;
            }

            var aiAgent = new AIAgent(btData);
            return aiAgent;
        }

        /// <summary>
        /// 将编辑器中ExternalNode节点需要替换为实际外部工程中的节点类型
        /// </summary>
        /// <param name="btData"></param>
        /// <returns></returns>
        private void ReplaceWithExternalNodes(BTData btData) {
            if (btData == null) {
                return;
            }
            
            var nodes = btData.TreeNodeData.GetNodes();
            for (var i = 0; i < nodes.Count; i++) {
                var node = nodes[i];
                if (node is not IExternalNode externalNode) {
                    continue;
                }
                
                // 创建对应的外部节点
                var newNode = CreateExternalNode(externalNode.TypeName);
                if (newNode == null) {
                    continue;
                }
                    
                newNode.TypeName = externalNode.TypeName;
                newNode.Properties = externalNode.Properties;
                
                if (newNode is not BTNode btNode) {
                    continue;
                }
                
                btNode.Guid = node.Guid;
                btNode.OnInit(btData.Blackboard);
                btData.TreeNodeData.ReplaceNode(i, btNode);
            }
        }
    }
}
