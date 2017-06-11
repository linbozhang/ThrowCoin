using UnityEngine;
using System.Collections;

public class WGTiger777 : MDBaseAlertView {

	public TweenScale tsContent;
	public TweenPosition tpLight;

	static WGTiger777 _Self = null;
	public static WGTiger777 Self {
		get{
			if(_Self == null)
			{
				_Self = WGTiger777.CreateTiger777View();
				SDK.AddChild(_Self.gameObject,WGRootManager.Self.goRootGameUI);
			}
			return _Self;
		}
	}


	public override void showView ()
	{
		base.showView ();

		tsContent.ESetActive(true);
		tsContent.PlayForward();
		InvokeBlock(tsContent.duration,()=>{
			showViewEnd();
		});
	}
	public override void showViewEnd ()
	{
		base.showViewEnd ();
		tpLight.ESetActive(true);
		tpLight.ResetToBeginning();

		InvokeBlock(1,()=>{
			hiddenView();
		});
	}
	public override void hiddenView ()
	{
		base.hiddenView ();
		tpLight.ESetActive(false);
		tsContent.PlayReverse();
		InvokeBlock(tsContent.duration,()=>{
			hiddenViewEnd();
		});
	}
	public override void hiddenViewEnd ()
	{
		base.hiddenViewEnd ();
		tsContent.ESetActive(false);
		_Self = null;
		Destroy(gameObject);
	}
	static WGTiger777 CreateTiger777View()
	{
		Object obj = Resources.Load("pbWGTiger777");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			WGTiger777 tig = go.GetComponent<WGTiger777>();
			return tig;
		}
		return null;
	}
}
