//------------------------------------------------------------
//        File:  NodeInspectorView.cs
//       Brief:  NodeInspectorView
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-02
//============================================================

using System;
using System.Collections.Generic;
using BTCore.Editor.Attributes;
using BTCore.Editor.Inspectors;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace BTCore.Editor
{
    public class NodeInspectorView : VisualElement
    {
        public new class UxmlFactory : UxmlFactory<NodeInspectorView, UxmlTraits> { }
        
        private readonly IMGUIContainer _container;

        private readonly Dictionary<Type, BTNodeInspector> _type2Inspectors = new Dictionary<Type, BTNodeInspector>();

        private readonly Dictionary<BTNodeInspector, UnityEditor.Editor> _inspector2Editors =
            new Dictionary<BTNodeInspector, UnityEditor.Editor>();

        private BTNodeInspector _selectedInspector;
        
        public NodeInspectorView() {
            InitNodeInspectors();

            _container = new IMGUIContainer();
            _container.style.flexGrow = 1;
            Add(_container);
        }

        private void InitNodeInspectors() {
            foreach (var inspectorType in TypeCache.GetTypesDerivedFrom<BTNodeInspector>()) {
                var attr = inspectorType.GetCustomAttribute<BTNodeAttribute>();
                if (attr == null) {
                    continue;
                }

                var inspector = ScriptableObject.CreateInstance(inspectorType) as BTNodeInspector;
                _type2Inspectors.Add(attr.NodeType, inspector);
            }
        }

        public void UpdateSelection(BTNodeView nodeView) {
            ClearSelection();
            
            if (!_type2Inspectors.TryGetValue(nodeView.Node.GetType(), out var inspector)) {
                return;
            }

            _selectedInspector = inspector;
            if (!_inspector2Editors.TryGetValue(inspector, out var editor)) {
                editor = UnityEditor.Editor.CreateEditor(inspector);
                _inspector2Editors.Add(inspector, editor);
            }
            
            // Node Inspector字段须在Reset里面重新进行初始化
            inspector.Reset();
            inspector.ImportData(nodeView.Node);

            _container.onGUIHandler = () => {
                if (editor.target) {
                    editor.OnInspectorGUI();
                }
            };
        }

        public void ClearSelection() {
            if (_container.onGUIHandler == null) {
                return;
            }
            
            _container.onGUIHandler = null;
        }
        
        public void UpdateNodeBindValues() {
            if (_selectedInspector == null) {
                return;
            }
            
            _selectedInspector.OnBlackboardKeyChanged();
        }
    }
}