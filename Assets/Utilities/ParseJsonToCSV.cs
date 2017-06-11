using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class ParseJsonToCSV
{
	string DebugFlag = " ConvertJsonToCSV --> ";
	static ParseJsonToCSV instance = null;
	
	public static List<string> ConvertJsonToCSV (StreamReader sr)
	{
		if(instance != null)
		{
			instance.ClearData();
			instance = null;
		}
		instance = new ParseJsonToCSV ();
		if(instance != null)
		{
			instance.mSR = sr;
			instance.ClearData();
			return instance.Convert ();
		}
		return null;
	}
	
	public static List<string> ForSQYJsonToCSV(StreamReader sr)
	{
		if (instance != null) {
			instance.ClearData ();
			instance = null;
		}
		instance = new ParseJsonToCSV ();
		if (instance != null) {
			instance.mSR = sr;
			instance.ClearData ();
			return instance.Conversion ();
		}
		return null;
	}
	
	void ClearData()
	{
		mKeys = new List<string>();
		allConvertedLine = new List<string>();
		curLineDic = null;
	}
	
	
	int showLog = 1;//Step DebugInfo
	int showWar = 2;	//Key-Value DebugInfo
	
	int debugFlag = 0;                         
	
	const int MaxLine = 1;
	const int MinJsonLength = 3;
	StreamReader mSR = null;
	List<string> mKeys = new List<string>();
	Dictionary<string, string> curLineDic = null;
	List<string> allConvertedLine = new List<string>();
	
	List<string> Convert()
	{
		if(mSR != null)
		{
			string line = null;
			try
			{
				//for(int i=0; i<MaxLine && (line=mSR.ReadLine())!=null; ++i)
				while((line=mSR.ReadLine()) != null)
				{
				//line 一行一行的解析
					//去掉行首尾空白
					line = line.Trim();
					if(string.IsNullOrEmpty(line) || !line.StartsWith("{"))
					{//注释行去掉
						continue;
					}
					//解析数据
					if(! ParseJsonLine(line))
					{
						ShowDebugError(DebugFlag + "ParseJsonLine==>" + line);
						return null;
					}
				}
				return allConvertedLine;
			}
			catch(System.Exception e) 
			{
				ShowDebugError(DebugFlag + "The process failed: "+e.ToString());
			}
		}
		else
		{
			ShowDebugError(DebugFlag + "but null");
		}
		
		return null;
	}
	
#region For SQY Format	
	List<string> Conversion()
	{
		if (mSR != null) 
		{
			string line = null;
			try 
			{
				//for (int i=0; i<MaxLine && (line=mSR.ReadLine())!=null; ++i)
				while((line=mSR.ReadLine()) != null)
				{//line 一行一行的解析
					//去掉行首尾空白
					line = line.Trim ();
					if (string.IsNullOrEmpty (line) || !line.StartsWith ("{")) 
					{//注释行去掉
						continue;
					}
					//解析数据
					if (! ParseLine (line)) 
					{
						ShowDebugError (DebugFlag + "ParseJsonLine==>" + line);
						return null;
					}
				}
				return allConvertedLine;
			}
			catch (System.Exception e) 
			{
				ShowDebugError (DebugFlag + "The process failed: " + e.ToString ());
			}
		} 
		else 
		{
			ShowDebugError (DebugFlag + "but null");
		}
		return null;
	}
	
	bool ParseLine(string line)
	{
		string myLine = line = line.Substring(1, line.Length-2);
		curLineDic = new Dictionary<string, string>();
		
		ShowDebugWarning (DebugFlag + "ParseLine Start==>" + line);
		while(!string.IsNullOrEmpty(myLine) && myLine.Length>MinJsonLength)
		{
			string _key = GetKey (ref myLine);
			if (string.IsNullOrEmpty (_key)) 
			{
				ShowDebugWarning (DebugFlag + "Key IsNullOrEmpty ==> " + myLine);
				return false;
			}
			ShowDebugWarning (DebugFlag + "  Key==>" + _key + "   newLine->" + myLine);
			string _value = GetValue (ref myLine);
			if (string.IsNullOrEmpty (_value)) 
			{
				ShowDebugWarning (DebugFlag + "Value IsNullOrEmpty ==> " + myLine);
				_value = "";
			}
			ShowDebugWarning (DebugFlag + "  Value==>" + _value + "   newLine->" + myLine);
			curLineDic.Add (_key, _value);
		}
		//如果有新列, 就加入
		AppendRow (MergerString (mKeys, new List<string> (curLineDic.Keys)));
		//添加本行数据
		AddLineData (curLineDic);
		return true;
	}
	
	string GetValue(ref string line)
	{
		line = line.Substring(1);
		line = line.Trim();
		string temp = "";
		int inCount = 0;
		bool inMid = false;
		bool inBig = false;
		string _value = "";
		for(int i=0; i < line.Length; ++i)
		{
			//ShowDebugWarning(i + " lineLen=" + line.Length + "  line~" + line);
			//ShowDebugLog("startIndex=" + i  + "length=" + > this.length
			temp = line.Substring(i, 1);
			ShowDebugLog(" GetValue->~" + temp + "~ value-> ~" + _value + "~    line~" + line.Substring(i));
			if(temp == "\"")
			{
				if((i<line.Length-1 && !stringEndFlag.Contains(line.Substring(i+1, 1))))
				{// " start
					if(i>0 && line.Substring(i-1, 1)=="\\")
					{//是转义
						
					}
					else
					{
						++inCount;
					}
				}
				else
				{//  " end
					--inCount;
				}
				_value += temp;
				
				if(inCount == 0 && !inBig && !inMid)
				{//   value
					ShowDebugWarning ("!!!!!!GetValue Return1->");
					line = line.Substring (_value.Length);
					return FillValue (_value);
				}
			}
			else if(temp == "[")
			{
				inMid = true;
				_value += temp;
			}
			else if(temp == "{")
			{
				inBig = true;
				_value += temp;
			}
			else if(temp == "]")
			{
				inMid = false;
				_value += temp;
				if(inCount==0 && !inBig && !inMid)
				{// value
					ShowDebugWarning ("!!!!!!GetValue Return2->");
					line = line.Substring(_value.Length);
					return FillValue (_value);
				}
				ShowDebugWarning ("inCount" + inCount + "inBig" + inBig + "inMid" + inMid);
			}
			else if(temp == "}")
			{
				inBig = false;
				_value += temp;
				if(inCount==0 && !inBig && !inMid)
				{// value
					ShowDebugWarning ("!!!!!!GetValue Return3->");
					line = line.Substring(_value.Length);
					return FillValue (_value);
				}
				ShowDebugWarning ("inCount" + inCount + "inBig" + inBig + "inMid" + inMid);
			}
			else if(temp == ",")
			{
				ShowDebugWarning ("inCount" + inCount + "inBig" + inBig + "inMid" + inMid);
				if(inCount==0 && !inBig && !inMid)
				{// value
					ShowDebugWarning ("!!!!!!GetValue Return4->");
					line = line.Substring(_value.Length);
					return FillValue (_value);
				}
				else
				{
					_value += temp;
				}
			}
			else
			{
				_value += temp;
			}
			
			if(i==line.Length-1 && inCount==0 && !inBig && !inMid)
			{//value
				ShowDebugWarning ("!!!!!!GetValue Return5->");
				line = line.Substring (_value.Length);
				return FillValue (_value);
			}
			//ShowDebugLog ("value->*" + _value + "*");
		}
		return "";
	}
#endregion
	
		
	
		
	bool ParseJsonLine (string line)
	{
		ShowDebugWarning (DebugFlag + "ParseJsonLine =*=*=*=*=*=*=>" + line);
		curLineDic = new Dictionary<string, string> ();
		//
		string myLine = line.Trim() + "";
		myLine = myLine.Substring(1, myLine.Length-2);
		while(!string.IsNullOrEmpty(myLine) && myLine.Length>MinJsonLength)
		{
			string _key = GetKey(ref myLine);
			if(string.IsNullOrEmpty(_key))
			{
				ShowDebugError(DebugFlag + "Key IsNullOrEmpty ==> " + myLine);
				return false;
			}
			string _value = GetKeyValue(ref myLine);
			if (string.IsNullOrEmpty (_value))
			{
				ShowDebugWarning (DebugFlag + "Value IsNullOrEmpty ==> " + myLine);
				_value = "";
			}
			curLineDic.Add(_key, _value);
		}
		//如果有新列, 就加入
		AppendRow (MergerString (mKeys, new List<string> (curLineDic.Keys)));
		//添加本行数据
		AddLineData (curLineDic);
		return true;
	}
	
	string GetKey(ref string line)
	{
		ShowDebugWarning (DebugFlag + "GetKey  ***Enter***-->" + line);
		string temp = line.Trim () + "";
		//去掉行首用于Json的分割格式符 ==>左括号, 逗号, 空格
		char[] tonken = new char[]{'{', ',', ' '};
		temp = temp.TrimStart (tonken);
		//作为变量键值必须是由双引号引起来的
		if(temp.StartsWith("\""))
		{
			//变量名 不会有空格
			temp = temp.TrimStart(new char[]{'"'});
			//截取引号之间的数据 ==> 变量名
			string key = temp.Substring(0, temp.IndexOf("\""));
			if(!string.IsNullOrEmpty(key))
			{
				ShowDebugWarning("GetKey  **Return**->~" + key + "~   ~" + temp);
				//截取后将该行的前面数据清掉 一定要去掉引号
				line = temp.Substring(key.Length+1);
				return key;
			}
		}
		return null;
	}
	
	string GetKeyValue(ref string line)
	{
		line = line.Trim ();
		//作为变量值必须在冒号后面
		if (line.StartsWith (":")) 
		{
			//去掉行首用于Json的分割格式符 ==>冒号
			char[] tonken = new char[]{' ', ':'};
			line = line.TrimStart (tonken);
			//截取数据 ==> 变量值
			ShowDebugWarning (DebugFlag + "ParseJsonValue <--***Enter***-->" + line);
			string _value = ParseJsonValue(ref line);
			ShowDebugWarning (DebugFlag + "ParseJsonValue <--**Return**-->" + line);
			if(!string.IsNullOrEmpty(_value))
			{
				//截取后将该行的前面数据清掉
				//line = temp.Substring (_value.Length, temp.Length-_value.Length);
				return FillValue(_value);
			}
		}
		return "";
	}
	
	string ParseJsonValue(ref string line)
	{
		line = line.Trim();
		if(!string.IsNullOrEmpty(line))
		{
			switch(line.Substring(0, 1))
			{
				case "\"": return GetStringValue(ref line);
				case "[" : return GetArrValue(ref line);
				case "{": return GetObjectValue(ref line);
				default: return GetNumberValue(ref line);
			}
		}
		return null;
	}
	
	string GetNumberValue(ref string line)
	{
		ShowDebugWarning (DebugFlag + "GetNumberValue Before->" + line);
		string _value = "";
		while (line!=null && line.Length>0) 
		{	
			string temp = line.Substring (0, 1);
			ShowDebugLog(temp);
			if (temp == ",") 
			{
				break;
			}
			else
			{
				_value += temp;
				line = line.Substring (1);
			}
		}
		ShowDebugWarning (DebugFlag + "GetNumberValue After->" + line);
		return _value.Trim();
	}
	
	List<string> stringEndFlag = new List<string> (new string[]{",", "]", "}"});
	string GetStringValue(ref string line)
	{
		ShowDebugWarning(DebugFlag + "GetStringValue In->" + line);
		string _value = "";
		string temp = "";
		bool inString = false;
		for(int index=0; index<line.Length; ++index)
		{
			temp = line.Substring(index, 1);
			ShowDebugLog (temp);
			if (temp == "\"")
			{
				if (index > 0 && line.Substring (index-1, 1) == "\\")
				{//是转义
					
				} 
				else 
				{
					inString = !inString;
				}
			}
			else if(stringEndFlag.Contains(temp))
			{
				if(! inString)
				{//string end
					//line 还包含 stringEndFlag
					line = line.Substring(_value.Length);
					break;
				}
			}
			_value += temp;
		}
		_value = _value.Trim();
		ShowDebugWarning (DebugFlag + "GetStringValue Out->~" + _value + "~   new Line->~" + line);
		return _value;
	}
	
	string GetArrValue(ref string line)
	{
		ShowDebugWarning (DebugFlag + "GetArrValue Start->" + line);
		string _value = "";
		string temp = "";
		if(line.StartsWith("["))
		{
			_value = "[";
			//去掉 [ 
			line = line.Substring(1);
			while(line!=null && line.Length>0)
			{
				temp = line.Substring(0, 1);
				ShowDebugLog(temp);
				if(temp == "[")
				{
					_value += GetArrValue(ref line);
				}
				else if(temp == "{")
				{
					_value += GetObjectValue(ref line);
				}
				else if(temp == "\"")
				{
					_value += GetStringValue(ref line);
				}
				else
				{
					_value += temp;
					line = line.Substring (1);
					if (temp == "]")
					{
						break;
					}
				}
			}
		}
		ShowDebugWarning (DebugFlag + "GetArrValue End->~" + _value + "~   new Line->~" + line);
		return _value;
	}
	
	string GetObjectValue(ref string line)
	{
		ShowDebugWarning (DebugFlag + "GetObjectValue Begin->" + line);
		string _value = "";
		string temp = "";
		if (line.StartsWith ("{")) 
		{
			_value = "{";
			//去掉 {
			line = line.Substring (1);
			while (line!=null && line.Length>0) 
			{
				temp = line.Substring (0, 1);
				ShowDebugLog (temp);
				if (temp == "[") 
				{
					_value += GetArrValue (ref line);
				} 
				else if (temp == "{") 
				{
					_value += GetObjectValue (ref line);
				} 
				else if (temp == "\"") 
				{
					_value += GetStringValue (ref line);
				} 
				else 
				{
					_value += temp;
					line = line.Substring (1);
					if (temp == "}") 
					{
						break;
					}
				}
			}
		}
		ShowDebugWarning (DebugFlag + "GetObjectValue Over->~" + _value + "~   new Line->~" + line);
		return _value;
	}
	
	string FillValue(string _value)
	{
		ShowDebugWarning(DebugFlag + "B FillValue=>" + _value);
		if(_value.StartsWith("\"") && _value.EndsWith("\""))
		{//去掉首尾引号
			_value =	 _value.Substring(1, _value.Length-2);
		}
		//将引号换成两个引号
		
		//_value.Replace("\"", "\"" + "\"");
		System.Text.StringBuilder sb = new System.Text.StringBuilder(_value);
		sb.Replace("\"", "\"\"");
		_value = sb.ToString();
		ShowDebugWarning (DebugFlag + "M FillValue=>" + _value);
		if(_value.IndexOf(",")!=-1 || _value.IndexOf("\"")!=-1)
		{
			_value = "\"" + _value + "\"";
		}
		ShowDebugWarning (DebugFlag + "E FillValue=>" + _value);	
		return _value;
	}
	
	List<string> MergerString(List<string> _old, List<string> _new)
	{
		List<string> ret = new List<string>();
		//把新的列追加在旧的列的后面  防止前面已填数据错乱
		foreach(string key in _new)
		{
			if(!_old.Contains(key))
			{
				_old.Add(key);
				ret.Add(key);
			}
		}
		return ret;
	}
	
	void AppendRow(List<string> rows)
	{
		if (allConvertedLine != null && rows!=null && rows.Count>0) 
		{	
			string _line = "";
			int count = 0;
			if(allConvertedLine.Count < 1)
			{
				//Title
				foreach(string item in rows)
				{
					_line += "," + item;
				}
				_line = _line.TrimStart(new char[]{','});
				allConvertedLine.Add(_line);
			}
			else
			{
				//Title
				_line = allConvertedLine [0];
				foreach (string item in rows) 
				{
					count ++;
					_line += "," + item;
				}
				allConvertedLine [0] = _line;
			
				string _add = new string(',', count);
				for(int index=1; index<allConvertedLine.Count; ++index)
				{
					_line = allConvertedLine [index];
					_line = _line + _add;
					allConvertedLine [index] = _line;
				}
			}
		}
	}
	
	void AddLineData(Dictionary<string, string> _data)
	{
		string _line = "";
		foreach(string key in mKeys)
		{
			if(_data.ContainsKey(key))
			{
				_line += "," + _data[key].ToString();
			}
			else
			{
				_line += ",";
			}
		}
		//去掉多添的一个逗号
		_line = _line.Substring (1);
		if((debugFlag & showLog) > 0)
		{
			Debug.Log ("line-->" + _line);
		}
		allConvertedLine.Add(_line);
	}
	
	void Print(string[] arr)
	{
		if(arr!=null && arr.Length>0)
		{
			ShowDebugWarning ("-----------Begin----------->" + arr.Length);
			foreach(string str in arr)
			{
				ShowDebugLog(str);
			}
			ShowDebugWarning ("------------End------------>" + arr.Length);
		}
		else
		{
			ShowDebugLog(DebugFlag + "Print is NullOrEmpty");
		}
	}
	
	void ShowDebugLog (string str)
	{
		if ((debugFlag & showLog) > 0) 
		{
			Debug.Log ("=====*=====~" + str + "~=====*=====");
		}
	}
	void ShowDebugWarning (object obj)
	{
		if ((debugFlag & showWar) > 0) 
		{
			Debug.LogWarning (obj);
		}
	}
	void ShowDebugError (object obj)
	{
		Debug.LogError(obj);
	}
}
