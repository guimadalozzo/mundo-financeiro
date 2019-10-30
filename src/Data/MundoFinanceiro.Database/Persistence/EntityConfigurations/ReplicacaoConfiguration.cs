using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;

namespace MundoFinanceiro.Database.Persistence.EntityConfigurations
{
    public class ReplicacaoConfiguration : IEntityTypeConfiguration<Replicacao>
    {
        public const int UrlMaxLength = 64;
        public const int DescricaoMaxLength = 64;
        
        public void Configure(EntityTypeBuilder<Replicacao> builder)
        {
            builder.ToTable("replicacoes");
            
            // Properties
            builder
                .Property(x => x.Url)
                .HasMaxLength(UrlMaxLength)
                .IsRequired();

            builder
                .Property(x => x.Descricao)
                .HasMaxLength(DescricaoMaxLength)
                .IsRequired();

            builder
                .Property(x => x.Ativo)
                .IsRequired()
                .HasDefaultValue(true);
            
            // Indexes
            builder
                .HasIndex(x => x.Url)
                .IsUnique();
        }
    }
}