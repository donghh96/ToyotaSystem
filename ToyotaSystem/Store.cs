using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace ToyotaSystem
{
    public class Store : AbstractDataRecord
    {
        private static Store myStore;
        private string _storeid;
        private string _outletaddress;
        private string _storename;
        private string _outletname;
        private List<User> _userList;
        private List<Customer> _custList;
        private string _storehours;
        private string _storestatus;
        private List<CarService> _carservicelist;
        private List<Car> _carList;

        public Store()
        {
        }
        public Store(string storeid)
        {
            _storeid = storeid;
        }

        public List<User> UserList
        {
            get => _userList;
            set
            {
                _userList = value;
            }
        }

        public List<Customer> CustList
        {
            get => _custList;
            set
            {
                _custList = value;
            }
        }

        public string OutletName
        {
            get => _outletname;
            set
            {
                _outletname = value;
            }
        }

        public string StoreId
        {
            get => _storeid;
            set
            {
                _storeid = value;
            }
        }

        public string OutletAddress
        {
            get => _outletaddress;
            set
            {
                _outletaddress = value;
            }
        }

        public string StoreHours
        {
            get => _storehours;
            set
            {
                _storehours = value;
            }
        }

        public string StoreStatus
        {
            get => _storestatus;
            set
            {
                _storestatus = value;
            }
        }

        public List<CarService> CarServiceList
        {
            get => _carservicelist;
            set
            {
                _carservicelist = value;
            }
        }

        public List<Car> CarList
        {
            get => _carList;
            set
            {
                _carList = value;
            }
        }

        public string ShowDetails()
        {
            _storename = "Toyota Store ";
            return _storename + _storeid + ", "+ _outletname + "," + _outletaddress;
        }

        public static Store getStore()
        {
            if (myStore == null)
            {
                myStore = new Store();
            }
            return myStore;
        }

        public void AddUser(User user)
        {
            _userList.Add(user);
        }

        public void AddCust(Customer cust)
        {
            _custList.Add(cust);
        }

        public void AddCarService(CarService cs)
        {
            _carservicelist.Add(cs);
        }

        public void AddCar(Car car)
        {
            _carList.Add(car);
        }

        public static Store getStoreFromDB(string storeid)
        {
            string sql_login = "select storename,storeaddress,storestatus,storehours, count(*) as num from stores where storeid='" + storeid + "'";
            string storename;
            string storeaddress;
            string storestatus;
            string storehours;
            int num;
            Store currentStore = getStore();

            using (SQLiteConnection c = new SQLiteConnection(SystemConstant.TablePath))
            {
                c.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(sql_login, c))
                {
                    using (SQLiteDataReader Re = cmd.ExecuteReader())
                    {
                        Re.Read();
                        storename = Convert.ToString(Re["storename"]);
                        storeaddress = Convert.ToString(Re["storeaddress"]);
                        storestatus = Convert.ToString(Re["storestatus"]);
                        storehours = Convert.ToString(Re["storehours"]);
                        num = Convert.ToInt32(Re["num"]);
                    }
                }
            }

            if (num == 1)
            {
                currentStore.OutletAddress = storeaddress;
                currentStore.OutletName = storename;
                currentStore.StoreHours = storehours;
                currentStore.StoreStatus = storestatus;
            }
            return currentStore;
        }

        public static DataTable GetAllStores()
        {
            string selectqry = "select * from stores";
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
            return "storeid";
        }

        public override object getPkValue()
        {
            return _storeid;
        }

        public override string getTableName()
        {
            return "stores";
        }
    }
}