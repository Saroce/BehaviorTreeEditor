//------------------------------------------------------------
//        File:  MemoryPackSerializer.cs
//       Brief:  MemoryPackSerializer
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2025-11-14
//============================================================

using System;

namespace BTCore.Runtime.Serializers
{
    public class MemoryPackSerializer : ISerializer
    {
        public byte[] Serialize(object obj) {
            return MemoryPack.MemoryPackSerializer.Serialize(obj);
        }

        public T Deserialize<T>(byte[] bytes) {
            try {
                return MemoryPack.MemoryPackSerializer.Deserialize<T>(bytes);
            }
            catch (Exception ex) {
                BTLogger.Error($"MemoryPack deserialize failed! ex: {ex}");
                return default;
            }
        }
    }
}