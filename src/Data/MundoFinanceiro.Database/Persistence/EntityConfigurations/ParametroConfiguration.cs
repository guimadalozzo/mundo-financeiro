using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MundoFinanceiro.Database.Contracts.Persistence.Domains;

namespace MundoFinanceiro.Database.Persistence.EntityConfigurations
{
    public class ParametroConfiguration : IEntityTypeConfiguration<Parametro>
    {
        public const byte ChaveMaxLength = 64;
        public const byte DescricaoMaxLength = 128;
        
        public void Configure(EntityTypeBuilder<Parametro> builder)
        {
            builder.ToTable("parametros");
            
            // Properties
            builder
                .Property(x => x.Chave)
                .IsRequired()
                .HasMaxLength(ChaveMaxLength);

            builder
                .Property(x => x.Descricao)
                .IsRequired()
                .HasMaxLength(DescricaoMaxLength);
            
            // Indexes
            builder
                .HasIndex(x => x.Chave)
                .IsUnique();
        }
    }
}