//------------------------------------------------------------
//        File:  BTDef.cs
//       Brief:  BTDef
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-09-29
//============================================================

using Newtonsoft.Json;

namespace BTCore.Runtime
{
    public static class BTDef
    {
        public static readonly JsonSerializerSettings SerializerSettingsAll = new JsonSerializerSettings()
            {TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented};
        
        public static readonly JsonSerializerSettings SerializerSettingsAuto = new JsonSerializerSettings()
            {TypeNameHandling = TypeNameHandling.Auto};
        
        // 默认保存Json配置BT文件名称
        public const string DefaultJsonFileName = "NewBT.json";
        // 对应BT数据序列化保存Json文件后缀
        public const string JsonDataExt = ".json";
        // 默认保存MemoryPack配置BT文件名称
        public const string DefaultMemoryPackFileName = "NewBT.bytes";
        // 对应BT数据序列化保存MemoryPack文件后缀
        public const string MemoryPackDataExt = ".bytes";
    }
    
    public enum NodeState
    {
        Inactive,
        Running,
        Success,
        Failure
    }
    
    public enum BTLogType
    {
        Debug,
        Warning,
        Error
    }

    public enum AbortType
    {
        None,
        Self,
        LowerPriority,
        Both
    }
}
