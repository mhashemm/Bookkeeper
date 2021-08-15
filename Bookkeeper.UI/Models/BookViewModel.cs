using System.Collections.Generic;
using Bookkeeper.Logic.Core;

namespace Bookkeeper.UI.Models
{
	public class BookViewModel
	{
		public Book Book { get; set; }
		public IEnumerable<TransactionEvent> TransactionEvents { get; set; }
		public string[] Types { get; } = new[] { "Deposited", "Withdrew" };
	}
}
