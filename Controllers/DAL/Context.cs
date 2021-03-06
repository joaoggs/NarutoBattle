﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace Controllers.DAL
{
    public class Context
    {
        public SQLiteConnection conn;
        public Context()
        {
            conn = new SQLiteConnection("Data Source=|DataDirectory|NBDB.db;Version=3;");
        }

        //===================== Account =====================

        public bool Change_Account(string account, string login, string pass, string victories, string loses, string type)
        {
            try
            {
                conn.Open();
                string sql = "UPDATE Account SET Login = '" + login + "', Password ='" + pass + "', Victories =" + victories + ", Loses =" + loses + ", Type = '" + type + "' WHERE Login = '" + account + "'";
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch
            {
                conn.Close();
                return false;
            }
        }

        public bool Reset_Pass_Account(string account, string password)
        {
            try
            {
                conn.Open();
                string sql = "UPDATE Account SET Password='" + password + "' WHERE Login='" + account + "'";
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();

                conn.Close();
                return true;
            }
            catch
            {
                conn.Close();
            }
            return false;
        }

        public bool Delete_Account(string account)
        {
            try
            {
                conn.Open();
                string sql = "DELETE FROM Account WHERE Login='" + account + "'";
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();

                conn.Close();
                return true;
            }
            catch
            {
                conn.Close();
            }
            return false;
        }

        public bool Reset_Account(string account)
        {
            try
            {

                conn.Open();
                string sql = "UPDATE Account SET Victories=0,Loses=0 WHERE Login='" + account + "'";
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }
            catch
            {
                conn.Close();
            }
            return false;

        }

        public List<string> Account_Status(string account)
        {
            try
            {
                conn.Open();
                List<string> status = new List<string>();
                string sql = "SELECT Victories, Loses FROM Account WHERE Login ='" + account + "'";
                using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();

                        status.Add(reader["Victories"].ToString());
                        status.Add(reader["Loses"].ToString());
                    }

                }


                conn.Close();
                return status;
            }
            catch
            {
                conn.Close();
            }
            return null;
        }

        public bool registerAccount(string name, string pass)
        {
            conn.Open();
            string sqlSelect = "SELECT Login FROM Account WHERE Login = '" + name + "'";
            SQLiteCommand commandSelect = new SQLiteCommand(sqlSelect, conn);
            int rows = commandSelect.ExecuteNonQuery();
            if (rows < 1)
            {
                string sql = "INSERT INTO Account (Login, Password, Victories, Loses) VALUES ('" + name + "', '" + pass + "', 0,0)";
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }

            conn.Close();
            return false;
        }

        public bool registerAccount(string login, string pass, string victories, string loses, string type)
        {
            conn.Open();
            string sqlSelect = "SELECT Login FROM Account WHERE Login = '" + login + "'";
            SQLiteCommand commandSelect = new SQLiteCommand(sqlSelect, conn);
            int rows = commandSelect.ExecuteNonQuery();
            if (rows < 1)
            {
                string sql = "INSERT INTO Account (Login, Password, Victories, Loses, Type) VALUES ('" + login + "', '" + pass + "', " + victories + "," + loses + ", " + type + "'')";
                SQLiteCommand command = new SQLiteCommand(sql, conn);
                command.ExecuteNonQuery();
                conn.Close();
                return true;
            }

            conn.Close();
            return false;
        }

        //===================== Others =====================

        public string Skill_Type(string char_select, int skill_select)
        {

            conn.Open();
            string sql = "SELECT Type FROM Skills WHERE Name = '" + char_select + "' AND Skill = " + skill_select;
            string type = "";
            using (SQLiteCommand command = new SQLiteCommand(sql, conn))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    reader.Read();
                    type = reader["Type"].ToString();

                }

            }

            conn.Close();

            return type;
        }

        public List<string> Skills(string skillNumber, string character)
        {
            List<string> chakras = new List<string>();
            try
            {
                conn.Open();
                string sql = "SELECT Taijutsu, Bloodline, Ninjutsu, Genjutsu FROM Skills WHERE Name = '" + character + "' AND Skill = " + skillNumber;

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader rdr = cmd.ExecuteReader())
                    {
                        rdr.Read();

                        chakras.Add(rdr["Taijutsu"].ToString());
                        chakras.Add(rdr["Bloodline"].ToString());
                        chakras.Add(rdr["Ninjutsu"].ToString());
                        chakras.Add(rdr["Genjutsu"].ToString());
                    }

                    conn.Close();
                }

                conn.Close();
                return chakras;
            }
            catch
            {
                conn.Close();
                chakras.Add("0");
                chakras.Add("0");
                chakras.Add("0");
                chakras.Add("0");

                return chakras;
            }
        }

        public string damage_skill(string skill, string name)
        {
            conn.Open();
            string sql = "SELECT Damage FROM Skills WHERE Name = '" + name + "' AND Skill = '" + skill + "'";
            using (SQLiteCommand command = new SQLiteCommand(sql, conn))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    reader.Read();

                    try
                    {
                        string damage = reader["Damage"].ToString();
                        conn.Close();

                        if (damage == "")
                            damage = "0";
                        return damage;
                    }
                    catch
                    {
                        conn.Close();
                        return "0";
                    }
                }

            }
        }

        public string Defense_Skill(string skill, string name)
        {
            conn.Open();
            string sql = "SELECT Defense, Turn FROM Skills WHERE Name = '" + name + "' AND Skill = '" + skill + "'";
            using (SQLiteCommand command = new SQLiteCommand(sql, conn))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {
                    reader.Read();

                    string defense = reader["Defense"].ToString();
                    if (defense == "")
                    {
                        defense = "0";
                    }
                    else
                    {
                        defense = defense +"t" + reader["Turn"].ToString();
                    }
                        
                    conn.Close();
                    return defense;
                }

            }
        }

        public string heal_skill(string skill, string name)
        {
            conn.Open();
            string sql = "SELECT Heal FROM Skills WHERE Name = '" + name + "' AND Skill = '" + skill + "'";
            using (SQLiteCommand command = new SQLiteCommand(sql, conn))
            {
                using (SQLiteDataReader reader = command.ExecuteReader())
                {

                    reader.Read();
                    try
                    {
                        string heal = reader["Heal"].ToString();
                        conn.Close();
                        if (heal == "")
                            heal = "0";
                        return heal;
                    }
                    catch
                    {
                        conn.Close();
                        return "0";
                    }
                }
            }



        }

        //===================== Others =====================

        public DataTable Grid_DataTable(string type)
        {
            string sql = "";
            conn.Open();
            if (type == "Account")
                sql = "SELECT Login, Password, Victories, Loses, Type FROM Account";
            else if (type == "Skills")
                sql = "SELECT Name, Skill, Type, Damage, Heal, Taijutsu, Bloodline, Ninjutsu, Genjutsu FROM Skills";
            else if (type == "Character")
                sql = "SELECT Name FROM Character";

            SQLiteCommand command = new SQLiteCommand(sql, conn);
            command.ExecuteNonQuery();

            SQLiteDataAdapter data = new SQLiteDataAdapter(command);
            DataTable dt = new DataTable("Account");
            data.Fill(dt);
            data.Update(dt);

            conn.Close();
            return dt;
        }

        public bool WL(string account, string result)
        {
            try
            {
                string sql = "";
                conn.Open();

                if (result == "Winner")
                    sql = "UPDATE Account SET Victories = (Victories + 1) WHERE Login = '" + account + "'";
                else if (result == "Loser")
                    sql = "UPDATE Account SET Loses =(Loses + 1) WHERE Login ='" + account + "'";

                using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                {
                    command.ExecuteNonQuery();

                }
                conn.Close();
                return true;
            }
            catch
            {
                conn.Close();
                return false;
            }

        }

        public bool loginAuthentication(string login, string pass)
        {
            try
            {
                conn.Open();
                string sql = "SELECT Login, Password FROM Account WHERE Login = '" + login + "' AND Password = '" + pass + "'";
                using (SQLiteCommand command = new SQLiteCommand(sql, conn))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        reader.Read();
                        if (login == reader["Login"].ToString() && pass == reader["Password"].ToString())
                        {
                            conn.Close();
                            return true;
                        }
                    }

                }
            }
            catch
            {
                conn.Close();
            }

            conn.Close();
            return false;
        }

        public List<string> return_Characters()
        {
            List<string> chars = new List<string>();
            conn.Open();

            string sql = "SELECT Name FROM Character";
            using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
            {
                using (SQLiteDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        chars.Add(rdr["Name"].ToString());
                    }
                }
            }
            conn.Close();
            return chars;
        }

    }
}
