//------------------------------------------------------------
//        File:  WaitInspector.cs
//       Brief:  WaitInspector
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
    [BTNode(typeof(Wait))]
    public class WaitInspector : BTNodeInspector
    {
        [ShowInInspector]
        [LabelText("Duration(ms)")]
        [LabelWidth(100)]
        [OnValueChanged("FieldValueChanged")]
        private int _duration;
        
        private Wait _waitData;

        public override void ImportData(BTNode data) {
            if (data is not Wait waitData) {
                return;
            }

            _waitData = waitData;
            _duration = waitData.Duration;
        }

        public override BTNode ExportData() {
            return _waitData;
        }

        protected override void FieldValueChanged() {
            if (_waitData == null) {
                return;
            }
            
            _waitData.Duration = _duration;
        }

        public override void Reset() {
            _duration = 0;
            _waitData = null;
        }
    }
}