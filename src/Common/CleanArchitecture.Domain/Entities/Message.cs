using System;

namespace BkashSNS.Domain.Entities
{
    public class Message
    {
        public string Id { get; set; }
        public string MerchantWallet { get; set; }
        public string CustomerWallet { get; set; }
        public string TerminalId { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string Timestamp { get; set; }
        public DateTime Timestamp2 { get; set; }
    }
}
