using UnityEngine;
using System.Collections;

public class WGMonoBehaviour : MonoBehaviour {

	public virtual void InvokeBlock(float waitTime,System.Action a)
	{
		StartCoroutine(doBlock(a,waitTime));
	}
	IEnumerator doBlock(System.Action a, float time)
	{
		yield return new WaitForSeconds(time);
		if(a!=null) a();
	}

}
