using System;
using Simple.Data;

namespace ConnectMysql
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            var db = Database.OpenConnection
                ("server=localhost;database=ScoreManagement;user=zhang;password=Zjx_520499;SslMode=none;Charset=utf8");

            var users = db.User.All();
            foreach(var user in users)
            {
                Console.WriteLine(user.Name);
            }
        }
    }
}
