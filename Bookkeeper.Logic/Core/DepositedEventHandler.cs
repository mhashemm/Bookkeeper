using System;
using System.Threading;
using System.Threading.Tasks;
using Bookkeeper.Logic.Infrastructure;
using MediatR;

namespace Bookkeeper.Logic.Core
{
	public class DepositedEventHandler : IRequestHandler<DepositedEvent>
	{
		private readonly DataContext _context;

		public DepositedEventHandler(DataContext context)
		{
			_context = context;
		}
		public async Task<Unit> Handle(DepositedEvent request, CancellationToken cancellationToken)
		{
			var tranEvent = new TransactionEvent(request.Book.Id, 0, request.Version, request.Value, DateTimeOffset.UtcNow);
			await _context.TransactionEvents.AddAsync(tranEvent);
			return await Unit.Task;
		}
	}
}
