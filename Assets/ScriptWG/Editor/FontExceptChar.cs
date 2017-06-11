using UnityEditor;
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
public class FontExceptChar : MonoBehaviour {



	[MenuItem("SQYAction/RemoveRepeatChar")]
	static void RemoveOther()
	{

//		Object obj = Selection.activeObject;
		string str = "";
		string last = "";
		foreach(Object obj in Selection.objects)
		{
			string path = AssetDatabase.GetAssetPath(obj);



			using(StreamReader sr = new StreamReader(path))
			{
				str = sr.ReadToEnd();
			}
			str.Replace('\n',' ');
	//		str = Regex.Replace(str, "(?s)(.)(?=.*\\1)", "");

			last += DelStringSame(str);
		}
		Debug.Log(last);

//		using( StreamWriter sw = new StreamWriter(path))
//		{
//			sw.Write(str);
//		}
	}
	#region ArrayList的示例应用
	/// 方法名：DelStringSame     
	/// 功能：   删除字符串中重复的元素
	/// </summary>    
	/// <param name="TempArray">所要删除的字符串</param>
	/// <returns>返回字符串</returns>   
	static string DelStringSame(string TempStr)
	{
		char[] TempArray = (char[])TempStr.ToCharArray();
		string newStr = "";
		for (int i = 0; i < TempArray.Length; i++)
		{
			if (!newStr.Contains(TempArray[i].ToString()))
			{
				newStr += TempArray[i].ToString();
			}
		}
		return newStr;
	}
	#endregion

}
