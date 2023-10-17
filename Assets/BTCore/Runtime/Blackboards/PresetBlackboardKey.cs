//------------------------------------------------------------
//        File:  PresetBlackboardKey.cs
//       Brief:  PresetBlackboardKey
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-10
//============================================================

namespace BTCore.Runtime.Blackboards
{
    public class IntKey : BlackboardKey<int>
    {
        public IntKey(string name) : base(name) {
        }
    }

    public class FloatKey : BlackboardKey<float>
    {
        public FloatKey(string name) : base(name) {
        }
    }

    public class DoubleKey : BlackboardKey<double>
    {
        public DoubleKey(string name) : base(name) {
        }
    }

    public class StringKey : BlackboardKey<string>
    {
        public StringKey(string name) : base(name) {
        }
    }
}