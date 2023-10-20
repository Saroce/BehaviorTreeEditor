//------------------------------------------------------------
//        File:  Selector.cs
//       Brief:  选择节点：从左到右执行，当有子节点返回Success，选择节点就会立刻返回Success，
//               不会执行下一个子节点，所有子节点都返回Failure，选择节点返回Failure
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-09-30
//============================================================

using System;

namespace BTCore.Runtime.Composites
{
    public class Selector : Composite
    {
        protected override void OnStart() {
            childIndex = 0;
        }

        protected override NodeState OnUpdate() {
            for (var i = childIndex; i < Children.Count; i++) {
                childIndex = i;
                var nodeState = Children[i].Update();
                
                switch (nodeState) {
                    case NodeState.Inactive:
                    case NodeState.Running:
                        return NodeState.Running;
                    case NodeState.Failure:
                        continue;
                    case NodeState.Success:
                        return NodeState.Success;
                    default:
                        throw new ArgumentOutOfRangeException($"Unknown node state:{nodeState}");
                }
            }

            return NodeState.Failure;
        }

        protected override void OnStop() {
            
        }
    }
}
