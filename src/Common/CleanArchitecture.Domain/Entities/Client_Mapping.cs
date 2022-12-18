using System;

namespace BkashSNS.Domain.Entities
{
    public class Client_Mapping
    {
        public string client_name_group { get; set; }
        public string id { get; set; }
        public string merchantWallet { get; set; }
        public DateTime create_date { get; set; }
    }
}
