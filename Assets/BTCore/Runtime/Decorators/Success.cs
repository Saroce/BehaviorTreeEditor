//------------------------------------------------------------
//        File:  Success.cs
//       Brief:  成功节点
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-01
//============================================================

namespace BTCore.Runtime.Decorators
{
    public class Success : Decorator
    {
        protected override void OnStart() {
        }

        protected override NodeState OnUpdate() {
            if (Child == null) {
                return NodeState.Failure;
            }

            var nodeState = Child.Update(DeltaTime);
            return nodeState == NodeState.Failure ? NodeState.Success : nodeState;
        }

        protected override void OnStop() {
        }
    }
}
