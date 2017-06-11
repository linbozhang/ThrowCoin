using UnityEngine;
using System.Collections;

public class WGParam : MonoBehaviour {


	public bool bUseRewardType = false;
	public int mRewardType = 0;
	public bool bP1=false;
	public int mParameter1 = 0;
	public bool bP2=false;
	public int mParameter2 = 0;


	public bool bTimeScale = false;

	public float fTimeScale =1;


	public bool bDoOnce = false;

	public static WGParam Instance;

	void Awake()
	{
		Instance = this;
	}
	void Start()
	{
	}
	void Update()
	{
		if(bDoOnce)
		{
			bDoOnce = false;
			if(bTimeScale)
			{
				Time.timeScale = fTimeScale;
			}
		}
	}


}
