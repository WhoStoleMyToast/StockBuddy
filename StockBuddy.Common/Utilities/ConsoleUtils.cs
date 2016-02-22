using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockBuddy.Common.Utilities
{
    public static class ConsoleUtils
    {
        private static int DisplayMenu()
        {
            System.Console.WriteLine(" ------------------------------------------");
            System.Console.WriteLine("|               Stock Buddy                |");
            System.Console.WriteLine(" ------------------------------------------");
            System.Console.WriteLine();
            System.Console.WriteLine("1. Sync History");
            System.Console.WriteLine("2. Run Real-Time Updater");
            System.Console.WriteLine("3. Exit");
            System.Console.WriteLine("");
            System.Console.Write("Choice: ");
            var result = System.Console.ReadLine();
            return Convert.ToInt32(result);
        }
    }
}
