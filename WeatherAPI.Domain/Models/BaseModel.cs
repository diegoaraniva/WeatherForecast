using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class BaseModel
{
    public int Id { get; set; }
    public bool IsEnabled { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime LastUpdate { get; set; }
}