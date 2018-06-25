using System;
using ScoreManagement.Domain;

namespace ScoreManagement.GtkSharp
{
    public partial class CourseInfoDlg : Gtk.Dialog
    {
        public Course course;
        public CourseInfoDlg(Course currentCourse = null)
        {
            Build();
            //when modify a course,currentCourse won't be null
            if(currentCourse != null)
            {
                course = currentCourse;
                IdEntry.Text = course.Id;
                NameEntry.Text = course.Name;
                TypeCombox.Active = (course.Type == "选修课") ? 0 : 1;
                CreditCombox.Active = Convert.ToInt32(course.Credit) - 1;
            }
            //when add a course,enable IdEntry
            else
            {
                IdEntry.CanFocus = true;
                IdEntry.Sensitive = true;
                IdEntry.IsEditable = true;
                Focus = IdEntry;
            }
        }

        protected void On_OKBtn_Clicked(object sender, EventArgs e)
        {
            //when add a course,this.course will be null
            if (course == null)
                course = new Course();
            course.Id = IdEntry.Text;
            course.Name = NameEntry.Text;
            course.Type = TypeCombox.ActiveText;
            course.Credit = Convert.ToInt32(CreditCombox.ActiveText);
        }

        protected void On_CancelBtn_Clicked(object sender, EventArgs e)
        {
            Destroy();
        }
    }
}
