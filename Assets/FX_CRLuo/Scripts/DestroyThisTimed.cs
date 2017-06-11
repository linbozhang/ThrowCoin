using UnityEngine;
using System.Collections;
//Create DestroyThisTimed by Song
// Copy Right 2014®
public class DestroyThisTimed : MonoBehaviour {

	public float destroyTime = 5;
	// Use this for initialization
	void Start () {
		Destroy(gameObject,destroyTime);
	}
	
}