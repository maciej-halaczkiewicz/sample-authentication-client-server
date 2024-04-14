using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace simple_authentication_client_infrastructure.Context;

public partial class TemplateAppContext : DbContext
{
    partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
    {
    }
}
