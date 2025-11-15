//------------------------------------------------------------
//        File:  ConditionHP.cs
//       Brief:  ConditionHP
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2024-06-21
//============================================================

using BTCore.Runtime.Conditions;
using MemoryPack;

namespace Examples.AbortType
{
    [MemoryPackable]
    public partial class ConditionHP : Condition
    {
        protected override bool Validate() {
            var haveHP = Blackboard.GetValue<int>("HP");
            return haveHP > 100;
        }
    }
}