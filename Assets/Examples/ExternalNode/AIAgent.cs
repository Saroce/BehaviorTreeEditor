﻿//------------------------------------------------------------
//        File:  AIAgent.cs
//       Brief:  AIAgent
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-18
//============================================================

using BTCore.Runtime;

namespace Examples.ExternalNode
{
    public interface IAIAgent
    {
        void Tick(int deltaTime);
    }
    
    public class AIAgent : IAIAgent
    {
        private readonly BTData _btData;
        
        public AIAgent(BTData btData) {
            _btData = btData;
        }
        
        public void Tick(int deltaTime) {
            _btData?.Update(deltaTime);
        }
    }
}
