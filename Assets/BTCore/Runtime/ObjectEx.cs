﻿//------------------------------------------------------------
//        File:  ObjectEx.cs
//       Brief:  ObjectEx
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2022-11-1
//============================================================

using System;
using System.Collections.Generic;
using System.Reflection;

namespace Core.Lite.Extensions
{
    public static class ObjectEx
    {
        private static readonly MethodInfo CloneMethod =
            typeof(object).GetMethod("MemberwiseClone", BindingFlags.NonPublic | BindingFlags.Instance);

        public static bool IsPrimitive(this Type type) {
            if (type == typeof(string)) {
                return true;
            }
            
            return (type.IsValueType & type.IsPrimitive);
        }

        public static object DeepCopy(this object originalObject) {
            return InternalCopy(originalObject, new Dictionary<object, object>(new ReferenceEqualityComparer()));
        }

        private static object InternalCopy(object originalObject, IDictionary<object, object> visited) {
            if (originalObject == null) {
                return null;
            }
            
            var typeToReflect = originalObject.GetType();
            if (IsPrimitive(typeToReflect)) {
                return originalObject;
            }
            if (visited.ContainsKey(originalObject)) {
                return visited[originalObject];
            }
            if (typeof(Delegate).IsAssignableFrom(typeToReflect)) {
                return null;
            }
            
            var cloneObject = CloneMethod.Invoke(originalObject, null);
            if (typeToReflect.IsArray) {
                var arrayType = typeToReflect.GetElementType();
                if (IsPrimitive(arrayType) == false) {
                    var clonedArray = (Array)cloneObject;
                    clonedArray.ForEach((array, indices) =>
                        array.SetValue(InternalCopy(clonedArray.GetValue(indices), visited), indices));
                }
            }

            visited.Add(originalObject, cloneObject);
            CopyFields(originalObject, visited, cloneObject, typeToReflect);
            RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);
            return cloneObject;
        }

        private static void RecursiveCopyBaseTypePrivateFields(object originalObject,
            IDictionary<object, object> visited, object cloneObject, Type typeToReflect) {
            if (typeToReflect.BaseType != null) {
                RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect.BaseType);
                CopyFields(originalObject, visited, cloneObject, typeToReflect.BaseType,
                    BindingFlags.Instance | BindingFlags.NonPublic, info => info.IsPrivate);
            }
        }

        private static void CopyFields(object originalObject, IDictionary<object, object> visited, object cloneObject,
            Type typeToReflect,
            BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public |
                                        BindingFlags.FlattenHierarchy, Func<FieldInfo, bool> filter = null) {
            foreach (var fieldInfo in typeToReflect.GetFields(bindingFlags)) {
                if (filter != null && filter(fieldInfo) == false) {
                    continue;
                }
                if (IsPrimitive(fieldInfo.FieldType)) {
                    continue;
                }
                
                var originalFieldValue = fieldInfo.GetValue(originalObject);
                var clonedFieldValue = InternalCopy(originalFieldValue, visited);
                fieldInfo.SetValue(cloneObject, clonedFieldValue);
            }
        }

        public static T DeepCopy<T>(this T original) => (T) ((object) original).DeepCopy();

        public class ReferenceEqualityComparer : EqualityComparer<object>
        {
            public override bool Equals(object x, object y) {
                return ReferenceEquals(x, y);
            }

            public override int GetHashCode(object obj) {
                return obj.GetHashCode();
            }
        }

        
    }
}