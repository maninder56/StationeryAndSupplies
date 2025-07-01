using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBaseContextLibrary;

[Table("products")]
[Index("CategoryId", Name = "CategoryID")]
public partial class Product
{
    [Key]
    [Column("ProductID")]
    public int ProductId { get; set; }

    [StringLength(200)]
    public string Name { get; set; } = null!;

    [Column(TypeName = "text")]
    public string? Descripttion { get; set; }

    [Precision(10)]
    public decimal Price { get; set; }

    public int Stock { get; set; }

    [Column(TypeName = "enum('active','inactive','archived')")]
    public string? Status { get; set; }

    [StringLength(200)]
    public string? ImageUrl { get; set; }

    [Column(TypeName = "date")]
    public DateTime? CreatedAt { get; set; }

    [Column("CategoryID")]
    public int CategoryId { get; set; }

    [InverseProperty("Product")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [ForeignKey("CategoryId")]
    [InverseProperty("Products")]
    public virtual Category Category { get; set; } = null!;

    [InverseProperty("Product")]
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    [InverseProperty("Product")]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}
