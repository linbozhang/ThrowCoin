using UnityEngine;
using System.Collections;

public class TransformAdd : MonoBehaviour {

	public bool Move_Key;
	public Vector3 Move_Add;

	public bool Rotation_Key;
	public Vector3 Rotation_Add;

	public bool Scale_Key;
	public Vector3 Scale_Add;


	public bool Time_Key = false;
	public float f_StartDelay = 0;
	public float f_LifeTime = 3;

	void Start () {
		if(Time_Key){
			Invoke("Die",f_LifeTime);
		}
	}
	
	void Die(){
		Destroy(this.gameObject);
	}

	void Update () {

		if(Time_Key){
			if(f_StartDelay > 0){
				f_StartDelay -= Time.deltaTime;
				return;
			}
		}

		if (Move_Key)
		{
			this.transform.Translate(Move_Add * Time.deltaTime);
		}

		if (Rotation_Key)
		{
			this.transform.Rotate(Rotation_Add * Time.deltaTime);
		}
		if (Scale_Key)
		{
			this.transform.localScale += Scale_Add * Time.deltaTime;
		}

	}
}
