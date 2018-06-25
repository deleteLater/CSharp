using System;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.IO;

namespace WriteDataIntoMysql
{
    class ProgramEntry
    {
		private static string connectionString = 
			ConfigurationManager.ConnectionStrings["zhang"].ConnectionString;
		private static string datafilePath =
			ConfigurationManager.AppSettings["datas"] ?? "test";
		
		public static void Main(string[] args)
		{
			using (var con = new MySqlConnection(connectionString))
			{
				//open connection
				con.Open();
				Console.WriteLine(con.State.ToString());

				/*
                 * insert string --Course
                 * string insert_string =
                 *      "INSERT INTO Course(CId,CName,PreCId,Credit,Semester) " +
                 *      "VALUES(@cid,@cname,@pre_cid,@credit,@semester)";
                 */

				/*
				 * insert string -- Student
				 * string insert_string =
				 *	"INSERT INTO Student(SId,SName,SSex,SBirthday,SDept,SComments)" +
				 *	"VALUES(@sid,@sname,@ssex,@sbirthday,@sdept,@scomments)";                  
                 */

				string insert_string =
					"INSERT INTO SC(SC_SId,SC_CId,Grade)" +
					"VALUES(@sc_sid,@sc_cid,@grade)";

				//prepare cmd
				var cmd = new MySqlCommand(insert_string,con);
				cmd.Parameters.Add(new MySqlParameter("@sc_sid", MySqlDbType.String));
				cmd.Parameters.Add(new MySqlParameter("@sc_cid", MySqlDbType.String));
				cmd.Parameters.Add(new MySqlParameter("@grade", MySqlDbType.Byte));
                
				//get data
				System.IO.StreamReader dataFile =
						  new System.IO.StreamReader(datafilePath);
				string line;

				while((line = dataFile.ReadLine()) != null){
					var data = line.Split(' ');
					for (int i = 0; i < data.Length; i++)
					{
						if (data[i] == "NULL")
							data[i] = null;
					}
					cmd.Parameters["@sc_sid"].Value = data[0];
					cmd.Parameters["@sc_cid"].Value = data[1];
					cmd.Parameters["@grade"].Value = data[2];
					try
					{
						cmd.ExecuteNonQuery();
					}
					catch(Exception err)
					{
						Console.WriteLine("Error: "+ err.Message);
					}
				}
				Console.WriteLine("Write Success!");
			}
		}
    }
}
