//------------------------------------------------------------
//        File:  IExternalNode.cs
//       Brief:  IExternalNode
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-18
//============================================================

using System.Collections.Generic;

namespace BTCore.Runtime.Externals
{
    public interface IExternalNode
    {
        /// <summary>
        /// 对应外部节点类型名称
        /// </summary>
        string TypeName { get; set; }
        /// <summary>
        /// PropertyValue统一用string存储，外部工程直接使用Convert转换为对应需要类型
        /// </summary>
        Dictionary<string, string> Properties { get; set; }
    }
}