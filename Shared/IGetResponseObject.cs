using System.Collections.Generic;

namespace Shared
{
    public interface IGetResponseObject
    {
        int Skip { get; }
        int Limit { get; }
        long Count { get; }
        IEnumerable<ValidationNotification> ValidationNotifications { get; }
        int StatusCode { get; }
    }
}