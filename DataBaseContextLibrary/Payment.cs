using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBaseContextLibrary;

[Table("payments")]
[Index("OrderId", Name = "OrderID", IsUnique = true)]
public partial class Payment
{
    [Key]
    [Column("PaymentID")]
    public int PaymentId { get; set; }

    [Column("OrderID")]
    public int OrderId { get; set; }

    [Precision(10)]
    public decimal Amount { get; set; }

    [StringLength(50)]
    public string PymentMethod { get; set; } = null!;

    [Column(TypeName = "enum('success','failed')")]
    public string? Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime PayedAt { get; set; }

    [ForeignKey("OrderId")]
    [InverseProperty("Payment")]
    public virtual Order Order { get; set; } = null!;
}
