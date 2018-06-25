using System;
using Gtk;

namespace ScoreManagement.GtkSharp
{
    public partial class ChangePwdDlg : Gtk.Dialog
    {
        DBAccess.DBAccess DB = new DBAccess.DBAccess();
        string userName;
        public ChangePwdDlg(string userName)
        {
            Build();
            this.userName = userName;
            Title = "Change Password";
        }

        protected void On_OKBtn_Clicked(object sender, EventArgs e)
        {
            string oldPwd = oldPwdEntry.Text;
            if(oldPwd == "" || oldPwd != DB.GetUserPwd(userName))
            {
                MessageDialog warning =
                    new MessageDialog(this, DialogFlags.Modal, MessageType.Warning,
                                      ButtonsType.Ok, "\nThe old password you provide is wrong!");
                warning.Title = "Warning";
                warning.Run();
                warning.Destroy();
                oldPwdEntry.Text = "";
            }
            else if(repeatNewPwdEntry.Text != newPwdEntry.Text)
            {
                MessageDialog warning =
                    new MessageDialog(this, DialogFlags.Modal, MessageType.Warning,
                                      ButtonsType.Ok, "\n两次新密码不一致！");
                warning.Title = "Warning";
                warning.Run();
                warning.Destroy();
                oldPwdEntry.Text = "";
                newPwdEntry.Text = "";
                repeatNewPwdEntry.Text = "";
            }
            else if (newPwdEntry.Text == "")
            {
                MessageDialog warning =
                    new MessageDialog(this, DialogFlags.Modal, MessageType.Warning,
                      ButtonsType.Ok, "\nNew Password Cannot be empty!!");
                warning.Title = "Warning";
                warning.Run();
                warning.Destroy();
                oldPwdEntry.Text = "";
                newPwdEntry.Text = "";
                repeatNewPwdEntry.Text = "";
            }
            else
            {
                DB.ChangeUserPwd(userName, newPwdEntry.Text);
                MessageDialog success =
                    new MessageDialog(this, DialogFlags.Modal, MessageType.Info,
                                      ButtonsType.Ok, "\nChange password success!");
                success.Title = "Success";
                success.Run();
                success.Destroy();
                Destroy();
            }
        }

        protected void On_CancelBtn_Clicked(object sender, EventArgs e)
        {
            Destroy();
        }
    
    }
}
