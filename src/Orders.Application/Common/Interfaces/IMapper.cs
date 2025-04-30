using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orders.Application.Common.Interfaces
{
    public interface IMapper<TSource, TTarget>
    {
        TTarget ToTarget(TSource source);
        TSource ToSource(TTarget target);
    }
}
