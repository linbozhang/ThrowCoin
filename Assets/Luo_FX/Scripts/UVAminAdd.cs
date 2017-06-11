using UnityEngine;
using System.Collections;

public class UVAminAdd : MonoBehaviour {

	public bool Use = false;

	public Material material;

	public float UAdd;

	public float VAdd;

	float UNow;

	float VNow;
	
	void Start(){
		UNow = 0;
		VNow = 0;
	}

	void Update()
	{
		if (Use)
		{
			UNow += UAdd;
			VNow += VAdd;

			material.SetTextureOffset("_MainTex", new Vector2(UNow, VNow));
		}
	}
}
