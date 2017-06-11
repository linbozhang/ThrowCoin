using UnityEngine;
using System.Collections;

public static class Core {

	public static Config cfg;

	public static FinalConfig fc;

	public static MDNetData nData;

	public static System.DateTime now{
		get{
			if(nData != null)
			{
			return WGTime.Unix2DateTime(nData.sysTime);
			}
			else
			{
				return System.DateTime.Now;
			}
		}
	}

	public static void Initialize()
	{
		cfg = new Config();
		nData = new MDNetData();
	}

}
