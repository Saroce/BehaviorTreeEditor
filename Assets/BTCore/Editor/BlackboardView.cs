//------------------------------------------------------------
//        File:  BlackboardView.cs
//       Brief:  BlackboardView
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-08
//============================================================

using System;
using BTCore.Editor.Inspectors;
using BTCore.Runtime.Blackboards;
using UnityEngine;
using UnityEngine.UIElements;

namespace BTCore.Editor
{
    public class BlackboardView : VisualElement, IDataSerializableEditor<Blackboard>
    {
        public new class UxmlFactory : UxmlFactory<BlackboardView, UxmlTraits> { }

        private readonly BlackboardInspector _blackboardInspector;

        public Action OnKeyListChanged;
        
        public BlackboardView() {
            // 打开BTEditorWindow的uxml文件，会初始化BlackboardView，导致Odin部分序列化报错，这里直接判空返回处理
            if (BTEditorWindow.Instance == null) {
                return;
            }
            
            var container = new IMGUIContainer();
            container.style.flexGrow = 1;
            Add(container);

            _blackboardInspector = ScriptableObject.CreateInstance<BlackboardInspector>();

            var editor = UnityEditor.Editor.CreateEditor(_blackboardInspector);
            container.onGUIHandler = () => {
                if (editor.target) {
                    editor.OnInspectorGUI();
                }
            };
        }

        public void ImportData(Blackboard data) {
            if (_blackboardInspector != null) {
                _blackboardInspector.ImportData(data);
            }

            _blackboardInspector.OnKeyListChanged ??= OnKeyListChanged;
        }

        public Blackboard ExportData() {
            return _blackboardInspector != null ? _blackboardInspector.ExportData() : null;
        }
    }
}
