using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class SDK {

	static public void AddChild (GameObject child,GameObject parent,bool reset = true)
	{
		if (child !=null && parent != null)
		{
			Transform t = child.transform;
			t.parent = parent.transform;
			if(reset)
			{
				t.localPosition = Vector3.zero;
				t.localRotation = Quaternion.identity;
            }
            t.localScale = Vector3.one;
			child.layer = parent.layer;
		}
	}
	static public float[] to3Float(Vector3 v3)
	{
		return new float[]{v3.x,v3.y,v3.z};
	}


    static public Object Load(string path)
    {
        float factor = Screen.width*1f/Screen.height;
        Object obj = null;
        if(factor>0.66f&factor<0.67f)//640 *960
        {
            obj = Resources.Load(path+"@4");
        }
        else if(factor>0.56f&&factor<0.57f)//640*1136
        {
        }
        if(obj == null)
        {
            obj = Resources.Load(path);
        }
        return obj;
    }


	public static string Md5Sum(string input) {
		System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create(); 
		byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
		byte[] hash = md5.ComputeHash(inputBytes); 

		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		for (int i = 0; i < hash.Length; i++) { 
			sb.Append(hash[i].ToString("X2")); 
		} 
		return sb.ToString(); 
	}

	public static string v3String(Vector3 v3)
	{
		string result = "("+v3.x+","+v3.y+","+v3.z+")";
		return result;
	}
	public static Vector3 toV3(float[] fl)
	{
		if(fl.Length>=3)
		{
			return new Vector3(fl[0],fl[1],fl[2]);
		}
		return Vector3.zero;
	}
	public static Vector3 toV3(List<float> fl)
	{
		if(fl.Count>=3)
		{
			return new Vector3(fl[0],fl[1],fl[2]);
		}
		return Vector3.zero;
	}
	public static Vector3 toV3(string s)
	{
		if(!string.IsNullOrEmpty(s))
		{
			string[] ms = s.Split(',');
			if(ms.Length>=3)
			{
				return new Vector3(float.Parse(ms[0]),float.Parse(ms[1]),float.Parse(ms[2]));
			}
		}
		return Vector3.zero;
	}

	public static string Serialize(object obj)
	{
		#if fastJSON
		return fastJSON.JSON.Instance.ToJSON(obj);
		#elif JsonFx
		return JsonFx.Json.JsonWriter.Serialize(obj);
		#else
		return WEMiniJSON.Json.Serialize(obj);
		#endif
	}
	public static  T Deserialize<T>(string json)
	{
		#if fastJSON
		return fastJSON.JSON.Instance.ToObject<T>(json);
		#elif JsonFx
		return JsonFx.Json.JsonReader.Deserialize<T>(json);
		#else
		return (T)WEMiniJSON.Json.Deserialize(json);
		#endif
	}

}
