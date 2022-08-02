using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MotiSectorAPI.Model;
using System.Transactions;

namespace MotiSectorAPI.DataAccess
{
    public class CPayment
    {
        private static string ConnectionString
            => ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public DateTime GetTodayDate()
        {
            var connection = new SqlConnection(ConnectionString);
            const string strGetRecord = @"SELECT GetDate()";
            var command = new SqlCommand
            {
                CommandText = strGetRecord,
                CommandType = CommandType.Text,
                Connection = connection
            };
            try
            {
                connection.Open();
                var dt = (DateTime)command.ExecuteScalar();
                return dt;
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }

        public int PrepareMotiSectorAPIPaymentOrder(decimal dAmount, string strTicketNo, string strLicence, string strDriverGuid, string UserName, Guid serviceGuid,
            Guid serviceApplicationGuid, string strMotiSectorAPIGuid, Payment objPPayment, string strSiteId)
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    using (connection)
                    {
                        connection.Open();

                        var order = new Orders();
                        var ordersDA = new CtblOrder();
                        var orderDetail = new COrderDetail();                        

                        order.DriverId = 0; // Convert.ToInt32(lngId);
                        order.ServiceID = 0;
                        order.InvoiceNo = "0";
                        order.Parent_guid = strDriverGuid;

                        order.PaidAll = false;
                        order.Remark = "";
                        order.LicenceNo = strLicence;
                        order.OrderedDate = DateTime.Today;
                        order.SiteID = strSiteId;
                        order.PaymentReason = "1";
                        order.AdditionalReason = 0;
                        order.OrderedBy = UserName;
                        order.RenewalFrom = DateTime.Today;
                        order.RenewalTo = DateTime.Today;
                        order.ServiceGuid = serviceGuid;
                        order.Description1 = "";
                        order.IsOther = false;
                        order.ServiceApplicationGuid = serviceApplicationGuid;
                        order.Main_guid = Guid.NewGuid().ToString();
                        bool IsOrderInserted = ordersDA.InsertPaymentOrder(order,connection);

                        //orderDetail.Description = GetMotiSectorAPIDescription(strMotiSectorAPIGuid, connection, IsByNewRule);
                        orderDetail.Amount = dAmount;
                        orderDetail.AccountCode = "1745";
                        orderDetail.NextService = 0;
                        orderDetail.OrderGuid = new Guid(order.Main_guid);
                        orderDetail.OrderID = Convert.ToInt32(order.OrderId);
                        orderDetail.PaymentReason = 1;
                        //orderDetail.Reference = ReferenceNo;
                        orderDetail.TicketNo = strTicketNo;
                        orderDetail.IsPenalty = true;
                        orderDetail.Interst = 0;
                        bool IsOrderDetailInserted = orderDetail.Insert(connection);
                        if (IsOrderInserted && IsOrderDetailInserted)
                            ts.Complete();

                        return order.OrderId;
                    }
                }
                //tran.Commit();
            }
            catch (Exception ex)
            {               
                throw new Exception("PrepareMotiSectorAPIPaymentOrder - " + ex.Message);
            }
            finally
            {              
                 
            }
        }

        public int PrepareMotiSectorAPIPaymentOrderFromRecalcualtion()
        {
            SqlConnection connection = new SqlConnection(ConnectionString);
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    using (connection)
                    {
                        connection.Open();  
                        var order = new Orders();
                        var ordersDA = new CtblOrder();
                        var orderDetail = new COrderDetail();
                        order.PaidAll = false;
                        order.OrderedDate = DateTime.Now;
                        bool IsOrderUpdated = ordersDA.UpdateMotiSectorAPIOrder(order, connection);
                        orderDetail.OrderID = Convert.ToInt32(order.OrderId);
                        bool IsOrderDetailUpdated = orderDetail.Update(connection);
                        if (IsOrderUpdated && IsOrderDetailUpdated)
                            ts.Complete();

                        return order.OrderId;
                    }
                }
                //tran.Commit();
            }
            catch (Exception ex)
            {
                throw new Exception("PrepareMotiSectorAPIPaymentOrder - " + ex.Message);
            }
            finally
            {

            }
        }
    }
}