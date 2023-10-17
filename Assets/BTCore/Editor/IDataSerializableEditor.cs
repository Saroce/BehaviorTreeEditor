//------------------------------------------------------------
//        File:  IDataSerializableEditor.cs
//       Brief:  IDataSerializableEditor
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-03-24
//============================================================

namespace BTCore.Editor
{
    public interface IDataSerializableEditor
    {
    }
    
    public interface IDataSerializableEditor<T> : IDataSerializableEditor
    {
        void ImportData(T data);

        T ExportData();
    }
}