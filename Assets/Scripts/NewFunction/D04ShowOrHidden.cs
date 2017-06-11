using UnityEngine;
using System.Collections;

public enum SHGO{
	None = 0,
	CommonTiger = 1,
	CustomerService = 2,
}

public class D04ShowOrHidden : MonoBehaviour {
	public SHGO myGo = SHGO.None;
	// Use this for initialization
	void Start () {
		if(myGo == SHGO.CommonTiger)
		{
			this.ESetActive(!YeHuoSDK.bCommonTiger);
		}
		if(myGo == SHGO.CustomerService)
		{
			this.ESetActive(YeHuoSDK.bKeFu);
		}
	}

}

