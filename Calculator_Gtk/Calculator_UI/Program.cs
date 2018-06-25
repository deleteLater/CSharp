using Gtk;
using Autofac;
using Autofac.Configuration;
using Calculate_Lib;

namespace Calculator_UI
{
    public class MainClass
    {
		//IoC container
		static IContainer container;

        public static void Main(string[] args)
        {
			//init app
            Application.Init();

            //init container
			InintializeContainer();
			//retrieve a ILog Service from container
			var log = container.Resolve<ILog>();

            //init window and show
			MainWindow win = new MainWindow(log);
            win.Show();

            //run
            Application.Run();
        }

		static void InintializeContainer()
		{
			var builder = new ContainerBuilder();
            //Register module through App.config
			builder.RegisterModule(new ConfigurationSettingsReader("autofac"));
            //build container through ContainerBuilder
			container = builder.Build();
		}
    }
}
