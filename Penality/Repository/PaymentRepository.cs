using System;
using System.Collections.Generic;
using System.Net;
using System.Web;
using MotiSectorAPI.Business;
using MotiSectorAPI.DataAccess;
using MotiSectorAPI.Model;

namespace MotiSectorAPI.Repository
{
    public class PaymentRepository
    {       
        public static Result SavePaymentEx(int intPaymentOrderNo, PaymentEx pmt)
        {
            //Location = Config.GetMobileLocation(),
            var r = new Result();
            //to-do use transaction 
            // pmt.CashierName,

        
            
            if (string.IsNullOrEmpty(pmt.BankCode))
                pmt.BankCode = string.Empty;
            if (string.IsNullOrEmpty(pmt.CheckNo))
                pmt.CheckNo = string.Empty;
            if (string.IsNullOrEmpty(pmt.PaymentMethod))
                pmt.PaymentMethod = string.Empty;
            //if (pmt.LocationCode.Equals(null))
            //    pmt.LocationCode = 0;


            try
            {
                var objOrdersBusiness = new OrdersBussiness();
                var order = new Orders
                {
                    OrderId = intPaymentOrderNo,
                    IsPaid = true,
                    IsVoid=false,
                    UserName = GetCurrentUser(),
                    UpdatedUsername = GetCurrentUser(),
                    CheckNo = pmt.CheckNo ?? string.Empty,
                    ReceiptNo = pmt.ReceiptNo,
                    //Location = Convert.ToInt32(pmt.BankCode),
                    SiteID = AccessRight.GetLocation(),
                    PaymentDate = DateTime.Today,
                    BankCode = pmt.BankCode,
                    BankName = pmt.BankName,
                    PaymentMethod = pmt.PaymentMethod,
                    ModeOfCollection = "5"
                };

                if (objOrdersBusiness.CheckPaymentIsDoneBythisPaymentOrderNo(pmt.InvoiceNo))
                {
                    r.ErrorCode = (int)HttpStatusCode.Forbidden;
                    r.ErrorMessage = "The Payment is already made by this payment order number!";
                    r.IsSaved = false;
                    return r;
                }
                if (objOrdersBusiness.CheckReceiptNoExist(pmt.ReceiptNo))
                {
                    r.ErrorCode = (int)HttpStatusCode.UpgradeRequired;
                    r.ErrorMessage = "The Receipt Number has already been used!";
                    r.IsSaved = false;
                    return r;
                }
                if (!objOrdersBusiness.isValidOrderNo(pmt.InvoiceNo))
                {
                    r.ErrorCode = (int)HttpStatusCode.SwitchingProtocols;
                    r.ErrorMessage = "Error! Invoice No. doesn't exist!";
                    r.IsSaved = false;
                    return r;
                }

                Orders objOrder = objOrdersBusiness.GetServiceApplicationDetailByInvoiceNo(pmt.InvoiceNo);
                if (objOrder == null)
                {
                    //r.ErrorCode = (int)HttpStatusCode.BadGateway;
                    //r.ErrorMessage = "Error! Payment Order No longer vailed ";
                    r.ErrorCode = (int)HttpStatusCode.SwitchingProtocols;
                    r.ErrorMessage = "Error! Invoice No. doesn't exist!";
                    r.IsSaved = false;
                    return r;
                }

                if(objOrder.Parent_guid == null)
                    objOrder.Parent_guid = Guid.Empty.ToString();

                order.ServiceGuid = objOrder.ServiceGuid;
                order.Parent_guid = objOrder.Parent_guid;
                order.MobileNo = objOrder.MobileNo;
                order.ReceiptNo = pmt.ReceiptNo;// objOrder.ReceiptNo;
                order.Amount = objOrder.Amount;
                order.ServiceName = objOrder.ServiceName;
                order.ServiceNameEnglish = objOrder.ServiceNameEnglish;
                if (objOrdersBusiness.UpdateEx(order))
                {
                    r.ErrorCode = 200;
                    r.ErrorMessage = string.Empty;
                    r.IsSaved = true;
                    return r;
                }
                else
                {
                    r.ErrorCode = (int)HttpStatusCode.InternalServerError;
                    r.ErrorMessage = "Internal server error";
                    r.IsSaved = false;
                    return r;
                }
            }
            catch (Exception ex)
            {
                r.ErrorMessage = ex.Message;
                r.IsSaved = false;
                return r;
            }
        }


