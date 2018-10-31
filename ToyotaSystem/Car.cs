using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace ToyotaSystem
{
    public class Car : AbstractDataRecord
    {
        private string _carRego;
        private string _carSoldDate;
        private double _carSoldPrice;
        private List<Service> _serviceList;

        public Car(string carrego, string cardate, double carprice)
        {
            _carRego = carrego;
            _carSoldDate = cardate;
            _carSoldPrice = carprice;
        }

        public Car(string carrego)
        {
            this._carRego = carrego;
        }

        public string CarRego
        {
            get => _carRego;
            set
            {
                _carRego = value;
            }
        }

        public List<Service> ServiceList
        {
            get => _serviceList;
            set
            {
                _serviceList = value;
            }
        }

        public string ShowDetails()
        {
            return "Car Rego Number: " + _carRego + ", Sold on: " + _carSoldDate + ", Sold at $" + _carSoldPrice;
        }

        public void AddService(Service service)
        {
            _serviceList.Add(service);
        }

        public static void AddCar(string carrego, string cardate, double carprice)
        {
            string sql = "insert into cars(carrego, cardate, carprice) values" + "('" + carrego + "','" + cardate + "','" + carprice.ToString() + "')";

            using (SQLiteConnection c = new SQLiteConnection(SystemConstant.TablePath))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public static bool isCarSoldByToyota(string carrego)
        {

            string sql = "select count(*) as num from cars where carrego='" + carrego + "'";
            int carnum;
            using (SQLiteConnection c = new SQLiteConnection(SystemConstant.TablePath))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql, c))
                {
                    using (SQLiteDataReader Re = cmd.ExecuteReader())
                    {
                        Re.Read();
                        carnum = Convert.ToInt32(Re["num"]);
                    }
                }
            }
            return carnum == 1 ? true : false;
        }

        public static DataTable GetAllCars()
        {
            string selectqry = "select * from cars";
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
            return "carrego";
        }

        public override object getPkValue()
        {
            return _carRego;
        }

        public override string getTableName()
        {
            return "cars";
        }
    }
}