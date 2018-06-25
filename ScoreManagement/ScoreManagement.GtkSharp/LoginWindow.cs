using System;
using Gtk;
using ScoreManagement.DBAccess;
using ScoreManagement.GtkSharp;

public partial class LoginWindow : Gtk.Window
{
    //logo resource path
    string res_path = "../../src/";

    //Access database
    DBAccess DB = new DBAccess();

    public LoginWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
        //set logo
        MainLogo.Pixbuf = new Gdk.Pixbuf(res_path + "admin.png");
        PwdLogo.Pixbuf = new Gdk.Pixbuf(res_path + "pwd.png");
        UserLogo.Pixbuf = new Gdk.Pixbuf(res_path + "user.png");

        //set title
        TitleLabel.Text = "Score Management System";
        TitleLabel.ModifyFont(Pango.FontDescription.FromString("Arial,Bold Italic 12"));

    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    protected void On_LoginBtn_Clicked(object sender, EventArgs e)
    {
        var exist = DB.ValidateUser(UserEntry.Text, PwdEntry.Text);
        if(!exist)
        {
            MessageDialog failed =
                new MessageDialog(this, DialogFlags.Modal | DialogFlags.NoSeparator,
                                  MessageType.Info, ButtonsType.Ok, "\nUsername or Password is wrong!");
            failed.Title = "Hint";
            failed.Run();
            failed.Destroy();
        }
        else
        {
            //Enter MainWindow
            var main = new MainWindow
            {
                Title = "Current User: " + UserEntry.Text
            };
            //Destory login window
            Destroy();
        }
    }

    protected void On_CancelBtn_Clicked(object sender, EventArgs e)
    {
        UserEntry.Text = "";
        PwdEntry.Text = "";
    }
}