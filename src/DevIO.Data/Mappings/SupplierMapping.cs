using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevIO.Data.Mappings
{
    public class SupplierMapping : IEntityTypeConfiguration<Supplier>
    {
        public void Configure(EntityTypeBuilder<Supplier> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Name)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(p => p.Document)
                .IsRequired()
                .HasColumnType("varchar(14)");

            // 1 : 1 => Fornecedor : Endereco
            builder.HasOne(f => f.Address)
                .WithOne(e => e.Supplier);

            // 1 : N => Fornecedor : Produtos
            builder.HasMany(f => f.Products)
                .WithOne(p => p.Supplier)
                .HasForeignKey(p => p.SupplierId);

            builder.ToTable("Suppliers");
        }
    }
}