using System;

namespace Calculate_Lib {
    public class Sub : ICalculate {
        public int Calculate (int v1, int v2) {
            var ret = v1 - v2;
            return ret;
        }
    }
}