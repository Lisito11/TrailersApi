using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using TrailersApi.Models;

#nullable disable

namespace TrailersApi
{
    public partial class TrailersDbContext : IdentityDbContext<UserTrailer, RoleUserTrailer,int>
    {
        public TrailersDbContext()
        {
        }

        public TrailersDbContext(DbContextOptions<TrailersDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Trailer> Trailers { get; set; }
        public virtual DbSet<UserTrailer> UserTrailers { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.UTF-8");

            modelBuilder.Entity<Trailer>(entity =>
            {
                entity.ToTable("trailer");

                entity.Property(e => e.Id)
                    .HasColumnName("trailerid")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.Director)
                    .HasMaxLength(150)
                    .HasColumnName("director");

                entity.Property(e => e.Fecha)
                    .HasColumnType("date")
                    .HasColumnName("fecha");

                entity.Property(e => e.Genero)
                    .HasMaxLength(150)
                    .HasColumnName("genero");

                entity.Property(e => e.Puntaje).HasColumnName("puntaje");

                entity.Property(e => e.Sipnosis).HasColumnName("sipnosis");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.Property(e => e.Titulo)
                    .HasMaxLength(150)
                    .HasColumnName("titulo");

                entity.Property(e => e.Url)
                    .HasMaxLength(300)
                    .HasColumnName("url");
            });

            modelBuilder.Entity<UserTrailer>(entity => {
                entity.ToTable("usertrailer");

                entity.Property(e => e.UserTrailerId)
                    .HasColumnName("usertrailerid")
                    .UseIdentityAlwaysColumn();

                entity.Property(e => e.FirstName)
                    .HasMaxLength(150)
                    .HasColumnName("firstname");
               
                entity.Property(e => e.LastName)
                    .HasMaxLength(150)
                    .HasColumnName("lastname");

                entity.Property(e => e.Email)
                   .HasMaxLength(150)
                   .HasColumnName("email");

            });
            base.OnModelCreating(modelBuilder);
            
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
