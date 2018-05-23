using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace algor
{
    public class Code2text : translater
    {
        public Code2text(string source) : base(source)
        {

        }

        public override string Translate(int n)
        {
            string returnStr = codestr;
            

            for (int i = 0; i < n; i++)
            {
                List<string> returnVal = split2List(returnStr);
                returnStr = "";

                returnVal = Transfer(returnVal);   //transfer 2 word into a code
                returnVal = Cut(returnVal);

                for (int j = 0; j < returnVal.Count; j++)
                {
                    returnStr = string.Concat(returnStr, returnVal[j]);
                }
            }
            
            

            return returnStr;
        }

        protected override List<string> Cut(List<string> input)
        {
            List<string> output = new List<string>();
            string temp = "";
            //int index = 0;

            for (int i = 0; i < input.Count; i++)
            {
                temp += input[i];

                if (vocabDB.Select("code = '" + temp + "'", sortStr).Length == 0)
                {
                    temp = temp.Remove(temp.Length - input[i].Length, input[i].Length);
                    output.Add(temp);

                    temp = "";
                    //index = 0;

                    i -= 1;
                }

                if (i == input.Count - 1)
                {
                    output.Add(temp);
                    break;
                }
                                
            }

            List<string> returnStr = new List<string>();

            for (int i = 0; i < output.Count; i++)
            {

                returnStr.Add(Get(output[i]));
            }
            

            return returnStr;
        }
                

        protected override string Get(string str)
        {
            return vocabDB.Select("code = '" + str + "'", sortStr)[0][1].ToString();
        }

        protected override List<string> Transfer(List<string> str)
        {
            List<string> output = new List<string>();

            for (int i = 0; i < str.Count / 2; i++)
            {
                string code = Splitcodes(str[2 * i], str[2 * i + 1]);
                var upNdown = code.Split(' ');
                output.Add(GetUpper(upNdown[0]) + GetLower(upNdown[1]));
            }

            return output;
        }

        private string Splitcodes(string up, string down)
        {
            string code = vocabDB.Select("vocab = '" + up + down + "'", sortStr)[0][3].ToString();
            DataRow[] dr = vocabDB.Select("vocab = '" + up + "'", sortStr);
            string output = "";

            for (int i = 0; i < dr.Length; i++)
            {
                if (code.StartsWith(dr[i][3].ToString()))
                {
                    output = string.Concat(output, dr[i][3].ToString(), " ", code.Remove(0, dr[i][3].ToString().Length));
                    //output += " ";
                    //output += ;

                    break;
                }
            }

            return output;
        }
    }
}
