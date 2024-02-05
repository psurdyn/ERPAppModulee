﻿namespace ERPAppModuleDb.Entities;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime? CreatedAt { get; set; }
}