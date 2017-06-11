using UnityEngine;
using System.Collections;

public class CRLuo_ParticleONOFF : MonoBehaviour {
	public string _ = "-=<控制粒子发射开关程序>=-";
	public string __ = "应用粒子对象，（空时只对当前粒子有效）";
	public ParticleSystem [] MyPart ;
	public string ___ = "测试开关键盘1/2";
	public bool Try_Key = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Try_Key)
		{

			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				Part_ON();
			}
			//如果按 2 键
			else if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				Part_OFF();
			}
		}
	
	}

	void Part_ON(){
		if (MyPart.Length > 0)
		{ 
			foreach(ParticleSystem temp in MyPart)
			{
				if (temp != null)
				{
					temp.enableEmission = true;
				}
			};
		
		}
		else
		{

			this.GetComponent<ParticleSystem>().enableEmission = true;
		}
	}
	void Part_OFF(){
		if (MyPart.Length > 0)
		{
			foreach (ParticleSystem temp in MyPart)
			{
				if (temp != null)
				{
					temp.enableEmission = false;
				}
			};

		}
		else
		{

			this.GetComponent<ParticleSystem>().enableEmission = false;
		}
	}
	void Part_Delete()
	{
		if (MyPart.Length > 0)
		{
			foreach (ParticleSystem temp in MyPart)
			{
				if (temp != null)
				{
					temp.enableEmission = false;
					Destroy(this.gameObject, temp.startLifetime);
				}
			};

		}
		else
		{
			this.GetComponent<ParticleSystem>().enableEmission = false;

			Destroy(this.gameObject, this.GetComponent<ParticleSystem>().startLifetime);
		}
	}

}
