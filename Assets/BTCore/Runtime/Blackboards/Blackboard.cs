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
        public List<BlackboardKey> Keys { get; set; } = new List<BlackboardKey>();

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

        public void SetValue<T>(string keyName, T value) {
            var foundKey = Find<T>(keyName);
            if (foundKey != null) {
                foundKey.Value = value;
                return;
            }

            var blackboardKey = new BlackboardKey<T>(keyName);
            blackboardKey.Value = value;
            Keys.Add(blackboardKey);
        }
    }
}