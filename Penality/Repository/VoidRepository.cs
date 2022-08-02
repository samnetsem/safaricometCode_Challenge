using System;
using MotiSectorAPI.Business;
using MotiSectorAPI.Model;
using MotiSectorAPI.DataAccess;

namespace MotiSectorAPI.Repository
{
    public class VoidRepository
    {
        public static Result SaveVoid(string strReceiptNo)
        {
            var r = new Result();
            try
            {
                var objorderBusiness = new OrdersBussiness();
                var objorder = new Orders();
                objorder = objorderBusiness.GetOrderByRecipitNo(strReceiptNo);
                if (objorder == null || objorder.OrderId == 0)
                {
                    r.IsSaved = false;
                    r.ErrorCode = 101;
                    r.ErrorMessage = "Receipt No. doesn't exist";
                    return r;
                }

                if (objorder.SiteID != AccessRight.GetLocation())
                {
                    r.IsSaved = false;
                    r.ErrorCode = 503;
                    r.ErrorMessage = "Sorry, You are not allowed to void transaction prepared by another Organization";
                    return r;
                }

                if (objorder.IsVoid == true && objorder.IsPaid == false)
                {
                    r.IsSaved = false;
                    r.ErrorCode = 504;
                    r.ErrorMessage = "Sorry, The transaction is already void";
                    return r;
                }
                //}

                r.IsSaved = objorderBusiness.VoidPayment(strReceiptNo, objorder.OrderId.ToString());
                r.ErrorCode = 200;
                r.ErrorMessage = string.Empty;
                return r;
            }
            catch (Exception ex)
            {
                r.IsSaved = false;
                r.ErrorMessage = ex.Message;
                //throw new Exception("Error in SavePayment - " + ex.Message);
                return r;
            }
        }

        public static Result VoidPayment(int intOrderId)
        {
            var r = new Result();
            try
            {
                var objorderBusiness = new OrdersBussiness();
                var objorder = new Orders();
                objorder = objorderBusiness.GetOrderByOrderNo(intOrderId);

                if (objorder == null || objorder.OrderId == 0)
                {
                    r.IsSaved = false;
                    r.ErrorCode = 101;
                    r.ErrorMessage = "Invoice Number doesn't exist";
                    return r;
                }
                
                if (objorder.SiteID != AccessRight.GetLocation())
                {
                    r.IsSaved = false;
                    r.ErrorCode = 503;
                    r.ErrorMessage = "Sorry, You are not allowed to void transaction prepared by another Organization";
                    return r;
                }

                if (objorder.IsVoid == true && objorder.IsPaid == false)
                {
                    r.IsSaved = false;
                    r.ErrorCode = 504;
                    r.ErrorMessage = "Sorry, The transaction is already void";
                    return r;
                }
                if (objorder.IsPaid == false)
                {
                    r.IsSaved = false;
                    r.ErrorCode = 303;
                    r.ErrorMessage = "Sorry, Only paid transactions can be voided";
                    return r;
                }
                //}

                r.IsSaved = objorderBusiness.VoidPaymentByOrderId(intOrderId);
                r.ErrorCode = 200;
                r.ErrorMessage = string.Empty;
                return r;
            }
            catch (Exception ex)
            {
                r.IsSaved = false;
                r.ErrorCode = 500;
                r.ErrorMessage = "Internal server error";
                //throw new Exception("Error in SavePayment - " + ex.Message);
                return r;
            }
        }
    }
}