using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace ToyotaSystem
{
    public class Customer : AbstractDataRecord
    {
        private string _username;
        private string _userid;
        private string _useraddress;
        private string _userphone;
        private string _useremail;
        private double _userbalance = 0.0;

        public Customer(string userid, string username, string userphone, string useraddress)
        {
            _userid = userid;
            _username = username;
            _userphone = userphone;
            _useraddress = useraddress;
        }

        public Customer(string userid)
        {
            _userid = userid;
        }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
            }
        }

        public string Userid
        {
            get => _userid;
        }

        public string Useraddress
        {
            get => _useraddress;
            set
            {
                _useraddress = value;
            }
        }

        public string Userphone
        {
            get => _userphone;
            set
            {
                _userphone = value;
            }
        }

        public double Userbalance
        {
            get => _userbalance;
        }

        public string Useremail
        {
            get => _useremail;
            set
            {
                _useremail = value;
            }
        }

        public string ShowDetails()
        {
            return "Your ID is" + _userid.ToString() + ". Your name is "
                + _username + ". You are Customer.";
        }

        public void AddBalance(double addnum)
        {
            _userbalance += addnum;
        }

        public static void AddCustomer(string custid, string custname, string custphone, string custaddress)
        {
            string sql = "insert into customers(customerid, customername, customerphone, customeraddress) values" +
              "('" + custid + "','" + custname + "','" + custphone + "','" + custaddress + "')";

            using (SQLiteConnection c = new SQLiteConnection(SystemConstant.TablePath))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static bool IsCustomerRegistered(string custid)
        {
            string sql = "select count(*) as num from customers where customerid='" + custid + "'";
            int custnum;
            using (SQLiteConnection c = new SQLiteConnection(SystemConstant.TablePath))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                {
                    using (SQLiteDataReader Re = cmd.ExecuteReader())
                    {
                        Re.Read();
                        custnum = Convert.ToInt32(Re["num"]);
                    }
                }
            }
            return custnum > 0 ? true : false;
        }

        public static DataTable GetAllCustomers()
        {
            string selectqry = "select * from customers";
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
        public static DataTable GetCustomer(string customerid)
        {
            string selectqry = "select * from customers where customerid like '%" + customerid + "%'";
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
            return "customerid";
        }

        public override object getPkValue()
        {
            return _userid;
        }

        public override string getTableName()
        {
            return "customers";
        }
    }
}