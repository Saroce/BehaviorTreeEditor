//------------------------------------------------------------
//        File:  JsonSerializer.cs
//       Brief:  JsonSerializer
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2025-11-14
//============================================================

using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;

namespace BTCore.Runtime.Serializers
{
    public class JsonSerializer : ISerializer
    {
        public byte[] Serialize<T>(T obj) {
            var json = JsonConvert.SerializeObject(obj, BTDef.SerializerSettingsAll);
            return Encoding.UTF8.GetBytes(json);
        }

        public T Deserialize<T>(byte[] bytes) {
            var json = Encoding.UTF8.GetString(bytes);
            return string.IsNullOrEmpty(json) ? default : JsonConvert.DeserializeObject<T>(json, BTDef.SerializerSettingsAuto);
        }

        public void SerializeAndSave<T>(T obj, string path) {
            var json = JsonConvert.SerializeObject(obj, BTDef.SerializerSettingsAll);
            File.WriteAllText(path, json);
        }

        public T ReadDataAndDeserialize<T>(string path) {
            var json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<T>(json, BTDef.SerializerSettingsAuto);
        }
    }
}