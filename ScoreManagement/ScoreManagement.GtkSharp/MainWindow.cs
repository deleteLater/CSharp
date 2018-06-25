using Gtk;
using System.Collections.Generic;
using ScoreManagement.Domain;
using System;
using System.Linq;

namespace ScoreManagement.GtkSharp
{
    public partial class MainWindow : Gtk.Window
    {
        //scoreTreeView variables
        ListStore scoreList =
            new ListStore(typeof(int), typeof(string), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int), typeof(int));
        readonly string[] scoreTitle = { "Id", "Name", "Ch", "Mt", "Eg", "Pt", "Ht", "Gp" };

        //courseTreeView variables
        ListStore courseList =
            new ListStore(typeof(string), typeof(string), typeof(string), typeof(int));
        readonly string[] courseTitle = { "ID", "Name", "Type", "Credit" };

        //Access database
        DBAccess.DBAccess DB = new DBAccess.DBAccess();

        void InitTreeViewColumn(TreeView treeView, string[] title, ListStore list, int columnWidth)
        {
            IList<TreeViewColumn> Columns = new List<TreeViewColumn>();
            for (int i = 0; i < title.Length; i++)
            {
                Columns.Add(new TreeViewColumn { MinWidth = columnWidth, Alignment = 0.50F });
            }
            for (int i = 0; i < Columns.Count; i++)
            {
                var column = Columns[i];
                var custom_header = new Gtk.Label(title[i]);
                custom_header.Show();
                column.Widget = custom_header;
                custom_header.ModifyFont(Pango.FontDescription.FromString("Arial 13"));

                var CellRender = new CellRendererText
                {
                    Xalign = 0.50F,
                    Background = "White",
                    Foreground = "Grey",
                    Font = "Lucida Console 10"
                };
                column.PackStart(CellRender, true);
                column.AddAttribute(CellRender, "text", i);
                treeView.AppendColumn(column);
            }
            treeView.Model = list;
        }

        //retrive all records from Course
        void UpdateCourseList()
        {
            courseList.Clear();
            foreach (var c in DB.GetCourseList())
            {
                courseList.AppendValues(c.Id, c.Name, c.Type, c.Credit);
            }
        }
        //retrive all records from Score
        void UpdateScoreList()
        {
            scoreList.Clear();
            foreach (var s in DB.GetScoreList())
            {
                scoreList.AppendValues(s.Id, s.Name, s.Ch, s.Mt, s.Eg, s.Pt, s.Ht, s.Gp);
            }
        }

        Course GetSelectedCourse()
        {
            TreeIter iter;
            CourseTreeView.Selection.GetSelected(out iter);
            string courseId = (string)courseList.GetValue(iter, 0);
            if (courseId == null)
            {
                MessageDialog info =
                    new MessageDialog(this, DialogFlags.Modal, MessageType.Info,
                                      ButtonsType.Ok, "\nPlease choose one item first!");
                info.Title = "Hint";
                info.Run();
                info.Destroy();
                return null;
            }
            return DB.GetCourseById(courseId);
        }

        public MainWindow() :
                base(Gtk.WindowType.Toplevel)
        {
            Build();
            //init TreeView
            InitTreeViewColumn(ScoreTreeView, scoreTitle, scoreList, 95);
            InitTreeViewColumn(CourseTreeView, courseTitle, courseList, 120);
            //load data
            UpdateScoreList();
            UpdateCourseList();
        }

        protected void OnDeleteEvent(object o, Gtk.DeleteEventArgs args)
        {
            Application.Quit();
            args.RetVal = 1;
        }

        protected void On_ScoreQueryBtn_Clicked(object sender, System.EventArgs e)
        {
            scoreList.Clear();
            if (ConditionEntry.Text == "")
            {
                //if have no condition,show all records
                foreach (var s in DB.GetScoreList())
                {
                    scoreList.AppendValues(s.Id, s.Name, s.Ch, s.Mt, s.Eg, s.Pt, s.Ht, s.Gp);
                }
            }
            else
            {
                IList<Score> records = new List<Score>();
                if (ConditionCombox.ActiveText == "Name")
                {
                    //Search by name support Fuzzy search
                    records = DB.GetScoreByName(ConditionEntry.Text);
                }
                else
                {
                    //Search by id only support precise search
                    int id;
                    try
                    {
                        id = Convert.ToInt32(ConditionEntry.Text);
                    }
                    catch (Exception)
                    {
                        //id format was wrong
                        MessageDialog error =
                            new MessageDialog(this, DialogFlags.Modal, MessageType.Error,
                                              ButtonsType.Ok, "\nCheck your input format!!");
                        error.Title = "Error";
                        error.Run();
                        error.Destroy();
                        return;
                    }
                    var res = DB.GetScoreById(id);
                    if (res != null)
                    {
                        records.Add(res);
                    }
                }

                //no records founded
                if (records.Count == 0)
                {
                    MessageDialog msg =
                        new MessageDialog(this, DialogFlags.Modal, MessageType.Info,
                                          ButtonsType.Ok, "\nNo record found in database.");
                    msg.Title = "Hint";
                    msg.Run();
                    msg.Destroy();
                }
                else
                {
                    //show founded records
                    foreach (var record in records)
                    {
                        scoreList.AppendValues(record.Id, record.Name,
                                               record.Ch, record.Mt, record.Eg, record.Pt, record.Ht, record.Gp);
                    }
                }
            }
        }

