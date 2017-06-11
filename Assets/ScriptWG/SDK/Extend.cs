using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Extend {

	public static void dealloc(this MonoBehaviour mb)
	{
		if(mb != null)
		{
			UnityEngine.Object.Destroy(mb.gameObject);
			UnityEngine.Object.Destroy(mb);
			mb = null;
		}
	}
//	public static T CreateViewFromResources<T>(string path)
//	{
//		Object obj = Resources.Load(path);
//		if(obj != null)
//		{
//			GameObject go = UnityEngine.Object.Instantiate(obj) as GameObject;
//
//			System.Type t = typeof(T);
//			t = go.GetComponent<>();
//			return t;
//		}
//		return null;
//	}
	public static void ESetActive(this Component comp,bool act)
	{
		if(comp != null)
		{
			comp.gameObject.SetActive(act);
		}
	}
	public static void ESetActive(this GameObject go,bool act)
	{
		if(go != null)
		{
			go.SetActive(act);
		}
	}

	public static float toFloat(this object num){
		if(num != null)
		{
			if(num is long)
			{
				return (float)(long)num;
			}
			else if(num is double)
			{
				return (float)(double)num;
			}
			else if(num is float)
			{
				return (float)num;
			}
			else if(num is string)
			{
				return float.Parse(num as string);
			}
		}
		return 0;
	}
	public static int toInt(this object num){
		if(num != null)
		{
			if(num is long)
			{
				return (int)(long)num;
			}
			else if(num is string)
			{
				if(!string.IsNullOrEmpty(num as string))
				{
				return int.Parse(num as string);
			}
			}
			else if(num is double)
			{
				return (int)(double)num;
			}
		}
		return 0;
	}
	public static double toDouble(this object num){
		if(num != null)
		{
			if(num is long)
			{
				return (double)(long)num;
			}
			else if(num is string)
			{
				if(!string.IsNullOrEmpty(num as string))
				{
					return double.Parse(num as string);
				}
			}
			else if(num is double)
			{
				return (double)num;
			}
		}
		return 0;
	}
}
