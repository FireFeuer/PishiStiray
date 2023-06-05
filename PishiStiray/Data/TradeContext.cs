using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PishiStiray.Model;

namespace PishiStiray.Data;

public partial class TradeContext : DbContext
{
    public TradeContext()
    {
    }

    public TradeContext(DbContextOptions<TradeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<IssueTable> IssueTables { get; set; }

    public virtual DbSet<ManufacturersTable> ManufacturersTables { get; set; }

    public virtual DbSet<Order1> Order1s { get; set; }

    public virtual DbSet<Orderproducttrue> Orderproducttrues { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProfessionTable> ProfessionTables { get; set; }

    public virtual DbSet<SuppliersTable> SuppliersTables { get; set; }

    public virtual DbSet<TypeOfAccessoryTable> TypeOfAccessoryTables { get; set; }

    public virtual DbSet<TypeOfPackagingTable> TypeOfPackagingTables { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Usertrue> Usertrues { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;user=root;password=1234;database=trade", ServerVersion.Parse("8.0.31-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<IssueTable>(entity =>
        {
            entity.HasKey(e => e.NumberIssue).HasName("PRIMARY");

            entity.ToTable("issue_table");

            entity.Property(e => e.NumberIssue).HasColumnName("number_issue");
            entity.Property(e => e.City)
                .HasMaxLength(45)
                .HasColumnName("city");
            entity.Property(e => e.House).HasColumnName("house");
            entity.Property(e => e.Index).HasColumnName("index");
            entity.Property(e => e.Issue)
                .HasMaxLength(100)
                .HasColumnName("issue");
        });

        modelBuilder.Entity<ManufacturersTable>(entity =>
        {
            entity.HasKey(e => e.Number).HasName("PRIMARY");

            entity.ToTable("manufacturers_table");

            entity.Property(e => e.Number)
                .ValueGeneratedNever()
                .HasColumnName("number");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Order1>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("order1");

            entity.HasIndex(e => e.PointIssue, "point_issue_idx");

            entity.HasIndex(e => e.Status, "status_idx");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Code).HasColumnName("code");
            entity.Property(e => e.DeliveryDate)
                .HasMaxLength(45)
                .HasColumnName("delivery_date");
            entity.Property(e => e.Fio)
                .HasMaxLength(45)
                .HasColumnName("FIO");
            entity.Property(e => e.OrderDate)
                .HasMaxLength(30)
                .HasColumnName("order_date");
            entity.Property(e => e.PointIssue).HasColumnName("point_issue");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasMany(d => d.ProductArticleNumbers).WithMany(p => p.Orders)
                .UsingEntity<Dictionary<string, object>>(
                    "Orderproduct",
                    r => r.HasOne<Product>().WithMany()
                        .HasForeignKey("ProductArticleNumber")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("orderproduct_ibfk_2"),
                    l => l.HasOne<Order1>().WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("orderproduct_ibfk_1"),
                    j =>
                    {
                        j.HasKey("OrderId", "ProductArticleNumber")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("orderproduct");
                        j.HasIndex(new[] { "ProductArticleNumber" }, "ProductArticleNumber");
                        j.IndexerProperty<int>("OrderId").HasColumnName("OrderID");
                        j.IndexerProperty<string>("ProductArticleNumber")
                            .HasMaxLength(100)
                            .UseCollation("utf8mb3_general_ci")
                            .HasCharSet("utf8mb3");
                    });
        });

        modelBuilder.Entity<Orderproducttrue>(entity =>
        {
            entity.HasKey(e => e.Key);

            entity.ToTable("orderproducttrue");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("ID");
            entity.Property(e => e.Article)
                .HasMaxLength(45)
                .HasColumnName("article");
            entity.Property(e => e.Sum)
                .HasMaxLength(45)
                .HasColumnName("sum");
            entity.Property(e => e.Key)
                .ValueGeneratedNever()
                .HasColumnName("key");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductArticleNumber).HasName("PRIMARY");

            entity.ToTable("product");

            entity.HasIndex(e => e.Manufacturer, "product_idx");

            entity.Property(e => e.ProductArticleNumber)
                .HasMaxLength(100)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Cost).HasColumnName("cost");
            entity.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");
            entity.Property(e => e.Discount)
                .HasMaxLength(30)
                .HasColumnName("discount");
            entity.Property(e => e.Manufacturer)
                .HasMaxLength(30)
                .HasColumnName("manufacturer");
            entity.Property(e => e.NowDiscount).HasColumnName("now_discount");
            entity.Property(e => e.Picture)
                .HasMaxLength(45)
                .HasColumnName("picture");
            entity.Property(e => e.ProductCategory).HasColumnName("product category");
            entity.Property(e => e.ProductName).HasColumnType("text");
            entity.Property(e => e.Provider).HasColumnName("provider");
            entity.Property(e => e.QuantityStock).HasColumnName("quantity_stock");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.UnitOfMeasurement).HasColumnName("unit_of_measurement");
        });

        modelBuilder.Entity<ProfessionTable>(entity =>
        {
            entity.HasKey(e => e.Number).HasName("PRIMARY");

            entity.ToTable("profession_table");

            entity.Property(e => e.Number)
                .ValueGeneratedNever()
                .HasColumnName("number");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
        });

        modelBuilder.Entity<SuppliersTable>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PRIMARY");

            entity.ToTable("suppliers_table");

            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Number).HasColumnName("number");
        });

        modelBuilder.Entity<TypeOfAccessoryTable>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PRIMARY");

            entity.ToTable("type_of_accessory_table");

            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Number).HasColumnName("number");
        });

        modelBuilder.Entity<TypeOfPackagingTable>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PRIMARY");

            entity.ToTable("type_of_packaging_table");

            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Number).HasColumnName("number");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("user");

            entity.HasIndex(e => e.UserRole, "UserRole");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserLogin).HasColumnType("text");
            entity.Property(e => e.UserName).HasMaxLength(100);
            entity.Property(e => e.UserPassword).HasColumnType("text");
            entity.Property(e => e.UserPatronymic).HasMaxLength(100);
            entity.Property(e => e.UserSurname).HasMaxLength(100);
        });

        modelBuilder.Entity<Usertrue>(entity =>
        {
            entity.HasKey(e => e.Login).HasName("PRIMARY");

            entity.ToTable("usertrue");

            entity.HasIndex(e => e.Role, "role_idx");

            entity.Property(e => e.Login)
                .HasMaxLength(45)
                .HasColumnName("login");
            entity.Property(e => e.Name)
                .HasMaxLength(45)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(45)
                .HasColumnName("password");
            entity.Property(e => e.Patronymic)
                .HasMaxLength(45)
                .HasColumnName("patronymic");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.Surname)
                .HasMaxLength(45)
                .HasColumnName("surname");

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Usertrues)
                .HasForeignKey(d => d.Role)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
