using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
namespace ADOTest
{
    public class DataService
    {
        protected SqlConnection Conexion = new SqlConnection();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdtext"></param>
        /// <returns></returns>
        public  DataSet ExecuteDataSet(string cmdtext)
        {
            //Conectar();
            SqlCommand cmd = new SqlCommand(cmdtext);
            cmd.Connection = Conexion;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            //Des//Conectar();
            return ds;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdtext"></param>
        /// <param name="cmdtype"></param>
        /// <returns></returns>
        public  DataSet ExecuteDataSet(string cmdtext, CommandType cmdtype)
        {
            //Conectar();
            SqlCommand cmd = new SqlCommand(cmdtext);
            cmd.Connection = Conexion;
            cmd.CommandType = cmdtype;
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            //Des//Conectar();
            return ds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdtext"></param>
        /// <param name="cmdtype"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public  DataSet ExecuteDataSet(string cmdtext, CommandType cmdtype, Dictionary<string, object> parametros)
        {
            //Conectar();
            SqlCommand cmd = new SqlCommand(cmdtext);
            cmd.Connection = Conexion;
            cmd.CommandType = cmdtype;
            foreach (KeyValuePair<string, object> parametro in parametros)
            {
                cmd.Parameters.Add(new SqlParameter(parametro.Key, parametro.Value));
            }
            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            adapter.Fill(ds);
            //Des//Conectar();
            return ds;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdtext"></param>
        /// <returns></returns>
        public  int ExecuteNonQuery(string cmdtext)
        {
            //Conectar();
            SqlCommand      cmd             =  new SqlCommand(cmdtext);
                            cmd.Connection  =  Conexion;
            int             resultado       =  cmd.ExecuteNonQuery();
            //Des//Conectar();
            return resultado;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdtext"></param>
        /// <param name="cmdtype"></param>
        /// <returns></returns>
        public  int ExecuteNonQuery(string cmdtext, CommandType cmdtype)
        {
            //Conectar();

            SqlCommand     cmd  =   new SqlCommand(cmdtext);
                cmd.Connection  =   Conexion;
            int      resultado  =   cmd.ExecuteNonQuery();

            //Des//Conectar();
            return resultado;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdtext"></param>
        /// <param name="cmdtype"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public  int ExecuteNonQuery(string cmdtext, CommandType cmdtype, Dictionary<string, object> parametros)
        {
            //
            //Conectar();

            SqlCommand          cmd     =   new SqlCommand(cmdtext);
                     cmd.Connection     =   Conexion;
                    cmd.CommandType     =   cmdtype;
            //
            foreach (KeyValuePair<string, object> parametro in parametros)
            {
                cmd.Parameters.Add(new SqlParameter(parametro.Key, parametro.Value));
            }
            //
            int          resultado      =   cmd.ExecuteNonQuery();
            
            //Des//Conectar();
            return resultado;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdtext"></param>
        /// <returns></returns>
        public  IDataReader ExecuteReader(string cmdtext)
        {
            //Conectar();
           
            SqlCommand         cmd  =   new SqlCommand(cmdtext);
                    cmd.Connection  =   Conexion;
            IDataReader     reader  =   cmd.ExecuteReader();

            //Des//Conectar();

            return reader;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdtext"></param>
        /// <param name="cmdtype"></param>
        /// <returns></returns>
        public  IDataReader ExecuteReader(string cmdtext, CommandType cmdtype)
        {
            //Conectar();

            SqlCommand  cmd        =      new SqlCommand(cmdtext);
            cmd.Connection         =      Conexion;
            cmd.CommandType        =      cmdtype;
            IDataReader  reader    =      cmd.ExecuteReader();

            //Des//Conectar();

            return reader;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cmdtext"></param>
        /// <param name="cmdtype"></param>
        /// <param name="parametros"></param>
        /// <returns></returns>
        public  IDataReader ExecuteReader(string cmdtext, CommandType cmdtype, Dictionary<string, object> parametros)
        {
            //Conectar();
            SqlCommand cmd     =   new SqlCommand(cmdtext);
                            
            cmd.Connection     =   Conexion;
            cmd.CommandType    =   cmdtype;

            foreach (KeyValuePair<string, object> parametro in parametros)
            {
                cmd.Parameters.Add(new SqlParameter(parametro.Key, parametro.Value));
            }
            
            IDataReader reader = cmd.ExecuteReader();
            
            //Des//Conectar();
            
            return reader;
        }
        /// <summary>
        /// 
        /// </summary>
        public  void Conectar()
        {
            Conexion.ConnectionString = ConfigurationManager.ConnectionStrings["Test"].ConnectionString;
            Conexion.Open();
        }
        /// <summary>
        /// 
        /// </summary>
        public  void Desconectar()
        {
            Conexion.Close();
        }
    }
}
