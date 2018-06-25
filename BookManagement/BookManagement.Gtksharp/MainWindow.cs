using Gtk;
using System.Collections.Generic;
using BookManagement.Generic;
using System;
using BookManagement.Gtksharp;

public partial class MainWindow : Gtk.Window
{
    //DI: setter injection
    public IRepository Repository { set; get; }

    //int Id,string Title,string Author,string Press,string ISBN,double Price
    ListStore BookList =
        new ListStore(typeof(int), typeof(string), typeof(string), typeof(string), typeof(string), typeof(double));

    //int lastId, newId = lastId + 1
    //in database,Id is auto_increment from 1000001   
    public int lastId;

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
        InitTreeView();
        SetSizeRequest(950, 500);
        Resizable = false;
    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    void InitTreeView()
    {
        string[] Title = { "Id", "Title", "Author", "Press", "ISBN", "Price" };
        IList<TreeViewColumn> Columns = new List<TreeViewColumn>
        {
            new TreeViewColumn { Title = "Id" ,Sizing = TreeViewColumnSizing.Fixed,MinWidth = 80,Alignment = 0.50F},
            new TreeViewColumn { Title = "Title" ,Sizing = TreeViewColumnSizing.Fixed,MinWidth = 200,Alignment = 0.50F},
            new TreeViewColumn { Title = "Author" ,Sizing = TreeViewColumnSizing.Fixed,MinWidth = 200,Alignment = 0.50F},
            new TreeViewColumn { Title = "Press" ,Sizing = TreeViewColumnSizing.Fixed,MinWidth = 200,Alignment = 0.50F},
            new TreeViewColumn { Title = "ISBN" ,Sizing = TreeViewColumnSizing.Fixed,MinWidth = 150,Alignment = 0.50F},
            new TreeViewColumn { Title = "Price" ,Sizing = TreeViewColumnSizing.Fixed,MinWidth = 15,Alignment = 0.50F}
        };

        for (int i = 0; i < Columns.Count; i++)
        {
            var column = Columns[i];
            var custom_header = new Gtk.Label(Title[i]);
            custom_header.Show();
            column.Widget = custom_header;
            custom_header.ModifyFont(Pango.FontDescription.FromString("Times Bold Italic 13"));

            var CellRender = new CellRendererText
            {
                Xalign = 0.50F,
                Background = "White",
                Foreground = "Black",
                Font = "Lucida Console"
            };
            column.PackStart(CellRender, true);
            column.AddAttribute(CellRender, "text", i);
            BooksView.AppendColumn(column);
        }
        BookList.Clear();
        BooksView.Model = BookList;
    }

    protected void On_InitdataBtn_Clicked(object sender, EventArgs e)
    {
        BookList.Clear();
        //Read book info from database
        foreach (var book in Repository.FindAll())
        {
            BookList.AppendValues
                    (book.Id, book.Title, book.Author, book.Press, book.ISBN, book.Price);
            lastId = book.Id;
        }
    }

    protected void On_AddBtn_Clicked(object sender, EventArgs e)
    {
        //show Addbook dialog
        var newId = lastId + 1;
        var dlg = new BookInfoDialog(newId,null);
        if(dlg.Run() == (int)ResponseType.Ok)
        {
            var nb = dlg.newBook;
            if (nb != null)
            {
                //update book in BookList(model)
                BookList.AppendValues
                        (nb.Id, nb.Title, nb.Author, nb.Press, nb.ISBN, nb.Price);
                //update book in database
                Repository.Add(nb);
                //update lastId
                lastId = nb.Id;
            }
        }
        dlg.Destroy();
    }

    protected void On_DeleteBtn_Clicked(object sender, EventArgs e)
    {
        TreeIter iter;
        BooksView.Selection.GetSelected(out iter);
        var id = BookList.GetValue(iter, 0);
        if(id == null)
        {
            MessageDialog warning =
                new MessageDialog(this, DialogFlags.Modal | DialogFlags.DestroyWithParent,
                                  MessageType.Warning, ButtonsType.Ok,
                                  "\nPlease choose a book first!");
            warning.Run();
            warning.Destroy();
            return;
        }
        MessageDialog question =
            new MessageDialog(this, DialogFlags.Modal | DialogFlags.DestroyWithParent,
                              MessageType.Question, ButtonsType.YesNo,
                              "\nAre you sure to delete book: \n\t\t\t{0}", BookList.GetValue(iter, 1))
            {
                Title = "Are you sure ?",
                HasSeparator = false
            };

        if (question.Run() == (int)ResponseType.Yes)
        {
            //update data in Booklist(model)
            BookList.Remove(ref iter);
            //update lastId
            lastId = Convert.ToInt32(id);
            //update data in database
            Repository.Remove(Convert.ToInt32(id));
        }
        question.Destroy();
    }

    protected void On_SearchBtn_Clicked(object sender, EventArgs e)
    {
        var dlg = new SearchBookDialog();
        do
        {
            var sid = dlg.searchId;
            if (sid != -1)
            {
                Console.WriteLine("Searching...");
                var book = Repository.FindById(dlg.searchId);
                dlg.SetResult(book);
            }
        } while (dlg.Run() != (int)ResponseType.Cancel);
    }

    protected void On_ModifyBtn_Clicked(object sender, EventArgs e)
    {
        TreeIter iter;
        BooksView.Selection.GetSelected(out iter);
        var id = BookList.GetValue(iter, 0);
        var book = Repository.FindById(Convert.ToInt32(id));
        if (id == null)
        {
            MessageDialog warning =
                new MessageDialog(this, DialogFlags.Modal | DialogFlags.DestroyWithParent,
                                  MessageType.Warning, ButtonsType.Ok,
                                  "\nPlease choose a book first!");
            warning.Run();
            warning.Destroy();
            return;
        }
        //modify book dialog
        var dlg = new BookInfoDialog(book.Id, book);
        if(dlg.Run() == (int)ResponseType.Ok)
        {
            var nb = dlg.newBook;
            if(nb != null)
            {
                //set newValue to BookList
                BookList.SetValues(iter, nb.Id, nb.Title, nb.Author, nb.Press, nb.ISBN, nb.Price);
                //updata newInfo to database
                Repository.Modify(nb);   
            }
        }
        dlg.Destroy();
    }
}