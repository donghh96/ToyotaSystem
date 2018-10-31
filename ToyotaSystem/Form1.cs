using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Diagnostics;

namespace ToyotaSystem
{
    public partial class Form1 : Form
    {
        SQLiteConnection m_dbConnection;
        string path, dbPath, TablePath;
        string recordClass, adminClick_id;
        bool loginStatus = false;
        Store currentStore = Store.getStore();

        // login user with id and password
        private void buttonLogin_Click(object sender, EventArgs e)
        {
            User currentUser;
            currentUser = getUserFromDB(textBoxUserid.Text);
            if (currentUser != null && currentUser.LoginUser(textBoxUserid.Text, textBoxPw.Text))
            {
                currentStore = getStoreFromDB(currentStore.StoreId);
                labelWelcome.Text = currentUser.Username + ": "
                    + currentUser.ShowDetails();
                labelStore.Text = currentStore.ShowDetails();
                loginStatus = true;

                switch (currentUser.Usertype)
                {
                    case "h":
                        groupBoxH.Visible = true;
                        //groupBoxM.Visible = false;
                        //groupBoxW.Visible = false;
                        //groupBoxA.Visible = false;
                        break;
                    case "m":
                        tabControl1.TabPages.Insert(1, tabManager);
                        //groupBoxM.Visible = true;
                        break;
                    case "w":
                        tabControl1.TabPages.Insert(1, tabWorker);
                        groupBoxW.Visible = true;
                        break;
                    case "a":
                        tabControl1.TabPages.Insert(1, tabAdmin);
                        groupBoxA.Visible = true;
                        break;

                    default:
                        groupBoxH.Visible = false;
                        //groupBoxM.Visible = false;
                        //groupBoxW.Visible = false;
                        //groupBoxA.Visible = false;
                        break;
                }
            }
            else
            {
                labelWelcome.Text = textBoxUserid.Text + ": Your login is not correct!";
                loginStatus = false;
            }
            if (loginStatus)
            {
                labelUsername.Visible = false;
                textBoxUserid.Visible = false;
                labelPw.Visible = false;
                textBoxPw.Visible = false;
                buttonLogin.Visible = false;
            }
        }

        // retrive user details from sqlite by userid
        private User getUserFromDB(string userid)
        {
            return User.getUserFromDB(userid);
        }

        private Store getStoreFromDB(string storeid)
        {
            return Store.getStoreFromDB(storeid);
        }

        private void postCarServiceToDB(string carserviceid, string bookdate, string custid, string carrego, string serviceid, string desc, string storeid)
        {
            if (Car.isCarSoldByToyota(carrego))
            {
                MessageBox.Show("Your car is not sold by our store!");
            }
            else if (Customer.IsCustomerRegistered(custid))
            {
                MessageBox.Show("You customer ID is not eixst! Please register a new customer ID");
            }
            else
            {
                CarService.NewCarRepairService(carserviceid, bookdate, custid, carrego, serviceid, desc, storeid);
            }
        }

        private void postCustomerToDB(string custid, string custname, string custphone, string custaddress)
        {
            Customer.AddCustomer(custid, custname, custphone, custaddress);
        }

        private void postCarToDB(string carrego, string cardate, double carprice)
        {
            Car.AddCar(carrego, cardate, carprice);
            MessageBox.Show("The car " + carrego + " has been added to our store!");
        }

