using System;

namespace SampleWebSocket.Context
{
    public class Session
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string User { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public DateTimeOffset LogonTime { get; set; }
        public string Status { get; set; }
    }
}
