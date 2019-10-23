using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;

namespace MundoFinanceiro.Database.Persistence.EntityConfigurations
{
    public class PapelConfiguration : IEntityTypeConfiguration<Papel>
    {
        public const int NomeMaxLength = 6;
        
        public void Configure(EntityTypeBuilder<Papel> builder)
        {
            builder.ToTable("papeis");
            
            // Properties
            builder
                .Property(x => x.Nome)
                .IsRequired()
                .HasMaxLength(NomeMaxLength);
            
            // Indexes
            builder
                .HasIndex(x => x.Nome)
                .IsUnique();
            
            // Relationships
            builder
                .HasMany(x => x.Fundamentos)
                .WithOne(x => x.Papel)
                .HasForeignKey(x => x.PapelId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}