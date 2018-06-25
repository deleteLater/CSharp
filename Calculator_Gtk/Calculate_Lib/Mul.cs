using System;

namespace Calculate_Lib {
    public class Mul : ICalculate {
        public int Calculate (int v1, int v2) {
            var ret = v1 * v2;
            return ret;
        }
    }
}