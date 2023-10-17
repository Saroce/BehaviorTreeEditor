//------------------------------------------------------------
//        File:  BTUtil.cs
//       Brief:  BTUtil
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-11
//============================================================

using System.Collections.Generic;
using BTCore.Runtime.Composites;
using BTCore.Runtime.Decorators;

namespace BTCore.Runtime
{
    public static class BTUtil
    {
        /// <summary>
        /// 给定行为树的根节点，获取对应的所有节点
        /// </summary>
        /// <param name="rootNode"></param>
        /// <returns></returns>
        public static List<BTNode> GetTreeNodes(BTNode rootNode) {
            var nodes = new List<BTNode>();
            if (rootNode == null) {
                return nodes;
            }

            nodes.Add(rootNode);

            switch (rootNode) {
                case EntryNode entryNode:
                    nodes.AddRange(GetTreeNodes(entryNode.Child));
                    break;
                case Composite composite:
                    foreach (var child in composite.Children) {
                        nodes.AddRange(GetTreeNodes(child));
                    }
                    break;
                case Decorator decorator:
                    nodes.AddRange(GetTreeNodes(decorator.Child));
                    break;
            }

            return nodes;
        }
    }
}