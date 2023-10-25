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
        public Blackboard Blackboard { get; set; }
    }
    
    public class BindValue<T> : BindValue
    {
        public string BindKeyName { get; set; }
        public T RawValue { get; set; }

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
