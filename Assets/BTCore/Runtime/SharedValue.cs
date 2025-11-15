//------------------------------------------------------------
//        File:  SharedValue.cs
//       Brief:  SharedValue
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-11
//============================================================

using BTCore.Runtime.Blackboards;
using MemoryPack;

namespace BTCore.Runtime
{
    public abstract partial class SharedValue
    {
        protected Blackboard Blackboard { get; set; }
    }
    
    [MemoryPackable]
    public partial class SharedValue<T> : SharedValue
    {
        public string ValueName { get; set; }
        public T RawValue { get; set; }

        public T Value {
            get {
                if (string.IsNullOrEmpty(ValueName) || Blackboard == null) {
                    return RawValue;
                }
                
                return Blackboard.GetValue<T>(ValueName);
            }
        }
    }
}