        public static ETResult SavePaymentET(int intPaymentOrderNo, PaymentEx pmt)
        {
            //Location = Config.GetMobileLocation(),
            var r = new ETResult();
            //to-do use transaction 
            // pmt.CashierName,                     


            if (string.IsNullOrEmpty(pmt.BankCode))
                pmt.BankCode = string.Empty;
            if (string.IsNullOrEmpty(pmt.CheckNo))
                pmt.CheckNo = string.Empty;
            if (string.IsNullOrEmpty(pmt.PaymentMethod))
                pmt.PaymentMethod = string.Empty;
            //if (pmt.LocationCode.Equals(null))
            //    pmt.LocationCode = 0;


            try
            {
                var objOrdersBusiness = new OrdersBussiness();
                var order = new Orders
                {
                    OrderId = intPaymentOrderNo,
                    IsPaid = true,
                    IsVoid = false,
                    UserName = GetCurrentUser(),
                    CheckNo = pmt.CheckNo ?? string.Empty,
                    ReceiptNo = pmt.ReceiptNo,
                    //Location = Convert.ToInt32(pmt.BankCode),
                    SiteID = AccessRight.GetLocation(),
                    PaymentDate = DateTime.Today,
                    BankCode = pmt.BankCode,
                    BankName = pmt.BankName,
                    PaymentMethod = pmt.PaymentMethod
                };


                Orders objOrder = objOrdersBusiness.GetServiceApplicationDetailByInvoiceNo(pmt.InvoiceNo);
                if (objOrder == null)
                {
                    r.ReceiptNo = pmt.ReceiptNo;
                    r.ErrorCode = (int)HttpStatusCode.BadGateway;
                    r.ErrorMessage = "Error! Payment Order No longer vailed ";
                    r.IsSaved = false;
                    return r;
                }
                else
                {
                    r.ReceiptNo = pmt.ReceiptNo;
                    r.AccountNo = objOrder.AccountNo;
                }

                if (objOrdersBusiness.CheckPaymentIsDoneBythisPaymentOrderNo(pmt.InvoiceNo))
                {
                    r.ReceiptNo = pmt.ReceiptNo;
                    r.ErrorCode = (int)HttpStatusCode.Forbidden;
                    r.ErrorMessage = "The Payment is already made by this payment order number!";
                    r.IsSaved = false;
                    return r;
                }
                if (objOrdersBusiness.CheckReceiptNoExist(pmt.ReceiptNo))
                {
                    r.ErrorCode = (int)HttpStatusCode.UpgradeRequired;
                    r.ErrorMessage = "The Receipt Number has already been used!";
                    r.IsSaved = false;
                    return r;
                }
                if (!objOrdersBusiness.isValidOrderNo(pmt.InvoiceNo))
                {
                    r.ErrorCode = (int)HttpStatusCode.SwitchingProtocols;
                    r.ErrorMessage = "Error! Invoice No. doesn't exist!";
                    r.IsSaved = false;
                    return r;
                }

                
                order.ServiceGuid = objOrder.ServiceGuid;
                order.Parent_guid = objOrder.Parent_guid;
                if (objOrdersBusiness.UpdateEx(order))
                {
                    r.ErrorCode = 200;
                    r.ErrorMessage = string.Empty;
                    r.IsSaved = true;
                    return r;
                }
                else
                {
                    r.ReceiptNo = pmt.ReceiptNo;
                    r.ErrorCode = (int)HttpStatusCode.InternalServerError;
                    r.ErrorMessage = "Internal server error";
                    r.IsSaved = false;
                    return r;
                }
            }
            catch (Exception ex)
            {
                r.ReceiptNo = pmt.ReceiptNo;
                r.ErrorMessage = ex.Message;
                r.IsSaved = false;
                return r;
            }
        }


        private static string GetCurrentUser()
        {

            try
            {
                var strUser = (HttpContext.Current.Session["CurrentUser"].ToString());
                return !string.IsNullOrEmpty(strUser) ? strUser : string.Empty;
            }
            catch
            {
                return string.Empty;
            }

        }



        public static PaymentList GetPaymentByReceiptNo(string strReceiptNo)
        {
            var objOrd = new OrdersBussiness();
            try
            {
                return objOrd.GetPaymentByReceiptNo(strReceiptNo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<PaymentList> GetPayments(DateTime PaymentDateFrom, DateTime PaymentDateTo, bool IsVoid, string UserID)
        {
            try
            {
                var objOrd = new OrdersBussiness();

                return objOrd.GetPayments(PaymentDateFrom, PaymentDateTo, IsVoid, UserID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static List<PaymentList> GetPaymentsByDriverID(DateTime PaymentDateFrom, DateTime PaymentDateTo)
        {
            try
            {
                var objOrd = new OrdersBussiness();

                return objOrd.GetPayments(PaymentDateFrom, PaymentDateTo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}