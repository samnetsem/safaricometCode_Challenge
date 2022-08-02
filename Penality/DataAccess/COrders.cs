using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using MotiSectorAPI.Model;
using System.Transactions;
using CUSTOR.Domain;
using CUSTOR.Bussiness;

namespace MotiSectorAPI.DataAccess
{
    public class CtblOrder
    {
        private readonly string _ConnectionString = string.Empty;

        public CtblOrder()
        {
            _ConnectionString = ConnectionString;
        }
        private string ConnectionString => ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        public bool InsertPaymentOrder(Orders objOrders, SqlConnection connection)
        {
            //var strSQL = @"INSERT [dbo].[Order]
            //    (
            //     OrderedDate,
            //     DriverId ,
            //     ServiceID ,
            //     LicenceNo ,
            //     PaymentReason ,
            //     additionalReason ,
            //     RenewalFrom ,
            //     RenewalTo ,
            //     PaidAll ,
            //     OrderedBy ,
            //     Remark ,
            //     parent_guid ,
            //     ServiceGuid ,
            //     ServiceApplicationGuid ,
            //        description1 ,
            //     IsOther,
            //        main_guid ,
            //        SiteID
            //    )
            //    VALUES
            //    (
            //     @OrderedDate,
            //     @DriverId ,
            //     @ServiceID ,
            //     @LicenceNo ,
            //     @PaymentReason ,
            //     @additionalReason ,
            //     @RenewalFrom ,
            //     @RenewalTo ,
            //     @PaidAll ,
            //     @OrderedBy ,
            //     @Remark ,
            //     @DriverGuid ,
            //     @ServiceGuid ,
            //     @ServiceApplicationGuid ,
            //        @description1 ,
            //     @IsOther,
            //        @main_guid,
            //        @SiteID
            //    )
            //    SELECT @OrderId=@@IDENTITY";

            var strSQL= @"INSERT INTO [dbo].[Order]
                                               ([OrderGuid], [ServiceApplicationGuid],[ServiceTypeCode],[OrderDate],[ValueForPersntage], [PaidBy], [SiteID], [UserName],[EventDatetime])
                                     VALUES    (@OrderGuid,  @ServiceApplicationGuid, @ServiceTypeCode, @OrderDate, @ValueForPersntage,  @PaidBy,  @SiteID,  @UserName, @EventDatetime)
                                                SELECT @OrderId=@@IDENTITY ";

            var command = new SqlCommand
            {
                CommandText = strSQL,
                CommandType = CommandType.Text,
                Connection = connection
            };
            command.Parameters.Clear();


            command.Parameters.Add(new SqlParameter("@OrderGuid", objOrders.OrderGuid));
            command.Parameters.Add(new SqlParameter("@ServiceApplicationGuid", objOrders.ServiceApplicationGuid));
            command.Parameters.Add(new SqlParameter("@ServiceTypeCode", objOrders.ServiceTypeCode));
            command.Parameters.Add(new SqlParameter("@OrderedDate", objOrders.OrderDate));
            command.Parameters.Add(new SqlParameter("@ValueForPersntage", objOrders.ValueForPersntage));
            command.Parameters.Add(new SqlParameter("@PaidBy", objOrders.PaidBy));
            command.Parameters.Add(new SqlParameter("@SiteID", objOrders.SiteID));
            command.Parameters.Add(new SqlParameter("@UserName", objOrders.UserName));
            command.Parameters.Add(new SqlParameter("@EventDatetime", objOrders.EventDatetime));
            command.Parameters.Add(new SqlParameter("@PaymentReason", objOrders.PaymentReason));
            
            command.Parameters["@OrderId"].Direction = ParameterDirection.Output;
            try
            {

                var _rowsAffected = command.ExecuteNonQuery();
                objOrders.OrderId = (int)command.Parameters["@OrderId"].Value;
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("COrders::Insert::Error!" + ex.Message, ex);
            }
            finally
            {
                command.Dispose();
            }
        }