        protected void On_ChangePwd_Activated(object sender, EventArgs e)
        {
            //retrive userName from title
            //Title format: 'Current User:  userName'
            Title.Trim();
            string userName = Title.Substring(Title.IndexOf(':') + 1);
            Console.WriteLine(userName.Length);
            var dlg = new ChangePwdDlg(userName.Trim());
            dlg.Run();
        }

        protected void On_AddCourseBtn_Clicked(object sender, EventArgs e)
        {
            var dlg = new CourseInfoDlg();
            dlg.Title = "Add Course";
            if(dlg.Run() == (int)ResponseType.Ok)
            {
                DB.AddCourse(dlg.course);
                dlg.Destroy();
                UpdateCourseList();
                MessageDialog success =
                    new MessageDialog(this, DialogFlags.Modal, MessageType.Info,
                                      ButtonsType.Ok, "\nAdd course success");
                success.Title = "Success";
                success.Run();
                success.Destroy();
            }
            else
            {
                dlg.Destroy();
            }
        }

        protected void On_ModifyCourseBtn_Clicked(object sender, EventArgs e)
        {
            //get selected course
            var seletedCourse = GetSelectedCourse();
            if(seletedCourse == null)
            {
                return;
            }
            //show course info dialog
            var dlg = new CourseInfoDlg(seletedCourse);
            if (dlg.Run() == (int)ResponseType.Ok)
            {
                DB.UpdateCourse(dlg.course);
                dlg.Destroy();
                UpdateCourseList();
                MessageDialog success =
                    new MessageDialog(this, DialogFlags.Modal, MessageType.Info,
                                      ButtonsType.Ok, "\nUpdate course success");
                success.Title = "Success";
                success.Run();
                success.Destroy();
            }
            else
            {
                dlg.Destroy();
            }
        }

        protected void On_DeleteCourseBtn_Clicked(object sender, EventArgs e)
        {
            //get selected course
            var seletedCourse = GetSelectedCourse();
            if(seletedCourse == null)
            {
                return;
            }
            MessageDialog warn =
                new MessageDialog(this, DialogFlags.Modal, MessageType.Info,
                                  ButtonsType.OkCancel, "\nAre you sure to delete course :"+seletedCourse.Name);
            warn.Title = "Warn";
            if(warn.Run() == (int)ResponseType.Ok)
            {
                DB.DeleteCourse(seletedCourse.Id);
                warn.Destroy();
                MessageDialog success =
                     new MessageDialog(this, DialogFlags.Modal, MessageType.Info,
                                       ButtonsType.Ok, "\nDelete course success");
                success.Title = "Success";
                success.Run();
                success.Destroy();
                UpdateCourseList();
            }
            else
            {
                warn.Destroy();
            }
        }

        protected void On_QueryCourseBtn_Clicked(object sender, EventArgs e)
        {
            courseList.Clear();
            IList<Course> courses = new List<Course>();
            //variables
            var name = NameEntry.Text;
            var id = IdEntry.Text;
            //credit maybe null,cannot use var there
            var credit = CreditCombox.ActiveText;

            //search by id
            if (id != "")
            {
                var course = DB.GetCourseById(id);
                if (course != null)
                {
                    courses.Add(course);
                }
            }
            //search by name and credit
            else if (name != "" && credit != null)
            {
                var result = DB.GetCourseByName(name).Intersect(DB.GetCourseByCredit(Convert.ToInt32(credit)),
                                                   new CourseEqualityComparer());
                courses = result.ToList();
            }
            //seach by name
            else if(name != "")
            {
                courses = DB.GetCourseByName(name);
            }
            //if credit==null,all records will be shown
            else if(credit == null)
            {
                courses = DB.GetCourseList();
            }
            //search by credit
            else
            {
                courses = DB.GetCourseByCredit(Convert.ToInt32(credit));
            }

            //if no records founded
            if (courses.Count == 0)
            {
                MessageDialog msg =
                    new MessageDialog(this, DialogFlags.Modal, MessageType.Info,
                                      ButtonsType.Ok, "\nNo record found in database");
                msg.Title = "Hint";
                msg.Run();
                msg.Destroy();
            }
            //show result
            else
            {
                foreach (var c in courses)
                {
                    courseList.AppendValues(c.Id, c.Name, c.Type, c.Credit);
                }
            }
        }

    }

    //when intersect two Course List it will be used
    class CourseEqualityComparer : IEqualityComparer<Course>
    {
        public bool Equals(Course x, Course y)
        {
            if (x.Id == y.Id)
                return true;
            return false;
        }

        public int GetHashCode(Course obj)
        {
            if (obj == null)
                return 0;
            return obj.Id.GetHashCode();
        }
    }
}
