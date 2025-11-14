//------------------------------------------------------------
//        File:  JsonSerializer.cs
//       Brief:  JsonSerializer
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2025-11-14
//============================================================

using System;
using System.Text;
using Newtonsoft.Json;

namespace BTCore.Runtime.Serializers
{
    public class JsonSerializer : ISerializer
    {
        public byte[] Serialize(object obj) {
            var json = JsonConvert.SerializeObject(obj, BTDef.SerializerSettingsAll);
            return Encoding.UTF8.GetBytes(json);
        }

        public T Deserialize<T>(byte[] bytes) {
            var json = Encoding.UTF8.GetString(bytes);
            if (string.IsNullOrEmpty(json)) {
                return default;
            }

            try {
                return JsonConvert.DeserializeObject<T>(json, BTDef.SerializerSettingsAuto);
            }
            catch (Exception ex) {
                BTLogger.Error($"Json deserialize failed! ex: {ex}");
                return default;
            }
        }
    }
}