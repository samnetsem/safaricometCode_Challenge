using System;
using System.Collections.Generic;
using System.Data;
using MotiSectorAPI.DataAccess;
using MotiSectorAPI.Model;

namespace MotiSectorAPI.Business
{
    public class OrdersBussiness
    {

        public bool UpdateEx(Orders objOrders)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.UpdateEx(objOrders);
        }

        public bool VoidPaymentByOrderId(int intID)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.VoidPaymentByOrderId(intID);
        }
        public bool VoidPayment(string strReceiptNo, string strInvoiceNo)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.VoidPayment(strReceiptNo, strInvoiceNo);
        }
        public Orders GetOrderByRecipitNo(string strReceiptNo)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.GetOrderByRecipitNo(strReceiptNo);
        }
        public Orders GetOrderByOrderNo(int OrderNo)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.GetOrderByOrderNo(OrderNo);
        }

        public PaymentReturnValue GetOrderByInvoiceNo(int OrderNo)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.GetOrderByInvoiceNo(OrderNo);
        }

        public bool CheckReceiptNoExist(string ReceiptNo)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.CheckReceiptNoExist(ReceiptNo);
        }


        public bool CheckPaymentIsDoneBythisPaymentOrderNo(int OrderNo)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.CheckPaymentIsDoneBythisPaymentOrderNo(OrderNo);
        }

        public bool isValidOrderNo(int OrderNo)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.isValidOrderNo(OrderNo);
        }

        public Orders GetServiceApplicationDetailByInvoiceNo(int OrderNo)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.GetServiceApplicationDetailByInvoiceNo(OrderNo);
        }

        public PaymentList GetPaymentByReceiptNo(string strReceiptNo)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.GetPaymentByReceiptNo(strReceiptNo);
        }
        public List<PaymentList> GetPayments(DateTime PaymentDateFrom, DateTime PaymentDateTo)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.GetPayments(PaymentDateFrom, PaymentDateTo);
        }
        public List<PaymentList> GetPayments(DateTime PaymentDateFrom, DateTime PaymentDateTo, bool IsVoid, string UserID)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.GetPayments(PaymentDateFrom, PaymentDateTo, IsVoid, UserID);
        }


    }
}