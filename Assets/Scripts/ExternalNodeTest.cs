//------------------------------------------------------------
//        File:  ExternalNodeTest.cs
//       Brief:  外部节点类型使用测试
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-18
//============================================================

using BTCore.Runtime;
using BTCore.Runtime.Externals.Example;
using UnityEngine;

public class ExternalNodeTest : MonoBehaviour
{
    [SerializeField]
    private TextAsset _btAsset;
    
    private IAIAgent _aiAgent;
    private readonly IAIService _aiService = new AIService();
    
    private void Start() {
        BTLogger.OnLogReceived += OnLogReceived;
        _aiAgent = _aiService.CreateAIAgent(_btAsset.text);
    }

    private void Update() {
        _aiAgent?.Tick((int) (Time.deltaTime * 1000));
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