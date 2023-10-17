//------------------------------------------------------------
//        File:  BindValueBase.cs
//       Brief:  BindValueBase
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2023-10-11
//============================================================

using BTCore.Runtime.Blackboards;

namespace BTCore.Runtime
{
    public abstract class BindValue
    {
        public Blackboard Blackboard;
    }
    
    public class BindValue<T> : BindValue
    {
        public string BindKeyName;
        public T RawValue;

        public T Value {
            get {
                if (string.IsNullOrEmpty(BindKeyName) || Blackboard == null) {
                    return RawValue;
                }
                
                return Blackboard.GetValue<T>(BindKeyName);
            }
        }
    }
}
