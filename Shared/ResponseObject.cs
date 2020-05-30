using System.Collections.Generic;
using System.Text.Json;
using System.Xml;

namespace Shared
{
    public abstract class ResponseObject
    {

        public IEnumerable<ValidationNotification> ValidationNotifications { get; protected set; }
        public int StatusCode { get; protected set; }
        public int Skip { get; protected set; }
        public int Limit { get; protected set; }
        public int Total { get; protected set; }
        
        public string ToJson()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
