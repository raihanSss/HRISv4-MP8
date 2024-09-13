using System;
using System.Collections.Generic;
using HRIS.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HRIS.Infrastructure;

public partial class hrisDbContext : IdentityDbContext<AppUser>
{

    public hrisDbContext(DbContextOptions<hrisDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Departments> Departments { get; set; }

    public virtual DbSet<Dependents> Dependents { get; set; }

    public virtual DbSet<Employees> Employees { get; set; }

    public virtual DbSet<Locations> Locations { get; set; }

    public virtual DbSet<Projects> Projects { get; set; }

    public virtual DbSet<Projemp> Projemp { get; set; }
    public virtual DbSet<LeaveRequest> LeaveRequests { get; set; }

    public DbSet<Workflow> Workflows { get; set; }
    public DbSet<WorkflowSequences> WorkflowSequences { get; set; }
    public DbSet<NextStepRules> NextStepRules { get; set; }
    public DbSet<Process> Processes { get; set; }
    public DbSet<WorkflowActions> WorkflowActions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Name=PostgreSQLConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Departments>(entity =>
        {
            entity.HasKey(e => e.IdDept).HasName("departments_pkey");

            entity.HasOne(d => d.HeadempNavigation).WithMany(p => p.Departments)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_heademp");

            entity.HasOne(d => d.IdLocationNavigation).WithMany(p => p.Departments)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_id_location");
        });

        modelBuilder.Entity<Dependents>(entity =>
        {
            entity.HasKey(e => e.IdDependent).HasName("dependents_pkey");

            entity.HasOne(d => d.IdEmpNavigation).WithMany(p => p.Dependents)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_id_emp");
        });

        modelBuilder.Entity<Employees>(entity =>
        {
            entity.HasKey(e => e.IdEmp).HasName("employees_pkey");

            entity.Property(e => e.Lastupdate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.IdDeptNavigation)
                .WithMany(p => p.Employees)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_id_dept");

            entity.HasOne(e => e.AppUser)
                .WithOne(u => u.Employee)
                .HasForeignKey<AppUser>(u => u.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Locations>(entity =>
        {
            entity.HasKey(e => e.IdLocation).HasName("locations_pkey");
        });

        modelBuilder.Entity<Projects>(entity =>
        {
            entity.HasKey(e => e.IdProj).HasName("projects_pkey");

            entity.HasOne(d => d.IdDeptNavigation)
                .WithMany(p => p.Projects)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_id_dept");

            entity.HasOne(d => d.IdLocationNavigation)
                .WithMany(p => p.Projects)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_id_location");
        });

        modelBuilder.Entity<LeaveRequest>(entity =>
        {
            entity.HasKey(e => e.IdLeave);

            entity.Property(e => e.StartDate)
                .IsRequired();

            entity.Property(e => e.EndDate)
                .IsRequired();

            entity.Property(e => e.LeaveType)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(e => e.Reason);

            // Foreign Key configuration
            entity.HasOne(e => e.Employee)
                .WithMany(e => e.LeaveRequests)
                .HasForeignKey(e => e.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Projemp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("projemp_pkey");

            entity.HasOne(d => d.IdEmpNavigation).WithMany(p => p.Projemp)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_projemp_id_emp");

            entity.HasOne(d => d.IdProjNavigation).WithMany(p => p.Projemp)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_projemp_id_proj");
        });

        modelBuilder.Entity<Workflow>(entity =>
        {
            entity.ToTable("Workflow");

            entity.HasKey(w => w.WorkflowId);

            entity.Property(w => w.WorkflowName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(w => w.Description)
                .HasMaxLength(1000);
        });

        modelBuilder.Entity<WorkflowSequences>(entity =>
        {
            entity.ToTable("WorkflowSequences");

            entity.HasKey(ws => ws.StepId);

            entity.Property(ws => ws.StepName)
                .IsRequired()
                .HasMaxLength(255);

            entity.HasOne(ws => ws.Workflow)
                .WithMany(w => w.WorkflowSequences)
                .HasForeignKey(ws => ws.WorkflowId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(ws => ws.RequiredRole)
                .HasMaxLength(255);

            entity.HasOne(ws => ws.Role)
            .WithOne()
            .HasForeignKey<WorkflowSequences>(ws => ws.RequiredRole)
            .OnDelete(DeleteBehavior.Cascade);



           
        });

        modelBuilder.Entity<NextStepRules>(entity =>
        {
            entity.ToTable("NextStepRules");

            entity.HasKey(nsr => nsr.RuleId);

            entity.Property(nsr => nsr.ConditionType)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(nsr => nsr.ConditionValue)
                .HasMaxLength(255);

            entity.HasOne(nsr => nsr.CurrentStep)
                .WithMany(ws => ws.NextStepRules)
                .HasForeignKey(nsr => nsr.CurrentStepId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(nsr => nsr.NextStep)
                .WithMany()
                .HasForeignKey(nsr => nsr.NextStepId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Process>(entity =>
        {
            entity.ToTable("Process");

            entity.HasKey(p => p.ProcessId);

            entity.Property(p => p.RequestType)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(p => p.RequestDate)
                .IsRequired();

            entity.HasOne(p => p.Workflow)
                .WithMany(w => w.Processes)
                .HasForeignKey(p => p.WorkflowId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.CurrentStep)
                .WithMany(ws => ws.Processes) // Pastikan ada navigasi di WorkflowSequences jika diperlukan
                .HasForeignKey(p => p.CurrentStepId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.LeaveRequest)
                .WithMany(lr => lr.Processes) // Pastikan ada navigasi di LeaveRequest jika diperlukan
                .HasForeignKey(p => p.LeaveRequestId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WorkflowActions>(entity =>
        {
            entity.ToTable("WorkflowActions");

            entity.HasKey(wa => wa.ActionId);

            entity.Property(wa => wa.Action)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(wa => wa.ActionDate)
                .IsRequired();

            entity.HasOne(wa => wa.Process)
                .WithMany(p => p.WorkflowActions)
                .HasForeignKey(wa => wa.ProcessId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(wa => wa.Step)
                .WithMany(ws => ws.WorkflowActions) 
                .HasForeignKey(wa => wa.StepId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
