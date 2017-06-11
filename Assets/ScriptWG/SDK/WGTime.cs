using UnityEngine;
using System.Collections;
using System;
public class WGTime{
	public static double timeSince1970
	{
		get{
			double epoch = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
			return epoch;
		}
	}
	public static double timeSince19700{
		get{
			return DateTime2Unix(DateTime.Now);
		}
	}
	/// <summary>
	/// 将Unix时间戳转换为DateTime类型时间
	/// </summary>
	/// <param name="d">double 型数字</param>
	/// <returns>DateTime</returns>
	public static System.DateTime Unix2DateTime(double d)
	{
		System.DateTime time = System.DateTime.MinValue;
		System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
		time = startTime.AddSeconds(d);
		return time;
	}

	/// <summary>
	/// 将c# DateTime时间格式转换为Unix时间戳格式
	/// </summary>
	/// <param name="time">时间</param>
	/// <returns>double</returns>
	public static double DateTime2Unix(System.DateTime time)
	{
		double intResult = 0;
		System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
		intResult = (time - startTime).TotalSeconds;
		return intResult;
	}
}
