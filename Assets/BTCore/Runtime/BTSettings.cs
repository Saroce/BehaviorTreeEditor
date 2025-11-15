//------------------------------------------------------------
//        File:  BTSettings.cs
//       Brief:  BTSettings
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-20
//============================================================

using BTCore.Runtime.Serializers;
using MemoryPack;

namespace BTCore.Runtime
{
    [MemoryPackable]
    public partial class BTSettings
    {
        public bool RestartWhenComplete { get; set; } = false;
        
        public SerializeType SerializeType = SerializeType.Json;
    }
}