        public bool UpdateEx(Orders objOrders)
        {
        
            SqlConnection connection = new SqlConnection(ConnectionString);
            try
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    using (connection)
                    {
                        connection.Open();
                        bool isOrderUpdated = UpdateOrder(objOrders, connection);
                        if (isOrderUpdated)
                        {  
                            ts.Complete();
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            finally
            {
                connection.Close();
            }
        }

        public bool UpdateOrder(Orders objOrders, SqlConnection connection)
        {
            ozekimessageout objozekimessageout = new ozekimessageout();
            ozekimessageoutBussiness objozekimessageoutBussiness = new ozekimessageoutBussiness();
            var orderResult = false;
            var strSQL = @"UPDATE [dbo].[Order]
                        SET 
	                        [ReceiptNo] = @ReceiptNo, [PaymentDate] = @PaymentDate, [IsPaid] = @Paid, [CheckNo] = @CheckNo, 
                            [UpdatedUsername] = @UpdatedUsername, BankCode=@BankCode, BankName=@BankName, PaymentMethod=@PaymentMethod,ModeOfCollection=@ModeOfCollection,IsVoid=@IsVoid
                        WHERE
	                        [OrderId] = @OrderId";
            var command = new SqlCommand
            {
                CommandText = strSQL,
                CommandType = CommandType.Text,
                Connection = connection
            };
            try
            {
                command.Parameters.Add(new SqlParameter("@ReceiptNo", objOrders.ReceiptNo));
                command.Parameters.Add(new SqlParameter("@CheckNo", objOrders.CheckNo));
                command.Parameters.Add(new SqlParameter("@Paid", objOrders.IsPaid));
                command.Parameters.Add(new SqlParameter("@IsVoid", objOrders.IsVoid));
                command.Parameters.Add(new SqlParameter("@UserName", objOrders.UserName));
                command.Parameters.Add(new SqlParameter("@UpdatedUsername", objOrders.UpdatedUsername));
                command.Parameters.Add(new SqlParameter("@OrderId", objOrders.OrderId));
                command.Parameters.Add(new SqlParameter("@PaymentDate", objOrders.PaymentDate));
                command.Parameters.Add(new SqlParameter("@BankCode", objOrders.BankCode));
                command.Parameters.Add(new SqlParameter("@BankName", objOrders.BankName));
                command.Parameters.Add(new SqlParameter("@PaymentMethod", objOrders.PaymentMethod));
                command.Parameters.Add(new SqlParameter("@ModeOfCollection", objOrders.ModeOfCollection));

                //command.ExecuteNonQuery();
                //if (objOrders.Parent_guid != Guid.Empty.ToString())
                //AssiginNextStep(connection, objOrders.ServiceGuid, new Guid(objOrders.Parent_guid));
                orderResult = command.ExecuteNonQuery() >= 1;
                if(orderResult)
                {
                    string Msg = "ውድ ተገልጋያችን " + objOrders.ServiceName + " ክፍያ ብር " + objOrders.Amount + " በደረሰኝ ቁጥር " + objOrders.ReceiptNo + " ተከፍሏል የሰነዶች ማረጋገጫና ምዝገባ አገልግሎት";
                    string MobileNo = objOrders.MobileNo;
                    DoSaveMessage(Msg,MobileNo, connection);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("Customer::Update::Error!" + ex.Message, ex);
            }
            finally
            {
                command.Dispose();
            }
        }
        protected bool DoSaveMessage(string Msg,string MobileNo, SqlConnection connection)
        {

            //string Msg = "የ ሰነዶች ማረጋገጫ ና ምዝገባ አገልግሎት " + objOrders.ServiceName + " ክፍያ ብር " + objOrders.Amount + " በደረሰኝ ቁጥር " + objOrders.ReceiptNo + " ተከፍሏል";
            ozekimessageout objozekimessageout = new ozekimessageout();
            try
            {
                //objozekimessageout.Id = 1;// Use Identity Number
                objozekimessageout.Sender = "9494";
                objozekimessageout.Receiver = MobileNo;
                objozekimessageout.Msg = Msg;
                objozekimessageout.Senttime = DateTime.Now.ToLongDateString();
                objozekimessageout.Receivedtime = string.Empty;
                objozekimessageout.Operator = string.Empty;
                objozekimessageout.Msgtype = string.Empty;
                objozekimessageout.Reference = string.Empty;
                objozekimessageout.Status = "send";
                objozekimessageout.Errormsg = string.Empty;


                ozekimessageoutBussiness objozekimessageoutBussiness = new ozekimessageoutBussiness();
                objozekimessageoutBussiness.Insertozekimessageout(objozekimessageout,connection);

                //else
                //{
                //    objozekimessageout.Id= 0;//Attention
                //    objozekimessageoutBussiness.Updateozekimessageout(objozekimessageout);
                //}

                string s=(" Record was saved successfully.");
                return true;
            }
            catch (Exception ex)
            {
                string z=(ex.Message);
                return false;
            }
        }
        //public void AssiginNextStep(SqlConnection connection, Guid ServiceGuid, Guid ServiceApplicationGuid)
        //{
        //    if (ServiceGuid.ToString() == "27f2120e-a3a7-4f51-98c9-e97192d65308")//አዲስ ንግድ ምዝገባ እና ንግድ ፍቃድ
        //    {
        //        UpdateServiceStatus(connection, 3, ServiceApplicationGuid, "አዲስ ንግድ ምዝገባ እና ንግድ ፍቃድ", "RegistrationNew.aspx");
        //    }
        //    else if (ServiceGuid.ToString() == "27f2120e-a3a7-4f51-98c9-e97192d65309")//አዲስ ንግድ ምዝገባ  ንግድ ፍቃድ እና ንግድ ምዝገባ
        //    {
        //        UpdateServiceStatus(connection, 3, ServiceApplicationGuid, "አዲስ ንግድ ምዝገባ እና ንግድ ፍቃድ", "RegistrationNew.aspx");
        //    }
        //    else if (ServiceGuid.ToString() == "9a015140-89f7-4477-b788-237bbc378975")//አዲስ ንግድ ምዝገባ
        //    {
        //        UpdateServiceStatus(connection, 3, ServiceApplicationGuid, "አዲስ ንግድ ምዝገባ", "RegistrationNew.aspx");
        //    }
        //    else if (ServiceGuid.ToString() == "ec957957-1349-45fd-8289-2eb8ab9063a0")// የንግድ ስም ስረዛ
        //    {
        //        UpdateServiceStatus(connection, 3, ServiceApplicationGuid, "ስረዛ", "TradeNameCancelation.aspx");
        //    }
        //    else if (ServiceGuid.ToString() == "9a015140-89f7-4477-b788-237bbc378983")//ቅድመ የንግድ ስም ምዝገባና ማጣሪያ
        //    {
        //        UpdateServiceStatus(connection, 3, ServiceApplicationGuid, "ቅድመ የንግድ ስም ምዝገባና ማጣሪያ", "TradeNameRegistration.aspx");
        //    }
        //    else if (ServiceGuid.ToString() == "5ddc8fe0-dcde-42c7-bc62-df01b6e0aacc" || ServiceGuid.ToString() == "5ddc8fe0-dcde-42c7-bc62-df01b6e0aadd" || ServiceGuid.ToString() == "5ddc8fe0-dcde-42c7-bc62-df01b6e0aabe")//የንግድ ተቋም ዋስትና
        //    {
        //        UpdateServiceStatus(connection, 3, ServiceApplicationGuid, "የንግድ ተቋም ዋስትና", "frmCollateral.aspx");
        //    }
        //    else if (ServiceGuid.ToString() == "9a015140-89f7-4477-b788-237bbc378976")//አዲስ ንግድ ፍቃድ
        //    {
        //        UpdateServiceStatus(connection, 3, ServiceApplicationGuid, "አዲስ ንግድ ፍቃድ", "Business.aspx");
        //    }
        //    else if (ServiceGuid.ToString() == "9a015140-89f7-4477-b788-237bbc378979")//የንግድ ፍቃድ እድሳት
        //    {
        //        UpdateServiceStatus(connection, 3, ServiceApplicationGuid, "የንግድ ፍቃድ እድሳት", "LicenceRenewal.aspx");
        //    }
        //    else if (ServiceGuid.ToString() == "10260b4f-0ce7-4c0f-bc35-565f3fec744e")//ፍራንቻይዚንግ
        //    {
        //        UpdateServiceStatus(connection, 3, ServiceApplicationGuid, "ፍራንቻይዚንግ", "Franchising.aspx");
        //    }
        //    else if (ServiceGuid.ToString() == "359eac6c-8c54-45f4-b0ce-00006dcf069a")//የካፒታል እቃዎች ምዝገባ
        //    {
        //        UpdateServiceStatus(connection, 3, ServiceApplicationGuid, "የካፒታል እቃዎች ምዝገባ", "CapitalGoodsRegistration.aspx");
        //    }
        //    else if (ServiceGuid.ToString() == "d6d7483d-d7f6-4044-84d7-6c19429c2eae")//የንግድ ምዝገባ እድሳት
        //    {
        //        UpdateServiceStatus(connection, 3, ServiceApplicationGuid, "የንግድ ምዝገባ እድሳት", "RegistrationRenewal.aspx");
        //    }
        //    else if (ServiceGuid.ToString() == "a524f293-272e-438a-9700-2454d3779317")//የንግድ ፍቃድ ምትክ
        //    {
        //        UpdateServiceStatus(connection, 3, ServiceApplicationGuid, "የንግድ ፍቃድ ምትክ", "LicenceAndRergistrationReplacment.aspx");
        //    }
        //    else if (ServiceGuid.ToString() == "a3410a3f-dff8-4b48-9fb4-9bca1612dbb1")//ንግድ ምዝገባ ምትክ
        //    {
        //        UpdateServiceStatus(connection, 3, ServiceApplicationGuid, "ንግድ ምዝገባ ምትክ", "LicenceAndRergistrationReplacment.aspx");
        //    }
        //    else if (ServiceGuid.ToString() == "e49ff59d-7af5-46aa-b438-c5c0126b0a03")//የንግድ ስም ምዝገባና ማጣሪያ
        //    {
        //        UpdateServiceStatus(connection, 3, ServiceApplicationGuid, "የንግድ ስም ምዝገባና ማጣሪያ", "TradeNameRegistration.aspx");
        //    }
        //    else if (ServiceGuid.ToString() == "c9ebd44c-25c4-4679-9c9e-2268732bcdbb")//ንግድ ምዝገባ ማሻሻያ
        //    {
        //        UpdateServiceStatus(connection, 3, ServiceApplicationGuid, "ንግድ ምዝገባ ማሻሻያ", "RegistrationNew.aspx");
        //    }
        //    else if (ServiceGuid.ToString() == "cb02e1ba-475f-4c67-aaba-2f85f1d038cc")
        //    {
        //        UpdateServiceStatus(connection, 3, ServiceApplicationGuid, "ንግድ ፍቃድ ማሻሻያ", "Business.aspx");
        //    }
        //    else if (ServiceGuid.ToString() == "21a41a50-aa0a-4947-bc0a-02d235a7e1ad")
        //    {
        //        UpdateServiceStatus(connection, 3, ServiceApplicationGuid, "ንግድ ማስፋፊያ", "Cancelation.aspx");
        //    }
        //    //Trade name replacement by biniam
        //    else if (ServiceGuid.ToString() == "11c54343-2f6d-427d-b32f-bfa64ae0113d")
        //    {
        //        UpdateServiceStatus(connection, 3, ServiceApplicationGuid, "የንግድ ስም ምትክ", "TradeNameAmendment.aspx");
        //    }
        //    else if (ServiceGuid.ToString().ToLower() == "a23514e2-5c7b-415e-bef7-000380c94e78".ToLower())
        //    {
        //        UpdateServiceStatus(connection, 3, ServiceApplicationGuid, "የንግድ ስም ማሻሻያ", "TradeNameAmendment.aspx");
        //    }
        //    else if (ServiceGuid.ToString().ToLower() == "7ad9ca7d-6200-414f-a352-2146a8410475".ToLower())
        //    {
        //        UpdateServiceStatus(connection, 3, ServiceApplicationGuid, "የንግድ ፍቃድ ስረዛ ማተሚያ", "CancelationLetter.aspx");
        //    }
        //}

        //public bool UpdateServiceStatus(SqlConnection connection, int status, Guid ServiceApplicationGuid, string NextStep, string URL)
        //{
        //    CVGeez objGeez = new CVGeez();
        //    var command = new SqlCommand
        //    {
        //        CommandText = @"UPDATE [dbo].[tblServiceApplication]
        //                        SET 
	       //                         [MainGuid] = @MainGuid,
	       //                         [Status] = @Status,
	       //                         [NextStep]=@NextStep,
        //                            [NextStepSort]=@NextStepSort,
	       //                         [CurrentStep]=@CurrentStep,
	       //                         [URL]=@URL
        //                        WHERE
	       //                         [MainGuid] = @MainGuid",
        //        CommandType = CommandType.Text,
        //        Connection = connection
        //    };
        //    try
        //    {
        //        command.Parameters.Add(new SqlParameter("@MainGuid", ServiceApplicationGuid));
        //        command.Parameters.Add(new SqlParameter("@Status", status));
        //        command.Parameters.Add(new SqlParameter("@NextStep", NextStep));
        //        command.Parameters.Add(new SqlParameter("@NextStepSort", objGeez.GetSortValueU(NextStep)));
        //        command.Parameters.Add(new SqlParameter("@CurrentStep", "Paid"));
        //        command.Parameters.Add(new SqlParameter("@URL", URL));
        //        //connection.Open();
        //        int _rowsAffected = command.ExecuteNonQuery();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("CtblServiceApplication::Update::Error!" + ex.Message, ex);
        //    }
        //}


        public bool VoidPaymentByOrderId(int strOrderId)
        {
            var strSQL = "UPDATE  [Order]   SET [IsVoid]=@IsVoid,  [IsPaid] = 0, VoidDate=GetDate()  where OrderId=@OrderId ";
            try
            {
                using (var ts = new TransactionScope())
                {
                    using (var con = new SqlConnection(ConnectionString))
                    {
                        con.Open();
                        var orderResult = false;
                        using (var cmd1 = con.CreateCommand())
                        {
                            cmd1.CommandType = CommandType.Text;

                            cmd1.CommandText = strSQL;
                            cmd1.Parameters.Add(new SqlParameter("@OrderId", strOrderId));
                            cmd1.Parameters.Add(new SqlParameter("@IsVoid", true));
                            orderResult = cmd1.ExecuteNonQuery() >= 1;
                        }
                        if (orderResult)
                        {
                            // Commit the transaction 
                            ts.Complete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return false;
                // the transaction scope will take care of rolling back
            }
            return true;
        }
        public bool VoidPayment(string strReceiptNo, string strInvoiceNo)
        {
            var strSQL = "UPDATE  [Order] SET [void]=@void, VoidDate=GetDate() where ReceiptNo=@ReceiptNo";
            try
            {
                using (var ts = new TransactionScope())
                {
                    using (var con = new SqlConnection(ConnectionString))
                    {
                        con.Open();
                        var orderResult = false;
                        using (var cmd1 = con.CreateCommand())
                        {
                            cmd1.CommandType = CommandType.Text;
                            cmd1.CommandText = strSQL;
                            cmd1.Parameters.Add(new SqlParameter("@ReceiptNo", strReceiptNo));
                            cmd1.Parameters.Add(new SqlParameter("@void", true));
                            orderResult = cmd1.ExecuteNonQuery() >= 1;
                        }
                        if (orderResult)
                        {
                            // Commit the transaction 
                            ts.Complete();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string s = ex.Message;
                return false;
                // the transaction scope will take care of rolling back
            }
            return true;
        }
        public Orders GetOrderByRecipitNo(string strReceiptNo)
        {
            var connection = new SqlConnection(_ConnectionString);
            var strSQL = @"SELECT  OrderId, OrderDate,IsPaid,IsVoid, SiteID FROM  [Order] WHERE ReceiptNo = @ReceiptNo ";
            var command = new SqlCommand { CommandText = strSQL, CommandType = CommandType.Text };
            var dTable = new DataTable("Order");
            var adapter = new SqlDataAdapter(command);
            command.Connection = connection;
            var Ord = new Orders();
            try
            {
                command.Parameters.Add(new SqlParameter("@ReceiptNo", strReceiptNo));
                connection.Open();
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                {
                    var dr = dTable.Rows[0];
                    Ord.OrderId = Convert.ToInt32(dr["OrderId"]);
                    Ord.IsPaid = !dr["ISPaid"].Equals(DBNull.Value) && Convert.ToBoolean(dr["ISPaid"].ToString());
                    Ord.IsVoid = !dr["IsVoid"].Equals(DBNull.Value) && Convert.ToBoolean(dr["IsVoid"].ToString());
                    Ord.SiteID = dr["Site"].Equals(DBNull.Value) ? "" : Convert.ToString(dr["Site"]);
                    Ord.OrderId = Convert.ToInt32(dr["OrderId"]);
                    return Ord;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetPaymentByReceiptNo: " + ex.Message);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }
        public Orders GetOrderByOrderNo(int OrderId)
        {
            var connection = new SqlConnection(_ConnectionString);
            var strSQL = @"SELECT   OrderId, OrderDate,IsPaid,IsVoid, SiteID FROM  [Order] WHERE OrderId = @OrderId ";
            var command = new SqlCommand { CommandText = strSQL, CommandType = CommandType.Text };
            var dTable = new DataTable("Order");
            var adapter = new SqlDataAdapter(command);
            command.Connection = connection;

            var Ord = new Orders();
            try
            {
                command.Parameters.Add(new SqlParameter("@OrderId", OrderId));
                connection.Open();
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                {
                    var dr = dTable.Rows[0];
                    Ord.OrderId = Convert.ToInt32(dr["OrderId"]);
                    Ord.IsPaid = dr["ISPaid"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["ISPaid"].ToString());
                    Ord.IsVoid = dr["IsVoid"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["IsVoid"].ToString());
                    Ord.SiteID = dr["SiteID"].Equals(DBNull.Value) ? "" : Convert.ToString(dr["SiteID"]);
                }
                return Ord;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetPaymentByReceiptNo: " + ex.Message);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }

        public PaymentReturnValue GetOrderByInvoiceNo(int OrderId)
        {
            var connection = new SqlConnection(_ConnectionString);
            var strSQL = @"SELECT   top(1)  [Order].OrderID,[Order].OrderDate,[Order].IsPaid,[Order].IsVoid,Sum(isnull(OrderDetail.Amount,0.00)) as TotalAmount,
                              [Order].PaidBy as CustomerName,[Order].UserName,[Order].AccountNumber,
	                           (SELECT top(1) ServiceNameEnglish FROM  [Service] where ServiceTypeCode= [Order].ServiceTypeCode) as ServiceTaken
                                                      From [Order]  
							                        inner join OrderDetail on [Order].OrderGuid=OrderDetail.OrderGuid
							
                                                    WHERE  [Order].OrderGuid=OrderDetail.OrderGuid   AND   [Order].OrderID = @OrderId
                                                    Group by [Order].OrderID,[Order].ServiceApplicationGuid,ReceiptNo,PaymentDate,[Order].ServiceTypeCode, IsPaid, IsVoid, [Order].OrderDate, checkno,[Order].PaidBy,[Order].UserName,[Order].AccountNumber 
                                                    order by PaymentDate desc";
            var command = new SqlCommand { CommandText = strSQL, CommandType = CommandType.Text };
            var dTable = new DataTable("Order");
            var adapter = new SqlDataAdapter(command);
            command.Connection = connection;

            var PaymentDetail = new PaymentReturnValue();
            try
            {
                command.Parameters.Add(new SqlParameter("@OrderId", OrderId));
                connection.Open();
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                {
                    var dr = dTable.Rows[0];
                    PaymentDetail.InvoiceNo = Convert.ToInt32(dr["OrderId"]);
                    PaymentDetail.OrderedDate = dr["OrderDate"].Equals(DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(dr["OrderDate"]);
                    PaymentDetail.Amount = dr["TotalAmount"].Equals(DBNull.Value) ? 0 : Convert.ToDecimal(dr["TotalAmount"]);
                    PaymentDetail.CustomerName = dr["CustomerName"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["CustomerName"]);
                    PaymentDetail.UserName = dr["UserName"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["UserName"]);
                    // PaymentDetail.Tin = dr["Tin"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["Tin"]);
                    PaymentDetail.AccountNumber = dr["AccountNumber"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["AccountNumber"]);
                    PaymentDetail.ServiceType = dr["ServiceTaken"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["ServiceTaken"]);
                    PaymentDetail.IsPaid = dr["ISPaid"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["ISPaid"].ToString());
                    PaymentDetail.IsVoid = dr["IsVoid"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["IsVoid"].ToString());
                    //DateTime MaximumDateBeforeRecaclulation = dr["DateWrittenOnModeOfCollection"].Equals(DBNull.Value) ? DateTime.MaxValue : Convert.ToDateTime(dr["DateWrittenOnModeOfCollection"]);
                    //if (DateTime.Today > MaximumDateBeforeRecaclulation)
                    //{
                    //    RecaclulatePayment(OrderId);
                    //    GetOrderByInvoiceNo(OrderId);
                    //}
                    PaymentDetail.ErrorCode = 200;
                    PaymentDetail.ErrorMessage = "";
                }
                else
                {
                    PaymentDetail.ErrorCode = 101;
                    PaymentDetail.ErrorMessage = "";
                }
                return PaymentDetail;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetPaymentByReceiptNo: " + ex.Message);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }

        public Orders GetServiceApplicationDetailByInvoiceNo(int OrderId)
        {
            var connection = new SqlConnection(_ConnectionString);
            //var strSQL = @"SELECT  AccountNumber, ServiceApplicationGuid,OrderId,MobileNo
            //                From [Order]
            //                WHERE  [Order].OrderID = @OrderId ";
            var strSQl = @"SELECT  AccountNumber, ServiceApplicationGuid,OrderId,MobileNo,Sum(isnull(OrderDetail.Amount,0.00)) as TotalAmount,
                                   (SELECT top(1) ServiceName FROM  [Service] where ServiceTypeCode= [Order].ServiceTypeCode) as ServiceTakenAm,
                                   (SELECT top(1) ServiceNameEnglish FROM  [Service] where ServiceTypeCode= [Order].ServiceTypeCode) as ServiceTakenEng,ReceiptNo
                                                  From [Order]
							inner join OrderDetail on [Order].OrderGuid=OrderDetail.OrderGuid
                            WHERE  [Order].OrderID = @OrderId
							Group by [Order].OrderID,[Order].ServiceApplicationGuid,[Order].AccountNumber, [Order].MobileNo,[Order].ServiceTypeCode,[Order].ReceiptNo";
            var command = new SqlCommand { CommandText = strSQl, CommandType = CommandType.Text };
            var dTable = new DataTable("Order");
            var adapter = new SqlDataAdapter(command);
            command.Connection = connection;

            var objOrders = new Orders();
            try
            {
                command.Parameters.Add(new SqlParameter("@OrderId", OrderId));
                connection.Open();
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                {
                    var dr = dTable.Rows[0];
                    objOrders.Parent_guid = dr["ServiceApplicationGuid"].ToString();
                    objOrders.AccountNo = dr["AccountNumber"].ToString();
                    objOrders.ServiceGuid = new Guid(dr["ServiceApplicationGuid"].ToString());
                    objOrders.MobileNo =  dr["MobileNo"].Equals(DBNull.Value) ? string.Empty : dr["MobileNo"].ToString();
                    objOrders.Amount = dr["TotalAmount"].Equals(DBNull.Value) ? 0 : Convert.ToDecimal(dr["TotalAmount"]);
                    objOrders.ReceiptNo = dr["ReceiptNo"].Equals(DBNull.Value) ? string.Empty : dr["ReceiptNo"].ToString();
                    objOrders.ServiceName = dr["ServiceTakenAm"].Equals(DBNull.Value) ? string.Empty : dr["ServiceTakenAm"].ToString();
                    objOrders.ServiceNameEnglish = dr["ServiceTakenEng"].Equals(DBNull.Value) ? string.Empty : dr["ServiceTakenEng"].ToString();
                }
                return objOrders;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetPaymentByReceiptNo: " + ex.Message);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }

        public PaymentReturnValueEtSwitch GetOrderByInvoiceNoForEtSwitch(int OrderId, string ETSRequestID)
        {
            var connection = new SqlConnection(_ConnectionString);
            var strSQL = @"SELECT top(1) Order.OrderId, Order.OrderDate, Order.ISPaid, Order.IsVoid, 
			                             Sum(isnull(amount,0.00)) as TotalAmount, 
                                        'CustomerName'= 'Daris'  , 
                                         'ServiceTaken'=  (SELECT ServiceNameEnglish FROM  Service where ServiceTypeCode= Order.ServiceTypeCode) ),
                                         
                                          Order.AccountCode, MaximumDateBeforeRecaclulation
                            From Order,OrderDetail
                            WHERE  Order.OrderGuid=OrderDetail.OrderGuid   AND   Order.OrderID = @OrderId
                            Group by Order.OrderID,Order.ServiceApplicationGuid,ReceiptNo,PaymentDate, IsPaid, IsVoid, Order.OrderDate, checkno ,AccountCode, DateWrittenOnModeOfCollection
                            order by PaymentDate desc";
            var command = new SqlCommand { CommandText = strSQL, CommandType = CommandType.Text };
            var dTable = new DataTable("Order");
            var adapter = new SqlDataAdapter(command);
            command.Connection = connection;

            var PaymentDetail = new PaymentReturnValueEtSwitch();
            try
            {
                command.Parameters.Add(new SqlParameter("@OrderId", OrderId));
                connection.Open();
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                {
                    var dr = dTable.Rows[0];
                    PaymentDetail.ETSRequestID = ETSRequestID;
                    PaymentDetail.InvoiceNo = Convert.ToInt32(dr["OrderId"]);
                    PaymentDetail.OrderedDate = dr["OrderDate"].Equals(DBNull.Value) ? DateTime.MinValue : Convert.ToDateTime(dr["OrderDate"]);
                    PaymentDetail.Amount = dr["TotalAmount"].Equals(DBNull.Value) ? 0 : Convert.ToDecimal(dr["TotalAmount"]);
                    PaymentDetail.CustomerName = dr["CustomerName"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["CustomerName"]);
                    //PaymentDetail.Tin = dr["Tin"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["Tin"]);
                   // PaymentDetail.AccountNumber = dr["AccountNumber"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["AccountNumber"]);
                    PaymentDetail.ServiceType = dr["ServiceTaken"].Equals(DBNull.Value) ? String.Empty : Convert.ToString(dr["ServiceTaken"]);
                    PaymentDetail.IsPaid = dr["ISPaid"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["ISPaid"].ToString());
                    PaymentDetail.IsVoid = dr["IsVoid"].Equals(DBNull.Value) ? false : Convert.ToBoolean(dr["IsVoid"].ToString());
                    DateTime MaximumDateBeforeRecaclulation = dr["MaximumDateBeforeRecaclulation"].Equals(DBNull.Value) ? DateTime.MaxValue : Convert.ToDateTime(dr["MaximumDateBeforeRecaclulation"]);
                    if (DateTime.Today > MaximumDateBeforeRecaclulation)
                    {
                        RecaclulatePayment(OrderId);
                        GetOrderByInvoiceNo(OrderId);
                    }
                }
                PaymentDetail.ErrorCode = 200;
                PaymentDetail.ErrorMessage = "";
                return PaymentDetail;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetPaymentByReceiptNo: " + ex.Message);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }

        private void RecaclulatePayment(int orderId)
        {
            //throw new NotImplementedException();
        }

        public bool CheckReceiptNoExist(string ReceiptNo)
        {
            var connection = new SqlConnection(ConnectionString);
            var strExists = @"Select OrderID  from [Order] where ReceiptNo  = @ReceiptNo";
            var command = new SqlCommand { CommandText = strExists, CommandType = CommandType.Text };
            command.Connection = connection;
            try
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("@ReceiptNo", ReceiptNo));
                var obj = command.ExecuteScalar();
                return obj != null;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }
        public bool isValidOrderNo(int PaymentOrderNumber)
        {
            var connection = new SqlConnection(ConnectionString);

            var strExists = @"Select OrderID  from [Order] where OrderId  = @PaymentOrderNumber";

            var command = new SqlCommand { CommandText = strExists, CommandType = CommandType.Text };
            command.Connection = connection;

            try
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("@PaymentOrderNumber", PaymentOrderNumber));
                var obj = command.ExecuteScalar();
                return obj != null;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }


        public bool GetParentGuid(int PaymentOrderNumber)
        {
            var connection = new SqlConnection(ConnectionString);
            var strExists = @" Select ParentGUid  from Order WHERE OrderId  = @PaymentOrderNumber";
            var command = new SqlCommand { CommandText = strExists, CommandType = CommandType.Text };
            command.Connection = connection;
            try
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("@PaymentOrderNumber", PaymentOrderNumber));
                var obj = command.ExecuteScalar();
                return obj != null;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }
        public bool CheckPaymentIsDoneBythisPaymentOrderNo(int PaymentOrderNumber)
        {
            var connection = new SqlConnection(ConnectionString);
            var strExists = @"Select OrderID  from Order where IsPaid = 1 AND OrderId  = @PaymentOrderNumber ";
            var command = new SqlCommand { CommandText = strExists, CommandType = CommandType.Text };
            command.Connection = connection;
            try
            {
                connection.Open();
                command.Parameters.Add(new SqlParameter("@PaymentOrderNumber", PaymentOrderNumber));
                var obj = command.ExecuteScalar();
                return obj != null;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }
        public PaymentList GetPaymentByReceiptNo(string strReceiptNo)
        {
            var connection = new SqlConnection(_ConnectionString);
            var strSQL = @" SELECT top(1) Order.OrderId, Order.OrderDate, Order.ISPaid, Order.IsVoid, 
			                      Order.ServiceApplicationGuid,Order.UserName,  
			                      Sum(isnull(amount,0.00)) as PaidAmount,  CheckNo as ChequeNo,
                                  'CustomerName'=  ([Order].PaidBy), 
                                 -- 'Tin'= (SELECT  isnull(tblServiceApplication.Tin,'')  From tblServiceApplication where MainGuid=Order.ParentGuid )
                    From Order,OrderDetail
                    WHERE  Order.OrderGuid=OrderDetail.OrderGuid   AND   Order.OrderID = 2645730
                    Group by Order.OrderID,Order.ServiceApplicationGuid,ReceiptNo,PaymentDate, IsPaid, IsVoid, Order.OrderDate, checkno 
                    order by PaymentDate desc";

            var command = new SqlCommand { CommandText = strSQL, CommandType = CommandType.Text };
            var dTable = new DataTable("PaymentList");
            var adapter = new SqlDataAdapter(command);
            command.Connection = connection;

            var pmt = new PaymentList();
            try
            {
                command.Parameters.Add(new SqlParameter("@ReceiptNo", strReceiptNo));
                connection.Open();
                adapter.Fill(dTable);
                if (dTable.Rows.Count > 0)
                {
                    var dr = dTable.Rows[0];
                    pmt.CashierName = dr["UserName"].Equals(DBNull.Value) ? string.Empty : dr["UserName"].ToString();
                    pmt.ServiceApplicationGuid = dr["ServiceApplicationGuid"].Equals(DBNull.Value) ? string.Empty : dr["ServiceApplicationGuid"].ToString();
                    pmt.ReceiptNo = dr["ReceiptNo"].Equals(DBNull.Value) ? string.Empty : dr["ReceiptNo"].ToString();
                    pmt.PaymentDate = dr["PaymentDate"].Equals(DBNull.Value) ? DateTime.MinValue : (DateTime)dr["PaymentDate"];
                    pmt.PaidAmount = dr["PaidAmount"].Equals(DBNull.Value) ? 0 : Convert.ToDecimal(dr["PaidAmount"]);
                    pmt.CustomerName = dr["FullName"].Equals(DBNull.Value) ? string.Empty : dr["FullName"].ToString();
                    pmt.Tin = dr["Tin"].Equals(DBNull.Value) ? string.Empty : dr["Tin"].ToString();
                    pmt.PaymentOrderNo = dr["PaymentOrderNo"].Equals(DBNull.Value) ? 0 : (int)dr["PaymentOrderNo"];
                    pmt.ChequeNo = dr["ChequeNo"].Equals(DBNull.Value) ? string.Empty : dr["ChequeNo"].ToString();
                }
                return pmt;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetPaymentByReceiptNo: " + ex.Message);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }
        public List<PaymentList> GetPayments(DateTime PaymentDateFrom, DateTime PaymentDateTo)
        {
            var RecordsList = new List<PaymentList>();
            var connection = new SqlConnection(_ConnectionString);
            SqlDataReader dr = null;
            var command = new SqlCommand();
            var strSQL = @" SELECT Order.UserName,Order.ServiceApplicationGuid,Order.OrderID as PaymentOrderNo, ReceiptNo,PaymentDate, 
                                    Sum(isnull(amount,0.00)) as PaidAmount,  CheckNo as ChequeNo,
                                     'CustomerName'=  ([Order].PaidBy), 
                                     -- 'Tin'= (SELECT  isnull(tblServiceApplication.Tin,'')  From tblServiceApplication where MainGuid=Order.ParentGuid )
                            From Order,OrderDetail
                            WHERE  Order.OrderGuid=OrderDetail.OrderGuid   AND Ispaid=1 AND 1 = 1  AND
                            (PaymentDate >=  @PaymentDateFrom) AND (PaymentDate <= @PaymentDateTo) AND (Order.ReceiptNo <> '0')     
                            Group by Order.OrderID,Order.ServiceApplicationGuid,ReceiptNo,PaymentDate, Order.Cashier,Order.LicenceNo, checkno 
                            order by PaymentDate desc ";


            command.CommandText = strSQL;
            command.CommandType = CommandType.Text;
            command.Connection = connection;
            try
            {
                command.Parameters.Add(new SqlParameter("@PaymentDateFrom", PaymentDateFrom.ToString("yyyy-MM-dd")));
                command.Parameters.Add(new SqlParameter("@PaymentDateTo", PaymentDateTo.ToString("yyyy-MM-dd")));
                connection.Open();
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var pmt = new PaymentList();

                    pmt.CashierName = dr["UserName"].Equals(DBNull.Value) ? string.Empty : dr["UserName"].ToString();
                    pmt.ServiceApplicationGuid = dr["ServiceApplicationGuid"].Equals(DBNull.Value) ? string.Empty : dr["ServiceApplicationGuid"].ToString();
                    pmt.ReceiptNo = dr["ReceiptNo"].Equals(DBNull.Value) ? string.Empty : dr["ReceiptNo"].ToString();
                    pmt.PaymentDate = dr["PaymentDate"].Equals(DBNull.Value)
                        ? DateTime.MinValue
                        : (DateTime)dr["PaymentDate"];
                    pmt.PaidAmount = dr["PaidAmount"].Equals(DBNull.Value) ? 0 : Convert.ToDecimal(dr["PaidAmount"]);
                    pmt.CustomerName = dr["CustomerName"].Equals(DBNull.Value) ? string.Empty : dr["CustomerName"].ToString();
                    pmt.Tin = dr["Tin"].Equals(DBNull.Value) ? string.Empty : dr["Tin"].ToString();
                    pmt.PaymentOrderNo = dr["PaymentOrderNo"].Equals(DBNull.Value) ? 0 : (int)dr["PaymentOrderNo"];
                    pmt.ChequeNo = dr["ChequeNo"].Equals(DBNull.Value) ? string.Empty : dr["ChequeNo"].ToString();
                    RecordsList.Add(pmt);
                }
                return RecordsList;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetPayments: " + ex.Message);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }
        public List<PaymentList> GetPayments(DateTime PaymentDateFrom, DateTime PaymentDateTo, bool IsVoid, string UserName)
        {
            var RecordsList = new List<PaymentList>();
            var connection = new SqlConnection(_ConnectionString);
            SqlDataReader dr = null;
            var command = new SqlCommand();
            var strSQL = @" SELECT Order.UserName,Order.ServiceApplicationGuid ,Order.OrderID as PaymentOrderNo, ReceiptNo,PaymentDate, 
                                    Sum(isnull(amount,0.00)) as PaidAmount,  CheckNo as ChequeNo,
                                    'CustomerName'=  ([Order].PaidBy), 
                                     -- 'Tin'= (SELECT  isnull(tblServiceApplication.Tin,'')  From tblServiceApplication where MainGuid=Order.ParentGuid )
                            From Order,OrderDetail
                            WHERE  Order.OrderGuid=OrderDetail.OrderGuid   AND IsPaid=1 AND 1 = 1  AND ( _X )  AND
                            (cast(PaymentDate as date) >=  @PaymentDateFrom) AND (cast(PaymentDate as date) <= @PaymentDateTo) AND (Order.ReceiptNo <> '0') AND Order.PaymentReason=1 AND Order.UserName=@UserName	    
                            Group by Order.OrderID,Order.ServiceApplicationGuid,ReceiptNo,PaymentDate, Order.UserName,Order.LicenceNo, checkno 
                            order by PaymentDate desc ";

            strSQL = strSQL.Replace("_X", !IsVoid ? "Void=0 or Void is null" : "Void=1");
            command.CommandText = strSQL;
            command.CommandType = CommandType.Text;
            command.Connection = connection;
            try
            {
                command.Parameters.Add(new SqlParameter("@PaymentDateFrom", PaymentDateFrom.ToString("yyy-MM-dd")));
                command.Parameters.Add(new SqlParameter("@PaymentDateTo", PaymentDateTo.ToString("yyy-MM-dd")));
                command.Parameters.Add(new SqlParameter("@UserName", UserName));
                connection.Open();
                dr = command.ExecuteReader();
                while (dr.Read())
                {
                    var pmt = new PaymentList();

                    pmt.CashierName = dr["UserName"].Equals(DBNull.Value) ? string.Empty : dr["UserName"].ToString();
                    pmt.CustomerName = dr["CustomerName"].Equals(DBNull.Value) ? string.Empty : dr["CustomerName"].ToString();
                    pmt.ReceiptNo = dr["ReceiptNo"].Equals(DBNull.Value) ? string.Empty : dr["ReceiptNo"].ToString();
                    pmt.PaymentDate = dr["PaymentDate"].Equals(DBNull.Value) ? DateTime.MinValue : (DateTime)dr["PaymentDate"];
                    pmt.PaidAmount = dr["PaidAmount"].Equals(DBNull.Value) ? 0 : Convert.ToDecimal(dr["PaidAmount"]);
                    pmt.Tin = dr["Tin"].Equals(DBNull.Value) ? string.Empty : dr["Tin"].ToString();
                    pmt.PaymentOrderNo = dr["PaymentOrderNo"].Equals(DBNull.Value) ? 0 : (int)dr["PaymentOrderNo"];
                    pmt.ChequeNo = dr["ChequeNo"].Equals(DBNull.Value) ? string.Empty : dr["ChequeNo"].ToString();
                    RecordsList.Add(pmt);
                }
                return RecordsList;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in GetPayments: " + ex.Message);
            }
            finally
            {
                connection.Close();
                command.Dispose();
            }
        }
        public bool Delete(int OrderID, SqlConnection connection)
        {

            var command = new SqlCommand();
            command.CommandText = "DELETE FROM [Order] WHERE OrderID = @OrderID";
            command.CommandType = CommandType.Text;
            command.Connection = connection;
            try
            {
                command.Parameters.Clear();
                command.Parameters.Add(new SqlParameter("@OrderId", OrderID));
                var _rowsAffected = command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("COrder::Delete::Error!" + ex.Message, ex);
            }
            finally
            {
                command.Dispose();
            }
        }
        public bool UpdateMotiSectorAPIOrder(Orders objOrders, SqlConnection connection)
        {
            var strSQL = @"UPDATE [dbo].[Order]  SET             
	                        OrderedDate =@OrderedDate
                        WHERE OrderId = @OrderId ";

            var command = new SqlCommand
            {
                CommandText = strSQL,
                CommandType = CommandType.Text,
                Connection = connection
            };
            command.Parameters.Clear();
            command.Parameters.Add(new SqlParameter("@OrderedDate", objOrders.OrderDate));
            //command.Parameters.Add(new SqlParameter("@OrderedBy", objOrders.OrderedBy));
            command.Parameters.Add(new SqlParameter("@OrderId", objOrders.OrderId));
            try
            {
                var _rowsAffected = command.ExecuteNonQuery();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("COrders::Insert::Error!" + ex.Message, ex);
            }
            finally
            {
                command.Dispose();
            }
        }
        public bool VoidTransactionForReversal(SqlConnection connection, SqlCommand command, int OrderId, string UserName, string VoidReason)
        {
            command.CommandText = "Update [Order] SET IsVoid=1,VoidDate=@VoidDate,VoidReason=@VoidReason,VoidBy=@VoidBy WHERE OrderId=@OrderId";
            command.CommandType = CommandType.Text;
            command.Connection = connection;

            command.Parameters.Clear();
            command.Parameters.Add(new SqlParameter("@OrderId", OrderId));
            command.Parameters.Add(new SqlParameter("@IsVoid", 1));
            command.Parameters.Add(new SqlParameter("@VoidBy", UserName));
            command.Parameters.Add(new SqlParameter("@VoidDate", DateTime.Now));
            command.Parameters.Add(new SqlParameter("@VoidReason", VoidReason));
            int xx = command.ExecuteNonQuery();
            return true;
        }
    }
}