using System;
using Gtk;
using BookManagement.Domain;
namespace BookManagement.Gtksharp
{
    public partial class SearchBookDialog : Gtk.Dialog
    {
        public int searchId = -1;
        public SearchBookDialog()
        {
            Build();
        }

        protected void On_OkBtn_Clicked(object sender, EventArgs e)
        {
            try
            {
                searchId = Convert.ToInt32(IdEntry.Text);
            }
            catch (Exception ex)
            {
                MessageDialog error =
                    new MessageDialog(this, DialogFlags.Modal | DialogFlags.DestroyWithParent,
                                      MessageType.Error, ButtonsType.Ok, ex.Message);
                error.Run();
                error.Destroy();
            }
        }

        protected void On_CancelBtn_Clicked(object sender, EventArgs e)
        {
            Destroy();
        }

        public void SetResult(Book book)
        {
            if(book != null)
            {
                ResultTextView.Buffer.Text = string.Format(
                                           "Book Info  : \n"   +
                                           "\t  Title  :{0}\n" +
                                           "\t  Author :{1}\n" +
                                           "\t  Press  :{2}\n" +
                                           "\t  ISBN   :{3}\n" +
                                           "\t  Price  :{4}\n",
                  book.Title, book.Author, book.Press, book.ISBN, book.Price);
            }
            else
            {
                ResultTextView.Buffer.Text = "Find Anything!";
            }
        }

        protected void OnDeleteEvent(object o, ResponseArgs args)
        {
            Destroy();
        }
    }
}
