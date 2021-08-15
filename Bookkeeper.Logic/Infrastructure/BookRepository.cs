using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookkeeper.Logic.Core;
using Microsoft.EntityFrameworkCore;

namespace Bookkeeper.Logic.Infrastructure
{
	public class BookRepository
	{
		private readonly DataContext _context;

		public BookRepository(DataContext context)
		{
			_context = context;
		}

		public async Task AddBookAsync(string name)
		{
			var book = new Book(name);
			await _context.Books.AddAsync(book);
			await SaveAllAsync();
		}

		public async Task<IEnumerable<Book>> GetBooksAsync()
		{
			return await _context.Books.ToListAsync();
		}

		public async Task<BookSnapshot> GetBookSnapshotAsync(Book book)
		{
			return await _context.BookSnapshots
							.Where(x => x.BookId == book.Id)
							.OrderByDescending(x => x.Version)
							.FirstOrDefaultAsync();
		}

		public async Task<List<IEvent>> GetLatestEventsForBookAsync(Book book)
		{
			var tranEvents = await _context.TransactionEvents.Where(x => x.BookId == book.Id && x.Version > book.LastVersion).ToListAsync();
			return tranEvents.Select<TransactionEvent, IEvent>(e => e.Type switch
			{
				0 => new DepositedEvent(book, e.Value, e.Version),
				1 => new WithdrewEvent(book, e.Value, e.Version),
				_ => null
			}).Where(x => x != null).ToList();
		}

		public async Task<Book> GetBookAsync(long id)
		{
			var book = await _context.Books.FindAsync(id);
			if (book == null) throw new ApplicationException("No book with that id");

			var snapshot = await GetBookSnapshotAsync(book);
			if (snapshot != null) book.Apply(snapshot);

			var events = await GetLatestEventsForBookAsync(book);
			events.ForEach(e => e.Process());
			if (events.Count >= 5) await TakeSnapshotAsync(book);

			return book;
		}

		public async Task TakeSnapshotAsync(Book book)
		{
			var bookSnapshot = new BookSnapshot(book.Id, book.Balance, book.LastVersion);
			await _context.BookSnapshots.AddAsync(bookSnapshot);
			await SaveAllAsync();
		}

		public async Task<IEnumerable<TransactionEvent>> GetTransactionEventsAsync(Book book)
		{
			return await _context.TransactionEvents.Where(x => x.BookId == book.Id).ToListAsync();
		}

		public async Task<bool> SaveAllAsync()
		{
			return await _context.SaveChangesAsync() > 0;
		}
	}
}
