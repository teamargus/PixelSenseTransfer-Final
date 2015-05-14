using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blackjack2
{
    class TM
    {
        public static void sendData(string s) {
            Console.WriteLine(s);
        }

        public static string getData() {
            string result = null;

            result = Console.ReadLine();

            return result;
        }
    }
}
