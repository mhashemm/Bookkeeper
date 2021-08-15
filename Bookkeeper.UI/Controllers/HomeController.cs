using System.Diagnostics;
using System.Threading.Tasks;
using Bookkeeper.Logic.Infrastructure;
using Bookkeeper.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Bookkeeper.UI.Controllers
{
	public class HomeController : Controller
	{
		private readonly BookRepository _bookRepository;

		public HomeController(BookRepository bookRepository)
		{
			_bookRepository = bookRepository;
		}

		public async Task<IActionResult> Index()
		{
			var books = await _bookRepository.GetBooksAsync();
			return View(books);
		}
		public async Task<IActionResult> AddBook(string name)
		{
			await _bookRepository.AddBookAsync(name);
			return Redirect("/Home/Index");
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
