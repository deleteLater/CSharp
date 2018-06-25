namespace Calculate_Lib
{
    public static class Factory
    {
        public static ICalculate ProduceCalculateTool(string oper)
        {
            ICalculate cal = new Add();
            switch (oper)
            {
                //default is Adding operation
                default:
                    break;
                case "-":
                    cal = new Sub();
                    break;
                case "*":
                    cal = new Mul();
                    break;
                case "/":
                    cal = new Div();
                    break;
            }
            return cal;
        }
    }
}