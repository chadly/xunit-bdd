using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Xunit
{
	/// <summary>
	/// The base specification class
	/// </summary>
	public abstract class Specification : IAsyncLifetime
	{
		Exception exception;
		static readonly ReaderWriterLockSlim sync = new ReaderWriterLockSlim();
		static readonly Dictionary<Type, bool> typeCache = new Dictionary<Type, bool>();

		/// <summary>
		/// The exception that was thrown when Observe was run; null if no exception was thrown.
		/// </summary>
		protected Exception ThrownException => exception;

		/// <summary>
		/// Initialize the test class all async-like.
		/// </summary>
		protected virtual Task InitializeAsync() => CommonTasks.Completed;

		/// <summary>
		/// Performs the action to observe the outcome of to validate the specification.
		/// </summary>
		protected abstract Task ObserveAsync();

		/// <summary>
		/// Cleanup the test class all async-like.
		/// </summary>
		protected virtual Task DisposeAsync() => CommonTasks.Completed;

		async Task IAsyncLifetime.InitializeAsync()
		{
			try
			{
				await ObserveAsync();
			}
			catch (Exception ex)
			{
				if (!HandleException(ex))
					throw;
			}
		}

		Task IAsyncLifetime.DisposeAsync()
		{
			return DisposeAsync();
		}

		bool HandleException(Exception ex)
		{
			exception = ex;
			return ShouldHandleException();
		}

		bool ShouldHandleException()
		{
			Type type = GetType();

			try
			{
				sync.EnterReadLock();

				if (typeCache.ContainsKey(type))
					return typeCache[type];
			}
			finally
			{
				sync.ExitReadLock();
			}

			try
			{
				sync.EnterWriteLock();

				if (typeCache.ContainsKey(type))
					return typeCache[type];

				var attrs = type.GetTypeInfo().GetCustomAttributes(typeof(HandleExceptionsAttribute), true).OfType<HandleExceptionsAttribute>();

				return typeCache[type] = attrs.Any();
			}
			finally
			{
				sync.ExitWriteLock();
			}
		}
	}
}