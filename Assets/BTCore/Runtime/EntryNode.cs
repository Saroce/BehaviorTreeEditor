﻿//------------------------------------------------------------
//        File:  EntryNode.cs
//       Brief:  行为树入口节点
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-01
//============================================================

using System;

namespace BTCore.Runtime
{
    public class EntryNode : BTNode
    {
        private BTNode _child;

        public string ChildGuid;

        public void SetChild(BTNode node) {
            _child = node;
        }

        public BTNode GetChild() => _child;
        
        protected override void OnStart() {
            
        }

        protected override NodeState OnUpdate() {
            return _child?.Update(DeltaTime) ?? NodeState.Failure;
        }

        protected override void OnStop() {
            
        }
    }
}
