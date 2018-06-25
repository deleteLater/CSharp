using System;
using MySql.Data.MySqlClient;
using System.Text;
using System.Configuration;
using Calculate_Lib;

namespace Calculator_DBLog
{
	/// <summary>
	/// A simple Logsystem based on Mysql
    /// </summary>
	public class DBLog : ILog
    {
        //Read connectString from App.config(Unencrypted)
        private string connectionString = 
			ConfigurationManager.ConnectionStrings["DBLog"].ConnectionString;

        /// <summary>
        /// Write the specified logContents into database. 
        /// </summary>
        /// <param name="logContents">Log contents</param>
        public void Write(string logContents)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                //prepare insert command
                var insert_cmd = new MySqlCommand(
                    "INSERT INTO Log(CONTENT) VALUES(@Contents)", connection);
                insert_cmd.Parameters.AddWithValue("Contents", logContents);
                //open connection and run insert command
                connection.Open();
                insert_cmd.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Read log contents from database.
        /// </summary>
        /// <returns>Log.Contents</returns>
        public string Read()
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                var logContents = new StringBuilder();
                //prepare query command
                var query_cmd = new MySqlCommand("SELECT CONTENT FROM Log", connection);
                //open connection and run query command
                connection.Open();
                var reader = query_cmd.ExecuteReader();
                //Append data to logContenes while reading form table 
                while (reader.Read())
                {
                    logContents.Append(reader.GetString(0) + Environment.NewLine);
                }
                return logContents.ToString();
            }
        }
    }
}