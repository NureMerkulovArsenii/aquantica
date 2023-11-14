using System;

namespace Aquantica.Infrastructure.Core.Entities;

public class BaseEntity
{
    public int Id { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime DateUpdated { get; set; }
}
