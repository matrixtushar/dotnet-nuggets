using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;

namespace MariaDBConnect
{
    class MariaDBConnect
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string uid;
        private string password;

        //Constructor
        public DBConnect()
        {
            Initialize();
        }

        private void Initialize()
        {
            server = "servername";
            database = "dbname";
            uid = "username";
            password = "password";
            string connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";
            connection = new MySqlConnection(connectionString);
        }

        public bool OpenConnection()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch(MySqlException ex)
            {
                switch(ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;

                    case 1045:
                        Console.WriteLine("Invalid username / password, please try again");
                        break;
                }
                return false;
            }
        }

        public bool CloseConnection()
        {
            try
            {
                connection.Close();
                return true;
            }
            catch(MySqlException ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public void Insert(string data)
        {
            data = data.Trim();

            string query = "INSERT INTO userinfo (FirstName,LastName,DisplayName,EmployeeID,DoJ,Domain,Site,WindowsID,EmailID,Password,MailSystemO365,MailSystemOnPrem,LicenseType,JobTitle,Department,CompanyName, ReportingManager,Agent,Status) VALUES ("+data+",'"+"new"+"'"+ ");";
            Console.WriteLine(query);
            //string query = "INSERT INTO demo (empid, firstname) VALUES('3724', 'Tushar')";
            
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            } 
        }

        
        public void Update(string query)
        {
            if(this.OpenConnection() ==true)
            {
                MySqlCommand cmd = new MySqlCommand();
                cmd.CommandText = query;
                cmd.Connection = connection;
                cmd.ExecuteNonQuery();
                this.CloseConnection();
            }

        }

        /*
        public void Delete()
        {

        }
        */

        public DataTable Select(string query)
        {
            DataTable tableData = new DataTable();
            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                MySqlDataReader dataReader = cmd.ExecuteReader();
                if(dataReader.HasRows)
                {
                    tableData.Load(dataReader);
                }
                
                dataReader.Close();
                this.CloseConnection();
                return tableData;

            }
            else
            {
                return null;
            }
        }

        public int Count(string query)
        {
            int count = -1;

            if (this.OpenConnection() == true)
            {
                MySqlCommand cmd = new MySqlCommand(query, connection);
                count = int.Parse(cmd.ExecuteScalar() + "");
                this.CloseConnection();
                return count;
            }
            else
            {
                return count;
            }
        }
    }
}
