//------------------------------------------------------------
//        File:  InspectorBase.cs
//       Brief:  InspectorBase
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-09
//============================================================

using Sirenix.OdinInspector;

namespace BTCore.Editor.Inspectors
{
    public abstract class InspectorBase : SerializedScriptableObject
    {
        protected abstract void FieldValueChanged();
        
        public abstract void Reset();
    }
}