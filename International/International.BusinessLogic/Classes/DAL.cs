using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace International.BusinessLogic.Classes
{
    class DataAccessLayer : IDisposable
    {

        private SqlDataAdapter myAdapter;
        private SqlConnection conn;
        string connectionString = "";


        /// <constructor>
        /// Initialise Connection
        /// </constructor>
        public DataAccessLayer()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["InternationalADO"].ToString();   
          
            myAdapter = new SqlDataAdapter();
            conn = new SqlConnection(connectionString);
        }

        /// <method>
        /// Open Database Connection if Closed or Broken
        /// </method>
        private SqlConnection openConnection()
        {
            if (conn.State == ConnectionState.Closed || conn.State == ConnectionState.Broken)
            {
                conn.Open();
            }
            return conn;
        }

        /// <method>
        /// Select Query with Parameter
        /// </method>
        public DataTable ExecuteSPWithReturnDataTableWithParameter(String _spName, Dictionary<string, string> spParameters)
        {
            using (SqlCommand myCommand = new SqlCommand())
            {
                DataTable dataTable = new DataTable();
                dataTable = null;
                DataSet ds = new DataSet();
                myCommand.Connection = openConnection();
                myCommand.CommandType = CommandType.StoredProcedure;
                myCommand.CommandText = _spName;
                myCommand.CommandTimeout = 1000;
                foreach (var item in spParameters)
                {
                    myCommand.Parameters.AddWithValue(item.Key, item.Value);
                }
                myAdapter.SelectCommand = myCommand;
                myAdapter.Fill(ds);
                dataTable = ds.Tables[0];
                conn.Close();
                return dataTable;
            }
        }


        /// <method>
        /// Select Query without Parameter
        /// </method>
        public DataTable ExecuteSPWithReturnDataTableWithoutParameter(String _SpName)
        {
            using (SqlCommand myCommand = new SqlCommand())
            {
                DataTable dataTable = new DataTable();
                dataTable = null;
                DataSet ds = new DataSet();

                myCommand.Connection = openConnection();
                myCommand.CommandText = _SpName;
                myCommand.CommandTimeout = 1000;
                myCommand.ExecuteNonQuery();
                myAdapter.SelectCommand = myCommand;
                myAdapter.Fill(ds);
                dataTable = ds.Tables[0];
                conn.Close();
                return dataTable;
            }
        }




        /// <method>
        /// This methed will Exec Stored Proc WithoutReturnData
        /// </method>
        public void ExecuteSPWithoutReturnData(String _query, Dictionary<string, string> spParameters)
        {
            using (SqlCommand myCommand = new SqlCommand())
            {
                myCommand.Connection = openConnection();
                myCommand.CommandType = CommandType.StoredProcedure;
                foreach (var item in spParameters)
                {
                    myCommand.Parameters.AddWithValue(item.Key, item.Value);
                }
                myCommand.CommandText = _query;
                myCommand.CommandTimeout = 1000;
                myCommand.ExecuteNonQuery();
                conn.Close();
            }

        }

        /// <method>
        /// This methed will Exec Stored Proc With Return Out Parameter....
        /// </method>
        public string ExecuteSPWithReturnData(String _query, Dictionary<string, string> spParameters, string OutParameter)
        {
            string result = "";
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.Connection = openConnection();
                cmd.CommandType = CommandType.StoredProcedure;
                foreach (var item in spParameters)
                {
                    cmd.Parameters.AddWithValue(item.Key, item.Value);
                }

                //Add the output parameter to the command object
                SqlParameter outPutParameter = new SqlParameter();
                outPutParameter.ParameterName = OutParameter;
                outPutParameter.SqlDbType = System.Data.SqlDbType.VarChar;
                outPutParameter.Size = 4000;
                outPutParameter.Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add(outPutParameter);
                cmd.CommandText = _query;
                cmd.ExecuteNonQuery();
                //Retrieve the value of the output parameter
                result = outPutParameter.Value.ToString();
                conn.Close();
                return result;
            }

        }


        /// <method>
        /// This methed will Exec Query and Return int Out Parameter...
        /// </method>
        public int ExecuteSPReturnIntParameter(String _query, Dictionary<string, string> spParameters)
        {
            int result;
            using (SqlCommand myCommand = new SqlCommand())
            {
                myCommand.Connection = openConnection();
                myCommand.CommandType = CommandType.StoredProcedure;
                foreach (var item in spParameters)
                {
                    myCommand.Parameters.AddWithValue(item.Key, item.Value);
                }
                myCommand.CommandText = _query;
                myCommand.CommandTimeout = 1000;
                result = Convert.ToInt32(myCommand.ExecuteScalar());
                conn.Close();
                return result;
            }

        }

        public void Dispose()
        {
            conn.Dispose();
            myAdapter.Dispose(); ;
        }


    }

}
