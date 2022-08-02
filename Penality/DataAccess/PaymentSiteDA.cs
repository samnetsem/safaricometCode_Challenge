using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MotiSectorAPI.Model;

namespace MotiSectorAPI.DataAccess
{
    public class PaymentSiteDA
    {
        private static string ConnectionString
            => ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public List<PaymentSite> GetSitesByRegion(string RegionCode)
        {
            var recordsList = new List<PaymentSite>();
            var connection = new SqlConnection(ConnectionString);
            SqlDataReader dr = null;

            const string strGetAllRecords =
                @"SELECT [Code], ParentCode,[SiteName],[SiteNameAmh],[ParentAdministrationCode],[OrgType]
                        FROM [dbo].[tblSite] WHERE IsActive=1 and (OrgType=3) AND (ParentCode<>-1) AND ParentAdministrationCode = @ParentAdministrationCode AND Code<>14 ORDER BY  Code";

            var command = new SqlCommand
            {
                CommandText = strGetAllRecords,
                CommandType = CommandType.Text,
                Connection = connection
            };

            try
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("@ParentAdministrationCode", RegionCode));
                dr = command.ExecuteReader();

                while (dr.Read())
                {
                    var objOrganization = new PaymentSite();
                    if (dr["Code"].Equals(DBNull.Value))
                        objOrganization.Code = string.Empty;
                    else
                        objOrganization.Code = (string) dr["Code"];
                    if (dr["ParentCode"].Equals(DBNull.Value))
                        objOrganization.ParentCode = string.Empty;
                    else
                        objOrganization.ParentCode = (string) dr["ParentCode"];

                    if (dr["SiteNameAmh"].Equals(DBNull.Value))
                        objOrganization.SiteNameAmh = string.Empty;
                    else
                        objOrganization.SiteNameAmh = (string) dr["SiteNameAmh"];
                    if (dr["SiteName"].Equals(DBNull.Value))
                        objOrganization.SiteName = string.Empty;
                    else
                        objOrganization.SiteName = (string) dr["SiteName"];


                    if (dr["ParentAdministrationCode"].Equals(DBNull.Value))
                        objOrganization.ParentAdministrationCode = string.Empty;
                    else
                        objOrganization.ParentAdministrationCode = (string) dr["ParentAdministrationCode"];
                    if (dr["OrgType"].Equals(DBNull.Value))
                        objOrganization.OrgType = string.Empty;
                    else
                        objOrganization.OrgType = (string) dr["OrgType"];

                    if (dr["SiteName"].Equals(DBNull.Value))
                        objOrganization.SiteName = string.Empty;
                    else
                        objOrganization.SiteName = (string) dr["SiteName"];

                    recordsList.Add(objOrganization);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("PaymentSite::GetList::Error!" + ex.Message, ex);
            }
            finally
            {
                connection.Close();
                dr.Close();
                command.Dispose();
            }
            return recordsList;
        }
    }
}