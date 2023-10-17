//------------------------------------------------------------
//        File:  BTEx.cs
//       Brief:  BTEx
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-05
//============================================================

using System.Collections.Generic;
using BTCore.Runtime.Composites;
using BTCore.Runtime.Decorators;

namespace BTCore.Runtime
{
    public static class BTEx
    {
        public static List<BTNode> GetChildren(this BTNode parent) {
            var children = new List<BTNode>();
            switch (parent) {
                case Composite composite: {
                    children.AddRange(composite.Children);
                    break;
                }
                case Decorator decorator: {
                    if (decorator.Child != null) {
                        children.Add(decorator.Child);   
                    }
                    break;
                }
                case EntryNode entryNode: {
                    if (entryNode.Child != null) {
                        children.Add(entryNode.Child);   
                    }
                    break;
                }
            }

            return children;
        }
    }
}
