using UnityEngine;
using System.Collections;

public class WGLevelUpViewController : MDBaseAlertView {
	public TweenScale tsContent;

	public override void showView ()
	{
		base.showView ();
		tsContent.ESetActive(true);

		tsContent.transform.localScale = tsContent.from;

		tsContent.PlayForward();

		mnIvokeBlock.InvokeBlock(0.25f,()=>{
			showViewEnd();
		});
	}
	public override void showViewEnd ()
	{
		base.showViewEnd ();
	}
	public override void hiddenView ()
	{
		base.hiddenView ();
		tsContent.PlayReverse();
		mnIvokeBlock.InvokeBlock(tsContent.duration,()=>{
			hiddenViewEnd();
		});

	}
	public override void hiddenViewEnd ()
	{
		base.hiddenViewEnd ();

	}



	public static WGLevelUpViewController CreaetViewController()
	{
		Object obj = Resources.Load("pbWGLevelUpViewController");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			WGLevelUpViewController lu = go.GetComponent<WGLevelUpViewController>();
			SDK.AddChild(go,WGRootManager.Self.goRootGameUI);
			return lu;
		}
		return null;
	}
}
