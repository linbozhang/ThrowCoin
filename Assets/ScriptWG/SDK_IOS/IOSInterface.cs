using UnityEngine;
using System.Collections;


public partial class IOSInterface:MonoBehaviour{


	public  static IOSInterface self = null;

	void Awake()
	{
		self = this;
		DontDestroyOnLoad(this.gameObject);
	}

}



