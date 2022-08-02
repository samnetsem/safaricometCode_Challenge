using System;

namespace Penality.Common
{
    public class Token
    {
        public int TokenId { get; set; }
        public string UserName { get; set; }
        public string AuthToken { get; set; }
        public DateTime IssuedOn { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}