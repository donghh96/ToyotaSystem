using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace ToyotaSystem
{
    public class User : AbstractDataRecord
    {
        private string _username;
        private string _userid;
        private string _userpw;
        private string _usertype;

        public User(string userid, string userpw, string username, string usertype)
        {
            _userid = userid;
            _userpw = userpw;
            _username = username;
            _usertype = usertype;
        }

        public User(string userid)
        {
            _userid = userid;
        }

        public string Username
        {
            get => _username;
        }

        public string Userid
        {
            get => _userid;
        }

        public string Usertype
        {
            get => _usertype;
        }

        public bool LoginUser(string userid, string userpw)
        {
            if(userid == _userid && userpw ==_userpw)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string ShowDetails()
        {
            string usertypedetail;
            switch(_usertype)
            {
                case "h":
                    usertypedetail = "Help desk staff";
                    break;
                case "m":
                    usertypedetail = "Manager";
                    break;
                case "w":
                    usertypedetail = "Worker";
                    break;
                case "a":
                    usertypedetail = "Administrator";
                    break;
                default:
                    usertypedetail = null;
                    break;
            }
            return "Your ID is" + _userid.ToString() + ". Your name is "
                + _username + ". You are " + usertypedetail + ".";
        }

        public static User getUserFromDB(string userid)
        {
            string sql_login = "select userpw,username,usertype,storeid, count(*) as num from users where userid='" + userid + "'";
            string userpw;
            string username;
            string usertype;
            string storeid;
            int num;
            using (SQLiteConnection c = new SQLiteConnection(SystemConstant.TablePath))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql_login, c))
                {
                    using (SQLiteDataReader Re = cmd.ExecuteReader())
                    {
                        Re.Read();
                        userpw = Convert.ToString(Re["userpw"]);
                        username = Convert.ToString(Re["username"]);
                        usertype = Convert.ToString(Re["usertype"]);
                        storeid = Convert.ToString(Re["storeid"]);
                        num = Convert.ToInt32(Re["num"]);
                    }
                }
            }

            User tempUser;
            if (num == 1)
            {
                tempUser = new User(userid, userpw, username, usertype);
                Store.getStore().StoreId = storeid;
            }
            else
                tempUser = null;

            return tempUser;
        }

        public static DataTable GetAllUsers()
        {
            string selectqry = "select * from users";
            DataTable Table;

            using (SQLiteConnection c = new SQLiteConnection(SystemConstant.TablePath))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(selectqry, c))
                {
                    using (SQLiteDataAdapter sql_data_adap = new SQLiteDataAdapter())
                    {
                        sql_data_adap.SelectCommand = cmd;
                        Table = new DataTable();
                        Table.Locale = System.Globalization.CultureInfo.InvariantCulture;
                        sql_data_adap.Fill(Table);
                    }
                }
            }
          
            return Table;
        }

        public override string getPkColumnName()
        {
            return "userid";
        }

        public override object getPkValue()
        {
            return _userid;
        }

        public override string getTableName()
        {
            return "users";
        }
    }
}