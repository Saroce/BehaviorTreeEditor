//------------------------------------------------------------
//        File:  INode.cs
//       Brief:  INode
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-09-29
//============================================================

namespace BTCore.Runtime
{
    public interface INode
    {
        string Name { get; }
        
        string Guid { get; }
        
        float PosX { get; }
        
        float PosY { get; }
    }
}
