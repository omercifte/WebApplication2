using System;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebApplication2.Models
{
    public partial class BlogDB : DbContext
    {
        public BlogDB()
            : base("name=BlogDB")
        {
        }

        public virtual DbSet<Etiket> Etiket { get; set; }
        public virtual DbSet<Kategori> Kategori { get; set; }
        public virtual DbSet<Kullanici> Kullanici { get; set; }
        public virtual DbSet<Makale> Makale { get; set; }
        public virtual DbSet<Yetki> Yetki { get; set; }
        public virtual DbSet<Yorum> Yorum { get; set; }
        public IEnumerable Katagori { get; internal set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Etiket>()
                .HasMany(e => e.Makale)
                .WithMany(e => e.Etiket)
                .Map(m => m.ToTable("EtiketMakale").MapLeftKey("EtiketId").MapRightKey("MakaleId"));

            modelBuilder.Entity<Kategori>()
                .HasMany(e => e.Makale)
                .WithRequired(e => e.Kategori)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.Makale)
                .WithRequired(e => e.Kullanici)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kullanici>()
                .HasMany(e => e.Yorum)
                .WithRequired(e => e.Kullanici)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Makale>()
                .HasMany(e => e.Yorum)
                .WithRequired(e => e.Makale)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Yetki>()
                .HasMany(e => e.Kullanici)
                .WithRequired(e => e.Yetki)
                .WillCascadeOnDelete(false);
        }
    }
}
