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
            string returnStr = codestr;

            for (int i = 0; i < n; i++)
            {
                
                List<string> returnVal = split2List(returnStr);
                returnStr = "";

                returnVal = Cut(returnVal);   //cut a str into vocabs code str
                returnVal = Transfer(returnVal);   //

                for (int j = 0; j < returnVal.Count; j++)
                {
                    returnStr = string.Concat(returnStr, Get(returnVal[j]));
                }
                /*
                for (int j = 0; j < returnVal.Count; i++)
                {
                    returnVal[j] = Get(returnVal[j]);
                }*/
            }

            

            return returnStr;
        }

        protected override List<string> Cut(List<string> input)
        {
            List<string> output = new List<string>();
            List<string[]> temp = new List<string[]>();
            
            //int count = 0;

            string str = "";
            for (int i = 0; i < input.Count; i++)
            {
                str = string.Concat(str, input[i]);
                
                if (vocabDB.Select("vocab = '" + str + "'", sortStr).Length == 0)
                {
                    str = str.Remove(str.Length - 1, 1);
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
                temp.Add(separate(output[i]));
            }

            output.Clear();

            for (int i = 0; i < temp.Count; i++)
            {
                for (int j = 0; j < temp[i].Length; j++)
                {
                    output.Add(temp[i][j]);
                }
            }
            

            return output;
        }

        private string[] separate(string input)
        {
            string code = vocabDB.Select("vocab = '" + input + "'", sortStr)[0][3].ToString();
            string[] output = new string[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                DataRow[] result = vocabDB.Select("vocab = '" + input[i] + "'", sortStr);
                for (int j = 0; j < result.Length; j++)
                {
                    if (code.StartsWith(result[j][3].ToString()))
                    {
                        output[i] = result[j][3].ToString();
                        code = code.Remove(0, output[i].Length);
                        break;
                    }
                }
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

            for (int i = 0; i < str.Count; i++)
            {
                string head = GetUpper(str[i]);
                string tail = GetLower(str[i]);

                string sql = "";
                sql = String.Concat(sql, "code LIKE '" + head + "*'", " AND code LIKE '*" + tail + "'", " AND LEN(vocab) = 2");
                //sql += ;
                //sql += ;

                 DataRow[] result = vocabDB.Select(sql, sortStr);
                
                for (int j = 0; j < result.Length; j++)
                {
                    string temp = result[j][3].ToString();
                    if (GetUpper(temp) + GetLower(temp) == str[i])
                    {
                        output.Add(temp);
                        break;
                    }
                }
            }

            /*
            List<string> output = new List<string>();
            List<string> temp = new List<string>();
            string head = "", tail = "";
            int index = 0;

            for (int i = 0; i < str.Count; i++)
            {
                //str = SplitUPnDOWN(str);
                int tempWordCount = 0;

                head = GetUpper(str[index]);
                if (tail != "")
                {
                    temp.Add(tail + GetUpper(str[i]));
                    
                }
                tail = GetLower(str[i]);

                string sql = "code LIKE '";
                sql += head + "*'";

                for (int j = 0; j < temp.Count; j++)
                {
                    sql += "AND code LIKE '%" + temp[j] + "%'";
                    tempWordCount += temp[j].Length;
                }
                sql += " AND code LIKE '*" + tail + "'";
                sql += " AND LEN(vocab) = " + (2 * (temp.Count + 1)).ToString();
                sql += " AND NOT LEN(code) > " + (4 * (temp.Count + 1) + tempWordCount + head.Length + tail.Length);

                if (vocabDB.Select(sql, sortStr).Length == 0)
                {
                    
                    temp.RemoveAt(temp.Count - 1);
                    tail = GetLower(str[i - 1]);

                    sql = "code LIKE '" + head + "*'";
                    for (int j = 0; j < temp.Count; j++)
                    {
                        sql += "AND code LIKE '%" + temp[j] + "%'";
                        tempWordCount += temp[j].Length;
                    }
                    sql += " AND code LIKE '*" + tail + "'";
                    sql += " AND LEN(vocab) = " + (2 * (temp.Count + 1)).ToString();
                    sql += " AND NOT LEN(code) > " + (4 * (temp.Count + 1) + tempWordCount + head.Length + tail.Length);
                    output.Add(vocabDB.Select(sql, sortStr)[0][3].ToString());

                    
                    DataRow[] result = vocabDB.Select(sql, sortStr);
                    for (int j = 0; j < result.Length; j++)
                    {
                        if (check(result[j][3].ToString()))
                        {
                            output.Add(result[j][3].ToString());
                            break;
                        }
                    }


                    index = i;
                    i -= 1;
                    temp.Clear();
                    head = "";
                    tail = "";
                    continue;
                }

                if (i == str.Count - 1)
                {
                    output.Add(vocabDB.Select(sql, sortStr)[0][3].ToString());
                    break;
                }

            }*/

            return output;

        }
        
    }
}
