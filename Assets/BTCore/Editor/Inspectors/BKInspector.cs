//------------------------------------------------------------
//        File:  BKInspector.cs
//       Brief:  Blackboard Key Inspector
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-14
//============================================================

using System;
using System.Reflection;
using BTCore.Runtime.Blackboards;
using Sirenix.OdinInspector;

namespace BTCore.Editor.Inspectors
{
    public abstract class BKInspector : IDataSerializableEditor<BlackboardKey>
    {
        [HorizontalGroup("Variable List")]
        [HideLabel]
        [DisplayAsString]
        public string KeyName;
        
        public abstract void ImportData(BlackboardKey data);
        
        public abstract BlackboardKey ExportData();

        public static BKInspector Create(Type keyType) {
            var genericType = typeof(BKInspector<>).MakeGenericType(keyType);
            return Activator.CreateInstance(genericType) as BKInspector;
        }
    }
    
    public class BKInspector<T> : BKInspector
    {
        [ShowInInspector]
        [HorizontalGroup("Variable List")]
        [HideLabel]
        [OnValueChanged("FieldValueChanged")]
        private  T _value;

        private BlackboardKey _blackboardKey;
        private FieldInfo _fieldInfo;

        public override void ImportData(BlackboardKey data) {
            if (data == null) {
                return;
            }
            
            _blackboardKey = data;
            KeyName = _blackboardKey.Name;
            _fieldInfo = data.GetType().GetField("Value");
            _value = (T) _fieldInfo.GetValue(data);
        }

        public override BlackboardKey ExportData() {
            return _blackboardKey;
        }

        protected void FieldValueChanged() {
            if (_blackboardKey == null || _fieldInfo == null) {
                return;
            }
            
            _fieldInfo.SetValue(_blackboardKey, _value);
        }

        public void Clear() {
            KeyName = null;
            _value = default;
            _blackboardKey = null;
        }
    }
}