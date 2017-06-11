using UnityEngine;
using System.Collections;

public class CRLuo_AlphaFlash : MonoBehaviour
{
	public GameObject InputObj;
	Material InputMat;
	public float FlashSpeed = 1;
	public float FlashMax = 1;
	public bool AutoDelete = false;
	public float DeleteTime;
	[System.NonSerialized]
	public Color MainColor;
	// Use this for initialization
	void Start () 
	{
		if (AutoDelete)
		{
			Invoke("DeleteObj", DeleteTime);
		}

		if (InputObj == null)
		{
			InputMat = gameObject.GetComponent<Renderer>().material;
		}
		else
		{
			InputMat = InputObj.GetComponent<Renderer>().material;
		}
		MainColor = InputMat.GetColor("_TintColor");

	}
	
	// Update is called once per frame
	void Update () 
	{
		InputMat.SetColor("_TintColor", new Color(MainColor.r, MainColor.g, MainColor.b, (Mathf.Sin(Time.time * FlashSpeed) + 1)) / 2f * FlashMax);
	}

	public void DeleteObj()
	{
		GetComponent<Animation>().CrossFade("Out");
		Destroy(this.gameObject, 1);
	}
}