        private void postWorkJobToDB(string workerid, string jobid)
        {
            // CarService ?
            CarService.AssignWorkerToRepairJob(workerid, jobid);
            MessageBox.Show("The job has been assigned to: " + workerid);
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void buttonBookJob_Click(object sender, EventArgs e)
        {
            if (textBox_CustomerID.Text.Length == 0 || textBox_BookingDate.Text.Length == 0 || textBox_CarRego.Text.Length == 0)
            {
                MessageBox.Show("Please fill out the required information!");
            }
            else if (!checkBoxPanel.Checked && !checkBoxElectrical.Checked && !checkBoxMechanical.Checked)
            {
                MessageBox.Show("Please selecet at least one service!");
            }
            else if (!radioButton_nz1.Checked && !radioButton_nz2.Checked && !radioButton_nz3.Checked && !radioButton_nz4.Checked)
            {
                MessageBox.Show("Please selecet a store outlet!");
            }
            else if (checkBoxNew.Checked)
            {
                Customer cust = new Customer(textBox_CustomerID.Text, textBoxName.Text, textBoxPhone.Text, textBoxAddress.Text);
                currentStore.CustList = new List<Customer>();
                currentStore.AddCust(cust);
                postCustomerToDB(textBox_CustomerID.Text, textBoxName.Text, textBoxPhone.Text, textBoxAddress.Text);
                List<string> serviceids = new List<string>();
                // default storeid nz1
                string storeid = "nz1";

                if (checkBoxPanel.Checked)
                {
                    serviceids.Add("sid1");
                }
                if (checkBoxElectrical.Checked)
                {
                    serviceids.Add("sid2");
                }
                if (checkBoxMechanical.Checked)
                {
                    serviceids.Add("sid3");
                }


                if (radioButton_nz1.Checked)
                {
                    storeid = "nz1";
                }
                else if (radioButton_nz2.Checked)
                {
                    storeid = "nz2";
                }
                else if (radioButton_nz3.Checked)
                {
                    storeid = "nz3";
                }
                else if (radioButton_nz4.Checked)
                {
                    storeid = "nz4";
                }

                for (int i = 0; i < serviceids.Count; i++)
                {
                    CarService cs = new CarService(textBox_BookingDate.Text);
                    currentStore.CarServiceList = new List<CarService>();
                    currentStore.AddCarService(cs);
                    postCarServiceToDB(cs.CarServiceID.ToString(), textBox_BookingDate.Text, textBox_CustomerID.Text, textBox_CarRego.Text, serviceids[i], textBox_Description.Text, storeid);
                }
            }
            else if (!checkBoxNew.Checked)
            {

                List<string> serviceids = new List<string>();
                // default storeid nz1
                string storeid = "nz1";

                if (checkBoxPanel.Checked)
                {
                    serviceids.Add("sid1");
                }
                if (checkBoxElectrical.Checked)
                {
                    serviceids.Add("sid2");
                }
                if (checkBoxMechanical.Checked)
                {
                    serviceids.Add("sid3");
                }


                if (radioButton_nz1.Checked)
                {
                    storeid = "nz1";
                }
                else if (radioButton_nz2.Checked)
                {
                    storeid = "nz2";
                }
                else if (radioButton_nz3.Checked)
                {
                    storeid = "nz3";
                }
                else if (radioButton_nz4.Checked)
                {
                    storeid = "nz4";
                }
                for (int i = 0; i < serviceids.Count; i++)
                {
                    CarService cs = new CarService(textBox_BookingDate.Text);
                    currentStore.CarServiceList = new List<CarService>();
                    currentStore.AddCarService(cs);
                    postCarServiceToDB(cs.CarServiceID.ToString(), textBox_BookingDate.Text, textBox_CustomerID.Text, textBox_CarRego.Text, serviceids[i], textBox_Description.Text, storeid);
                }
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            loginStatus = false;
            groupBoxH.Visible = false;
            //groupBoxM.Visible = false;
            groupBoxW.Visible = false;

            labelWelcome.Text = "Welcome to Repair Services System! Please login your Employ ID for management!";
            labelStore.Text = "Toyota Store Service Order";
            if (!loginStatus)
            {
                labelUsername.Visible = true;
                textBoxUserid.Visible = true;
                labelPw.Visible = true;
                textBoxPw.Visible = true;
                buttonLogin.Visible = true;

            }
        }

        private void buttonShowWorker_Click(object sender, EventArgs e)
        {
            dataGridViewShowWorkers.DataSource = CarService.GetWorkerJobsOfManager(textBoxUserid.Text);
        }

        private void buttonShowCar_Click(object sender, EventArgs e)
        {
            dataGridViewShowCar.DataSource = CarService.GetJobsOfACar(textBox_H_CarGego.Text);
        }

        private void buttonShowCust_Click(object sender, EventArgs e)
        {
            dataGridViewShowCust.DataSource = Customer.GetCustomer(textBox_H_CustomerID.Text);
        }

        private void checkBoxNew_CheckedChanged(object sender, EventArgs e)
        {
            labelName.Visible = !labelName.Visible;
            textBoxName.Visible = !textBoxName.Visible;
            labelPhone.Visible = !labelPhone.Visible;
            textBoxPhone.Visible = !textBoxPhone.Visible;
            labelAddress.Visible = !labelAddress.Visible;
            textBoxAddress.Visible = !textBoxAddress.Visible;
        }

        private void radioButtonnz1_CheckedChanged(object sender, EventArgs e)
        {
            currentStore = getStoreFromDB("nz1");
            labelStoreAddress.Text = "Store NZ1: " + currentStore.OutletAddress;
        }


        private void radioButtonnz2_CheckedChanged(object sender, EventArgs e)
        {
            currentStore = getStoreFromDB("nz2");
            labelStoreAddress.Text = "Store NZ2: " + currentStore.OutletAddress;
        }

        private void radioButtonnz3_CheckedChanged(object sender, EventArgs e)
        {
            currentStore = getStoreFromDB("nz3");
            labelStoreAddress.Text = "Store NZ3: " + currentStore.OutletAddress;
        }

        private void radioButtonnz4_CheckedChanged(object sender, EventArgs e)
        {
            currentStore = getStoreFromDB("nz4");
            labelStoreAddress.Text = "Store NZ4: " + currentStore.OutletAddress;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewShowCust_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void buttonAddCar_Click(object sender, EventArgs e)
        {
            try
            {
                Car car = new Car(textBoxCRego.Text, textBoxCarDate.Text, double.Parse(textBoxCarPrice.Text));
                currentStore.CarList = new List<Car>();
                currentStore.AddCar(car);
                postCarToDB(textBoxCRego.Text, textBoxCarDate.Text, double.Parse(textBoxCarPrice.Text));
            }
            catch (Exception err)
            {
                err.ToString();
                MessageBox.Show("Please fill correct information! " + err.ToString());
            }
        }

        private void dataGridViewShowCar_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string strcolumn = dataGridViewShowCar.Columns[e.ColumnIndex].HeaderText;
                string recordId = dataGridViewShowCar.Rows[e.RowIndex].Cells[0].Value.ToString();
                string value = dataGridViewShowCar.CurrentCell.Value.ToString();

                IDataRecord record = getDataRecord("cars", recordId);
                record.updateProperty(strcolumn, value);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }
        }

        private void dataGridViewShowCust_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string strcolumn = dataGridViewShowCust.Columns[e.ColumnIndex].HeaderText;
                string recordId = dataGridViewShowCust.Rows[e.RowIndex].Cells[0].Value.ToString();
                string value = dataGridViewShowCust.CurrentCell.Value.ToString();

                IDataRecord record = getDataRecord("cars", recordId);
                record.updateProperty(strcolumn, value);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }
        }

