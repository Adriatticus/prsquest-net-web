using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace prsquest_api_controllers.Models;

[Table("User")]
[Index("CharacterName", Name = "charname", IsUnique = true)]
[Index("Username", Name = "uname", IsUnique = true)]
public partial class User
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Username { get; set; } = null!;

    [StringLength(10)]
    [Unicode(false)]
    public string Password { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [StringLength(12)]
    [Unicode(false)]
    public string PhoneNumber { get; set; } = null!;

    [StringLength(75)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string CharacterName { get; set; } = null!;

    [StringLength(25)]
    [Unicode(false)]
    public string Class { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string? Title { get; set; }

    public bool Reviewer { get; set; }

    public bool Admin { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
