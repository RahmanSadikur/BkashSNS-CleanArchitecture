


namespace BkashSNS.Domain.Entities
{
    public class Messages
    {
        public string Type { get; set; }
        public string MessageId { get; set; }
        public string Token { get; set; }
        public string TopicArn { get; set; }
        public object Message { get; set; }
        //public Message Message2 { get; set; }
        public string SubscribeURL { get; set; }
        public string Timestamp { get; set; }
        public string SignatureVersion { get; set; }
        public string Signature { get; set; }
        public string SigningCertURL { get; set; }
        public string Subject { get; set; }
        public string UnsubscribeURL { get; set; }
    }
}
