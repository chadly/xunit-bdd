using System;

namespace Xunit.Extensions
{
	public delegate void MethodThatThrows();

	public static class MethodThatThrowsExtensions
	{
		public static Exception GetException(this MethodThatThrows method)
		{
			Exception exception = null;

			try
			{
				method();
			}
			catch (Exception ex)
			{
				exception = ex;
			}

			return exception;
		}
	}
}