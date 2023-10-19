//------------------------------------------------------------
//        File:  ExternalNodeFactory.cs
//       Brief:  ExternalNodeFactory
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-18
//============================================================

using System;
using System.Collections.Generic;
using BTCore.Runtime.Externals;

namespace Examples.ExternalNode
{
    public class ExternalNodeFactory
    {
        private readonly Dictionary<string, Type> _name2Types = new Dictionary<string, Type>();

        public void AddNodeType(string typeName, Type type) {
            if (_name2Types.ContainsKey(typeName)) {
                return;
            }
            
            _name2Types.Add(typeName, type);
        }

        public IExternalNode CrateNode(string name) {
            if (!_name2Types.ContainsKey(name)) {
                return null;
            }

            var type = _name2Types[name];
            return Activator.CreateInstance(type) as IExternalNode;
        }
    }
}