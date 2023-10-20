//------------------------------------------------------------
//        File:  Parallel.cs
//       Brief:  并行节点：并行执行所有的子节点，所有的子节点都返回Success，并行节点才会返回Success。
//               只要有一个子节点返回Failure，并行节点就会立刻返回Failure
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-09-30
//============================================================

using System;
using System.Collections.Generic;

namespace BTCore.Runtime.Composites
{
    public class Parallel : Composite
    {
        private readonly List<NodeState> _childrenState = new List<NodeState>();

        protected override void OnStart() {
            childIndex = 0;
            _childrenState.Clear();
            Children.ForEach(_ => _childrenState.Add(NodeState.Inactive));
        }

        protected override NodeState OnUpdate() {
            var childrenComplete = true;
            for (var i = 0; i < _childrenState.Count; i++) {
                var nodeState = _childrenState[i];
                
                switch (nodeState) {
                    case NodeState.Inactive:
                    case NodeState.Running: {
                        _childrenState[i] = Children[i].Update();
                        childrenComplete = false;
                        break;
                    }
                    case NodeState.Failure: {
                        AbortRunningChildren();
                        return NodeState.Failure;
                    }
                    case NodeState.Success:
                        continue;
                    default:
                        throw new ArgumentOutOfRangeException($"Unknown node state:{nodeState}");
                }
            }

            return childrenComplete ? NodeState.Success : NodeState.Running;
        }

        protected override void OnStop() {
            
        }

        private void AbortRunningChildren() {
            for (var i = 0; i < _childrenState.Count; i++) {
                if (_childrenState[i] != NodeState.Running) {
                    continue;
                }
                
                Children[i].Abort();
            }
        }
    }
}
