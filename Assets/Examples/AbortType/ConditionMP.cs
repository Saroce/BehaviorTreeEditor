//------------------------------------------------------------
//        File:  ConditionMP.cs
//       Brief:  ConditionMP
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
    public partial class ConditionMP : Condition
    {
        protected override bool Validate() {
            var haveMP = Blackboard.GetValue<int>("MP");
            return haveMP > 100;
        }
    }
}