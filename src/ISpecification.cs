using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Xunit.Extensions
{
    public interface ISpecification
    {
		Exception ThrownException { get; set; }

	    void Observe();
    }
}
