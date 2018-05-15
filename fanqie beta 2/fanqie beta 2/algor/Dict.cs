using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace algor
{
    public class Dict
    {
        private char[] shen;
        private char[] yun;
        private char[] mid;
        private char[] tone;

        public Dict()
        {
            shen = new char[] { '1', 'q', 'a', 'z', '2', 'w', 's', 'x', 'e', 'd', 'c', 'r', 'f', 'v', '5', 't', 'g', 'b', 'y', 'h', 'n' };
            yun = new char[] { '8', 'i', 'k', ',', '9', 'o', 'l', '.', '0', 'p', ';', '/', '-' };
            mid = new char[] { 'u', 'j', 'm' };
            tone = new char[] { '3', '4', '6', '7' };


        }

        public bool isinShen(char i)
        {
            int result = Array.IndexOf(shen, i);

            if (result == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool isinYun(char i)
        {
            int result = Array.IndexOf(yun, i);

            if (result == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool isinMid(char i)
        {
            int result = Array.IndexOf(mid, i);

            if (result == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool isinTone(char i)
        {
            int result = Array.IndexOf(tone, i);

            if (result == -1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
