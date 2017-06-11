using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class WGDropCheck : MonoBehaviour {

	private WGGameWorld _ShareWorld;//
	void Start()
	{
		if(_ShareWorld == null)
			_ShareWorld = WGGameWorld.Instance;
	}

	void  OnTriggerEnter ( Collider other  )
	{
		if(other.tag.Equals("Coin"))
		{
			other.tag = "InCoin";
		}
		else if(other.tag.Equals("COLLECTION"))
		{
			other.tag = "InCollection";
		}
		_ShareWorld.DropQueue.Enqueue(other.gameObject);
	}

}

