using UnityEngine;
using System.Collections;

public class FX_Disappear : MonoBehaviour {

	public float f_Time = 5f;
	public float Show_Time = 0f;
	public float Stop_Time = 0f;
	// Use this for initialization



	void Start () {

		if (Show_Time != 0 || Stop_Time != 0)
		{
			this.gameObject.SetActive(false);

		Invoke("Show",Show_Time);
		Invoke("Stop",Stop_Time);
		}
		

		Destroy(this.gameObject, f_Time);
	}





	void Show()
	{
		this.gameObject.SetActive(true);
	}

	void Stop()
	{
			this.transform.parent = null;
	}

}