//------------------------------------------------------------
//        File:  MemoryPackDynamicUnionRegister.cs
//       Brief:  MemoryPackDynamicUnionRegister
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2025-11-15
//============================================================

using BTCore.Runtime;
using BTCore.Runtime.Actions;
using BTCore.Runtime.Composites;
using BTCore.Runtime.Conditions;
using BTCore.Runtime.Decorators;
using BTCore.Runtime.Externals;
using BTCore.Runtime.Unity.Conditions;
using Examples.AbortType;
using MemoryPack;
using MemoryPack.Formatters;

namespace Examples
{
    /// <summary>
    /// Note that ModuleInitializer is not supported in Unity, so the formatter must be manually registered.
    /// 注意：抽象类/接口和他们的派生类分布在不同的程序集，为了实现正确的序列化、反序列化，需要手动进行注册，开始运行时调用一次即可
    /// </summary>
    public static class MemoryPackDynamicUnionRegister
    {
        public static void RegisterDynamicUnion() {
            var nodeFormatter = new DynamicUnionFormatter<BTNode>(
                (0, typeof(EntryNode)),
                (1, typeof(Parallel)),
                (2, typeof(Selector)),
                (3, typeof(Sequence)),
                (4, typeof(RandomSequence)),
                (5, typeof(RandomSelector)),
                (6, typeof(Failure)),
                (7, typeof(Inverter)),
                (8, typeof(Repeater)),
                (9, typeof(Success)),
                (10, typeof(UntilSuccess)),
                (11, typeof(RandomProbability)),
                (12, typeof(Log)),
                (13, typeof(Wait)),
                (14, typeof(ActionAttack)),
                (15, typeof(ActionSkill)),
                (16, typeof(ActionWait)),
                (17, typeof(ActionWork)),
                (18, typeof(ConditionHP)),
                (19, typeof(ConditionMP)),
                (20, typeof(IsKeyDown)),
                (21, typeof(ExternalAction)),
                (22, typeof(ExternalCondition))
            );
            
            MemoryPackFormatterProvider.Register(nodeFormatter);
        }
    }
}