using Gtk;
using BookManagement.Domain;
using System;

namespace BookManagement.Gtksharp
{
    public partial class BookInfoDialog : Gtk.Dialog
    {
        public Book newBook;

        public BookInfoDialog(int newId,Book book)
        {
            Build();
            if(book == null)
            {
                //do create new book function
                Title = "Add Book";
                IdEntry.Text = newId.ToString();
            }
            else
            {
                //do modify book function
                Title = "Modify Book";
                LoadBookInfo(book);
            }
            SetSizeRequest(450, 380);
            HasSeparator = false;
        }

        protected void On_OkBtn_Clicked(object sender, EventArgs e)
        {
            GenerateNewBook();
            if (newBook != null)
            {
                MessageDialog success =
                    new MessageDialog(this, DialogFlags.Modal,
                                       MessageType.Info, ButtonsType.Ok, "\nOperation Finished!");
                success.Run();
                success.Destroy();
            }
        }

        protected void On_CanceledBtn_Clicked(object sender, EventArgs e)
        {
            Destroy();
        }

        void GenerateNewBook()
        {
            try
            {
                //ISBN format 333-1-333-55555-1
                if (IEntryOne.Text.Length != 3 || IEntryTwo.Text.Length != 1 ||
                   IEntryThree.Text.Length != 3 || IEntryFour.Text.Length != 5 ||
                   IEntryFive.Text.Length != 1)
                {
                    //ISBN format error
                    MessageDialog format_error =
                        new MessageDialog(this, DialogFlags.Modal | DialogFlags.DestroyWithParent,
                                          MessageType.Error, ButtonsType.Ok, "\nCheck your ISBN format!");
                    format_error.Run();
                    format_error.Destroy();
                    return;
                }
                newBook = new Book
                {
                    Id = Convert.ToInt32(IdEntry.Text),
                    Title = TitleEntry.Text,
                    Author = AuthorEntry.Text,
                    Press = PressEntry.Text,
                    ISBN = IEntryOne.Text + '-' + IEntryTwo.Text + '-' + IEntryThree.Text + '-' + IEntryFour.Text + '-' + IEntryFive.Text,
                    Price = Convert.ToDouble(PriceEntry.Text)
                };
            }
            catch (Exception)
            {
                MessageDialog error =
                    new MessageDialog(this, DialogFlags.Modal,
                                      MessageType.Error, ButtonsType.Ok, "\nCheck your price format!!");
                error.Run();
                error.Destroy();
            }
        }

        void LoadBookInfo(Book book)
        {
            IdEntry.Text = book.Id.ToString();
            TitleEntry.Text = book.Title;
            AuthorEntry.Text = book.Author;
            PressEntry.Text = book.Press;
            //ISBN format 333-1-333-55555-1
            IEntryOne.Text = book.ISBN.Substring(0, 3);
            IEntryTwo.Text = book.ISBN.Substring(4, 1);
            IEntryThree.Text = book.ISBN.Substring(6, 3);
            IEntryFour.Text = book.ISBN.Substring(10, 5);
            IEntryFive.Text = book.ISBN.Substring(16, 1);

            PriceEntry.Text = book.Price.ToString();
        }

    }
}