namespace MotiSectorAPI.Model
{
    public class PaymentEx
    {
        public int InvoiceNo { get; set; }
        public string ReceiptNo { get; set; }
        public string CheckNo { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string PaymentMethod { get; set; }
    }
}
