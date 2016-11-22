using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using SeamsCore.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SeamsCore.Infrastructure
{
    public class SeamsContext : IdentityDbContext<User>
    {
        private IDbContextTransaction _currentTransaction;

        public SeamsContext(DbContextOptions<SeamsContext> options) : base(options)
        {
        }

        public DbSet<Page> Pages { get; set; }
        public DbSet<PageSlot> PageSlots { get; set; }
        public DbSet<PageSlotHtml> PageSlotHtmls { get; set; }
        public DbSet<PageTemplate> PageTemplates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.RemovePluralizingTableNameConvention();
            base.OnModelCreating(modelBuilder);
        }

        public void BeginTransaction()
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = Database.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void CloseTransaction()
        {
            CloseTransaction(exception: null);
        }

        public void CloseTransaction(Exception exception)
        {
            try
            {
                if (_currentTransaction != null && exception != null)
                {
                    _currentTransaction.Rollback();
                    return;
                }

                SaveChanges();

                _currentTransaction?.Commit();
            }
            catch (Exception)
            {
                if (_currentTransaction?.GetDbTransaction().Connection != null)
                {
                    _currentTransaction.Rollback();
                }

                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

    }
}
