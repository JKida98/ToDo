using System.ComponentModel.DataAnnotations;

namespace ToDo.Database.Models;

public abstract class BaseModel
{
    [Key] public Guid Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}