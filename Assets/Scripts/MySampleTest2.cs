using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MySampleTest2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
//	
//		Debug.Log( WGTime.timeSince1970.ToString()+"####"+WGTime.timeSince19700.ToString());
//
//
//		Debug.Log("="+Application.dataPath+"\n"+Application.persistentDataPath+"\n");

		Config cf = new Config();

		Debug.Log(SDK.Serialize(cf));
	
	}
	

}

