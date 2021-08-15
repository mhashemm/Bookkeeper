using System.Collections.Generic;
using System.Linq;

namespace Bookkeeper.Logic.Core
{
	public abstract class AggregateRoot : Entity
	{
		protected readonly List<IEvent> _events = new();
		public IReadOnlyList<IEvent> Events => _events.ToList();

		protected void RaiseEvent(IEvent e)
		{
			_events.Add(e);
		}
		public void ClearEvents()
		{
			_events.Clear();
		}
	}
}
