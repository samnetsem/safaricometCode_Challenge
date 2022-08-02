namespace MotiSectorAPI.Model
{
    public class Result
    {
        public bool IsSaved { get; set; }
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }
    }

    public class ActionResult
    {
        public bool IsSuccess { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class ETResult
    {
        public bool IsSaved { get; set; }
        public string ReceiptNo { get; set; }
        public string AccountNo { get; set; }
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }
    }
}