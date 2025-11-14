//------------------------------------------------------------
//        File:  BTSettings.cs
//       Brief:  BTSettings
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-20
//============================================================

using BTCore.Runtime.Serializers;

namespace BTCore.Runtime
{
    // TODO 缺少编辑器设置面板
    public class BTSettings
    {
        public bool RestartWhenComplete { get; set; } = false;
        
        public SerializeType SerializeType = SerializeType.MemoryPack;
    }
}