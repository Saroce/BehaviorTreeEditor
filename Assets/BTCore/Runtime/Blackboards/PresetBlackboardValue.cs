//------------------------------------------------------------
//        File:  PresetBlackboardValue.cs
//       Brief:  PresetBlackboardValue
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-10
//============================================================

using MemoryPack;

namespace BTCore.Runtime.Blackboards
{
    [MemoryPackable]
    public partial class IntValue : BlackboardValue<int>
    {
        public IntValue(string name) : base(name) {
        }
    }

    [MemoryPackable]
    public partial class FloatValue : BlackboardValue<float>
    {
        public FloatValue(string name) : base(name) {
        }
    }

    [MemoryPackable]
    public partial class DoubleValue : BlackboardValue<double>
    {
        public DoubleValue(string name) : base(name) {
        }
    }

    [MemoryPackable]
    public partial class StringValue : BlackboardValue<string>
    {
        public StringValue(string name) : base(name) {
        }
    }
}