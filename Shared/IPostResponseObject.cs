using System.Collections.Generic;

namespace Shared
{
    public interface IPostResponseObject
    {    
            IEnumerable<ValidationNotification> ValidationNotifications { get; }
            int StatusCode { get; }
    }
}