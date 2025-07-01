using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBaseContextLibrary;

[Table("orders")]
[Index("UserId", Name = "UserID")]
public partial class Order
{
    [Key]
    [Column("OrderID")]
    public int OrderId { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Precision(10)]
    public decimal? TotalAmount { get; set; }

    [Column(TypeName = "enum('pending','shipped','cancelled','delivered')")]
    public string? Status { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime OrderDate { get; set; }

    [Column(TypeName = "text")]
    public string ShippingAddress { get; set; } = null!;

    [Precision(10)]
    public decimal ShippingCost { get; set; }

    [InverseProperty("Order")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [InverseProperty("Order")]
    public virtual Payment? Payment { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Orders")]
    public virtual User User { get; set; } = null!;
}
