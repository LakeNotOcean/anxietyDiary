using Microsoft.EntityFrameworkCore;
using Domain.User;
using System;
using Domain.DiaryExpensions;
using Domain.Diaries;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Collections.Generic;
using System.Linq;

namespace Persistance
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<LastUserView> UsersViews { get; set; }
        public DbSet<UserDoctor> UserDoctors { get; set; }

        public DbSet<UserRequest> UsersRequests { get; set; }

        public DbSet<DiaryDescription> Descriptions { get; set; }

        public DbSet<DiaryCategory> Categories { get; set; }

        // Дневники
        public DbSet<InitialDiary> InitialDiaries { get; set; }
        public DbSet<HumanBobyDiary> HumanBobyDiaries { get; set; }
        public DbSet<AcitvityDiary> AcitvityDiaries { get; set; }
        public DbSet<WrongRulesDiary> WrongRulesDiaries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            buildUsers(ref modelBuilder);
            buildDiaries(ref modelBuilder);
        }

        private void buildDiaries(ref ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DiaryDescription>().Property(d => d.Type).HasConversion<int>();

            modelBuilder.Entity<DiaryColumn>().Property(d => d.ValueType).HasConversion<int>();

            var splitStringConverter = new ValueConverter<List<string>, string>(
                v => string.Join(";", v),
                v => v.Split(new[] { ';' }).ToList());

            modelBuilder.Entity<WrongRulesDiary>().Property(d => d.Column2).HasConversion(splitStringConverter);

        }

        private void buildUsers(ref ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().Property(user => user.FirstName).HasDefaultValue(string.Empty);
            modelBuilder.Entity<User>().Property(user => user.SecondName).HasDefaultValue(string.Empty);
            modelBuilder.Entity<User>().Property(user => user.Description).HasDefaultValue(string.Empty);
            modelBuilder.Entity<User>().Property(u => u.RoleId).HasConversion<int>();
            modelBuilder.Entity<User>().Property(u => u.LastRecordModify).HasDefaultValue(DateTime.UnixEpoch);
            modelBuilder.Entity<Role>().Property(role => role.Id).HasConversion<int>();
            modelBuilder.Entity<LastUserView>().Property(v => v.LastViewDate).HasDefaultValue(DateTime.UnixEpoch);
            modelBuilder.Entity<UserRequest>().Property(userRequest => userRequest.Request).HasConversion<int>();
        }
    }
}