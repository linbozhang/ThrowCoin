using UnityEngine;
using System.Collections;

public class WGRoteObject : MonoBehaviour {

	public Vector3 v3Rotate = Vector3.one;

	public bool ignoreTimeScale = false;
	float myTime = 0;
	Transform tran;

	void Start()
	{
		tran = transform;
		myTime = Time.realtimeSinceStartup;
	}
	// Update is called once per frame
	void Update () {
		if(ignoreTimeScale)
		{
			tran.Rotate(v3Rotate*(Time.realtimeSinceStartup-myTime));
			myTime = Time.realtimeSinceStartup;
		}
		else
		{
			tran.Rotate(v3Rotate*Time.deltaTime);
		}
	}
}
