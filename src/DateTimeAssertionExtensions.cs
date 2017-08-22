using System;

namespace Xunit.Extensions
{
	/// <summary>
	/// Extensions which provide assertions to classes derived from <see cref="DateTime"/>.
	/// </summary>
	public static class DateTimeAssertionExtensions
	{
		/// <summary>
		/// Verifies that the specified DateTime is within one second from now.
		/// </summary>
		public static void ShouldBeWithinOneSecondFromNow(this DateTime actual)
		{
			ShouldBeWithinOneSecondFromNow((DateTime?)actual);
		}

		/// <summary>
		/// Verifies that the specified DateTime is within one second from now.
		/// </summary>
		public static void ShouldBeWithinOneSecondFromNow(this DateTime? actual)
		{
			if (!actual.HasValue)
				Assert.True(false, "The value is null; therefore not within one second from now.");

			DateTime dateValue = actual.Value;
			DateTime now = (dateValue.Kind == DateTimeKind.Utc) ? DateTime.UtcNow : DateTime.Now;

			ShouldBeWithinOneSecondFrom(actual, now);
		}

		/// <summary>
		/// Verifies that the actual DateTime is within one second from the expected DateTime.
		/// </summary>
		public static void ShouldBeWithinOneSecondFrom(this DateTime actual, DateTime expected)
		{
			ShouldBeWithinOneSecondFrom((DateTime?)actual, expected);
		}

		/// <summary>
		/// Verifies that the actual DateTime is within one second from the expected DateTime.
		/// </summary>
		public static void ShouldBeWithinOneSecondFrom(this DateTime? actual, DateTime expected)
		{
			if (!actual.HasValue)
				Assert.True(false, String.Format("The actual value is null; therefore not within one second from {0}.", expected));

			DateTime dateValue = actual.Value;
			TimeSpan oneSecond = new TimeSpan(0, 0, 1);

			DateTime lower = expected.Subtract(oneSecond);
			DateTime upper = expected.Add(oneSecond);

			Assert.InRange(dateValue, lower, upper);
		}
	}
}