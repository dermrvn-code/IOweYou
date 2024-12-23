using System.ComponentModel.DataAnnotations.Schema;

namespace IOweYou.Models;

public abstract class Entity
{
    [Column(TypeName = "char(36)")]
    public Guid ID { get; set; }
}