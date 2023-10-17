//------------------------------------------------------------
//        File:  Blackboard.cs
//       Brief:  Blackboard
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-09
//============================================================

using System.Collections.Generic;

namespace BTCore.Runtime.Blackboards
{
    public class Blackboard
    {
        public List<BlackboardKey> Keys = new List<BlackboardKey>();

        public BlackboardKey<T> Find<T>(string keyName) {
            var foundKey = Keys.Find(key => key.Name == keyName);

            if (foundKey == null) {
                BTLogger.Error($"Find blackboard key failed, keyName: {keyName}");
                return null;
            }

            if (foundKey is not BlackboardKey<T> blackboardKey) {
                BTLogger.Error($"Find blackboard key failed, expected: {typeof(T)} got: {foundKey.Type}");
                return null;
            }

            return blackboardKey;
        }

        public T GetValue<T>(string keyName) {
            var blackboardKey = Find<T>(keyName);
            return blackboardKey != null ? blackboardKey.Value : default;
        }

        public void SetValue<T>(string keyName, T Value) {
            var blackboardKey = Find<T>(keyName);
            if (blackboardKey != null) {
                blackboardKey.Value = Value;
            }
        }
    }
}