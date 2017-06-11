using UnityEngine;
using System.Collections;

public class WGConnectingView : MDBaseAlertView {


	public GameObject goContent;

	public override void showView ()
	{
		base.showView ();

	}
	public override void hiddenView ()
	{
		base.hiddenView();
		hiddenViewEnd(); 
	}
	public override void hiddenViewEnd ()
	{
		base.hiddenViewEnd ();
	}

	public static WGConnectingView CreateConnectingView()
	{
		Object obj = Resources.Load("pbConnectingView");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			WGConnectingView cv = go.GetComponent<WGConnectingView>();
			return cv;
		}
		return null;
	}
}
