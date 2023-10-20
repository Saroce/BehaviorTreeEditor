//------------------------------------------------------------
//        File:  Failure.cs
//       Brief:  失败节点
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-01
//============================================================

namespace BTCore.Runtime.Decorators
{
    public class Failure : Decorator
    {
        protected override void OnStart() {
        }

        protected override NodeState OnUpdate() {
            if (Child == null) {
                return NodeState.Failure;
            }

            var nodeState = Child.Update();
            return nodeState == NodeState.Success ? NodeState.Failure : nodeState;
        }

        protected override void OnStop() {
            
        }
    }
}
