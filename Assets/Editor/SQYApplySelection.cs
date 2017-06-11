using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;

public class SQYApplySelection : MonoBehaviour {


	[MenuItem("IvuTools/Disconnect %2")]
	static void Dusconnect() { 
		foreach(Transform aTransform in Selection.transforms){
			PrefabUtility.DisconnectPrefabInstance(aTransform);
		}
	}




}
