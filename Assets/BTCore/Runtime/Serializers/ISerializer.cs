//------------------------------------------------------------
//        File:  ISerializer.cs
//       Brief:  ISerializer
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2025-11-14
//============================================================

namespace BTCore.Runtime.Serializers
{
    public interface ISerializer
    {
        byte[] Serialize<T>(T obj);

        T Deserialize<T>(byte[] bytes);

        void SerializeAndSave<T>(T obj, string path);

        T ReadDataAndDeserialize<T>(string path);
    }
}