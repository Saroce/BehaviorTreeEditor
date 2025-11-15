//------------------------------------------------------------
//        File:  BehaviorTree.cs
//       Brief:  BehaviorTree
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-01
//============================================================

using System;
using System.Diagnostics;
using BTCore.Runtime.Serializers;
using Examples;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace BTCore.Runtime.Unity
{
    public class BehaviorTree : MonoBehaviour
    {
        [SerializeField]
        private TextAsset _btAsset;
        
        public SerializeType SerializeType = SerializeType.Json;
        
        public BTree BTree { get; private set; }

        private void Start() {
            BTLogger.OnLogReceived += OnLogReceived;
            MemoryPackDynamicUnionRegister.RegisterDynamicUnion();
            CreateBTree();
            BTree?.Enable();
        }

        public void CreateBTree() {
            if (_btAsset == null) {
                Debug.LogError("Please assign bt asset file.");
                return;
            }
            
            try {
                var serializer = SerializerFactory.CreateSerializer(SerializeType);
                BTree = serializer.Deserialize<BTree>(_btAsset.bytes);
                BTree?.RebuildTree();
            }
            catch (Exception e) {
                Debug.LogError($"BT data deserialize failed, please check bt asset file!\n{e}");
            }
        }

        private void Update() {
            if (BTree == null) {
                return;
            }
            
            BTree.Tick();

            // 这是测试BothAbort案例的按键输入，按下Z建直接加蓝200满足ConditionMP条件，按下X建直接抽空蓝量，来模拟ConditionMP条件变化
            if (Input.GetKeyDown(KeyCode.Z)) {
                BTree.Blackboard.SetValue("MP", 200);
            }
            
            if (Input.GetKeyDown(KeyCode.X)) {
                BTree.Blackboard.SetValue("MP", 0);
            }
        }
        
        private void OnLogReceived(string message, BTLogType logType) {
            switch (logType) {
                case BTLogType.Debug:
                    Debug.Log(message);
                    break;
                case BTLogType.Warning:
                    Debug.LogWarning(message);
                    break;
                case BTLogType.Error:
                    Debug.LogError(message);
                    break;
            }
        }
    }
}
