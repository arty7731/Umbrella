﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace UmbrellaService.DataLevel.Database
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DatabaseEntities : DbContext
    {
        public DatabaseEntities()
            : base("name=DatabaseEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<AccountHistory> AccountHistories { get; set; }
        public virtual DbSet<AccountsProject> AccountsProjects { get; set; }
        public virtual DbSet<AccountsTask> AccountsTasks { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<History> Histories { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectStatu> ProjectStatus { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<TaskStatu> TaskStatus { get; set; }
    }
}
