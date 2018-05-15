using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace algor
{
    public class Text2code : translater
    {
        public Text2code(string source) : base(source)
        {
        }

        /*
        protected DataTable vocabDB;
        protected string codestr;
        protected string sortStr = "index ASC";
        */

        public override string Translate(int n) 
        {
            string returnStr = "";
            List<string> returnVal = split2List(codestr);

            for (int i = 0; i < n; i++)
            {
                returnVal = Cut(returnVal);   //cut a str into vocabs code str
                returnVal = Transfer(returnVal);   //
                
                for (int j = 0; j < returnVal.Count; i++)
                {
                    returnVal[j] = Get(returnVal[j]);
                }
            }

            for (int i = 0; i < returnVal.Count; i++)
            {
                returnStr += returnVal[i];
            }

            return returnStr;
        }

        protected override List<string> Cut(List<string> input)
        {
            List<string> output = new List<string>();
            
            //int count = 0;

            string str = "";
            for (int i = 0; i < input.Count; i++)
            {
                str += input[i];
                
                if (vocabDB.Select("vocab = '" + str + "'", sortStr).Length == 0)
                {
                    str = str.Remove(i, 1);
                    output.Add(str);
                    str = "";
                    i -= 1;
                }

                if (i == input.Count - 1)
                {
                    output.Add(str);
                    break;
                }
            }

            for (int i = 0; i < output.Count; i++)
            {
                output[i] = vocabDB.Select("vocab = '" + output[i] + "'", sortStr)[0][3].ToString();
            }

            return output;
        }

        protected override string Get(string str)
        {
            return vocabDB.Select("code = '" + str + "'", sortStr)[0][1].ToString();
        }

        protected override List<string> Transfer(List<string> str)
        {
            List<string> output = new List<string>();
            List<string> temp = new List<string>();
            string head = "", tail = "";
            int index = 0;

            for (int i = 0; i < str.Count; i++)
            {
                head = GetUpper(str[index]);
                if (tail != "")
                {
                    temp.Add(tail);
                    temp.Add(GetUpper(str[i]));
                }
                tail = GetLower(str[i]);

                string sql = "code LIKE '";

                sql += head + "*";
                for (int j = 0; j < temp.Count; j++)
                {
                    sql += temp[j] + "*";
                }
                sql += tail + "'";
                sql += " AND LEN(vocab) = " + (temp.Count / 2 + 1);

                if (vocabDB.Select(sql, sortStr).Length == 0)
                {
                    sql += head + "*";
                    for (int j = 0; j < temp.Count - 2; j++)
                    {
                        sql += temp[j] + "*";
                    }
                    sql += temp[temp.Count - 1] + "'";
                    sql += " AND LEN(vocab) = " + (temp.Count / 2 ) + "'";
                    output.Add(vocabDB.Select(sql, sortStr)[0][3].ToString());

                    index = i;
                    temp.Clear();
                    head = "";
                    tail = "";
                }

                if (i == str.Count - 1)
                {
                    output.Add(vocabDB.Select(sql, sortStr)[0][3].ToString());
                    break;
                }

            }

            return output;
        }
    }
}
