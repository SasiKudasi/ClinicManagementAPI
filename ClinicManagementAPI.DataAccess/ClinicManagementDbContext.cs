using System;
using ClinicManagementAPI.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementAPI.DataAccess
{
	public class ClinicManagementDbContext : DbContext
	{
		public ClinicManagementDbContext(DbContextOptions <ClinicManagementDbContext> options)
			: base(options)
		{
		}

		public DbSet<DoctorEntity> Doctors { get; set; }
		public DbSet<PatientEntity> Patients { get; set; }
		public DbSet<RegionEntity> Regions { get; set; }
		public DbSet<RoomEntity> Rooms { get; set; }
		public DbSet<SpecializationEntity> Specializations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Конфигурация для связи многих пациентов с одним участком
            modelBuilder.Entity<PatientEntity>()
                .HasOne(p => p.Region)
                .WithMany(r => r.Patients)
                .HasForeignKey(p => p.RegionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Конфигурация для связи многих врачей с одной специализацией
            modelBuilder.Entity<DoctorEntity>()
                .HasOne(d => d.Specialization)
                .WithMany(s => s.Doctors)
                .HasForeignKey(d => d.SpecializationId)
                .OnDelete(DeleteBehavior.Restrict);

            // Конфигурация для связи многих врачей с одним участком
            modelBuilder.Entity<DoctorEntity>()
                .HasOne(d => d.Region)
                .WithMany(r => r.Doctors)
                .HasForeignKey(d => d.RegionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Конфигурация для связи многих врачей с одним кабинетом
            modelBuilder.Entity<DoctorEntity>()
                .HasOne(d => d.Room)           // Один врач связан с одним кабинетом
                .WithMany(r => r.Doctors)      // Один кабинет может иметь много врачей
                .HasForeignKey(d => d.RoomId)  // Врач содержит внешний ключ для кабинета
                .OnDelete(DeleteBehavior.Restrict);


        }
    }
}