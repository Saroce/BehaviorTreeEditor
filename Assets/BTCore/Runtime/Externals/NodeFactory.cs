//------------------------------------------------------------
//        File:  NodeFactory.cs
//       Brief:  NodeFactory
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-18
//============================================================

using System;
using System.Collections.Generic;

namespace BTCore.Runtime.Externals
{
    public class NodeFactory
    {
        private readonly Dictionary<string, Type> _name2Types = new Dictionary<string, Type>();

        public void AddNodeMap(string typeName, Type type) {
            if (_name2Types.ContainsKey(typeName)) {
                return;
            }
            
            _name2Types.Add(typeName, type);
        }

        public BTNode CrateNode(string name) {
            if (!_name2Types.ContainsKey(name)) {
                return null;
            }

            var type = _name2Types[name];
            return Activator.CreateInstance(type) as BTNode;
        }
    }
}