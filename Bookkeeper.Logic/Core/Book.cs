using System;

namespace Bookkeeper.Logic.Core
{
	public class Book : AggregateRoot
	{
		public string Name { get; private set; }

		public decimal Balance { get; private set; }
		public long LastVersion { get; private set; }

		public Book(string name)
		{
			Name = name;
		}

		public void Apply(BookSnapshot snapshot)
		{
			Balance = snapshot.Value;
			LastVersion = snapshot.Version;
		}

		public void Apply(DepositedEvent e)
		{
			Balance += e.Value;
			LastVersion = e.Version;
		}
		public void Apply(WithdrewEvent e)
		{
			if (Balance - e.Value < 0)
			{
				throw new InvalidOperationException("Balance is not enough");
			}
			Balance -= e.Value;
			LastVersion = e.Version;
		}

		public void Deposite(decimal value)
		{
			DepositedEvent e = new(this, value, ++LastVersion);
			Apply(e);
			RaiseEvent(e);
		}

		public void Withdraw(decimal value)
		{
			WithdrewEvent e = new(this, value, ++LastVersion);
			Apply(e);
			RaiseEvent(e);
		}
	}
}
