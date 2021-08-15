namespace Bookkeeper.Logic.Core
{
	public class BookSnapshot : Entity
	{

		public long Version { get; init; }
		public decimal Value { get; init; }
		public long BookId { get; init; }
		public BookSnapshot(long bookId, decimal value, long version)
		{
			BookId = bookId;
			Value = value;
			Version = version;
		}
	}
}
