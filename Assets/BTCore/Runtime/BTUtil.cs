//------------------------------------------------------------
//        File:  BTUtil.cs
//       Brief:  BTUtil
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-11
//============================================================

using System;
using System.Collections.Generic;
using BTCore.Runtime.Composites;
using BTCore.Runtime.Conditions;
using BTCore.Runtime.Decorators;

namespace BTCore.Runtime
{
    public static class BTUtil
    {
        public static void TravelNode(BTNode root, Action<BTNode> traveller) {
            if (root == null) {
                return;
            }
            
            traveller(root);
            root.GetChildren().ForEach(child => TravelNode(child, traveller));
        }

        public static List<BTNode> GetChildren(this BTNode parent) {
            var children = new List<BTNode>();
            if (parent == null) {
                return children;
            }

            switch (parent) {
                case EntryNode entryNode: {
                    var child = entryNode.GetChild();
                    if (child != null) {
                        children.Add(child);
                    }
                    break;
                }
                case Composite composite: {
                    children.AddRange(composite.GetChildren());
                    break;
                }
                case Decorator decorator: {
                    var child = decorator.GetChild();
                    if (child != null) {
                        children.Add(child);
                    }
                    break;
                }
            }

            return children;
        }
    }
}