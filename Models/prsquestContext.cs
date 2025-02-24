using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace prsquest_api_controllers.Models;

public partial class prsquestContext : DbContext
{
    public prsquestContext()
    {
    }

    public prsquestContext(DbContextOptions<prsquestContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LineItem> LineItems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Vendor> Vendors { get; set; }
}

