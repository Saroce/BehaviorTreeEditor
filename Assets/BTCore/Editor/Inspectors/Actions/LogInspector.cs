//------------------------------------------------------------
//        File:  LogInspector.cs
//       Brief:  LogInspector
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-08
//============================================================

using BTCore.Editor.Attributes;
using BTCore.Runtime;
using BTCore.Runtime.Actions;
using Sirenix.OdinInspector;

namespace BTCore.Editor.Inspectors.Actions
{
    [BTNode(typeof(Log))]
    public class LogInspector : BTNodeInspector
    {
        [ShowInInspector]
        [LabelText("Log")]
        [LabelWidth(100)]
        [OnValueChanged("FieldValueChanged")]
        private BVInspector<string> _message = new BVInspector<string>();
        
        private Log _logData;

        public override void ImportData(BTNode data) {
            if (data is not Log logData) {
                return;
            }

            _logData = logData;
            _message.ImportData(logData.Message);
        }

        public override BTNode ExportData() {
            return _logData;
        }

        protected override void FieldValueChanged() {
            if (_logData == null) {
                return;
            }

            _logData.Message = _message.ExportData();
        }

        public override void Reset() {
            _message = new BVInspector<string>();
            _logData = null;
        }
    }
}