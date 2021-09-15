using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace SpeechToText.Provider
{
    public class SQLProvider : IDisposable
    {
        const string DBName = "DBConnection";
        SqlConnection cnn = null;

        public SQLProvider()
        {
            cnn=Connect();
        }

        private SqlConnection Connect()
        {
            try
            {
                return new SqlConnection(ConfigurationManager.ConnectionStrings[DBName].ConnectionString);
            }
            catch
            {
                throw new HttpException("Error connecting to database");
            }

        }

        public async Task<int> ExecuteScalarAsync(string sql, string text)
        {
            SqlParameter nameParam = new SqlParameter("@text", text);
            cnn.Open();
            SqlCommand command = new SqlCommand(sql, cnn);
            
            command.Parameters.Add(nameParam);
            var res= await command.ExecuteScalarAsync();
            cnn.Close();
            int iRet = (res == null) ? -1 : (int)res;
            return iRet;
        }

        public async Task<object> ExecuteNonScalarAsync(string FileName, string text, string type)
        {
            var prms = new[]
            {
                new SqlParameter("@filename", FileName),
                new SqlParameter("@Text", text),
                new SqlParameter("@type", type)
            };
            
            cnn.Open();
            SqlCommand command = new SqlCommand(Properties.Resources.InsertToSTTFalse, cnn);
            foreach(var p in prms)
            {
                command.Parameters.Add(p);
            }
            
            var res = await command.ExecuteNonQueryAsync();
            cnn.Close();
            return res;
        }


        public void Dispose()
        {
            try
            {
                if (cnn != null)
                    cnn.Dispose();
            }
            catch
            {
                // Do nothing
            }
            cnn = null;
        }
    }
}