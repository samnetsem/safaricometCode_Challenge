using System;
using System.Net;

namespace MotiSectorAPI.Model
{
    public class PaymentReturnValue
    {
        public int InvoiceNo { get; set; }
        public DateTime OrderedDate { get; set; }     
        public decimal Amount { get; set; }        
        public bool IsVoid { get; set; }
        public bool IsPaid { get; set; }       
        public string CustomerName { get; set; }
        public string UserName { get;set; }
        public string ServiceType { get; set; }
        //public string Tin { get; set; }
        public string AccountNumber { get; set; }        
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class PaymentReturnValueEtSwitch : PaymentReturnValue
    {
        public string ETSRequestID { get; set; }
    }
}