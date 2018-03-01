using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations.Model;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data.Sql;

namespace MaterialPanel
{
    public static class toolkit
    {
        private static Random random = new Random();

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static void CreateTable()
        {
            var sConn = new SQLiteConnection("Data Source=buttons.sqlite;Version=3;");
            sConn.Open();
            using (SQLiteCommand mCmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS buttons ('buttonid' TEXT PRIMARY KEY ,'buttonname' TEXT,'buttontype' TEXT,'buttonscript' TEXT);", sConn))
            {
                mCmd.ExecuteNonQuery();
            }
            using (SQLiteCommand mCmd = new SQLiteCommand("CREATE UNIQUE INDEX IF NOT EXISTS unibtn ON buttons (buttonid)", sConn))
            {
                mCmd.ExecuteNonQuery();
            }


        }

        public static void DeleteButton(string btnID)
        {
            var sConn = new SQLiteConnection("Data Source=buttons.sqlite;Version=3;");
            sConn.Open();
            
            using (SQLiteCommand mCmd = new SQLiteCommand("delete from buttons WHERE buttonid='" + btnID + "';", sConn))
            {
                mCmd.ExecuteNonQuery();
                Console.WriteLine(mCmd.CommandText);
            }

        }

        public static string getScript(String btnID)
        {
            using (SQLiteConnection connect = new SQLiteConnection("Data Source=buttons.sqlite;Version=3;"))
            {
                String holder = "";
                connect.Open();
                using (SQLiteCommand fmd = connect.CreateCommand())
                {
                    fmd.CommandText = @"SELECT * FROM buttons where buttonid='"+btnID+"';";

                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        holder = (string)r["buttonscript"];
                    }

                    connect.Close();

                }

                return holder;
            }
        }

        public static string getType(String btnID)
        {
            using (SQLiteConnection connect = new SQLiteConnection("Data Source=buttons.sqlite;Version=3;"))
            {
                String holder = "";
                connect.Open();
                using (SQLiteCommand fmd = connect.CreateCommand())
                {
                    fmd.CommandText = @"SELECT * FROM buttons where buttonid='" + btnID + "';";

                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        holder = (string)r["buttontype"];
                    }

                    connect.Close();

                }

                return holder;
            }
        }

        public static void GetImportedButtonList()
        {
            List<string> ImportedFiles = new List<string>();

            using (SQLiteConnection connect = new SQLiteConnection("Data Source=buttons.sqlite;Version=3;"))
            {
                connect.Open();
                using (SQLiteCommand fmd = connect.CreateCommand())
                {
                    fmd.CommandText = @"SELECT * FROM 'buttons';";
                    SQLiteDataReader r = fmd.ExecuteReader();
                    while (r.Read())
                    {
                        MainWindow.generateAndAddBtn((string)r["buttonid"], (string)r["buttonname"]);
                    }

                    connect.Close();
                }
            }
            return;
        }


    }
}
