using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;

public class GB2312UnicodeConverter
{
	/// <summary>
	/// 汉字转换为Unicode编码
	/// </summary>
	/// <param name="str">要编码的汉字字符串</param>
	/// <returns>Unicode编码的的字符串</returns>
	public static string ToUnicode (string str)
	{
		byte[] bts = Encoding.Unicode.GetBytes (str);
		string r = "";
		for (int i = 0; i < bts.Length; i += 2)
			r += "\\u" + bts [i + 1].ToString ("x").PadLeft (2, '0') + bts [i].ToString ("x").PadLeft (2, '0');
		return r;
	}
	/// <summary>
	/// 将Unicode编码转换为汉字字符串
	/// </summary>
	/// <param name="str">Unicode编码字符串</param>
	/// <returns>汉字字符串</returns>
	public static string ToGB2312 (string str)
	{
		if (str == null)
			return str;
		string r = "";
		MatchCollection mc = Regex.Matches (str, @"\\u([\w]{2})([\w]{2})", RegexOptions.None | RegexOptions.IgnoreCase);
		byte[] bts = new byte[2];
		foreach (Match m in mc) {
			bts [0] = (byte)int.Parse (m.Groups [2].Value, NumberStyles.HexNumber);
			bts [1] = (byte)int.Parse (m.Groups [1].Value, NumberStyles.HexNumber);
			r = Encoding.Unicode.GetString (bts);

			str = str.Replace (m.Groups [0].Value, r);
		}
		return str;
	}
 }