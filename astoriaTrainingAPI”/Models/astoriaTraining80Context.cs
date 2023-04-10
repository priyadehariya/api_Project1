using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace astoriaTrainingAPI_.Models
{
    public partial class astoriaTraining80Context : DbContext
    {
      

        public astoriaTraining80Context(DbContextOptions<astoriaTraining80Context> options)
            : base(options)
        {
        }

        public virtual DbSet<AllowanceMaster> AllowanceMaster { get; set; }
        public virtual DbSet<CompanyMaster> CompanyMaster { get; set; }
        public virtual DbSet<DesignationMaster> DesignationMaster { get; set; }
        public virtual DbSet<EmployeeAllowanceDetail> EmployeeAllowanceDetail { get; set; }
        public virtual DbSet<EmployeeAttendance> EmployeeAttendance { get; set; }
        public virtual DbSet<EmployeeMaster> EmployeeMaster { get; set; }
        public virtual DbSet<UserInfo> UserInfo { get; set; }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AllowanceMaster>(entity =>
            {
                entity.HasKey(e => e.AllowanceId)
                    .HasName("PK__Allowanc__7B12D0419C2492B7");

                entity.Property(e => e.AllowanceId).HasColumnName("AllowanceID");

                entity.Property(e => e.AllowanceDescription)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.AllowanceName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<CompanyMaster>(entity =>
            {
                entity.HasKey(e => e.CompanyId)
                    .HasName("PK__CompanyM__2D971C4C09C0CDBD");

                entity.Property(e => e.CompanyId).HasColumnName("CompanyID");

                entity.Property(e => e.CompanyDescription)
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<DesignationMaster>(entity =>
            {
                entity.HasKey(e => e.DesignationId)
                    .HasName("PK__Designat__BABD603EC9DD2F21");

                entity.Property(e => e.DesignationId).HasColumnName("DesignationID");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DesignationName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<EmployeeAllowanceDetail>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeKey, e.AllowanceId, e.ClockDate })
                    .HasName("PK__Employee__BDFD953FE5440ED4");

                entity.Property(e => e.AllowanceId).HasColumnName("AllowanceID");

                entity.Property(e => e.ClockDate).HasColumnType("date");

                entity.Property(e => e.AllowanceAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Allowance)
                    .WithMany(p => p.EmployeeAllowanceDetail)
                    .HasForeignKey(d => d.AllowanceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EmployeeA__Allow__02FC7413");

                entity.HasOne(d => d.EmployeeKeyNavigation)
                    .WithMany(p => p.EmployeeAllowanceDetail)
                    .HasForeignKey(d => d.EmployeeKey)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EmployeeA__Emplo__02084FDA");
            });

            modelBuilder.Entity<EmployeeAttendance>(entity =>
            {
                entity.HasKey(e => new { e.EmployeeKey, e.ClockDate })
                    .HasName("PK__Employee__571C3619F9408DEF");

                entity.Property(e => e.ClockDate).HasColumnType("date");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Remarks)
                    .IsRequired()
                    .HasMaxLength(400)
                    .IsUnicode(false);

                entity.Property(e => e.TimeIn).HasColumnType("datetime");

                entity.Property(e => e.TimeOut).HasColumnType("datetime");

                entity.HasOne(d => d.EmployeeKeyNavigation)
                    .WithMany(p => p.EmployeeAttendance)
                    .HasForeignKey(d => d.EmployeeKey)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__EmployeeA__Emplo__05D8E0BE");
            });

            modelBuilder.Entity<EmployeeMaster>(entity =>
            {
                entity.HasKey(e => e.EmployeeKey)
                    .HasName("PK__Employee__8749E50AEEBA11CD");

                entity.HasIndex(e => e.EmployeeId)
                    .HasName("UQ__Employee__7AD04FF042F65C29")
                    .IsUnique();

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.EmpCompanyId).HasColumnName("EmpCompanyID");

                entity.Property(e => e.EmpDesignationId).HasColumnName("EmpDesignationID");

                entity.Property(e => e.EmpFirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmpGender)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.EmpHourlySalaryRate).HasColumnType("decimal(18, 3)");

                entity.Property(e => e.EmpJoiningDate).HasColumnType("datetime");

                entity.Property(e => e.EmpLastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmpResignationDate).HasColumnType("datetime");

                entity.Property(e => e.EmployeeId)
                    .IsRequired()
                    .HasColumnName("EmployeeID")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.EmpCompany)
                    .WithMany(p => p.EmployeeMaster)
                    .HasForeignKey(d => d.EmpCompanyId)
                    .HasConstraintName("FK__EmployeeM__EmpCo__7B5B524B");

                entity.HasOne(d => d.EmpDesignation)
                    .WithMany(p => p.EmployeeMaster)
                    .HasForeignKey(d => d.EmpDesignationId)
                    .HasConstraintName("FK__EmployeeM__EmpDe__7C4F7684");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("PK__UserInfo__1788CC4CC724006E");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.EmailId)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
