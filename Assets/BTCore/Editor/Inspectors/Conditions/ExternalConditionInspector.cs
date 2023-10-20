//------------------------------------------------------------
//        File:  ExternalCondition.cs
//       Brief:  ExternalCondition
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-18
//============================================================

using BTCore.Editor.Attributes;
using BTCore.Runtime.Externals;

namespace BTCore.Editor.Inspectors.Conditions
{
    [BTNode(typeof(ExternalCondition))]
    public class ExternalConditionInspector : ExternalNodeInspector
    {
        protected override string GetInfoMessage() {
            return "必须填写外部Condition类型名称!";
        }

        protected override string GetTitleName() {
            return "External Condition";
        }
    }
}