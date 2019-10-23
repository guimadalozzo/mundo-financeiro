using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;

namespace MundoFinanceiro.Database.Persistence.EntityConfigurations
{
    public class FundamentoConfiguration : IEntityTypeConfiguration<Fundamento>
    {
        public void Configure(EntityTypeBuilder<Fundamento> builder)
        {
            builder.ToTable("fundamentos");
            
            // Properties
            builder
                .Property(x => x.LPA)
                .IsRequired();
            
            builder
                .Property(x => x.VPA)
                .IsRequired();
            
            builder
                .Property(x => x.ROE)
                .IsRequired();
            builder
                .Property(x => x.ROIC)
                .IsRequired();
            
            builder
                .Property(x => x.ValorMercado)
                .IsRequired();
            
            builder
                .Property(x => x.Data)
                .HasDefaultValueSql("NOW()")
                .IsRequired();
        }
    }
}