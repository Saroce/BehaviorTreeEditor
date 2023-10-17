//------------------------------------------------------------
//        File:  BlackboardInspector.cs
//       Brief:  BlackboardInspector
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-09
//============================================================

using System;
using System.Collections;
using System.Collections.Generic;
using BTCore.Runtime.Blackboards;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace BTCore.Editor.Inspectors
{
    public class BlackboardInspector : InspectorBase, IDataSerializableEditor<Blackboard>
    {
        [ShowInInspector]
        [LabelText("Name:")]
        [LabelWidth(100)]
        private string _keyName;
        
        [ShowInInspector]
        [LabelText("Type:")]
        [LabelWidth(100)]
        [ValueDropdown("GetKeyTypeNames")]
        [InlineButton("AddBlackboardKey", "Add")]
        private string _keyTypeName = "Int";

        [ShowInInspector]
        [TableList(AlwaysExpanded = true, HideToolbar = true, ShowIndexLabels = true, ShowPaging = true)]
        [ShowIf("_showKeyInspectors")]
        [OnValueChanged("FieldValueChanged")]
        private List<BKInspector> _keyInspectors = new List<BKInspector>();

        private Blackboard _blackboard;
        private readonly Dictionary<string, Type> _typeName2KeyTypes = new Dictionary<string, Type>();
        private bool _showKeyInspectors => _keyInspectors.Count > 0;

        [HideInInspector]
        public Action OnKeyListChanged;

        public void ImportData(Blackboard data) {
            _blackboard = data;
            _keyInspectors = data.Keys.ConvertAll(keyData => {
                var inspector = BKInspector.Create(keyData.Type);
                inspector.ImportData(keyData);
                return inspector;
            });
        }

        public Blackboard ExportData() {
            return _blackboard;
        }
        
        private void AddBlackboardKey() {
            if (string.IsNullOrEmpty(_keyName)) {
                BTEditorWindow.Instance.ShowNotification("变量名不能为空!");
                return;
            }
            
            var foundKey = _keyInspectors.Find(key => key.KeyName == _keyName);
            if (foundKey != null) {
                BTEditorWindow.Instance.ShowNotification("已存在同名变量!");
                return;
            }

            if (!_typeName2KeyTypes.TryGetValue(_keyTypeName, out var keyType)) {
                return;
            }

            var keyData = BlackboardKey.Create(keyType, _keyName);
            if (keyData != null) {
                _blackboard.Keys.Add(keyData);
                var inspector = BKInspector.Create(keyData.Type);
                inspector.ImportData(keyData);
                _keyInspectors.Add(inspector);
            }
        }

        private IEnumerable GetKeyTypeNames() { 
            if (_typeName2KeyTypes.Count > 0) {
                return _typeName2KeyTypes.Keys;
            }
            
            foreach (var type in TypeCache.GetTypesDerivedFrom<BlackboardKey>()) {
                if (type.IsGenericType) {
                    continue;
                }
                _typeName2KeyTypes.Add(type.Name.Replace("Key", ""), type);
            }
            
            return _typeName2KeyTypes.Keys;;
        }
        
        protected override void FieldValueChanged() {
            _blackboard.Keys = _keyInspectors.ConvertAll(inspector => inspector.ExportData());
            OnKeyListChanged?.Invoke();
        }

        public override void Reset() {
            
        }
    }
}