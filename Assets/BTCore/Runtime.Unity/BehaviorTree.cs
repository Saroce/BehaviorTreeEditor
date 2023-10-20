//------------------------------------------------------------
//        File:  BehaviorTree.cs
//       Brief:  BehaviorTree
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-01
//============================================================

using System;
using Newtonsoft.Json;
using UnityEngine;

namespace BTCore.Runtime.Unity
{
    public class BehaviorTree : MonoBehaviour
    {
        [SerializeField]
        private TextAsset _btAsset;
        
        public BTData BTData { get; private set; }

        private void Start() {
            Application.targetFrameRate = 60;
            BTLogger.OnLogReceived += OnLogReceived;

            try {
                BTData = JsonConvert.DeserializeObject<BTData>(_btAsset.text, BTDef.SerializerSettingsAuto);
                BTData?.RebuildTree();
            }
            catch (Exception e) {
                Debug.LogError($"BT data deserialize failed, please check bt asset file!\n{e}");
            }
        }

        private void Update() {
            BTData?.Update();
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
