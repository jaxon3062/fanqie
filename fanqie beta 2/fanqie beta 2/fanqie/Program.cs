/*
    Title: Fanqie Beta 2.0
    Description: New version of fanqie transfer system, new transfer rules, and new instructions.
    Instructions: 
        1. Declare a new "translater" object, but initialize it as "Text2code" or "Code2text", with input string argument, whether if you need.
        2. Call the "Translate" method with dimension argument. 
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using algor;
using System.Diagnostics;

namespace fanqie
{
    class Program
    {
        static void Main(string[] args)
        {
            int dimension = 1;
            int testtimes = 1;
            string input = Console.ReadLine();

            for (int i = 0; i < testtimes; i++)
            {


                Stopwatch timer = new Stopwatch();
                long totalTime = 0;
                


                translater tran = new Text2code(input);


                timer.Restart();
                string code = tran.Translate(dimension);
                timer.Stop();
                totalTime += timer.ElapsedMilliseconds;
                Console.WriteLine(code);
                Console.WriteLine("coding used time = " + timer.ElapsedMilliseconds);

                tran = new Code2text(code);


                timer.Restart();
                string text = tran.Translate(dimension);
                timer.Stop();
                totalTime += timer.ElapsedMilliseconds;
                Console.WriteLine(text);
                Console.WriteLine("encoding used time = " + timer.ElapsedMilliseconds);

                Console.WriteLine("used time = " + totalTime);
                

            }
            Console.ReadKey();

        }
    }
}
