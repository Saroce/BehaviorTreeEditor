//------------------------------------------------------------
//        File:  Repeater.cs
//       Brief:  重复节点
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-01
//============================================================

namespace BTCore.Runtime.Decorators
{
    public class Repeater : Decorator
    {
        public int RepeatCount = 1; // 设定为负数，一直循环执行
        
        private int _counter = 0;
        
        protected override void OnStart() {
            _counter = 0;
        }

        protected override NodeState OnUpdate() {
            if (Child == null) {
                return NodeState.Failure;
            }
            
            if (RepeatCount < 0) {
                Child.Update();
                return NodeState.Running;
            }

            if (_counter >= RepeatCount) {
                return NodeState.Success;
            }

            _counter++;
            var nodeState = Child.Update();
            return nodeState != NodeState.Success ? nodeState : NodeState.Running;
        }

        protected override void OnStop() {
        }
    }
}
