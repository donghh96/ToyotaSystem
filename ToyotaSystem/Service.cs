using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace ToyotaSystem
{
    public class Service : AbstractDataRecord 
    {
        private string _serviceId;
        private string _serviceName;
        private List<Car> _carList;

        public Service(string serviceId)
        {
            _serviceId = serviceId;
        }

        public string ServiceName
        {
            get => _serviceName;
        }

        public List<Car> CarList
        {
            get => _carList;
            set
            {
                _carList = value;
            }
        }

        public string ServiceId
        {
            get => _serviceId;
        }

        public string ShowDetails()
        {
            return "Service Name: " + _serviceName;
        }

        public void AddCar(Car car)
        {
            _carList.Add(car);
        }

        public static DataTable GetAllServices()
        {
            string selectqry = "select * from services";
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
            return "serviceid";
        }
        public override object getPkValue()
        {
            return _serviceId;
        }
        public override string getTableName()
        {
            return "services";
        }
    }
}