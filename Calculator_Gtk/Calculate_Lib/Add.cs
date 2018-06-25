using System;

namespace Calculate_Lib {
    public class Add : ICalculate {
        public int Calculate (int v1, int v2) {
            var ret = v1 + v2;
            return ret;
        }
    }
}