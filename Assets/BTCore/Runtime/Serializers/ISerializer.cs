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
        byte[] Serialize(object obj);

        T Deserialize<T>(byte[] bytes);
    }
}