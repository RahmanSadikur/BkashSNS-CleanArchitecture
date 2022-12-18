using System;

namespace BkashSNS.Domain.Entities
{
    public class Message
    {
        public string id { get; set; }
        public string merchantWallet { get; set; }
        public string customerWallet { get; set; }
        public string terminalId { get; set; }
        public string transactionId { get; set; }
        public decimal amount { get; set; }
        public string timestamp { get; set; }
        public DateTime timestamp2 { get; set; }
    }
}
