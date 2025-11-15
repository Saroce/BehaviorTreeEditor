//------------------------------------------------------------
//        File:  UntilSuccess.cs
//       Brief:  UntilSuccess
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-25
//============================================================

using MemoryPack;

namespace BTCore.Runtime.Decorators
{
    [MemoryPackable]
    public partial class UntilSuccess : Decorator
    {
        protected override void OnStop() {
            
        }

        public override void OnChildExecute(int childIndex, NodeState nodeState) {
            State = nodeState;
        }

        public override bool CanExecute() {
            return State != NodeState.Success;
        }
    }
}
