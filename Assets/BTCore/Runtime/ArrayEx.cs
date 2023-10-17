//------------------------------------------------------------
//        File:  ArrayEx.cs
//       Brief:  ArrayEx
//
//      Author:  Saroce, Saroce233@163.com
//
//    Modified:  2022-11-01
//============================================================

using System;

namespace Core.Lite.Extensions
{
    public static class ArrayEx
    {
        private class ArrayTraverse
        {
            public readonly int[] Position;
            private readonly int[] _maxLengths;

            public ArrayTraverse(Array array) {
                _maxLengths = new int[array.Rank];
                for (var i = 0; i < array.Rank; ++i) {
                    _maxLengths[i] = array.GetLength(i) - 1;
                }

                Position = new int[array.Rank];
            }

            public bool Step() {
                for (var i = 0; i < Position.Length; ++i) {
                    if (Position[i] < _maxLengths[i]) {
                        Position[i]++;
                        for (var j = 0; j < i; j++) {
                            Position[j] = 0;
                        }

                        return true;
                    }
                }

                return false;
            }
        }
        
        public static void ForEach(this Array array, Action<Array, int[]> action) {
            if (array.LongLength == 0) {
                return;
            }
            
            var walker = new ArrayTraverse(array);
            do {
                action(array, walker.Position);
            }
            while (walker.Step());
        }
    }

}