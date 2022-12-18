using System;

namespace BkashSNS.Domain.Entities
{
    public class Client_Log
    {
        public DateTime timestamp { get; set; }
        public string id { get; set; }
        public string Response { get; set; }
        public string Message { get; set; }
        public string Error { get; set; }
        public string merchantWallet { get; set; }
    }
}
