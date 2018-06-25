using System;
namespace Calculate_Lib
{
    public interface ILog
    {
		void Write(string logContents);
		string Read();
    }
}
