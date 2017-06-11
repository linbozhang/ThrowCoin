using UnityEngine;
using System.Collections;

public class D04PowerTipView : MDBaseAlertView {



	public override void OnBtnOk ()
	{
		base.OnBtnOk ();
	}


	public static D04PowerTipView CreatePowerPayView()
	{
		Object obj = Resources.Load("pbD04PowerTipView");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			D04PowerTipView t = go.GetComponent<D04PowerTipView>();
			SDK.AddChild(go,WGRootManager.Self.goRootGameUI);
			return t;
		}
		return null;
	}
}
