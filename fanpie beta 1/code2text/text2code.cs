/*
 * 
 * 使用方法:new text2code(string 文字,DataTable vocab_phonetic)
 * 如果要改Text,用text2code.setText(string 文字)
 * 轉換用convert(int 轉換次數)
 * 取得編碼用getCode()
 * 
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.Linq;
using System.Web;

namespace text2code
{
	class text2code
	{
		private string text;
		private string code;
		private DataTable vocab_phonetic;
		public List<string> words = new List<string>();
		private List<string> vocabList = new List<string>();
		private List<string> codeList = new List<string>();
		
		private static char[] Shen = new char[] { '1', 'q', 'a', 'z', '2', 'w', 's', 'x', 'e', 'd', 'c', 'r', 'f', 'v', '5', 't', 'g', 'b', 'y', 'h', 'n'};
		private static char[] Yun = new char[] { '8', 'i', 'k', ',', '9', 'o', 'l', '.', '0', 'p', ';', '/', '-'};
		private static char[] Mid = new char[] { 'u', 'j', 'm'};
		private static char[] Tone = new char[] { '3', '4', '6', '7'};
		
		public text2code(String text,DataTable dt){		//取得資料庫以及設定待轉換文字
			vocab_phonetic = dt;		
			setText(text);
		}
		public void setText(string s){		//更換待轉換文字
			text = s;
			code = getCode();
			findCode();
		}
		public string getCode(){	//將待轉換文字分割成多個可讀單字
			string re = "";
			int counter = 0;
			while(counter<text.Length){
				string selectString = text.Substring(counter);
				Console.WriteLine(selectString);
				int errorTick = 0;
				while(true){
					string selectedWord = selectString.Substring(0,selectString.Length - errorTick);
					Console.WriteLine(selectedWord);
					string r = selectVocab(selectedWord);
					if(r == "error"){
						errorTick++;
					}else{
						re += r;
						vocabList.Add(selectedWord);
						codeList.Add(r);
						counter += selectedWord.Length;
						break;
					}
				}
			}
			return re;
		}
		public string convert(int times){		//給外部轉換的method 參數times為加密次數
			List<string> word = words;
			List<string> temp = new List<string>();
			for(int i = 0;i<times;i++){
				foreach(string s in word){
					string trans = transform(s);
					temp.Add(trans[0].ToString());
					temp.Add(trans[1].ToString());
				}
				word.Clear();
				for(int j = 0;j<temp.Count;j++){
					word.Add(selectVocab(temp[j].ToString()));
				}
				if(i != times -1 ){
					temp.Clear();
				}
			}
			string re = "";
			foreach(string s in temp){
				re += s;
			}
			return re;
		}
		private string transform(string code){		//將單一中文字轉換成反切音
			string a = "";
			string b = "";
			string re = "";
			for(int i = 0;i<code.Length;i++){
				if(Shen.Contains(code[i]) || Mid.Contains(code[i])){
					a += code[i];
				}else{
					b += code[i];
				}
			}
			re += choosedWordRandomTone(a);
			re += choosedToneRandomWord(b);
			return re;
		}
		private void findCode(){		//將單字分割成個別中文字的讀音
			for(int i = 0;i<vocabList.Count;i++){
				string vocab = vocabList[i];
				string code = codeList[i];
				int counter = 0;
				for(int j = 0;j<vocab.Length;j++){
					string select = selectCode(vocab[j].ToString(),code,counter);
					words.Add(select);
					counter += select.Length;
					
				}
			}
		}
		private string selectCode(string vocab,string code,int counter){		//在資料庫中搜尋單字以得到正確讀音
			string s = "vocab = '" + vocab + "'";
			DataRow[] dr = vocab_phonetic.Select(s);
			foreach(DataRow d in dr){
				if(d[3].ToString() == code.Substring(counter,d[3].ToString().Length)){
					return d[3].ToString();
				}
			}
			return "error";
		}
		private string selectVocab(string vocab){		//以單一中文字尋找該中文字使用最頻繁的讀音
			string s = "vocab = '" + vocab + "'";
			DataRow[] dr = vocab_phonetic.Select(s);
			if(dr.Length<1)return "error";
			return dr[0][3].ToString();
		}
		private string choosedWordRandomTone(string w){		//以選擇的注音上半部隨機轉換成反切音前段
			Random rd = new Random();
			if(w.Length<1)return "error";
			while(true){
				string t = w;
				int a = rd.Next(0,Yun.Length + 1);
				if(a<1){
					t += "";
				}else{
					t += Yun[a-1];
				}
				int i = rd.Next(0,5);
				if(i<1){
					t +=  "";
				}else{
					t += Tone[(i - 1)].ToString();
				}
				string s = selectRandomWord(t);
				if(s != "error"){
					return s;
				}
				Console.WriteLine("重新選字");
			}
			
		}
		private string choosedToneRandomWord(string w){		//以選擇的注音下半部隨機轉換成反切音後段
			Random rd = new Random();
			
			while(true){
				string t = "";
				int a = rd.Next(0,Shen.Length);
				if(a > 0){
					t += Shen[(a - 1)];
				}
				int b = rd.Next(0,Mid.Length);
				if(b > 0){
					t += Mid[b];
				}
				t += w;
				if(t.Length>1){
					string s = selectRandomWord(t);
					if(s != "error"){
						return s;
					}
				}
				Console.WriteLine("重新選字");
			}
		}
		private string selectRandomWord(string sql){		//以選擇的完整注音轉換成該讀音使用頻率最高的中文字
			if(sql.Length < 1){
				return "error";
			}
			string SQL = "code = '" + sql + "'";
			DataRow[] dr = vocab_phonetic.Select(SQL);
			if(dr.Length<1){
				return "error";
			}else{
				return dr[0][1].ToString();
			}
		}
	}
	class Connection
	{
		private OleDbConnection connect;
		public Connection(){
			openConnection();
		}
		private void openConnection(){		//開啟資料庫連結
			connect = new OleDbConnection{ConnectionString = getConnectionString()};
		}
		public void closeConnection(){		//關閉資料庫連結
			connect.Close();
		}
		private String getConnectionString(){		//傳回連結資料庫所需參數
			return "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\\vocab_frequency.accdb";
		}
		public DataTable getDataTable(){		//給外部呼叫以取得資料庫
			DataTable d = new DataTable();
			OleDbDataAdapter da = new OleDbDataAdapter("select * from vocabDB",connect);
			DataSet ds = new DataSet();
			ds.Clear();
			da.Fill(ds);
			d = ds.Tables[0];
			return d;
		}
	}
}