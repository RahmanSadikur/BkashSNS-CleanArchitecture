using System;

namespace BkashSNS.Domain.Entities
{
    public class ClientMapping
    {
        public string ClientNameGroup { get; set; }
        public string Id { get; set; }
        public string MerchantWallet { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
