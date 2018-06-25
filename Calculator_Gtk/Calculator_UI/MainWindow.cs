using System;
using Gtk;
using Calculate_Lib;
using Calculator_DBLog;

public partial class MainWindow : Gtk.Window
{
	private ILog log;
	public MainWindow(ILog log) : base(Gtk.WindowType.Toplevel)
	{
		Build();
		this.log = log;
	}

    protected void When_ExitBtn_Clicked(object sender, EventArgs e) => Application.Quit();

    protected void When_CalculateBtn_Clicked(object sender, EventArgs e)
    {
        try
        {
            //get operands
            int operandX = Convert.ToInt32(Entry_OperandX.Text);
            int operandY = Convert.ToInt32(Entry_OperandY.Text);

            //get operator
            string oper = ComboBox_Operators.ActiveText;

            //produce suitable object depends on oper
            ICalculate cal = Factory.ProduceCalculateTool(oper);

            //calculate
            int result = cal.Calculate(operandX, operandY);

            //show result
            Label_Result.Text = string.Format("Result          : {0}", result.ToString());

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    protected void When_WriteLogBtn_Clicked(object sender, EventArgs e)
    {
        //format log string
        string contents = 
            $"{DateTime.Now.ToString()} => Equation:{Entry_OperandX.Text}{ComboBox_Operators.ActiveText}{Entry_OperandY.Text} {Label_Result.Text.Replace(" ", "")}";
		//wirte log
		log.Write(contents);
        //success tip
        TextV_Log.Buffer.Text = string.Format("{0} => {1}{2}",
                                               DateTime.Now.ToString(),
                                               "Write Log Success!",
                                                 Environment.NewLine);
    }

    protected void When_ViewLogBtn_Clicked(object sender, EventArgs e)
    {
        //read log and show
		TextV_Log.Buffer.Text = (log.Read());
    }
}
