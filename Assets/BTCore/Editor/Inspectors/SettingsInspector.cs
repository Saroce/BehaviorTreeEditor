//------------------------------------------------------------
//        File:  SettingsInspector.cs
//       Brief:  SettingsInspector
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2025-11-15
//============================================================

using System;
using BTCore.Runtime;
using BTCore.Runtime.Serializers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace BTCore.Editor.Inspectors
{
    [HideLabel]
    [HideReferenceObjectPicker]
    public class SettingsInspector : InspectorBase, IDataSerializable<BTSettings>
    {
        [ShowInInspector]
        [LabelText("RestartWhenComplete:")]
        [LabelWidth(150)]
        [OnValueChanged("OnFieldValueChanged")]
        private bool _restartWhenComplete;
        
        [ShowInInspector]
        [LabelText("SerializeType:")]
        [LabelWidth(150)]
        [OnValueChanged("OnFieldValueChanged")]
        private SerializeType _serializeType;

        private BTSettings _settings;
        
        [HideInInspector]
        public Action OnValueChanged;
        
        protected override void OnFieldValueChanged() {
            _settings.RestartWhenComplete = _restartWhenComplete;
            _settings.SerializeType = _serializeType;
            OnValueChanged?.Invoke();
        }

        public override void Reset() {
            _restartWhenComplete = false;
            _serializeType = SerializeType.Json;
        }

        public void ImportData(BTSettings data) {
            _settings = data;
            _restartWhenComplete = _settings.RestartWhenComplete;
            _serializeType = _settings.SerializeType;
        }

        public BTSettings ExportData() {
            return _settings;
        }
    }
}