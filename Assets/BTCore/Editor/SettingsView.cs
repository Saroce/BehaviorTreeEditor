//------------------------------------------------------------
//        File:  SettingsView.cs
//       Brief:  SettingsView
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2025-11-15
//============================================================

using System;
using BTCore.Editor.Inspectors;
using BTCore.Runtime;
using UnityEngine;
using UnityEngine.UIElements;

namespace BTCore.Editor
{
    public class SettingsView : VisualElement, IDataSerializable<BTSettings>
    {
        public new class UxmlFactory : UxmlFactory<SettingsView, UxmlTraits> { }

        private readonly IMGUIContainer _container;
        private SettingsInspector _settingsInspector;
        
        public Action OnValueChanged;
        
        public SettingsView() {
            if (BTEditorWindow.Instance == null) {
                return;
            }

            style.backgroundColor = new Color(0.12f, 0.12f, 0.12f, 0.70f);
            
            _container = new IMGUIContainer();
            _container.style.flexGrow = 1;
            Add(_container);

            CreateSettingsInspector();
        }

        private void CreateSettingsInspector() {
            if (_settingsInspector != null) {
                return;
            }

            _settingsInspector = ScriptableObject.CreateInstance<SettingsInspector>();
            var editor = UnityEditor.Editor.CreateEditor(_settingsInspector);
            if (_container != null) {
                _container.onGUIHandler = () => {
                    if (editor.target) {
                        editor.OnInspectorGUI();
                    }
                };
            }
        }


        public void ImportData(BTSettings data) {
            if (_settingsInspector == null) {
                CreateSettingsInspector();
            }

            if (_settingsInspector != null) {
                _settingsInspector.ImportData(data);
            }
            
            _settingsInspector.OnValueChanged ??= OnValueChanged;
        }

        public BTSettings ExportData() {
            return _settingsInspector != null ? _settingsInspector.ExportData() : null;
        }
    }
}