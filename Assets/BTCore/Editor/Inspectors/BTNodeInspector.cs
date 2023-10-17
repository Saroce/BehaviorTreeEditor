//------------------------------------------------------------
//        File:  BTNodeInspector.cs
//       Brief:  BTNodeInspector
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-08
//============================================================

using BTCore.Runtime;
using Sirenix.OdinInspector;

namespace BTCore.Editor.Inspectors
{
    public abstract class BTNodeInspector : InspectorBase, IDataSerializableEditor<BTNode>
    {
        public abstract void ImportData(BTNode data);

        public abstract BTNode ExportData();

        public void OnBlackboardKeyChanged() {
            foreach (var fieldInfo in GetType().GetFields(BTEditorDef.BindValueFlags)) {
                if (fieldInfo.FieldType.IsSubclassOf(typeof(BVInspector))) {
                    var bvInspector = fieldInfo.GetValue(this) as BVInspector;
                    bvInspector?.RebindKeyName();
                }
            }
        }
    }
}