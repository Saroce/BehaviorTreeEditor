//------------------------------------------------------------
//        File:  ActionWait.cs
//       Brief:  ActionWait
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2024-06-21
//============================================================

using BTCore.Runtime;
using BTCore.Runtime.Actions;
using MemoryPack;
using UnityEngine;

namespace Examples.AbortType
{
    [MemoryPackable]
    public partial class ActionWait : Action
    {
        private float _startTime;
        private float _duration = 2000;
        
        protected override void OnStart() {
            base.OnStart();
            Debug.Log("==============施法前摇开始=============");
            _startTime = Time.time;
            
        }

        protected override NodeState OnUpdate() {
            var elapseTime = (Time.time - _startTime) * 1000;
            return elapseTime > _duration ? NodeState.Success : NodeState.Running;
        }
         
        protected override void OnStop() {
            Debug.Log("==============施法前摇结束=============");
            // State = NodeState.Inactive;
        }
    }
}