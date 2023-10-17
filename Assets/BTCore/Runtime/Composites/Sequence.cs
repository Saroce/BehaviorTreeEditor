//------------------------------------------------------------
//        File:  Sequence.cs
//       Brief:  顺序节点: 从左到右挨个顺序执行，当所有子节点都返回Success时，它才返回Success。
//              当某个子节点返回Failure时，顺序节点就会立刻返回Failure
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-09-29
//============================================================

using System;

namespace BTCore.Runtime.Composites
{
    public class Sequence : Composite
    {
        protected override void OnStart() {
            childIndex = 0;
        }

        protected override NodeState OnUpdate() {
            for (var i = childIndex; i < Children.Count; i++) {
                childIndex = i;
                var nodeState = Children[i].Update(DeltaTime);
                
                switch (nodeState) {
                    case NodeState.Inactive:
                    case NodeState.Running:
                        return NodeState.Running;
                    case NodeState.Failure:
                        return NodeState.Failure;
                    case NodeState.Success:
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException($"Unknown node state:{nodeState}");
                }
            }
            
            return NodeState.Success;
        }

        protected override void OnStop() {
        }
    }
}
