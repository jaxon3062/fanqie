using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace code2text
{
    public class code2text
    {
        private OleDbConnection connect;
        
        public DataTable vocabDB;
        private string codestr;
        private string sortStr = "index ASC";

        public code2text(string source)
        {
            
            vocabDB = new DataTable();

            setStr(source);
            connect = new OleDbConnection(Properties.Settings.Default.datastring);

            OleDbDataAdapter dataadaptt = new OleDbDataAdapter("SELECT*FROM vocabDB", connect);
            dataadaptt.Fill(vocabDB);

            //vocabDB.Dispose();
            dataadaptt.Dispose();
            connect.Dispose();
            
        }

        public void setStr(string source)
        {
            codestr = source;
        }

        public string Convert2TEXT(int n)   //外部呼叫函數
        {
            List<string> textStr = new List<string>();
            string returnVal = "";

            for (int i = 0; i < codestr.Length; i++)
            {
                
                textStr.Add(codestr[i].ToString());
            }


            for (int i = 0; i < n; i++)   //translate n times
            {
                
                if (i == 0)
                {
                    textStr = Translate(textStr, -1);   //combine two txt to a code
                }
                //if (i != 0)
                else
                {
                    textStr = Translate(textStr, 0);   //combine two code to a code
                }

                if(i == n - 1)
                {
                    returnVal = getTxt(textStr);
                }
            }

            return returnVal;
        }
                    
        private List<string> Translate(List<string> str, int isLast)   //編一次碼
        {
                        
            List<string> temp = new List<string>();
            //string strCode = "";
            
            
            if (isLast == 0)
            {
                temp = code2code(str);
            }
            else if (isLast == -1)
            {
                for (int i = 0; i < (str.Count) / 2; i++)
                {
                    char upcode, botcode;

                    upcode = str[2 * i][0];
                    botcode = str[2 * i + 1][0];

                    temp.Add(GetCode(upcode, botcode));
                }
            }

            return temp;
        }

        private List<string> code2code(List<string> str)
        {
            List<string> returnVal = new List<string>();

            for (int i = 0; i < str.Count / 2; i++)
            {
                returnVal.Add(GetUpper(str[i * 2]) + GetLower(str[i * 2 + 1]));
            }

            return returnVal;
        }

        private string getTxt(List<string> str)   //a list of code to a list of vocab
        {
            string returnStr = "";
            string searchStr = "";
            List<string> input = str;
            /*
            string input = "";

            for (int i = 0; i < str.Count(); i++)
                input += str[i];
                */

            int index = input.Count;//input.Length;


            do
            {
                if (searchStr == "")
                {
                    for (int i = 0; i < input.Count; i++)
                        searchStr += input[i];
                    index = input.Count;
                }

                if (toVocab(searchStr, ref returnStr) == true)
                {

                    List<string> temp = new List<string>();
                    //string temp = "";

                    for (int i = index; i < input.Count; i++)
                    {
                        temp.Add(input[i]);
                        //temp += input[i];                        
                    }
                    input = temp;
                    

                    searchStr = "";
                }
                else
                {
                    index -= 1;
                    searchStr = "";

                    for (int i = 0; i < index; i++)
                    {
                        searchStr += input[i];
                    }
                }

                                
            } while (input.Count != 0);

            return returnStr;
        }

        private bool toVocab(string codeStr, ref string vocabStr)
        {
            string sql = "code = '" + codeStr + "'";
            DataRow[] dr = vocabDB.Select(sql, sortStr);

            if (dr.Length == 0)
            {
                return false;
            }
            else
            {
                vocabStr += dr[0][1];
                return true;
            }
        }

        private string GetWord(string input)   //turn a code into a word
        {
            string sql = "code = '" + input + "'";
            //Random rand = new Random();

            DataRow[] dr;
            dr = vocabDB.Select(sql, sortStr);

            //int i = rand.Next(dr.Length);

            return dr[0][1].ToString();
        }

        private string GetCode(char a, char b)   //把2個字轉成拼音代碼
        {
            string codeA, codeB;
            codeA = GetCharCode(a);
            codeB = GetCharCode(b);

            string result = GetUpper(codeA) + GetLower(codeB);

            return result;
        }

        private string GetUpper(string a)   //get a code's upper part
        {
            var arr = a.ToCharArray();
            Dict dic = new Dict();
            string result = "";

            for (int i = 0; i < arr.Length; i++)
            {
                if (dic.isinShen(arr[i]) || dic.isinMid(arr[i]))
                {
                    result = result + arr[i].ToString();
                }
            }     

            return result;
        }

        private string GetLower(string b)   //get a code's lower part
        {
            var arr = b.ToCharArray();
            Dict dic = new Dict();
            string result = "";

            for (int i = 0; i < arr.Length; i++)
            {
                if (dic.isinYun(arr[i]) || dic.isinTone(arr[i]))
                {
                    result = result + arr[i].ToString();
                }
            }

            return result;
        }

        private string GetCharCode(char chr)   //把一個字轉成代碼
        {
            string sql = "vocab = '" + chr + "'";
            
            DataRow[] dr = vocabDB.Select(sql, sortStr);
            List<DataRow> candinate = new List<DataRow>();

            for (int i = 0; i < dr.Length; i++)
            {
                if (IsFst(dr[i][3].ToString(), chr))
                {
                    candinate.Add(dr[i]);
                }
            }

            return candinate[0][3].ToString();
        }

        private bool IsFst(string str, char chr)
        {
            string sql = "code = '" + str + "'";
            DataRow[] dr = vocabDB.Select(sql, sortStr);

            if (dr[0][1].ToString() == chr.ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool checkStr()
        {
            char[] chrStr = codestr.ToCharArray();
            for (int i = 0; i < chrStr.Length; i++)
            {
                string code = vocabDB.Select("vocab = '" + chrStr[i] + "'", sortStr)[0][3].ToString();
                if (IsFst(code, chrStr[i]))
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }

    public class Dict
    {
        private char[] shen;
        private char[] yun;
        private char[] mid;
        private char[] tone;

        public Dict()
        {
            shen = new char[] { '1', 'q', 'a', 'z', '2', 'w', 's', 'x', 'e', 'd', 'c', 'r', 'f', 'v', '5', 't', 'g', 'b', 'y', 'h', 'n'};
            yun = new char[] { '8', 'i', 'k', ',', '9', 'o', 'l', '.', '0', 'p', ';', '/', '-'};
            mid = new char[] { 'u', 'j', 'm'};
            tone = new char[] { '3', '4', '6', '7'};

            
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
