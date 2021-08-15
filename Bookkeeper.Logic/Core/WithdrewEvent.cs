using MediatR;

namespace Bookkeeper.Logic.Core
{
	public record WithdrewEvent(Book Book, decimal Value, long Version) : IEvent, IRequest
	{
		public void Process()
		{
			Book.Apply(this);
		}
	};
}
