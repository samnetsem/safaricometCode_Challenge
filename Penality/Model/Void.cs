using System;

namespace MotiSectorAPI.Models
{
    public class Void
    {
        public int PaymentOrderNo { get; set; }
        public string VoidReason { get; set; }
        public DateTime VoidDate { get; set; }
    }
}