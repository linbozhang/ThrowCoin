using System;
using System.IO;
using System.Text;
using System.Collections.Generic;


namespace Parse
{
	public class ParseCSV
	{
		/// <summary>
		/// Splits the by dou hao.Replace "" with []
		/// </summary>
		/// <returns>The by dou hao.</returns>
		/// <param name="content">Content.</param>

		public static List<string> splitDouHao(string content)
		{
			List<string> lstCtt = new List<string>();
			bool inDQ = false;
			string temp = "";
			bool hasMid = false;//has [ ]
			bool hasBig = false;//has { }
			for(int i=0;i<content.Length;i++)
			{
				string str = content.Substring(i,1);
				
				if(str.Equals(",") && !inDQ)
				{
					temp = temp.Replace("\"\"","\"");//replace "" with "
					lstCtt.Add(temp.ToString());

					temp = "";
					
					if(i==content.Length-1)//the last string is null
					{
						lstCtt.Add(null);
					}
					
				}
				else{
					if(str.Equals("\""))
					{
						if(hasBig||hasMid)
						{
							temp+=str;
						}
						else{
							inDQ = !inDQ;
						}
					}
					else if(str.Equals("["))
					{
						hasMid = true;
						temp+=str;
					}
					else if(str.Equals("]"))
					{
						hasMid = false;
						temp+=str;
					}
					else if(str.Equals("{"))
					{
						hasBig = true;
						temp+=str;
					}
					else if(str.Equals("}"))
			        {
						hasBig = false;
						temp+=str;
					}
					else
					{
						temp+=str;
					}
					
					if(i == content.Length-1)//add last string
					{
						temp = temp.Replace("\"\"","\"");
						lstCtt.Add(temp.ToString());

					}
				}
			}
			return lstCtt;
		}

		public static void ParseCSV2JOSN(string path,bool needEmpty)
		{
			try {
				
				using (StreamReader sr = new StreamReader(path)) {
					
					string source = sr.ReadLine ().ToString();
					
					List<string> myKeywords = ParseCSV.splitDouHao(source);
					
					StreamWriter sw = File.CreateText (path.Replace("csv","bytes"));

					string temp = string.Empty;
					List<string> contents ;
					while (sr.Peek() >= 0) {
						
						temp = sr.ReadLine ().ToString ();
						contents = ParseCSV.splitDouHao(temp);
						while(contents.Count<myKeywords.Count)
						{
							temp += "\\n"+sr.ReadLine ().ToString ();

							contents = ParseCSV.splitDouHao(temp);
						}
						if(myKeywords != null&&!myKeywords.Equals(string.Empty))
						{

							string willWrite ="{";
							for(int i=0;i<myKeywords.Count;i++)
							{
								int num =0;
								float fnum = 0f;


								if(string.IsNullOrEmpty(contents[i]))
								{
									if(needEmpty)
									{
										willWrite += "\""+myKeywords[i]+"\":";
									}
								}
								else{
									willWrite += "\""+myKeywords[i]+"\":";
								}
								if(string.IsNullOrEmpty(contents[i]))
								{
									if(needEmpty)
									{
										willWrite += "\""+contents[i]+"\"";
									}
								}
								else if(int.TryParse(contents[i],out num))
								{
									willWrite += num.ToString();
								}
								else if(float.TryParse(contents[i],out fnum))
								{
									willWrite += fnum.ToString();
								}
								else if(contents[i].Contains("["))
								{
									willWrite += contents[i].Trim();
								}
								else if(contents[i].Contains("{"))
								{
									willWrite += contents[i].Trim();
								}
								else if(!myKeywords[i].Equals(string.Empty))
								{
									willWrite += "\""+contents[i]+"\"";
								}

								if(i<myKeywords.Count-1)
								{
									if(willWrite.Equals(string.Empty))
									{

									}
									else{
										willWrite +=",";
									}

								}
								

							}
							 willWrite += "}";
							sw.WriteLine(GB2312UnicodeConverter.ToGB2312(willWrite));
						}
						
					}
					sw.Close ();
					
				}
			} catch (Exception e) {
				UnityEngine.Debug.Log("The process failed: "+e.ToString());
			}
		}

