//------------------------------------------------------------
//        File:  BVInspector.cs
//       Brief:  Bind Value Inspector
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-11
//============================================================

using System.Collections;
using System.Collections.Generic;
using BTCore.Runtime;
using Sirenix.OdinInspector;

namespace BTCore.Editor.Inspectors
{
    public abstract class BVInspector
    {
        public abstract void RebindKeyName();
    }
    
    [HideReferenceObjectPicker]
    [InlineProperty]
    public class BVInspector<T> : BVInspector, IDataSerializableEditor<BindValue<T>>
    {
        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup]
        [ShowIf("_showRawValue")]
        [InlineButton("SelectBlackboardKey", "▼")]
        [OnValueChanged("FieldValueChanged")]
        private T _rawValue;

        [ShowInInspector]
        [HideLabel]
        [HorizontalGroup]
        [ValueDropdown("GetKeyNames")]
        [ShowIf("_showKeyName")]
        [InlineButton("EditRawValue", "↺")]
        [OnValueChanged("FieldValueChanged")]
        private string _bindKeyName = NONE_KEY;
        
        private const string NONE_KEY = "(None)";
        
        private void SelectBlackboardKey() {
            _showRawValue = false;
        }
        
        private void EditRawValue() {
            _showRawValue = true;
            _bindKeyName = NONE_KEY;
        }

        private BindValue<T> _bindValue;
        private bool _showRawValue = true;
        private bool _showKeyName => !_showRawValue;

        private IEnumerable GetKeyNames() {
            var blackboard = BTEditorWindow.Instance.Blackboard;
            if (blackboard == null) {
                return new List<string>() { NONE_KEY };
            }

            var keyNames = new List<string>();
            foreach (var blackboardKey in blackboard.Keys) {
                if (blackboardKey.Type == typeof(T)) {
                    keyNames.Add(blackboardKey.Name);
                }
            }

            return keyNames;
        }

        public void ImportData(BindValue<T> data) {
            _bindValue = data;
            _showRawValue = string.IsNullOrEmpty(data.BindKeyName);
            
            if (_showRawValue) {
                _rawValue = data.RawValue;
            }
            else {
                _bindKeyName = data.BindKeyName;
            }
        }

        public BindValue<T> ExportData() {
            return _bindValue;
        }

        private void FieldValueChanged() {
            _bindValue.RawValue = _rawValue;
            _bindValue.BindKeyName = _bindKeyName == NONE_KEY ? string.Empty : _bindKeyName;
        }

        public override void RebindKeyName() {
            if (_bindKeyName == NONE_KEY) {
                return;
            }
            
            var blackboard = BTEditorWindow.Instance.Blackboard;
            if (blackboard == null) {
                return;
            }

            var isFind = false;
            foreach (var key in blackboard.Keys) {
                if (key.Type == typeof(T) && key.Name == _bindKeyName) {
                    isFind = true;
                }
            }

            _bindKeyName = isFind ? _bindKeyName : NONE_KEY;
        }
    }
}
