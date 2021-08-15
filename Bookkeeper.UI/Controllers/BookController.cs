using System.Threading.Tasks;
using Bookkeeper.Logic.Infrastructure;
using Bookkeeper.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookkeeper.UI.Controllers
{
	public class BookController : Controller
	{
		private readonly BookRepository _bookRepository;

		public BookController(BookRepository bookRepository)
		{
			_bookRepository = bookRepository;
		}

		public async Task<IActionResult> Deposite(long id, decimal value)
		{
			var book = await _bookRepository.GetBookAsync(id);
			book.Deposite(value);
			await _bookRepository.SaveAllAsync();
			return Redirect($"/Book/Index/{id}");
		}

		public async Task<IActionResult> Withdraw(long id, decimal value)
		{
			var book = await _bookRepository.GetBookAsync(id);
			book.Withdraw(value);
			await _bookRepository.SaveAllAsync();
			return Redirect($"/Book/Index/{id}");
		}

		public async Task<IActionResult> Index(long id)
		{
			var book = await _bookRepository.GetBookAsync(id);
			var transactionEvents = await _bookRepository.GetTransactionEventsAsync(book);
			return View(new BookViewModel { Book = book, TransactionEvents = transactionEvents });
		}
	}
}
