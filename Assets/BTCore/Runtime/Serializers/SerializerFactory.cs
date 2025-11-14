//------------------------------------------------------------
//        File:  SerializerFactory.cs
//       Brief:  SerializerFactory
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2025-11-14
//============================================================

using System;

namespace BTCore.Runtime.Serializers
{
    public static class SerializerFactory
    {
        public static ISerializer CreateSerializer(SerializeType serializeType) {
            return serializeType switch {
                SerializeType.Json => new JsonSerializer(),
                SerializeType.MemoryPack => new MemoryPackSerializer(),
                _ => throw new ArgumentOutOfRangeException(nameof(serializeType), serializeType, null)
            };
        }
    }
}