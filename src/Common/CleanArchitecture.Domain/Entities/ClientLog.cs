using System;

namespace BkashSNS.Domain.Entities
{
    public class ClientLog
    {
        public DateTime Timestamp { get; set; }
        public string Id { get; set; }
        public string Response { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public string MerchantWallet { get; set; }
    }
}
