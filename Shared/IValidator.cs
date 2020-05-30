﻿using System.Collections.Generic;

namespace Shared
{
    public interface IValidator
    {
        List<ValidationNotification> ValidationNotifications { get; }
        bool IsValid { get; }
        void Validate();
        int Skip { get; }
        int Limit { get; }
    }
}