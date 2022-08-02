using System;
using System.Data;
using System.Data.SqlTypes;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using Safari.Domain;

namespace Safari.DataAccess
{
    public class tblStudentDataAccess
    {



        private string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }
        }



        public DataTable GetRecords()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            string strGetAllRecords = @"SELECT [ID],[Name],[grade],[department],[Remark] FROM [dbo].[tblStudent]  ORDER BY  [ID] ASC";

            SqlCommand command = new SqlCommand() { CommandText = strGetAllRecords, CommandType = CommandType.Text };
            DataTable dTable = new DataTable("tblStudent");
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            command.Connection = connection;

            try
            {
                connection.Open();
                adapter.Fill(dTable);
            }
            catch (Exception ex)
            {
                throw new Exception("tblStudent::GetRecords::Error!" + ex.Message, ex);
            }
            finally
            {
                connection.Close();
                command.Dispose();
                adapter.Dispose();
            }
            return dTable;
        }



        public tblStudent GetRecord(int ID)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            tblStudent objtblStudent = new tblStudent();
            string strGetRecord = @"SELECT [ID],[Name],[grade],[department],[Remark] FROM [dbo].[tblStudent] WHERE [ID]=@ID";

            SqlCommand command = new SqlCommand() { CommandText = strGetRecord, CommandType = CommandType.Text };
            DataTable dTable = new DataTable("tblStudent");
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            command.Connection = connection;

            try
            {
                command.Parameters.Add(new SqlParameter("@ID", ID));
                connection.Open();
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                {
                    objtblStudent.ID = (int)dTable.Rows[0]["ID"];
                    if (dTable.Rows[0]["Name"].Equals(DBNull.Value))
                        objtblStudent.Name = null;
                    else
                        objtblStudent.Name = (string)dTable.Rows[0]["Name"];
                    if (dTable.Rows[0]["grade"].Equals(DBNull.Value))
                        objtblStudent.Grade = null;
                    else
                        objtblStudent.Grade = (string)dTable.Rows[0]["grade"];
                    if (dTable.Rows[0]["department"].Equals(DBNull.Value))
                        objtblStudent.Department = null;
                    else
                        objtblStudent.Department = (string)dTable.Rows[0]["department"];
                    if (dTable.Rows[0]["Remark"].Equals(DBNull.Value))
                        objtblStudent.Remark = null;
                    else
                        objtblStudent.Remark = (string)dTable.Rows[0]["Remark"];
                }

            }
            catch (Exception ex)
            {
                throw new Exception("tblStudent::GetRecord::Error!" + ex.Message, ex);
            }
            finally
            {
                connection.Close();
                command.Dispose();
                adapter.Dispose();
            }
            return objtblStudent;
        }

        public bool Insert(tblStudent objtblStudent)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            string strInsert = @"INSERT INTO [dbo].[tblStudent]
                                            ([ID],[Name],[grade],[department],[Remark])
                                     VALUES    (@ID,@Name,@grade,@department,@Remark)";

            SqlCommand command = new SqlCommand() { CommandText = strInsert, CommandType = CommandType.Text };
            command.Connection = connection;

            try
            {
                command.Parameters.Add(new SqlParameter("@ID", objtblStudent.ID));
                command.Parameters.Add(new SqlParameter("@Name", objtblStudent.Name));
                command.Parameters.Add(new SqlParameter("@grade", objtblStudent.Grade));
                command.Parameters.Add(new SqlParameter("@department", objtblStudent.Department));
                command.Parameters.Add(new SqlParameter("@Remark", objtblStudent.Remark));


                connection.Open();
                int _rowsAffected = command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("tblStudent::Insert::Error!" + ex.Message, ex);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }

        public bool Update(tblStudent objtblStudent)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            string strUpdate = @"UPDATE [dbo].[tblStudent] SET     [ID]=@ID,    [Name]=@Name,    [grade]=@grade,    [department]=@department,    [Remark]=@Remark WHERE [ID]=@ID";

            SqlCommand command = new SqlCommand() { CommandText = strUpdate, CommandType = CommandType.Text };
            command.Connection = connection;

            try
            {
                command.Parameters.Add(new SqlParameter("@ID", objtblStudent.ID));
                command.Parameters.Add(new SqlParameter("@Name", objtblStudent.Name));
                command.Parameters.Add(new SqlParameter("@grade", objtblStudent.Grade));
                command.Parameters.Add(new SqlParameter("@department", objtblStudent.Department));
                command.Parameters.Add(new SqlParameter("@Remark", objtblStudent.Remark));


                connection.Open();
                int _rowsAffected = command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("tblStudent::Update::Error!" + ex.Message, ex);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }

        public bool Delete(int ID)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            string strDelete = @"DELETE FROM [dbo].[tblStudent] WHERE [ID]=@ID";

            SqlCommand command = new SqlCommand() { CommandText = strDelete, CommandType = CommandType.Text };
            command.Connection = connection;

            try
            {
                command.Parameters.Add(new SqlParameter("@ID", ID));


                connection.Open();
                int _rowsAffected = command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("tblStudent::Delete::Error!" + ex.Message, ex);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }



        public List<tblStudent> GetList()
        {
            List<tblStudent> RecordsList = new List<tblStudent>();
            SqlConnection connection = new SqlConnection(ConnectionString);
            SqlDataReader dr = null;

            string strGetAllRecords = @"SELECT [ID],[Name],[grade],[department],[Remark] FROM [dbo].[tblStudent]  ORDER BY  [ID] ASC";

            SqlCommand command = new SqlCommand() { CommandText = strGetAllRecords, CommandType = CommandType.Text };
            command.Connection = connection;

            try
            {
                connection.Open();
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    tblStudent objtblStudent = new tblStudent();
                    if (dr["ID"].Equals(DBNull.Value))
                        objtblStudent.ID = 0;
                    else
                        objtblStudent.ID = (int)dr["ID"];
                    if (dr["Name"].Equals(DBNull.Value))
                        objtblStudent.Name = null;
                    else
                        objtblStudent.Name = (string)dr["Name"];
                    if (dr["grade"].Equals(DBNull.Value))
                        objtblStudent.Grade = null;
                    else
                        objtblStudent.Grade = (string)dr["grade"];
                    if (dr["department"].Equals(DBNull.Value))
                        objtblStudent.Department = null;
                    else
                        objtblStudent.Department = (string)dr["department"];
                    if (dr["Remark"].Equals(DBNull.Value))
                        objtblStudent.Remark = null;
                    else
                        objtblStudent.Remark = (string)dr["Remark"];
                    RecordsList.Add(objtblStudent);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("tblStudent::GetList::Error!" + ex.Message, ex);
            }
            finally
            {
                connection.Close();
                dr.Close();
                command.Dispose();
            }
            return RecordsList;
        }


        public bool Exists(int ID)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);

            string strExists = @"SELECT ID FROM [dbo].[tblStudent] WHERE [ID]=@ID";

            SqlCommand command = new SqlCommand() { CommandText = strExists, CommandType = CommandType.Text };
            command.Connection = connection;
            
            try
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("@ID", ID));
                return ((int)command.ExecuteScalar() > 0);
            }
            catch (Exception ex)
            {
                throw new Exception("tblStudent::Exists::Error!" + ex.Message, ex);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }


    }
}