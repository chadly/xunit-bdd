using System;
using System.Threading.Tasks;

namespace Xunit.Extensions
{
	public interface ISpecification
	{
		Exception ThrownException { get; set; }

		Task ObserveAsync();
	}
}
