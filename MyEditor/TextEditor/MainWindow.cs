using System;
using Gtk;
using GtkDemo;

public partial class MainWindow : Gtk.Window
{
	//records current filePath
	private string curFile;
    
	//used to change font/color style
	private TextTag font_tag;
    private TextTag color_tag;

	public MainWindow() : base(Gtk.WindowType.Toplevel)
	{
		Build();
		//init TextView
		this.TextView.Buffer.TagTable.Add(color_tag = new TextTag("color_tag"));
		this.TextView.Buffer.TagTable.Add(font_tag = new TextTag("font_tag"));

        this.SetPosition(WindowPosition.CenterAlways);
	}

	protected void OnDeleteEvent(object sender, DeleteEventArgs a)
	{
		Application.Quit();
		a.RetVal = true;
	}

	protected void On_NewAction_Activated(object sender, EventArgs e)
	{
		//clear textView buffer
		this.TextView.Buffer.Clear();
        //when create a new file,it's filepath is empty
		curFile = string.Empty;
		//change title name
		this.Title = "Untitled Document";
	}

	protected void On_OpenAction_Activated(object sender, EventArgs e)
	{
		//create and display a FileChooserDialog
		FileChooserDialog fileChooser =
			new FileChooserDialog("Choose File", this, FileChooserAction.Open,
								  "Cancle", ResponseType.Cancel,
								  "Ok", ResponseType.Ok);
		//if user choose ok,copy text to textview
		if (fileChooser.Run() == (int)ResponseType.Ok)
		{
			curFile = fileChooser.Filename;
			TextView.Buffer.Text =
				        System.IO.File.ReadAllText(curFile);

			//change title
			this.Title = fileChooser.Filename.Substring(curFile.LastIndexOf('/') + 1);

			this.Resize(640, 480);
		}
		fileChooser.Destroy();
	}

	protected void On_SaveAction_Activated(object sender, EventArgs e)
	{
		if(curFile == string.Empty)
		{
			//if curFile haven't been save yet
			//call save_as function
			On_SaveAsAction_Activated(new object(),new EventArgs());
		}
		//Write new content into file
		System.IO.File.WriteAllText(curFile,
									TextView.Buffer.Text);
	}

	protected void On_SaveAsAction_Activated(object sender, EventArgs e)
	{
		//create and display a FileChooserDialog
        FileChooserDialog fileChooser =
            new FileChooserDialog("Save File", this, FileChooserAction.Save,
                                  "Cancle", ResponseType.Cancel,
                                  "Save", ResponseType.Ok);
        
		//click ok
        if (fileChooser.Run() == (int)ResponseType.Ok)
        {
            //write contents into the file
            //if the file not exist,create it and write
            //if already exist,overwrite it
            System.IO.File.WriteAllText(fileChooser.Filename,
                                        TextView.Buffer.Text);
        }
        
		//success message
        MessageDialog md =
            new MessageDialog(this, DialogFlags.DestroyWithParent,
                              MessageType.Info, ButtonsType.Ok,
                              "Save Success!");
		md.Run();
        
		//destory windows
        fileChooser.Destroy();
        md.Destroy();
	}

	protected void On_PrintAction_Activated(object sender, EventArgs e)
	{
        var printer = new MyPrinter(curFile);
        printer.Run();
	}

	protected void On_PrintpreAction_Activated(object sender, EventArgs e)
	{
        //TODO
	}

	protected void ON_CloseAction_Activated(object sender, EventArgs e)
	{
		Application.Quit();
	}

	protected void On_AboutAction_Activated(object sender, EventArgs e)
	{
		//create a AboutDialog
		AboutDialog about = new AboutDialog();
		about.ProgramName = "SimpleEditor";
		about.Version = "0.1";
		about.Copyright = "(c)Zhang TongXie";
		about.Comments = @"SimpleEditor is a very simple tool for editing file!";
		about.Website = "https://github.com/deleteLater";
		about.Logo = new Gdk.Pixbuf("logo.png");
		about.Run();
		about.Destroy();
	}

	protected void On_FontAction_Activated(object sender, EventArgs e)
	{
		FontSelectionDialog fontDlg =
			new FontSelectionDialog("Select Your Font");
		fontDlg.PreviewText = "样例字体  Example Text";
		//if click ok
		if(fontDlg.Run() == (int)ResponseType.Ok)
		{
            //get fontDesc
			Pango.FontDescription selectFont =
				     Pango.FontDescription.FromString(fontDlg.FontName);

			//set font
			font_tag.FontDesc = selectFont;

			//if user selected some text
			//change selected text font
			//else change all text font
			TextIter start, end;
			if (TextView.Buffer.GetSelectionBounds(out start, out end))
				TextView.Buffer.ApplyTag(font_tag, start, end);
			else
				TextView.ModifyFont(selectFont);
		}
        //detory window
		fontDlg.Destroy();
	}

	protected void On_ColorAction_Activated(object sender, EventArgs e)
	{
		ColorSelectionDialog colorDlg = new ColorSelectionDialog("Select Color");
		colorDlg.SetDefaultSize(300, 220);
        //if click ok
		if(colorDlg.Run() == (int)ResponseType.Ok)
		{
			//get color
			Gdk.Color selectColor = colorDlg.ColorSelection.CurrentColor;
            
			//set color_tag
			color_tag.ForegroundGdk = selectColor;

			//if user selected some text
            //change selected text color
            //else change all text color
			TextIter start, end;
			if (TextView.Buffer.GetSelectionBounds(out start, out end))
				TextView.Buffer.ApplyTag(color_tag, start, end);
			else
				TextView.ModifyText(StateType.Normal, selectColor);
		}
        //destory window
		colorDlg.Destroy();
	}

}