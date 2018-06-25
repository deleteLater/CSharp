using System;

namespace Calculate_Lib {
    public class Div : ICalculate {
        public int Calculate (int v1, int v2) {
            if (Math.Abs (v2) == 0) {
                throw new DivideByZeroException("Divisor Cannot be Zero!");
            }
            var ret = v1 / v2;
            return ret;
        }
        public double Calculate(double v1,double v2){
            if(Math.Abs(v2) - Double.Epsilon < 1e-6){
                throw new DivideByZeroException("Divisor Cannot be Zero!");
            }
            var ret = v1 /v2;
            return ret;
        }
    }
}