        private void dataGridViewAdmin_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string strcolumn = dataGridViewAdmin.Columns[e.ColumnIndex].HeaderText;
                string recordId = dataGridViewAdmin.Rows[e.RowIndex].Cells[0].Value.ToString();
                string value = dataGridViewAdmin.CurrentCell.Value.ToString();
                IDataRecord record = getDataRecord(recordClass, recordId);
                record.updateProperty(strcolumn, value);
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message.ToString());
            }
        }

        private IDataRecord getDataRecord(string recordClass, string recordId)
        {
            if (recordClass.Equals("cars")) //TODO change real one
            {
                return new Car(recordId);
            }
            else if (recordClass.Equals("users"))
            {
                return new User(recordId);
            }
            else if (recordClass.Equals("customers"))
            {
                return new Customer(recordId);
            }
            else if (recordClass.Equals("stores"))
            {
                return new Store(recordId);
            }
            else if (recordClass.Equals("services"))
            {
                return new Service(recordId);
            }
            else if (recordClass.Equals("car_services"))
            {
                return new CarService(recordId);
            } else
            {
                throw new ArgumentOutOfRangeException("");
            }
        }

        private void buttonShowJobs_Click(object sender, EventArgs e)
        {
            OpenDB();
            string csstatus = "";
            string workerid = textBox_M_WorkerID.Text.Length == 0 ? "" : textBox_M_WorkerID.Text;
            if (checkBoxAllOpen.Checked && !checkBoxAllClose.Checked)
            {
                csstatus = "open";
            }
            else if (!checkBoxAllOpen.Checked && checkBoxAllClose.Checked)
            {
                csstatus = "closed";
            }
            else if (checkBoxAllOpen.Checked && checkBoxAllClose.Checked)
            {
                csstatus = "";
            }

            if (workerid == "")
            {
                dataGridViewOpen.DataSource = CarService.GetAllJobs();
            } else
            {
                dataGridViewOpen.DataSource = CarService.GetJobsOfAWorker("", csstatus);
            }
            //string selectqry = "select * from cars_services where cars_servicesstatus like '%" +
            //    csstatus + "%' and cars_servicesworkerid like '%" + workerid + "%' order by cars_servicespriority";
            //SQLiteCommand cmd = new SQLiteCommand(selectqry, m_dbConnection);

            //SQLiteDataAdapter sql_data_adap = new SQLiteDataAdapter();
            //sql_data_adap.SelectCommand = cmd;
            //DataTable ManagerTable = new DataTable();
            //ManagerTable.Locale = System.Globalization.CultureInfo.InvariantCulture;
            //sql_data_adap.Fill(ManagerTable);
            //dataGridViewOpen.DataSource = ManagerTable;
            //m_dbConnection.Close();
        }

        private void buttonAssginJob_Click(object sender, EventArgs e)
        {
            try
            {
                postWorkJobToDB(textBox_M_WorkerID.Text, textBox_M_JobID.Text);
            }
            catch (Exception err)
            {
                err.ToString();
                MessageBox.Show("Please fill correct information! " + err.ToString());
            }
        }

        private void buttonShowMyJobs_Click(object sender, EventArgs e)
        {
            string csstatus = "";
            string workerid = textBoxUserid.Text;
            if (checkBoxMyOpen.Checked && !checkBoxMyClose.Checked)
            {
                csstatus = "open";
            }
            else if (!checkBoxMyOpen.Checked && checkBoxMyClose.Checked)
            {
                csstatus = "closed";
            }
            else if (checkBoxMyOpen.Checked && checkBoxMyClose.Checked)
            {
                csstatus = "";
            }

            dataGridView_W_Jobs.DataSource = CarService.GetJobsOfAWorker(workerid, csstatus);
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            int row = dataGridView_W_Jobs.CurrentRow.Index;
            string jobid = dataGridView_W_Jobs.Rows[row].Cells["cars_serviceid"].Value.ToString();
            string jobstatus = dataGridView_W_Jobs.Rows[row].Cells["cars_servicesstatus"].Value.ToString();

            if (jobstatus == "closed")
            {
                MessageBox.Show("The job " + jobid + " is already closed, please select an open one.");
            }
            else
            {
                CarService.CloseJob(jobid);
                MessageBox.Show("The job " + jobid + " has been succesfully closed!");
            }
        }

        private void buttonShowUsers_Click(object sender, EventArgs e)
        {
            recordClass = "users";
            adminClick_id = "userid";
            dataGridViewAdmin.DataSource = User.GetAllUsers();
        }

        private void buttonShowCustomers_Click(object sender, EventArgs e)
        {
            recordClass = "customers";
            adminClick_id = "customerid";
            dataGridViewAdmin.DataSource = Customer.GetAllCustomers();
        }

        private void buttonShowStore_Click(object sender, EventArgs e)
        {
            recordClass = "stores";
            adminClick_id = "storeid";
            dataGridViewAdmin.DataSource = Store.GetAllStores();
        }

        private void buttonShowCars_Click(object sender, EventArgs e)
        {
            recordClass = "cars";
            adminClick_id = "carrego";
            dataGridViewAdmin.DataSource = Car.GetAllCars();
        }

        private void buttonShowService_Click(object sender, EventArgs e)
        {
            recordClass = "services";
            adminClick_id = "serviceid";
            dataGridViewAdmin.DataSource = Service.GetAllServices();
        }

        private void buttonShowAllJobs_Click(object sender, EventArgs e)
        {
            recordClass = "cars_services";
            adminClick_id = "cars_serviceid";
            dataGridViewAdmin.DataSource = CarService.GetAllJobs();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string x = AppDomain.CurrentDomain.BaseDirectory;
            textBox_BookingDate.Text = DateTime.Today.ToShortDateString();
            path = x + "toyota.db";
            dbPath = x + "toyota.db; Version=3";
            TablePath = "Data source=" + x + "toyota.db";
            SystemConstant.TablePath = TablePath;

            // Hide some tabs
            tabControl1.TabPages.Remove(tabManager);
            tabControl1.TabPages.Remove(tabAdmin);
            tabControl1.TabPages.Remove(tabWorker);
        }

        void OpenDB()
        {
            m_dbConnection = new SQLiteConnection(TablePath);
            m_dbConnection.Open();
        }
    }
}
