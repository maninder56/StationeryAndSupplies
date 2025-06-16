using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace DataBaseContextLibrary;

[Table("cart")]
[Index("UserId", Name = "UserID", IsUnique = true)]
[Index("UserId", Name = "UserID_2", IsUnique = true)]
public partial class Cart
{
    [Key]
    [Column("CartID")]
    public int CartId { get; set; }

    [Column("UserID")]
    public int UserId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [InverseProperty("Cart")]
    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    [ForeignKey("UserId")]
    [InverseProperty("Cart")]
    public virtual User User { get; set; } = null!;
}
