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
using Newtonsoft.Json;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace BTCore.Runtime.Unity
{
    public class BehaviorTree : MonoBehaviour
    {
        [SerializeField]
        private TextAsset _btAsset;
        
        public BTree BTree { get; private set; }

        private void Start() {
            BTLogger.OnLogReceived += OnLogReceived;
            CreateBTree();
            BTree?.Enable();
        }

        public void CreateBTree() {
            if (_btAsset == null) {
                Debug.LogError("Please assign bt asset file.");
                return;
            }
            
            try {
                var stopWatch = Stopwatch.StartNew();
                BTree = JsonConvert.DeserializeObject<BTree>(_btAsset.text, BTDef.SerializerSettingsAuto);
                stopWatch.Stop();
                Debug.Log($"总共耗时：{stopWatch.Elapsed.TotalMilliseconds}毫秒");
                BTree?.RebuildTree();
            }
            catch (Exception e) {
                Debug.LogError($"BT data deserialize failed, please check bt asset file!\n{e}");
            }
        }

        private void Update() {
            BTree?.Update();
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
