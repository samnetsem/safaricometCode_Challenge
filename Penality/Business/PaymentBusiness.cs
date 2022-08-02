using System;
using MotiSectorAPI.DataAccess;
using MotiSectorAPI.Model;

namespace MotiSectorAPI.Business
{
    public class PaymentBusiness
    {
        public DateTime GetTodayDate()
        {
            var objPaymentDataAcess = new CPayment();
            return objPaymentDataAcess.GetTodayDate();
        }

        public int PrepareMotiSectorAPIPaymentOrder(decimal dAmount, string strTicketNo, string strLicence,
            string strDriverGuid, string UserName, Guid serviceGuid, Guid serviceApplicationGuid, string strMotiSectorAPIGuid,
            Payment objPayment, string SiteID)
        {
            var objPaymentDataAcess = new CPayment();
            return objPaymentDataAcess.PrepareMotiSectorAPIPaymentOrder(dAmount, strTicketNo, strLicence, strDriverGuid,
                UserName, serviceGuid, serviceApplicationGuid, strMotiSectorAPIGuid, objPayment, SiteID);
        }

        public PaymentReturnValue GetPaymentDetailByInvoiceNoEx(int OrderNo)
        {
            var objPaymentInfoDataAccess = new CtblOrder();
            return objPaymentInfoDataAccess.GetOrderByInvoiceNo(OrderNo);
        }

        public PaymentReturnValue GetPaymentDetailByInvoiceNo(int OrderNo,  string ETSRequestID)
        {
            var objPaymentInfoDataAccess = new CtblOrder();
            return objPaymentInfoDataAccess.GetOrderByInvoiceNoForEtSwitch(OrderNo, ETSRequestID);
        }
    }
}