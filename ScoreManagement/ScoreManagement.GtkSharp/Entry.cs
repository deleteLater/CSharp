using Gtk;

namespace ScoreManagement.GtkSharp
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            //load my gtk themes
            //Gtk.Rc.Parse("../../src/gtkrc");
            Application.Init();

            //start with login
            LoginWindow win = new LoginWindow();
            win.Show();

            Application.Run();
        }
    }
}
