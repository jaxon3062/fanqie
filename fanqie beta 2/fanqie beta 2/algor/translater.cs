using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.OleDb;

namespace algor
{
    public abstract class translater
    {
        private OleDbConnection connect;

        protected DataTable vocabDB;
        protected string codestr;
        protected string sortStr = "index ASC";

        public translater(string source)
        {

            vocabDB = new DataTable();

            setStr(source);
            connect = new OleDbConnection(Properties.Settings.Default.datastring);

            OleDbDataAdapter dataadaptt = new OleDbDataAdapter("SELECT*FROM vocabDB", connect);
            dataadaptt.Fill(vocabDB);

            dataadaptt.Dispose();
            connect.Dispose();

        }

        protected string GetUpper(string a)   //get a code's upper part
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

        protected string GetLower(string b)   //get a code's lower part
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

        public void setStr(string source)
        {
            codestr = source;
        }

        protected List<string> split2List(string str)
        {
            List<string> returnStr = new List<string>();

            for (int i = 0; i < str.Length; i++)
            {
                returnStr.Add(str[i].ToString());
            }

            return returnStr;
        }

        public abstract string Translate(int n);   //out-calling function

        protected abstract List<string> Cut(List<string> input);   //cut a string into pieces and turn into code

        protected abstract List<string> Transfer(List<string> str);   //transfer a code str into password code str

        protected abstract string Get(string str);   //get a result 
    }
}
