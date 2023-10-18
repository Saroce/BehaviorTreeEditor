//------------------------------------------------------------
//        File:  RepeaterInspector.cs
//       Brief:  RepeaterInspector
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-16
//============================================================

using BTCore.Editor.Attributes;
using BTCore.Runtime;
using BTCore.Runtime.Decorators;
using Sirenix.OdinInspector;

namespace BTCore.Editor.Inspectors.Decorators
{
    [BTNode(typeof(Repeater))]
    public class RepeaterInspector : BTNodeInspector
    {
        [ShowInInspector]
        [LabelText("Repeat Count(?)")]
        [LabelWidth(100)]
        [OnValueChanged("OnFieldValueChanged")]
        [PropertyTooltip("循环次数，设定为负数一直循环执行")]
        private int _count;

        private Repeater _repeater;
        
        public override void ImportData(BTNode data) {
            if (data is not Repeater repeater) {
                return;
            }

            _repeater = repeater;
            _count = repeater.RepeatCount;
        }

        public override BTNode ExportData() {
            return _repeater;
        }
        
        protected override void OnFieldValueChanged() {
            if (_repeater == null) {
                return;
            }

            _repeater.RepeatCount = _count;
        }

        public override void Reset() {
            _count = 0;
            _repeater = null;
        }
    }
}