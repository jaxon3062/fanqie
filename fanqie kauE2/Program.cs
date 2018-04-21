using System;
using System.Diagnostics;
using System.Text;
using text2code;
using System.Collections.Generic;
using System.Collections;

namespace fanqie_kauE2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            string input = Console.ReadLine();
            float right = 0;
            int codingErr = 0;
            int times = 1;
            Queue<string> log = new Queue<string>();
            Stopwatch timer = new Stopwatch();
            long totalTime = 0;
            Stopwatch mushi = new Stopwatch();
            long mushisTime = 0;
            
            //code2text.code2text c2t = new code2text.code2text(input);
            //text2code.text2code t2c = new text2code.text2code(input, c2t.vocabDB);

            for (int i = 0; i < times; i++)
            {
                timer.Restart();
                string secretStr, output;
                
                code2text.code2text c2t = new code2text.code2text(input);
                mushi.Restart();
                text2code.text2code t2c = new text2code.text2code(input, c2t.vocabDB);

                
                secretStr = t2c.convert(4);
                c2t.setStr(secretStr);
                if (c2t.checkStr() == false)
                {
                    codingErr += 1;
                    i -= 1;
                    continue;
                }
                mushi.Stop();
                output = c2t.Convert2TEXT(4);

                float correct = (float)isCorrect(input, output) / (float)input.Length;

                right += correct;
                

                t2c.Dispose();
                c2t.Dispose();

                timer.Stop();
                totalTime += timer.ElapsedMilliseconds;
                mushisTime += mushi.ElapsedMilliseconds;
                log.Enqueue((i + 1).ToString() + ". " + input + " => " + secretStr + " => " + output + "\n correct rate = " + correct.ToString() +
                    " used time = " + timer.ElapsedMilliseconds.ToString() ); //+ " Mushi's used time: " + mushi.ElapsedMilliseconds.ToString());
            }

            
            for (int i = 0; i < times; i++)
            {
                Console.WriteLine(log.Dequeue());
            }

            Console.WriteLine("Total Correct Rate = {0} % ,  Total used time: {1} ms  (Coding Error: {2} times)", 
                (int)(right / times * 100), totalTime, codingErr);/*, Mushi ejaculate time: {3} ms*/

            Console.ReadKey(true);
            
        }

        static int isCorrect(string inStr, string outStr)
        {
            int corNum = 0;
            char[] chrIn = inStr.ToCharArray();
            char[] chrOut = outStr.ToCharArray();

            for (int i = 0; i < chrIn.Length; i++)
            {
                if (inStr[i] == outStr[i])
                {
                    corNum++;
                }
            }

            return corNum;
        }
    }
}
