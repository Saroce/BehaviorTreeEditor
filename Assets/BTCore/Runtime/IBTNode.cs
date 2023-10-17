//------------------------------------------------------------
//        File:  IBTNode.cs
//       Brief:  IBTNode
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-09-29
//============================================================

namespace BTCore.Runtime
{
    public interface IBTNode : INode
    {
        NodeState Update(int deltaTime);
    }
}
