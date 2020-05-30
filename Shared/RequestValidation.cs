using System.Collections.Generic;

namespace Shared
{
    public interface IRequestValidationExclude
    {
        List<ValidationNotification> ValidationNotifications { get; protected set; }
        bool IsValid { get; protected set; }
        void Validate();
        int Skip { get; }
        int Limit { get; }
    }
}