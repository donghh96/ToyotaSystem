using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace ToyotaSystem
{
    public class CarService : AbstractDataRecord
    {
        private static int _carserviceid = new Random().Next(1, 10000);
        private double _carserviceprice;
        private string _carservicebookdate;
        private string _carservicepickdate;
        private string _carservicestatus;
        private int _carservicepriority;

        public CarService(string bookdate)
        {
            _carserviceid++;
            _carservicebookdate = bookdate;
        }

        public double CarServicePrice
        {
            get => _carserviceprice;
            set
            {
                _carserviceprice = value;
            }
        }

        public int CarServiceID
        {
            get => _carserviceid;
        }

        public string CarServiceStatus
        {
            get => _carservicestatus;
            set
            {
                _carservicestatus = value;
            }
        }

        public string CarServicePickDate
        {
            get => _carservicepickdate;
            set
            {
                _carservicepickdate = value;
            }
        }

        public string ShowDetails()
        {
            return "Service " + _carserviceid + " Booked on: " + _carservicebookdate;
        }

        public static void NewCarRepairService(string carserviceid, string bookdate, string custid, string carrego, string serviceid, string desc, string storeid)
        {
            string sql = "insert into cars_services(cars_serviceid, cars_servicesbookdate, customerid, carrego,serviceid,csdesc,storeid,cars_servicesstatus,cars_servicespriority,cars_servicesworkerid) values" +
              "('" + carserviceid + "','" + bookdate + "','" + custid + "','" + carrego + "','" + serviceid + "','" + desc + "','" + storeid + "','open','3','unassigned')";

            using (SQLiteConnection c = new SQLiteConnection(SystemConstant.TablePath))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static void AssignWorkerToRepairJob(string workerid, string jobid)
        {
            string sql = "update cars_services set cars_servicesworkerid='" + workerid + "' where cars_serviceid='" + jobid + "'";
            using (SQLiteConnection c = new SQLiteConnection(SystemConstant.TablePath))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static DataTable GetAllJobs()
        {
            string selectqry = "select * from cars_services";
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

        public static void CloseJob(string jobid)
        {
            string sql = "update cars_services set cars_servicesstatus='closed' where cars_serviceid='" + jobid + "'";

            using (SQLiteConnection c = new SQLiteConnection(SystemConstant.TablePath))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static DataTable GetJobsOfAWorker(string workerid, string csstatus)
        {
            DataTable Table;

            string sql = "select * from cars_services where cars_servicesstatus like '%" +
                csstatus + "%' and cars_servicesworkerid like '%" + workerid + "%' order by cars_servicespriority";

            using (SQLiteConnection c = new SQLiteConnection(SystemConstant.TablePath))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
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

        public static DataTable GetWorkerJobsOfManager(string managerid)
        {
            DataTable Table;

            string sql = "select userid, username, count(cars_serviceid) as job_number " +
                         "FROM users left join cars_services on userid = cars_servicesworkerid " +
                         "where managerid like '%" + managerid + "%' and cars_servicesstatus = 'open'";
            using (SQLiteConnection c = new SQLiteConnection(SystemConstant.TablePath))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
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

        public static DataTable GetJobsOfACar(string carreg)
        {
            DataTable Table;

            string sql = "select * from cars_services where carrego like '%" + carreg + "%'";
            using (SQLiteConnection c = new SQLiteConnection(SystemConstant.TablePath))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
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
            return "cars_serviceid";
        }

        public override object getPkValue()
        {
            return _carserviceid;
        }

        public override string getTableName()
        {
            return "cars_services";
        }
    }
}