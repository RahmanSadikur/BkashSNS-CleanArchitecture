using System;

namespace BkashSNS.Domain.Entities
{
    public class PaymentRecord
    {
        public string Id { get; set; }
        public string MerchantWallet { get; set; }
        public string CustomerWallet { get; set; }
        public string TerminalId { get; set; }
        public string TransactionId { get; set; }
        public decimal Amount { get; set; }
        public string TimestampString { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
