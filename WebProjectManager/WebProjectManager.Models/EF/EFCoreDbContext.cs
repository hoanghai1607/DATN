using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebProjectManager.Models.Entities;
using WebProjectManager.Models.Extentions;

namespace WebProjectManager.Models.EF
{
    public class EFCoreDbContext : IdentityDbContext<User, Role, Guid>
    {
        public EFCoreDbContext()
        {
        }

        public EFCoreDbContext(DbContextOptions<EFCoreDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccountVerification> AccountVerifications { get; set; } = null!;
        public virtual DbSet<Tab> Tabs { get; set; } = null!;
        public virtual DbSet<Card> Cards { get; set; } = null!;
        public virtual DbSet<CardUserMember> CardUserMembers { get; set; } = null!;
        public virtual DbSet<Invite> Invites { get; set; } = null!;
        public virtual DbSet<MemberProject> MemberProjects { get; set; } = null!;
        public virtual DbSet<Project> Projects { get; set; } = null!;
        public virtual DbSet<Role> Roles { get; set; } = null!;
        public virtual DbSet<WebProjectManager.Models.Entities.Task> Tasks { get; set; } = null!;
        public virtual DbSet<Ticket> Tickets { get; set; } = null!;
        public virtual DbSet<TicketProject> TicketProjects { get; set; } = null!;
        public virtual DbSet<Timesheet> Timesheets { get; set; } = null!;
        public virtual DbSet<TimesheetProject> TimesheetProjects { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<TaskUserMember> TaskUserMembers { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<AccountVerification>(entity =>
            {
                entity.ToTable("AccountVerification");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Code)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.CreateOn).HasColumnType("datetime");

                entity.Property(e => e.ExpiryOn).HasColumnType("datetime");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.AccountVerifications)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK__AccountVe__IdUse__15502E78");
            });
            modelBuilder.Entity<Tab>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.TimeExpiry).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Tabs)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__Tabs__CreatedBy__1B0907CE");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Tabs)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK__Tabs__ProjectId__1BFD2C07");
            });
            modelBuilder.Entity<Card>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Image).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.TimeExpiry).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Cards)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__Cards__CreatedBy__1B0907CE");

                entity.HasOne(d => d.Tab)
                    .WithMany(p => p.Cards)
                    .HasForeignKey(d => d.TabId)
                    .HasConstraintName("FK__Cards__TabId__1BFD2C07");
            });

            modelBuilder.Entity<CardUserMember>(entity =>
            {
                entity.ToTable("CardUserMember");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.CardUserMembers)
                    .HasForeignKey(d => d.CardId)
                    .HasConstraintName("FK__CardUserM__CardI__33D4B598");

                entity.HasOne(d => d.MemberNavigation)
                    .WithMany(p => p.CardUserMembers)
                    .HasForeignKey(d => d.Member)
                    .HasConstraintName("FK__CardUserM__Membe__31EC6D26");

            
            });

            modelBuilder.Entity<Invite>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Content).HasMaxLength(255);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.TimeExpiry).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Invites)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__Invites__Created__2A4B4B5E");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Invites)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK__Invites__Project__2B3F6F97");
            });

            modelBuilder.Entity<MemberProject>(entity =>
            {
                entity.ToTable("MemberProject");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.TimeExpiry).HasColumnType("datetime");

                entity.HasOne(d => d.IdUserNavigation)
                    .WithMany(p => p.MemberProjects)
                    .HasForeignKey(d => d.IdUser)
                    .HasConstraintName("FK__MemberPro__IdUse__2E1BDC42");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.MemberProjects)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK__MemberPro__Proje__2F10007B");
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Image).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.TimeExpiry).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Projects)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__Projects__Create__182C9B23");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(255);
            });

            modelBuilder.Entity<WebProjectManager.Models.Entities.Task>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Comment).HasMaxLength(255);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Icon).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.TimeExpiry).HasColumnType("datetime");

                entity.Property(e => e.Type).HasMaxLength(50);

                entity.HasOne(d => d.Card)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.CardId)
                    .HasConstraintName("FK__Tasks__CardId__1FCDBCEB");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__Tasks__CreatedBy__1ED998B2");
            });

            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Content).HasMaxLength(255);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.TimeExpiry).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Tickets)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__Tickets__Created__36B12243");
            });

            modelBuilder.Entity<TicketProject>(entity =>
            {
                entity.ToTable("TicketProject");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TicketProjects)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__TicketPro__Creat__398D8EEE");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.TicketProjects)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK__TicketPro__Proje__3A81B327");

                entity.HasOne(d => d.Ticket)
                    .WithMany(p => p.TicketProjects)
                    .HasForeignKey(d => d.TicketId)
                    .HasConstraintName("FK__TicketPro__Ticke__3B75D760");
            });

            modelBuilder.Entity<Timesheet>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Content).HasMaxLength(255);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Icon).HasMaxLength(255);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.TimeBegin).HasColumnType("datetime");

                entity.Property(e => e.TimeExpiry).HasColumnType("datetime");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.Timesheets)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__Timesheet__Creat__22AA2996");
            });

            modelBuilder.Entity<TimesheetProject>(entity =>
            {
                entity.ToTable("TimesheetProject");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TimesheetProjects)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK__Timesheet__Creat__25869641");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.TimesheetProjects)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK__Timesheet__Proje__267ABA7A");

                entity.HasOne(d => d.Timesheet)
                    .WithMany(p => p.TimesheetProjects)
                    .HasForeignKey(d => d.TimesheetId)
                    .HasConstraintName("FK__Timesheet__Times__276EDEB3");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(255);

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false);


                entity.Property(e => e.Firstname).HasMaxLength(255);

                entity.Property(e => e.Lastname).HasMaxLength(255);

                

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

                entity.Property(e => e.UrlAvatar).HasMaxLength(255);

                
            });
            modelBuilder.Entity<TaskUserMember>(entity =>
            {
                entity.ToTable("TaskUserMember");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.TaskNavigation)
                    .WithMany(p => p.TaskUserMember)
                    .HasForeignKey(d => d.TaskId)
                    .HasConstraintName("FK__TaskUserM__TaskI__33D4B598");

                entity.HasOne(d => d.MemberNavigation)
                    .WithMany(p => p.TaskUserMember)
                    .HasForeignKey(d => d.Member)
                    .HasConstraintName("FK__TaskUserM__Membe__31EC6D26");


            });
            modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims");
            modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims");
            modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => x.UserId);
            modelBuilder.Seed();
        }

    }
}
