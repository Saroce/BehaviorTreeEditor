//------------------------------------------------------------
//        File:  Wait.cs
//       Brief:  等待节点
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-09-29
//============================================================

using System;

namespace BTCore.Runtime.Actions
{
    public class Wait : Action
    {
        public int Duration = 1000;    // 等待时长(单位ms)
        
        private int _elapsedTime;
        
        protected override void OnStart() {
            _elapsedTime = 0;
        }

        protected override NodeState OnUpdate() {
            _elapsedTime += DeltaTime;
            return _elapsedTime > Duration ? NodeState.Success : NodeState.Running;
        }

        protected override void OnStop() {
            
        }
    }
}
