using UnityEngine;
using System.Collections;

public class WGRootManager : MonoBehaviour {

	public GameObject goRootMainUI;
	public GameObject goRoot3D;
	public GameObject goRootTopUI;
	public GameObject goRootTopUI1;
	public GameObject goRootGameUI;

	public static WGRootManager Self;
	void  Awake()
	{
		Self = this;
	}
	void Start()
	{
		goRootTopUI1.ESetActive(false);
	}

}
