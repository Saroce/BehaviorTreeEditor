//------------------------------------------------------------
//        File:  ProbabilityInspector.cs
//       Brief:  ProbabilityInspector
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-16
//============================================================

using BTCore.Editor.Attributes;
using BTCore.Runtime;
using BTCore.Runtime.Conditions;
using Sirenix.OdinInspector;

namespace BTCore.Editor.Inspectors.Conditions
{
    [BTNode(typeof(RandomProbability))]
    public class RandomProbabilityInspector : BTNodeInspector
    {
        [ShowInInspector]
        [LabelText("Probability(?)")]
        [LabelWidth(100)]
        [OnValueChanged("OnFieldValueChanged")]
        [PropertyTooltip("概率范围0 ~ 100")]
        private BVInspector<int> _probability = new BVInspector<int>();

        private RandomProbability _probabilityData;
        
        public override void ImportData(BTNode data) {
            if (data is not RandomProbability probability) {
                return;
            }

            _probabilityData = probability;
            _probability.ImportData(_probabilityData.Probability);
        }

        public override BTNode ExportData() {
            return _probabilityData;
        }
        
        protected override void OnFieldValueChanged() {
            if (_probabilityData == null) {
                return;
            }

            _probabilityData.Probability = _probability.ExportData();
        }

        public override void Reset() {
            _probability = new BVInspector<int>();
            _probabilityData = null;
        }
    }
}