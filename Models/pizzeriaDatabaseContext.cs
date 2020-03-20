using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Lab1
{
    public partial class pizzeriaDatabaseContext : DbContext
    {
        public pizzeriaDatabaseContext()
        {
        }

        public pizzeriaDatabaseContext(DbContextOptions<pizzeriaDatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Animatronic> Animatronic { get; set; }
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<Meal> Meal { get; set; }
        public virtual DbSet<MealOrder> MealOrder { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<Pizzeria> Pizzeria { get; set; }
        public virtual DbSet<PizzeriaEvent> PizzeriaEvent { get; set; }
        public virtual DbSet<PizzeriaMeal> PizzeriaMeal { get; set; }
        public virtual DbSet<Review> Review { get; set; }
        public virtual DbSet<Species> Species { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=MARINA-HOME\\SQLEXPRESS;Database=pizzeriaDatabase;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Animatronic>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AnimatronicInfo).HasColumnType("text");

                entity.Property(e => e.AnimatronicName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PizzeriaId).HasColumnName("PizzeriaID");

                entity.Property(e => e.SpeciesId).HasColumnName("SpeciesID");

                entity.HasOne(d => d.Pizzeria)
                    .WithMany(p => p.Animatronic)
                    .HasForeignKey(d => d.PizzeriaId)
                    .HasConstraintName("FK_Animatronic_Pizzeria");

                entity.HasOne(d => d.Species)
                    .WithMany(p => p.Animatronic)
                    .HasForeignKey(d => d.SpeciesId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Animatronic_Species");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EventInfo).HasColumnType("text");

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EventPrice).HasColumnType("decimal(6, 2)");
            });

            modelBuilder.Entity<Meal>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CategoryId).HasColumnName("CategoryID");

                entity.Property(e => e.MealInfo).HasColumnType("text");

                entity.Property(e => e.MealName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MealPrice).HasColumnType("decimal(6, 2)");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Meal)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Meal_Category");
            });

            modelBuilder.Entity<MealOrder>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MealId).HasColumnName("MealID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.HasOne(d => d.Meal)
                    .WithMany(p => p.MealOrder)
                    .HasForeignKey(d => d.MealId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MealOrder_Meal");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.MealOrder)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MealOrder_Orders");
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Users");
            });

            modelBuilder.Entity<Pizzeria>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnType("text");

                entity.Property(e => e.PizzeriaInfo).HasColumnType("text");

                entity.Property(e => e.PizzeriaName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PizzeriaEvent>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.PizzeriaId).HasColumnName("PizzeriaID");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.PizzeriaEvent)
                    .HasForeignKey(d => d.EventId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PizzeriaEvent_Event");

                entity.HasOne(d => d.Pizzeria)
                    .WithMany(p => p.PizzeriaEvent)
                    .HasForeignKey(d => d.PizzeriaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PizzeriaEvent_Pizzeria");
            });

            modelBuilder.Entity<PizzeriaMeal>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.MealId).HasColumnName("MealID");

                entity.Property(e => e.PizzeriaId).HasColumnName("PizzeriaID");

                entity.HasOne(d => d.Meal)
                    .WithMany(p => p.PizzeriaMeal)
                    .HasForeignKey(d => d.MealId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PizzeriaMeal_Meal");

                entity.HasOne(d => d.Pizzeria)
                    .WithMany(p => p.PizzeriaMeal)
                    .HasForeignKey(d => d.PizzeriaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PizzeriaMeal_Pizzeria");
            });

            modelBuilder.Entity<Review>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PizzeriaId).HasColumnName("PizzeriaID");

                entity.Property(e => e.ReviewDate).HasColumnType("datetime");

                entity.Property(e => e.ReviewText)
                    .IsRequired()
                    .HasColumnType("text");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Pizzeria)
                    .WithMany(p => p.Review)
                    .HasForeignKey(d => d.PizzeriaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Review_Pizzeria");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Review)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Review_Users");
            });

            modelBuilder.Entity<Species>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.SpeciesName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
