using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWarsPlanetStats
{
    internal static class StringExtensions
    {
        public static int? ToIntOrNull(this string? input)
        {
            //int? result = null;
            //if (int.TryParse(input, out int resultParsed))
            //{
            //    result = resultParsed;
            //}
            //return result;

            return int.TryParse(input, out int resultParsed) ? resultParsed : null;
        }
    }
}
