using UnityEngine;
using System.Collections;

public class WGBulletCheck : MonoBehaviour {

	public bool bCheckAchievement = false;
	// Use this for initialization
	void Start () {
	
	}
	

	void  OnTriggerEnter ( Collider other  )
	{
		if(other.tag.Equals("Coin"))
		{
			WGGameWorld.Instance.HideObj(other.gameObject);
		}
	}

}
