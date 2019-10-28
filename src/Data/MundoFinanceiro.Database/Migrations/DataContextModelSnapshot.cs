﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MundoFinanceiro.Database.Persistence;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MundoFinanceiro.Database.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("MundoFinanceiro.Database.Contracts.Persistence.Domains.Fundamento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<DateTime>("Data")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("data")
                        .HasDefaultValueSql("NOW()");

                    b.Property<double>("LPA")
                        .HasColumnName("lpa");

                    b.Property<short>("PapelId")
                        .HasColumnName("papel_id");

                    b.Property<double>("ROE")
                        .HasColumnName("roe");

                    b.Property<double>("ROIC")
                        .HasColumnName("roic");

                    b.Property<double>("VPA")
                        .HasColumnName("vpa");

                    b.Property<decimal>("ValorMercado")
                        .HasColumnName("valor_mercado");

                    b.HasKey("Id")
                        .HasName("pk_fundamentos");

                    b.HasIndex("PapelId")
                        .HasName("ix_fundamentos_papel_id");

                    b.ToTable("fundamentos");
                });

            modelBuilder.Entity("MundoFinanceiro.Database.Contracts.Persistence.Domains.Papel", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<bool>("Ativo")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ativo")
                        .HasDefaultValue(true);

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnName("nome")
                        .HasMaxLength(6);

                    b.HasKey("Id")
                        .HasName("pk_papeis");

                    b.HasIndex("Nome")
                        .IsUnique()
                        .HasName("ix_papeis_nome");

                    b.ToTable("papeis");
                });

            modelBuilder.Entity("MundoFinanceiro.Database.Contracts.Persistence.Domains.Parametro", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id");

                    b.Property<string>("Chave")
                        .IsRequired()
                        .HasColumnName("chave")
                        .HasMaxLength(64);

                    b.Property<string>("Descricao")
                        .IsRequired()
                        .HasColumnName("descricao")
                        .HasMaxLength(128);

                    b.Property<decimal?>("ValorNumerico")
                        .HasColumnName("valor_numerico");

                    b.Property<string>("ValorTexto")
                        .HasColumnName("valor_texto");

                    b.HasKey("Id")
                        .HasName("pk_parametros");

                    b.HasIndex("Chave")
                        .IsUnique()
                        .HasName("ix_parametros_chave");

                    b.ToTable("parametros");
                });

            modelBuilder.Entity("MundoFinanceiro.Database.Contracts.Persistence.Domains.Fundamento", b =>
                {
                    b.HasOne("MundoFinanceiro.Database.Contracts.Persistence.Domains.Papel", "Papel")
                        .WithMany("Fundamentos")
                        .HasForeignKey("PapelId")
                        .HasConstraintName("fk_fundamentos_papeis_papel_id")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
