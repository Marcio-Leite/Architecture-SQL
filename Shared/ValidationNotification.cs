using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;

namespace Shared
{
    [DataContract]
    public class ValidationNotification : IEquatable<ValidationNotification>
    {
        public ValidationNotification(string field, string message)
        {
            Field = field;
            Message = message;
        }
        
        public ValidationNotification(IDictionary<string,string> message, string additionalMessage = "")
        {
            Field = message.First().Key;
            Message = message.First().Value + " " + additionalMessage;
        }

        [DataMember(Name = "field")]
        public string Field { get; }
        [DataMember(Name = "message")]
        public string Message { get; }

        public bool Equals(ValidationNotification other)
        {
            var thisString = JsonSerializer.Serialize(this);
            var otherString = JsonSerializer.Serialize(other);
            return String.Equals(thisString, otherString);
        }
    }
}