#nullable enable

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartesianExplosion
{
    public class Transaction
    {
        public long TransactionId { get; set; }

        public decimal Amount { get; set; }

        public string Load1 { get; set; } = default!;

        public string Load2 { get; set; } = default!;

        public string Load3 { get; set; } = default!;

        public string Load4 { get; set; } = default!;

        public string Load5 { get; set; } = default!;

        public string Load6 { get; set; } = default!;

        public string Load7 { get; set; } = default!;

        public string Load8 { get; set; } = default!;

        public string Load9 { get; set; } = default!;
    }

    public class Payout
    {
        public long PayoutId { get; set; }

        public decimal AmountGoingOut { get; set; }

        public decimal BalanceAfter { get; set; }

        public decimal BalanceBefore { get; set; }

        public DateTime PayoutDate { get; set; }

        public long? TransactionId { get; set; }

        public Transaction? Transaction { get; set; }
    }

    public class Payin
    {
        public long PayinId { get; set; }

        public decimal AmountGoingIn { get; set; }

        public decimal BalanceAfter { get; set; }

        public decimal BalanceBefore { get; set; }

        public DateTime PayinDate { get; set; }

        public long? TransactionId { get; set; }

        public Transaction? Transaction { get; set; }
    }

    public class Summary
    {
        public long SummaryId { get; set; }

        public string Name { get; set; } = default!;

        public string Description { get; set; } = default!;

        public List<Payout> payouts { get; set; } = new List<Payout>();

        public List<Payin> payins { get; set; } = new List<Payin>();
    }

    public class CartesianExplosionContext : DbContext
    {
        private const string connectionString = "Host=localhost;Database=my_db;Username=my_user;Password=my_pw";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Payout>()
                .HasOne(e => e.Transaction)
                .WithMany()
                .HasForeignKey(e => e.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder
                .Entity<Payin>()
                .HasOne(e => e.Transaction)
                .WithMany()
                .HasForeignKey(e => e.TransactionId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            modelBuilder
                .Entity<Summary>()
                .HasMany(x => x.payins)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder
                .Entity<Summary>()
                .HasMany(x => x.payouts)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }

        DbSet<Summary> Summaries { get; set; } = default!;

        DbSet<Payout> Payouts { get; set; } = default!;

        DbSet<Payin> Payins { get; set; } = default!;

        DbSet<Transaction> Transactions { get; set; } = default!;
    }

    public class CartesianExplosionTest
    {
        const int transactionCount = 5;
        const int payinCount = 500;
        const int payoutCount = 500;
        const long summaryId = 1;

        public CartesianExplosionTest()
        {
            using var db = new CartesianExplosionContext();

            db.Set<Transaction>().RemoveRange(db.Set<Transaction>());
            db.Set<Summary>().RemoveRange(db.Set<Summary>());

            for (var id = 1; id <= transactionCount; id++)
            {
                Transaction transaction = new Transaction()
                {
                    TransactionId = id,
                    Amount = 2413,
                    Load1 = "Much extra load to get from db",
                    Load2 = "Much extra load to get from db",
                    Load3 = "Much extra load to get from db",
                    Load4 = "Much extra load to get from db",
                    Load5 = "Much extra load to get from db",
                    Load6 = "Much extra load to get from db",
                    Load7 = "Much extra load to get from db",
                    Load8 = "Much extra load to get from db",
                    Load9 = "Much extra load to get from db",
                };

                db.Add(transaction);
                db.SaveChanges();
            }

            Summary summary = new Summary()
            {
                SummaryId = summaryId,
                Name = "MyName",
                Description = "MyDescription",
            };

            for (var id = 0; id < payinCount; id++)
            {
                Payin payin = new Payin()
                {
                    AmountGoingIn = 52,
                    BalanceAfter = 52,
                    BalanceBefore = 104,
                    PayinDate = new DateTime(2022, 1, 1),
                    TransactionId = (id % transactionCount) + 1,
                };
                summary.payins.Add(payin);
            }

            for (var id = 0; id < payoutCount; id++)
            {
                Payout payout = new Payout()
                {
                    AmountGoingOut = 52,
                    BalanceAfter = 52,
                    BalanceBefore = 0,
                    PayoutDate = new DateTime(2022, 1, 1),
                    TransactionId = (id % transactionCount) + 1,
                };
                summary.payouts.Add(payout);
            }

            db.Add(summary);
            db.SaveChanges();
        }

        [Benchmark]
        public async Task AllOut()
        {
            using var db = new CartesianExplosionContext();

            var all = await db
                .Set<Summary>()
                .Include(x => x.payouts)
                    .ThenInclude(x => x.Transaction)
                .Include(x => x.payins)
                    .ThenInclude(x => x.Transaction)
                .SingleAsync(x => x.SummaryId == summaryId);

            var transactions = new List<Transaction>();

            var payoutTransactions = all.payouts
                .Where(x => x.Transaction != null)
                .Select(x => x.Transaction!)
                .ToList();

            var payinTransactions = all.payins
                .Where(x => x.Transaction != null)
                .Select(x => x.Transaction!)
                .ToList();

            transactions.AddRange(payoutTransactions);
            transactions.AddRange(payinTransactions);

            var final = transactions
                .Distinct()
                .ToList();
        }

        [Benchmark]
        public async Task HalfOut()
        {
            using var db = new CartesianExplosionContext();

            var allIn = await db
                .Set<Summary>()
                .Include(x => x.payins)
                    .ThenInclude(x => x.Transaction)
                .SingleAsync(x => x.SummaryId == summaryId);

            var allOut = await db
                .Set<Summary>()
                .Include(x => x.payouts)
                    .ThenInclude(x => x.Transaction)
                .SingleAsync(x => x.SummaryId == summaryId);

            var transactions = new List<Transaction>();

            var payoutTransactions = allOut.payouts
                .Where(x => x.Transaction != null)
                .Select(x => x.Transaction!)
                .ToList();

            var payinTransactions = allIn.payins
                .Where(x => x.Transaction != null)
                .Select(x => x.Transaction!)
                .ToList();

            transactions.AddRange(payoutTransactions);
            transactions.AddRange(payinTransactions);

            var final = transactions
                .Distinct()
                .ToList();
        }

        [Benchmark]
        public async Task SmartOut()
        {
            using var db = new CartesianExplosionContext();

            var allIn = db
                .Set<Summary>()
                .Include(x => x.payins)
                    .ThenInclude(x => x.Transaction)
                .Where(x => x.SummaryId == summaryId)
                .SelectMany(x => x.payins)
                .Select(x => x.Transaction);

            var allOut = db
                .Set<Summary>()
                .Include(x => x.payins)
                    .ThenInclude(x => x.Transaction)
                .Where(x => x.SummaryId == summaryId)
                .SelectMany(x => x.payins)
                .Select(x => x.Transaction);

            var final = await allIn
                .Concat(allOut)
                .Distinct()
                .ToListAsync();
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run(typeof(Program).Assembly);
        }
    }
}
