//------------------------------------------------------------
//        File:  Composite.cs
//       Brief:  复合节点基类
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-09-29
//============================================================

using System;
using System.Collections.Generic;

namespace BTCore.Runtime.Composites
{
    public abstract class Composite : BTNode
    {
        protected int childIndex;
        
        protected List<BTNode> Children = new List<BTNode>();

        public List<string> ChildrenGuids { get; set; } = new List<string>();

        public void AddChild(BTNode node) {
            if (node == null) {
                return;
            }
            
            Children.Add(node);
        }

        public List<BTNode> GetChildren() => Children;
    }
}
