using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarWarsPlanetStats
{
    internal static  class TablePrinter
    {
        public static void Print<T>(IEnumerable<T> planets) 
        {
            const int columnWidth = -15;
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                Console.Write($"{{0, {columnWidth}}}|",property.Name);
            }
            Console.WriteLine();
            Console.WriteLine(
                new string('-', properties.Length * (-columnWidth + 1)));
            
            foreach (var planet in planets) 
            {
                foreach (var property in properties)
                {
                    Console.Write($"{{0, {columnWidth}}}|", property.GetValue(planet));
                }
                Console.WriteLine();
            }
        }

    }
}
