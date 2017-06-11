using UnityEngine;
using System.Collections;

public class D04ColorCtrl : MonoBehaviour {

	public Color mColor1;
	public Color mColorClear;

	public UIWidget curWidget;

	public bool bNeedUpdate = false;

	// Use this for initialization
	void Start () {
		freshUI();
	}
	void Update()
	{
		if(bNeedUpdate)
		{
			freshUI();
		}
	}
	void freshUI()
	{
		if(curWidget == null)
		{
			curWidget = this.GetComponent<UIWidget>();
		}
		if(YeHuoSDK.bMsgClear)
		{
			curWidget.color = mColorClear;
		}
		else
		{
			curWidget.color = mColor1;
		}
	}
}
