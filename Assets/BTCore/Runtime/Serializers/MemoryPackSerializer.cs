//------------------------------------------------------------
//        File:  MemoryPackSerializer.cs
//       Brief:  MemoryPackSerializer
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2025-11-14
//============================================================

using System.IO;

namespace BTCore.Runtime.Serializers
{
    public class MemoryPackSerializer : ISerializer
    {
        public byte[] Serialize<T>(T obj) {
            return MemoryPack.MemoryPackSerializer.Serialize(obj);
        }

        public T Deserialize<T>(byte[] bytes) {
            return MemoryPack.MemoryPackSerializer.Deserialize<T>(bytes);
        }

        public void SerializeAndSave<T>(T obj, string path) {
            var bytes = MemoryPack.MemoryPackSerializer.Serialize(obj);
            File.WriteAllBytes(path, bytes);
        }

        public T ReadDataAndDeserialize<T>(string path) {
            var bytes = File.ReadAllBytes(path);
            return MemoryPack.MemoryPackSerializer.Deserialize<T>(bytes);
        }
    }
}