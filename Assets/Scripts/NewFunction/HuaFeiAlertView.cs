using UnityEngine;
using System.Collections;

public class HuaFeiAlertView : MDBaseAlertView {

	public TweenScale tsContent;


	public override void showView ()
	{
		base.showView ();
		tsContent.ESetActive(true);
		
		tsContent.PlayForward();
		
		mnIvokeBlock.InvokeBlock(0.25f,()=>{
			showViewEnd();
		});
	}
	public override void hiddenView ()
	{
		base.hiddenView ();
		tsContent.PlayReverse();
		mnIvokeBlock.InvokeBlock(tsContent.duration,()=>{
			hiddenViewEnd();
		});
	}

	public static HuaFeiAlertView CreateAlertView()
	{
		Object obj = Resources.Load("pbHuaFeiAlertView");
		if (obj != null)
		{
			GameObject go = Instantiate(obj)as GameObject;
			SDK.AddChild(go,WGRootManager.Self.goRootTopUI);
			HuaFeiAlertView v= go.GetComponent<HuaFeiAlertView>();
			return v;
		}
		return null;
	}
}