		public static List<string> splitBaseDouHao(string content)
		{
			List<string> lstCtt = new List<string>();
			bool inString = false;
			string temp = "";
			bool hasMid = false;//has [ ]
			bool hasBig = false;//has { }
			for(int i=0;i<content.Length;i++)
			{
				string str = content.Substring(i,1);

				if(str.Equals(",") && !inString)
				{
					temp = temp.Replace("\"\"","\"");//replace "" with "
					lstCtt.Add(temp.ToString());

					temp = "";

					if(i==content.Length-1)//the last string is null
					{
						lstCtt.Add(null);
					}
				}
				else{
					if(str.Equals("\""))
					{
						if(hasBig||hasMid)
						{
							temp+=str;
						}
						else{
							inString = !inString;
						}
					}
					else
					{
						temp+=str;
					}

					if(i == content.Length-1)//add last string
					{
						temp = temp.Replace("\"\"","\"");
						lstCtt.Add(temp.ToString());
					}
				}
			}
			return lstCtt;
		}
		public static void ParseCSVFormat2JOSN(string path)
		{
			try {

				using (StreamReader sr = new StreamReader(path)) {

					string source = sr.ReadLine ().ToString();

					List<string> myKeywords = ParseCSV.splitBaseDouHao(source);

					StreamWriter sw = File.CreateText (path.Replace("csv","bytes"));

					string temp = string.Empty;
					List<string> contents ;
					while (sr.Peek() >= 0) {

						temp = sr.ReadLine ().ToString ();
						contents = ParseCSV.splitBaseDouHao(temp);
						while(contents.Count<myKeywords.Count)
						{
							temp += "\\n"+sr.ReadLine ().ToString ();

							contents = ParseCSV.splitBaseDouHao(temp);
						}
						if(myKeywords != null)
						{

							string willWrite ="{";
							for(int i=0;i<myKeywords.Count;i++)
							{
								int num =0;
								float fnum = 0f;

								if(!string.IsNullOrEmpty(contents[i]))
								{
									willWrite += Replace(myKeywords[i],contents[i]);
								}
								if(i<myKeywords.Count-1)
								{
									if(willWrite.Equals(string.Empty))
									{

									}
									else{
										willWrite +=",";
									}
								}
							}
							willWrite += "}";
							sw.WriteLine(GB2312UnicodeConverter.ToGB2312(willWrite));
						}

					}
					sw.Close ();

				}
			} catch (Exception e) {
				UnityEngine.Debug.Log("The process failed: "+e.ToString());
			}
		}
	
	 	public static void SQYJsonToCSV(string path)
		{
			try 
			{
				using (StreamReader sr = new StreamReader(path)) 
				{
					List<string> contents = ParseJsonToCSV.ForSQYJsonToCSV(sr);
					if(contents!= null && contents.Count>0)
					{
						StreamWriter sw = File.CreateText (path.Replace("bytes","csv"));
						foreach(string item in contents)
						{
							sw.WriteLine(GB2312UnicodeConverter.ToGB2312(item));
						}
						sw.Close ();
					}
					else
					{
						UnityEngine.Debug.LogError ("JsonToCSV null");
					}
				}
			} 
			catch (Exception e) 
			{
				UnityEngine.Debug.Log("The process failed: "+e.ToString());
			}
		}
		
		
		public static void JsonToCSV(string path)
		{
			try 
			{
				using (StreamReader sr = new StreamReader(path)) 
				{
					List<string> contents = ParseJsonToCSV.ConvertJsonToCSV(sr);
					if(contents!= null && contents.Count>0)
					{
						StreamWriter sw = File.CreateText (path.Replace("bytes","csv"));
						foreach(string item in contents)
						{
							sw.WriteLine(GB2312UnicodeConverter.ToGB2312(item));
						}
						sw.Close ();
					}
					else
					{
						UnityEngine.Debug.LogError ("JsonToCSV null");
					}
				}
			} 
			catch (Exception e) 
			{
				UnityEngine.Debug.Log("The process failed: "+e.ToString());
			}
		}
		
		
		public static string Replace(string str,params object[] objs)
	{
		string newString = "";
		
		string[] strs = str.Split('#');
		
		for(int i=0;i<strs.Length;i++)
		{
			newString +=strs[i];
			if(i<objs.Length)
			{
				newString += ""+objs[i];
			}
		}
		return newString;
	}
	}
}

