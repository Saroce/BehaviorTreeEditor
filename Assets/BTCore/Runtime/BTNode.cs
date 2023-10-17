//------------------------------------------------------------
//        File:  BTNode.cs
//       Brief:  BTNode
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-09-29
//============================================================

using System;
using System.Reflection;
using BTCore.Runtime.Blackboards;

namespace BTCore.Runtime
{
    public abstract class BTNode : IBTNode
    {
        public string Name { get; set; }
        public string Guid { get; set; }
        public float PosX { get; set; }
        public float PosY { get; set; }
        
        [NonSerialized]
        public NodeState State = NodeState.Inactive;
        
        protected Blackboard Blackboard;
        protected int DeltaTime { private set; get; } // 增量时间(ms)

        private bool _started;
        
        public NodeState Update(int deltaTime) {
            DeltaTime = deltaTime;
            
            if (!_started) {
                OnStart();
                _started = true;
            }
            
            State = OnUpdate();

            if (State != NodeState.Running) {
                OnStop();
                _started = false;
            }

            return State;
        }

        public virtual void OnInit(Blackboard blackboard) {
            Blackboard = blackboard;
            
            // 这里为了方便对Node里面的BindValue统一进行了赋值处理，每个节点只会在初始化调用一次
            // 觉得反射影响运行时性能的，可以覆盖重写, 手动赋值
            foreach (var bindValueFieldInfo in GetType().GetFields()) {
                if (bindValueFieldInfo.FieldType.IsSubclassOf(typeof(BindValue))) {
                    var bindValue = bindValueFieldInfo.GetValue(this) as BindValue;
                    if (bindValue == null) {
                        continue;
                    }

                    var fieldInfo = bindValue.GetType().GetField("Blackboard");
                    fieldInfo?.SetValue(bindValue, blackboard);
                }
            }   
        }
        
        protected abstract void OnStart();
        protected abstract NodeState OnUpdate();
        protected abstract void OnStop();
    }
}
