using StockBuddy.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBuddy.Common.Utilities
{
    public static class MapperUtils
    {
        public static IEnumerable<History> Map(IEnumerable<Historic> source)
        {
            return AutoMapper.Mapper.Map<IEnumerable<Historic>, IEnumerable<History>>(source);
            //return AutoMapper.Mapper.Map<TSource, TDestination>(source);
        }
    }
}
