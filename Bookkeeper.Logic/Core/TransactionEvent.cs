using System;

namespace Bookkeeper.Logic.Core
{
	public class TransactionEvent : Entity
	{
		public long BookId { get; set; }
		public byte Type { get; set; }
		public long Version { get; set; }
		public decimal Value { get; set; }
		public DateTimeOffset Occurred { get; set; }
		public TransactionEvent(long bookId, byte type, long version, decimal value, DateTimeOffset occurred)
		{
			BookId = bookId;
			Type = type;
			Version = version;
			Value = value;
			Occurred = occurred;
		}
	}
}
