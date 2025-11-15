//------------------------------------------------------------
//        File:  BlackboardValue.cs
//       Brief:  BlackboardValue
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-10
//============================================================

using System;
using MemoryPack;

namespace BTCore.Runtime.Blackboards
{
    /// <summary>
    /// 若有扩展非预设类型黑板变量，可参考BTNode手动进行注册
    /// </summary>
    [MemoryPackable]
    [MemoryPackUnion(0, typeof(IntValue))]
    [MemoryPackUnion(1, typeof(FloatValue))]
    [MemoryPackUnion(2, typeof(DoubleValue))]
    [MemoryPackUnion(3, typeof(StringValue))]
    public abstract partial class BlackboardValue
    {
        /// <summary>
        /// 对应黑板变量的名称(唯一)
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 对应黑板变量的类型
        /// </summary>
        public Type Type { get; }

        protected BlackboardValue(string name, Type type) {
            Name = name;
            Type = type;
        }

        public static BlackboardValue Create(Type type, string name) {
            return Activator.CreateInstance(type, name) as BlackboardValue;
        }
    }
    
    [MemoryPackable]
    public partial class BlackboardValue<T> : BlackboardValue
    {
        public T Value { get; set; }
        
         public BlackboardValue(string name) : base(name, typeof(T)) {
            
        }
    }
}