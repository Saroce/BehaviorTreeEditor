//------------------------------------------------------------
//        File:  BTNodeAttribute.cs
//       Brief:  BTNodeAttribute
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-08
//============================================================

using System;
using BTCore.Runtime;

namespace BTCore.Editor.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class BTNodeAttribute : Attribute
    {
        public Type NodeType { get; }
        
        public BTNodeAttribute(Type nodeType) {
            NodeType = nodeType;
        }
    }
}