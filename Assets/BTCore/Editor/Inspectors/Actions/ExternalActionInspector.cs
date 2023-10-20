//------------------------------------------------------------
//        File:  ExternalActionInspector.cs
//       Brief:  ExternalActionInspector
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-18
//============================================================

using BTCore.Editor.Attributes;
using BTCore.Runtime.Externals;
using Sirenix.OdinInspector;

namespace BTCore.Editor.Inspectors.Actions
{
    [BTNode(typeof(ExternalAction))]
    public class ExternalActionInspector : ExternalNodeInspector
    {
        // [Title("External Action")]
        // [DisplayAsString]
        // private string _label = string.Empty;
        
        protected override string GetInfoMessage() {
            return "必须填写外部Action类型名称!";
        }

        protected override string GetTitleName() {
            return "External Action";
        }
    }
}