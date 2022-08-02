using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MotiSectorAPI.Model;

namespace MotiSectorAPI.DataAccess
{
    public class CustomerDA
    {
        private string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString; }
        }

        public byte[] GetPhoto(string strID)
        {
            var connection = new SqlConnection(ConnectionString);

            var objDriver = new Driver();
            var strGetRecord = @"SELECT [Photo] FROM [dbo].[tblDriver] WHERE [Main_Guid]=@ID";


            var command = new SqlCommand { CommandText = strGetRecord, CommandType = CommandType.Text };
            var dTable = new DataTable();
            var adapter = new SqlDataAdapter(command);
            command.Connection = connection;

            try
            {
                command.Parameters.Add(new SqlParameter("@ID", strID));

                connection.Open();
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                    return dTable.Rows[0]["Photo"].Equals(DBNull.Value) ? null : (byte[])dTable.Rows[0]["Photo"];
            }
            catch (Exception ex)
            {
                throw new Exception("Driver::GetRecord::Error!" + ex.Message, ex);
            }
            finally
            {
                connection.Close();
                command.Dispose();
                adapter.Dispose();
            }
            return null;
        }
    }
}