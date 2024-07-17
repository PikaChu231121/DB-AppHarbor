using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AppHarbor.Server.Models;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Announcement> Announcements { get; set; }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<ApplicationReview> ApplicationReviews { get; set; }

    public virtual DbSet<BanMerchant> BanMerchants { get; set; }

    public virtual DbSet<BanUser> BanUsers { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Favourite> Favourites { get; set; }

    public virtual DbSet<Merchant> Merchants { get; set; }

    public virtual DbSet<Operation> Operations { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Relationship> Relationships { get; set; }

    public virtual DbSet<Report> Reports { get; set; }

    public virtual DbSet<ReportReview> ReportReviews { get; set; }

    public virtual DbSet<TokenId> TokenIds { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("User Id=c##appharbor;Password=st_31112;Data Source=118.25.195.145;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##APPHARBOR")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008489");

            entity.ToTable("admin");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("\"C##APPHARBOR\".\"ADMIN_SEQ\".\"NEXTVAL\"")
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Avatar)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("AVATAR");
            entity.Property(e => e.Credit)
                .HasColumnType("NUMBER(8,2)")
                .HasColumnName("CREDIT");
            entity.Property(e => e.Nickname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("NICKNAME");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.RegisterTime)
                .HasColumnType("DATE")
                .HasColumnName("REGISTER_TIME");
            entity.Property(e => e.State)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("STATE");
        });

        modelBuilder.Entity<Announcement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008507");

            entity.ToTable("announcement");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("\"C##APPHARBOR\".\"ANNOUNCEMENT_SEQ\".\"NEXTVAL\"")
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.AdminId)
                .HasColumnType("NUMBER")
                .HasColumnName("ADMIN_ID");
            entity.Property(e => e.Content)
                .IsUnicode(false)
                .HasColumnName("CONTENT");
            entity.Property(e => e.PublishTime)
                .HasColumnType("DATE")
                .HasColumnName("PUBLISH_TIME");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("TITLE");

            entity.HasOne(d => d.Admin).WithMany(p => p.Announcements)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008508");
        });

        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008495");

            entity.ToTable("application");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("\"C##APPHARBOR\".\"APPLICATION_SEQ\".\"NEXTVAL\"")
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Category)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("CATEGORY");
            entity.Property(e => e.Description)
                .IsUnicode(false)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.DownloadCount)
                .HasColumnType("NUMBER")
                .HasColumnName("DOWNLOAD_COUNT");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER")
                .HasColumnName("PRICE");
            entity.Property(e => e.Image)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("IMAGE");
            entity.Property(e => e.MerchantId)
                .HasColumnType("NUMBER")
                .HasColumnName("MERCHANT_ID");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("NAME");
            entity.Property(e => e.ReleaseState)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("RELEASE_STATE");
            entity.Property(e => e.Version)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("VERSION");
            entity.Property(e => e.Package)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PACKAGE");

            entity.HasOne(d => d.Merchant).WithMany(p => p.Applications)
                .HasForeignKey(d => d.MerchantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008496");
        });

        modelBuilder.Entity<ApplicationReview>(entity =>
        {
            entity.HasKey(e => new { e.ApplicationId, e.AdminId }).HasName("SYS_C008552");

            entity.ToTable("application_review");

            entity.Property(e => e.ApplicationId)
                .HasColumnType("NUMBER")
                .HasColumnName("APPLICATION_ID");
            entity.Property(e => e.AdminId)
                .HasColumnType("NUMBER")
                .HasColumnName("ADMIN_ID");
            entity.Property(e => e.Result)
                .IsUnicode(false)
                .HasColumnName("RESULT");
            entity.Property(e => e.ReviewTime)
                .HasColumnType("DATE")
                .HasColumnName("REVIEW_TIME");

            entity.HasOne(d => d.Admin).WithMany(p => p.ApplicationReviews)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008554");

            entity.HasOne(d => d.Application).WithMany(p => p.ApplicationReviews)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008553");
        });

        modelBuilder.Entity<BanMerchant>(entity =>
        {
            entity.HasKey(e => new { e.AdminId, e.MerchantId }).HasName("SYS_C008541");

            entity.ToTable("ban_merchant");

            entity.Property(e => e.AdminId)
                .HasColumnType("NUMBER")
                .HasColumnName("ADMIN_ID");
            entity.Property(e => e.MerchantId)
                .HasColumnType("NUMBER")
                .HasColumnName("MERCHANT_ID");
            entity.Property(e => e.Reason)
                .IsUnicode(false)
                .HasColumnName("REASON");
            entity.Property(e => e.Time)
                .HasColumnType("DATE")
                .HasColumnName("TIME");

            entity.HasOne(d => d.Admin).WithMany(p => p.BanMerchants)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008542");

            entity.HasOne(d => d.Merchant).WithMany(p => p.BanMerchants)
                .HasForeignKey(d => d.MerchantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008543");
        });

        modelBuilder.Entity<BanUser>(entity =>
        {
            entity.HasKey(e => new { e.AdminId, e.UserId }).HasName("SYS_C008536");

            entity.ToTable("ban_user");

            entity.Property(e => e.AdminId)
                .HasColumnType("NUMBER")
                .HasColumnName("ADMIN_ID");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");
            entity.Property(e => e.Reason)
                .IsUnicode(false)
                .HasColumnName("REASON");
            entity.Property(e => e.Time)
                .HasColumnType("DATE")
                .HasColumnName("TIME");

            entity.HasOne(d => d.Admin).WithMany(p => p.BanUsers)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008537");

            entity.HasOne(d => d.User).WithMany(p => p.BanUsers)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008538");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008501");

            entity.ToTable("comment");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("\"C##APPHARBOR\".\"COMMENT_SEQ\".\"NEXTVAL\"")
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.ApplicationId)
                .HasColumnType("NUMBER")
                .HasColumnName("APPLICATION_ID");
            entity.Property(e => e.Content)
                .IsUnicode(false)
                .HasColumnName("CONTENT");
            entity.Property(e => e.PublishTime)
                .HasColumnType("DATE")
                .HasColumnName("PUBLISH_TIME");
            entity.Property(e => e.Score)
                .HasColumnType("NUMBER")
                .HasColumnName("SCORE");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Application).WithMany(p => p.Comments)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008502");

            entity.HasOne(d => d.User).WithMany(p => p.Comments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008503");
        });

        modelBuilder.Entity<Favourite>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008525");

            entity.ToTable("favourite");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("\"C##APPHARBOR\".\"FAVOURITE_SEQ\".\"NEXTVAL\"")
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.ApplicationId)
                .HasColumnType("NUMBER")
                .HasColumnName("APPLICATION_ID");
            entity.Property(e => e.CreateTime)
                .HasColumnType("DATE")
                .HasColumnName("CREATE_TIME");
            // entity.Property(e => e.Name)
            //     .HasMaxLength(255)
            //     .IsUnicode(false)
            //     .HasColumnName("NAME");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");
            entity.Property(e => e.Visibility)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("VISIBILITY");

            entity.HasOne(d => d.Application).WithMany(p => p.Favourites)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008527");

            entity.HasOne(d => d.User).WithMany(p => p.Favourites)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008526");
        });

        modelBuilder.Entity<Merchant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008484");

            entity.ToTable("merchant");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("\"C##APPHARBOR\".\"MERCHANT_SEQ\".\"NEXTVAL\"")
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Avatar)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("AVATAR");
            entity.Property(e => e.Credit)
                .HasColumnType("NUMBER(8,2)")
                .HasColumnName("CREDIT");
            entity.Property(e => e.Nickname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("NICKNAME");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.RegisterTime)
                .HasColumnType("DATE")
                .HasColumnName("REGISTER_TIME");
            entity.Property(e => e.State)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("STATE");
        });

        modelBuilder.Entity<Operation>(entity =>
        {
            entity.HasKey(e => new { e.ApplicationId, e.Version }).HasName("SYS_C008560");

            entity.ToTable("operation");

            entity.Property(e => e.ApplicationId)
                .HasColumnType("NUMBER")
                .HasColumnName("APPLICATION_ID");
            entity.Property(e => e.Version)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("VERSION");
            entity.Property(e => e.AdminId)
                .HasColumnType("NUMBER")
                .HasColumnName("ADMIN_ID");
            entity.Property(e => e.OperationTime)
                .HasColumnType("DATE")
                .HasColumnName("OPERATION_TIME");
            entity.Property(e => e.OperationType)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("OPERATION_TYPE");

            entity.HasOne(d => d.Admin).WithMany(p => p.Operations)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008562");

            entity.HasOne(d => d.Application).WithMany(p => p.Operations)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008561");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008519");

            entity.ToTable("order");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("\"C##APPHARBOR\".\"ORDER_SEQ\".\"NEXTVAL\"")
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Amount)
                .HasColumnType("NUMBER(8,2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.ApplicationId)
                .HasColumnType("NUMBER")
                .HasColumnName("APPLICATION_ID");
            entity.Property(e => e.BuyerId)
                .HasColumnType("NUMBER")
                .HasColumnName("BUYER_ID");
            entity.Property(e => e.ReceiverId)
                .HasColumnType("NUMBER")
                .HasColumnName("RECEIVER_ID");
            entity.Property(e => e.Time)
                .HasColumnType("DATE")
                .HasColumnName("TIME");
            entity.Property(e => e.Type)
                .HasMaxLength(255)
                .HasColumnName("TYPE");

            entity.HasOne(d => d.Application).WithMany(p => p.Orders)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008520");

            entity.HasOne(d => d.Buyer).WithMany(p => p.OrderBuyers)
                .HasForeignKey(d => d.BuyerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008521");

            entity.HasOne(d => d.Receiver).WithMany(p => p.OrderReceivers)
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008522");
        });

        modelBuilder.Entity<Relationship>(entity =>
        {
            entity.HasKey(e => new { e.User1Id, e.User2Id }).HasName("SYS_C008531");

            entity.ToTable("relationship");

            entity.Property(e => e.User1Id)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_1_ID");
            entity.Property(e => e.User2Id)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_2_ID");
            entity.Property(e => e.CreateTime)
                .HasColumnType("DATE")
                .HasColumnName("CREATE_TIME");
            entity.Property(e => e.Relationship1)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("RELATIONSHIP");

            entity.HasOne(d => d.User1).WithMany(p => p.RelationshipUser1s)
                .HasForeignKey(d => d.User1Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008532");

            entity.HasOne(d => d.User2).WithMany(p => p.RelationshipUser2s)
                .HasForeignKey(d => d.User2Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008533");
        });

        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008512");

            entity.ToTable("report");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("\"C##APPHARBOR\".\"REPORT_SEQ\".\"NEXTVAL\"")
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.ApplicationId)
                .HasColumnType("NUMBER")
                .HasColumnName("APPLICATION_ID");
            entity.Property(e => e.Content)
                .IsUnicode(false)
                .HasColumnName("CONTENT");
            entity.Property(e => e.Time)
                .HasColumnType("DATE")
                .HasColumnName("TIME");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Application).WithMany(p => p.Reports)
                .HasForeignKey(d => d.ApplicationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008514");

            entity.HasOne(d => d.User).WithMany(p => p.Reports)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008513");
        });

        modelBuilder.Entity<ReportReview>(entity =>
        {
            entity.HasKey(e => e.ReportId).HasName("SYS_C008546");

            entity.ToTable("report_review");

            entity.Property(e => e.ReportId)
                .HasColumnType("NUMBER")
                .HasColumnName("REPORT_ID");
            entity.Property(e => e.AdminId)
                .HasColumnType("NUMBER")
                .HasColumnName("ADMIN_ID");
            entity.Property(e => e.ReceiveTime)
                .HasColumnType("DATE")
                .HasColumnName("RECEIVE_TIME");
            entity.Property(e => e.Result)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("RESULT");
            entity.Property(e => e.ReviewTime)
                .HasColumnType("DATE")
                .HasColumnName("REVIEW_TIME");

            entity.HasOne(d => d.Admin).WithMany(p => p.ReportReviews)
                .HasForeignKey(d => d.AdminId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008548");

            entity.HasOne(d => d.Report).WithOne(p => p.ReportReview)
                .HasForeignKey<ReportReview>(d => d.ReportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008547");
        });

        modelBuilder.Entity<TokenId>(entity =>
        {
            entity.HasKey(e => e.Token).HasName("SYS_C008569");

            entity.ToTable("token_id");

            entity.Property(e => e.Token)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("token");
            entity.Property(e => e.ExpireDate)
                .HasColumnType("DATE")
                .HasColumnName("expire_date");
            entity.Property(e => e.Id)
                .HasColumnType("NUMBER")
                .HasColumnName("id");

            entity.HasOne(d => d.IdNavigation).WithMany(p => p.TokenIds)
                .HasForeignKey(d => d.Id)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("SYS_C008570");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008479");

            entity.ToTable("user");

            entity.Property(e => e.Id)
                .HasDefaultValueSql("\"C##APPHARBOR\".\"USER_SEQ\".\"NEXTVAL\"")
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Avatar)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("AVATAR");
            entity.Property(e => e.Credit)
                .HasColumnType("NUMBER(8,2)")
                .HasColumnName("CREDIT");
            entity.Property(e => e.Nickname)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("NICKNAME");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.RegisterTime)
                .HasColumnType("DATE")
                .HasColumnName("REGISTER_TIME");
            entity.Property(e => e.State)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("STATE");
        });
        modelBuilder.HasSequence("ADMIN_SEQ");
        modelBuilder.HasSequence("ANNOUNCEMENT_SEQ");
        modelBuilder.HasSequence("APPLICATION_SEQ");
        modelBuilder.HasSequence("COMMENT_SEQ");
        modelBuilder.HasSequence("FAVOURITE_SEQ");
        modelBuilder.HasSequence("MERCHANT_SEQ");
        modelBuilder.HasSequence("ORDER_SEQ");
        modelBuilder.HasSequence("REPORT_SEQ");
        modelBuilder.HasSequence("USER_SEQ");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
