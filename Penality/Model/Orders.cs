using System;

namespace MotiSectorAPI.Model
{
    public class Orders
    {
        public int OrderId { get; set; }
        public string ServiceAppGuid { get; set; }

        public Guid OrderGuid { get; set; }

        public Guid ServiceApplicationGuid { get; set; }

        public string ServiceTypeCode { get; set; }

        public DateTime OrderDate { get; set; }


        public decimal AmountUsedForCalculation { get; set; }


        public string PaidBy { get; set; }


        public int NoCopy { get; set; }

        public int NoPeople { get; set; }
        public int NoService { get; set; }

        public decimal ValueForPersntage { get; set; }


        public string ReceiptNo { get; set; }

        public string FileNo { get; set; }

        public string FileNo_Sort { get; set; }


        public DateTime PaymentDate { get; set; }

        public string ModeOfCollection { get; set; }

        public string CheckNo { get; set; }

        public DateTime DateWrittenOnModeOfCollection { get; set; }


        public bool IsPaid { get; set; }

        public bool IsVoid { get; set; }

        public string VoidByGuid { get; set; }

        public DateTime VoidDate { get; set; }

        public string VoidReason { get; set; }


        public Guid OrderDetailGuid { get; set; }

        public string Description { get; set; }

        public string AccountCode { get; set; }

        public string CaseNumber { get; set; }

        public decimal Amount { get; set; }
        public decimal TotalAmount { get; set; }

        public string PaymentDisplayDate { get; set; }

        public string AmharicMoney { get; set; }

        public string PaymentReason { get; set; }

        public string FullName { get; set; }

        public string SiteID { get; set; }

        public string UserName { get; set; }

        public DateTime EventDatetime { get; set; }

        public string UpdatedUsername { get; set; }

        public DateTime UpdatedEventDatetime { get; set; }
        public string Main_guid { get; set; }
        public string Parent_guid { get; set; }
        public Guid ServiceGuid { get; set; }
       
        public DateTime OrderedDate { get; set; }
        
        public string InvoiceNo { get; set; }
        public string AccountNo { get; set; }
     
        public int DriverId { get; set; }
       
        public int ServiceID { get; set; }
        public string LicenceNo { get; set; }
       
        public int AdditionalReason { get; set; }
        public string Description1 { get; set; }
        public bool IsOther { get; set; }
        public string Session { get; set; }
        public DateTime RenewalFrom { get; set; }
        public DateTime RenewalTo { get; set; }
        public bool PaidAll { get; set; }
        public string OrderedBy { get; set; }
        public string CashierUserName { get; set; }
        public string Remark { get; set; }
        public int NextService { get; set; }
        public bool Posted { get; set; }
        public string VoucherNo { get; set; }
        public DateTime PostedDate { get; set; }
        public bool IsFederal { get; set; }
       
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string PaymentMethod { get; set; }

        public string MobileNo { get; set; }
        public string ServiceName { get; set; }
        public string ServiceNameEnglish { get; set; }

    }
}