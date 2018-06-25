using Simple.Data;
using System.Configuration;
using ScoreManagement.Domain;
using System.Collections.Generic;
using System.Linq;

namespace ScoreManagement.DBAccess
{
    public class DBAccess
    {
        readonly string _connectionString =
            ConfigurationManager.ConnectionStrings["MysqlDb"].ToString();
        
        protected dynamic Db 
        { 
            get { return Database.OpenConnection(_connectionString); }
        }

        /*
         * Table: User
         */ 
        public string GetUserPwd(string userName)
        {
            var user = Db.User.Find(Db.User.Name == userName);
            return user.Pwd;
        }
        public bool ValidateUser(string userName,string userPwd)
        {
            var user = Db.User.Find(Db.User.Name == userName);
            if (user != null && user.Pwd == userPwd)
                return true;
            return false;
        }
        public void ChangeUserPwd(string userName,string newPwd)
        {
            var user = new User { Name = userName, Pwd = newPwd };
            Db.User.Update(user);
        }
        /*
         * Table: Score
         */ 
        public IList<Score> GetScoreList()
        {
            IEnumerable<Score> scores = Db.Score.All();
            return scores.ToList();
        }
        //Fuzzy search
        public IList<Score> GetScoreByName(string name)
        {
            IEnumerable<Score> score = Db.Score.All().Where(Db.Score.Name.Like(name+'%'));
            return score.ToList();
        }
        //Precise search
        public Score GetScoreById(int id)
        {
            var score = Db.Score.Find(Db.Score.Id == id);
            return score;
        }
        
        /*
         * Table: Course
         */
        public IList<Course> GetCourseList()
        {
            IEnumerable<Course> courses = Db.Course.All();
            return courses.ToList();
        }
        //Fuzzy search
        public IList<Course> GetCourseByName(string name)
        {
            IEnumerable<Course> courses = Db.Course.All().Where(Db.Course.Name.Like(name + '%'));
            return courses.ToList();
        }
        //precise search
        public Course GetCourseById(string id)
        {
            var course = Db.Course.Find(Db.Course.Id == id);
            return course;
        }
        public IList<Course> GetCourseByCredit(int credit)
        {
            IEnumerable<Course> courses = Db.Course.All().Where(Db.Course.Credit == credit);
            return courses.ToList();
        }
        public void UpdateCourse(Course course)
        {
            //change string type to enum
            int type = (course.Type == "选修课") ? 1 : 2;
            Db.Course.UpdateById(Id: course.Id,Name: course.Name,Type: type,Credit: course.Credit);
        }
        public void DeleteCourse(string id)
        {
            Db.Course.DeleteById(id);
        }
        public void AddCourse(Course course)
        {
            int type = (course.Type == "选修课") ? 1 : 2;
            Db.Course.Insert(Id: course.Id, Name: course.Name, Type: type, Credit: course.Credit);
        }
    }
}
