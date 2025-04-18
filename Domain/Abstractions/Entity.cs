﻿namespace Domain.Abstractions;

public abstract class Entity
{
    protected Entity() { Id = Guid.NewGuid(); }

    public Guid Id { get; set; }
    public bool IsDeleted { get; set; } = false;
}