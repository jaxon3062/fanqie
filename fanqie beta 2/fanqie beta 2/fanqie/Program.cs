using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using algor;

namespace fanqie
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string input = Console.ReadLine();

            translater tran = new Text2code(input);

            

            Console.WriteLine(tran.Translate(1));
            Console.ReadKey();
            

            
        }
    }
}
