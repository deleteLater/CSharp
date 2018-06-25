using Gtk;
using Autofac;
using Autofac.Configuration;

namespace BookManagement.Gtksharp
{
    class MainClass
    {
        // Ioc Container
        private static IContainer _container;

        public static void Main(string[] args)
        {
            //init App
            Application.Init();
            //init Ioc container
            InitializeContainer();
            //retrive MainWindow from container to finish property injection 
            var win = _container.Resolve<MainWindow>();
            //retrive IRepository from container
            //show window
            win.Show();
            //run App
            Application.Run();
        }

        private static void InitializeContainer()
        {
            // container builder
            var builder = new ContainerBuilder();
            // read configuration from App.config
            var config = new ConfigurationSettingsReader("autofac");
            // register module based on config
            builder.RegisterModule(config);
            // register MainWindow so as to inject all the properties the window has
            // builder.RegisterType<BookRepository>().As<IRepository>();
            // builder.RegisterType<MainWindow>().PropertiesAutowired();
            builder.RegisterAssemblyTypes(typeof(MainWindow).Assembly).PropertiesAutowired();
            // construct a container
            _container = builder.Build();
        }
    }
}