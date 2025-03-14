﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace prsquest_api_controllers.Models;

[Table("LineItem")]
[Index("RequestId", "ProductId", Name = "reqpdt", IsUnique = true)]
public partial class LineItem
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column("RequestID")]
    public int RequestId { get; set; }

    [Column("ProductID")]
    public int ProductId { get; set; }

    public int Quantity { get; set; }

    //[ForeignKey("ProductId")]
    //[InverseProperty("LineItems")]
    public virtual Product? Product { get; set; } = null!;

    //[ForeignKey("RequestId")]
    //[InverseProperty("LineItems")]
    public virtual Request? Request { get; set; } = null!;
}
