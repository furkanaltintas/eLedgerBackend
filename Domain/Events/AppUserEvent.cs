﻿using MediatR;

namespace Domain.Events;

public class AppUserEvent : INotification
{
    public Guid UserId { get; private set; }

    public AppUserEvent(Guid userId)
    {
        UserId = userId;
    }
}