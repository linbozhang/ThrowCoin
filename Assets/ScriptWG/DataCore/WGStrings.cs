using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;


public class WGStrings {


	Dictionary <int,MFText> szConfig ;


    private static WGStrings _instance =null;
    public static WGStrings instance {
        get{
            if(_instance == null)
            {
                _instance = new WGStrings();
            }
            return _instance;
        }
    }

    private WGStrings(){
		szConfig = new Dictionary<int, MFText>();
    }

    public void InitDataWithAsset(Dictionary<int,MFText> dic)
    {
		szConfig = dic;
    }

    /*
    public void loadDataFromPath(string path)
    {
        FileStream fs = File.Open (path, FileMode.Open);
        StreamReader sr = null;

        string line = null;
        try {
            sr = new StreamReader(fs);
            if(sr != null) {
                while(!string.IsNullOrEmpty(line = sr.ReadLine()) ) {
                    if(!line.StartsWith ("#")) {
                        SDK.Log("==="+line);
                        Dictionary<string,object> t = MiniJSON.Json.Deserialize(line) as Dictionary<string,object>;
//                        szConfig.Add(t["ID"].ToString(),t["txt"].ToString());
                    }
                }
            }
        } catch(IOException ex) {
            SDK.Log(line+";"+ex.ToString());
        } finally {
            if(fs != null) { fs.Close(); fs = null; }
            if(sr != null) { sr.Close(); sr = null; }
        }
    }
    public IEnumerator loadDataWithWWW(string path)
    {
        WWW www = new WWW("file:///"+path);
        yield return www;
        StreamReader sr = null;
        string line = null;
        try {
            sr = new StreamReader(new MemoryStream(www.bytes));
            if(sr != null) {
                while(!string.IsNullOrEmpty(line = sr.ReadLine()) ) {
                    if(!line.StartsWith ("#")) {
                        Dictionary<string,object> t = MiniJSON.Json.Deserialize(line) as Dictionary<string,object>;
//                        config.Add(t["ID"].ToString(),t["txt"].ToString());
                    }
                }
            }
        } catch(IOException ex) {
            SDK.Log(line+";"+ex.ToString());
        } finally {
            if(sr != null) { sr.Close(); sr = null; }
        }
    }
*/
    public static string getText(int id)
    {
        if(instance.szConfig.ContainsKey(id))
        {

			if(IOS.systemLanguage() == UnityEngine.SystemLanguage.Chinese)
			{
				return instance.szConfig[id].txtCN;
			}
			else
			{
				return instance.szConfig[id].txtEN;
			}
        }
        return "文字缺失:"+id;
    }
    public static List<string>getTexts(params object[] objs)
    {
        List<string> strs = new List<string>();
        foreach(object obj in objs)
        {
            string s = obj as string;
            if(s==null)
            {
                strs.Add(getText((int)obj));
            }
            else{
                strs.Add(s);
            }
        }
        return strs;
    }
	public static string getFormate(int id,params object[] objs)
	{
		return format(getText(id),objs);
	}
	public static string format(string s,params object[] objs)
	{
		string[] message = s.Split('#');
		string result="";
		for(int i=0;i<message.Length;i++)
		{
			result += message[i];
			if(i<objs.Length)
			{
				if(objs[i] is string)
				{
					result += objs[i] as string;
				}
				else{
					result += objs[i].ToString();
				}
			}
		}
		return result;
	}
	public static string getFormateInt(int id,params object[] objs)
	{
		return formatInt(getText(id),objs);
	}
	public static string formatInt(string s,params object[] objs)
	{
		string[] message = s.Split('#');
		string result="";
		for(int i=0;i<message.Length;i++)
		{
			result += message[i];
			if(i<objs.Length)
			{
				if(objs[i] is string)
				{
					result += objs[i] as string;
				}
				else if (objs[i] is int)
				{
					result += getText((int)objs[i]);
				}
				else{
					result += objs[i].ToString();
				}
			}
		}
		return result;
	}
   
}

public class MFText{
	public int ID;
	public string txtEN;
	public string txtCN;
}


























