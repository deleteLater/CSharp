using Gtk;

namespace ScoreManagement.GtkSharp
{
    using DBAccess;
    public partial class MainWindows : Gtk.Window
    {
        DBAccess DB = new DBAccess();
        public MainWindows() :
                base(Gtk.WindowType.Toplevel)
        {
            Build();
        }
        protected void OnDeleteEvent(object o, Gtk.DeleteEventArgs args)
        {
            Application.Quit();
            args.RetVal = 1;
        }
    }
}
