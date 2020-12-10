using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TransactionStorage.Data.Models;

namespace TransactionStorage.Data.TransactionProvider
{
    public class TransactionContext : DbContext
    {
        public TransactionContext(DbContextOptions<TransactionContext> options)
             : base(options)
        {
        }
        
        public DbSet<Transactions> Transactions { get; set; }
    }
}
