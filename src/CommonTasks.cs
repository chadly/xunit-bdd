using System.Threading.Tasks;

namespace Xunit
{
	static class CommonTasks
	{
		public static readonly Task Completed = Task.FromResult(0);
	}
}