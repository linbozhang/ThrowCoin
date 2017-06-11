using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CopyTrans : MonoBehaviour {


	public List<GameObject> szTrans;
	public List<GameObject> szTarget;

	[ContextMenu("Copy")]
	public void Copy()
	{
		if(szTrans.Count != szTarget.Count)
		{
			Debug.LogError("count do not equal!!");
			return;
		}
		for(int i=0,max = szTrans.Count;i<max;i++)
		{
			GameObject ct =szTrans[i];
			GameObject go = szTarget[i];


			go.transform.localPosition = ct.transform.localPosition;

			go.transform.eulerAngles = ct.transform.eulerAngles;

			go.transform.localScale = ct.transform.localScale;



		}
	}

}
