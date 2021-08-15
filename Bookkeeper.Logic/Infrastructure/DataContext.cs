using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bookkeeper.Logic.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bookkeeper.Logic.Infrastructure
{
	public class DataContext : DbContext
	{
		private readonly IMediator _mediator;

		public DbSet<Book> Books { get; set; }
		public DbSet<TransactionEvent> TransactionEvents { get; set; }
		public DbSet<BookSnapshot> BookSnapshots { get; set; }
		public DataContext(DbContextOptions options, IMediator mediator) : base(options)
		{
			_mediator = mediator;
		}
		protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.Entity<Book>((x) =>
			{
				x.ToTable("Books").HasKey(b => b.Id);
				x.Ignore(b => b.Events);
				x.Ignore(b => b.LastVersion);
				x.Ignore(b => b.Balance);
			});

			builder.Entity<TransactionEvent>((x) =>
			{
				x.ToTable("TransactionEvents").HasKey(b => b.Id);
				x.Property(t => t.Value).HasColumnType("DECIMAL").HasPrecision(8, 2);
			});

			builder.Entity<BookSnapshot>((x) =>
			{
				x.ToTable("BookSnapshots").HasKey(b => b.Id);
				x.Property(t => t.Value).HasColumnType("DECIMAL").HasPrecision(8, 2);
			});
		}
		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
		{
			var aggregateRootsWithEvents = ChangeTracker
					.Entries()
					.Where(x => x.Entity is AggregateRoot root && root.Events.Any())
					.Select(x => (AggregateRoot)x.Entity)
					.ToList();

			foreach (var aggregateRoot in aggregateRootsWithEvents)
			{
				foreach (var domainEvent in aggregateRoot.Events)
				{
					await _mediator.Send(domainEvent).ConfigureAwait(false);
				}
				aggregateRoot.ClearEvents();
			}

			return await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
		}

		public override int SaveChanges()
		{
			return SaveChangesAsync().GetAwaiter().GetResult();
		}
	}
}
