using UnityEngine;
using System.Collections;
//Create WGDeleteObjectAfterTime by Song
// Copy Right 2014®
public class WGDeleteObjectAfterTime : MonoBehaviour {
	public float time=1;

	void Start()
	{
		StartCoroutine(DeleteGameObject());
	}
	IEnumerator DeleteGameObject()
	{
		yield return new WaitForSeconds(time);
		Destroy(this.gameObject);
	}
}