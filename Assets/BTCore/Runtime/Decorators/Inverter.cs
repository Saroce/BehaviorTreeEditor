//------------------------------------------------------------
//        File:  Inverter.cs
//       Brief:  反转节点
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-01
//============================================================

using System;

namespace BTCore.Runtime.Decorators
{
    public class Inverter : Decorator
    {
        protected override void OnStart() {
        }

        protected override NodeState OnUpdate() {
            if (Child == null) {
                return NodeState.Failure;
            }

            var nodeState = Child.Update();
            switch (nodeState) {
                case NodeState.Inactive:
                case NodeState.Running:
                    return nodeState;
                case NodeState.Failure:
                    return NodeState.Success;
                case NodeState.Success:
                    return NodeState.Failure;
                default:
                    throw new ArgumentOutOfRangeException($"Unknown node state:{nodeState}");
            }
        }

        protected override void OnStop() {
        }
    }
}
