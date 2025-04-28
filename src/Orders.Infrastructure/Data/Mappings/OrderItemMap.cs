using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orders.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Infrastructure.Data.Mappings
{
    public class OrderItemMap : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.ProductId)
                .IsRequired();

            builder.Property(i => i.ProductName)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(i => i.UnitPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(i => i.Quantity)
                .IsRequired();

            builder.Property(i => i.CreatedAt)
                .IsRequired();
        }
    }
}
