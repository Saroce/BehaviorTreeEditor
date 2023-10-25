//------------------------------------------------------------
//        File:  BlackboardKey.cs
//       Brief:  BlackboardKey
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-10
//============================================================

using System;

namespace BTCore.Runtime.Blackboards
{
    public abstract class BlackboardKey
    {
        public string Name { get; }
        public Type Type { get; }

        protected BlackboardKey(string name, Type type) {
            Name = name;
            Type = type;
        }

        public static BlackboardKey Create(Type type, string name) {
            return Activator.CreateInstance(type, name) as BlackboardKey;
        }
    }

    // var blackboardKey = new BlackboardKey<MyClass>(keyName);
    // blackboardKey.Value = classData;
    public class BlackboardKey<T> : BlackboardKey
    {
        public T Value { get; set; }
        
         public BlackboardKey(string name) : base(name, typeof(T)) {
            
        }
    }
}