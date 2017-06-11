using UnityEngine;
using System.Collections;

public class WGShowUnlockBear : MDBaseAlertView {


	public TweenScale tsContent;

	public UISprite spGrayBackground;

	public UILabel labName;

	bool willHidden = false;

	BCObj mBearObj;

	public void FreshUnlockBearView(int id)
	{
		WGDataController _dtCtl = WGDataController.Instance;

		mBearObj = _dtCtl.GetBCObj(id);

//		WGBearParam bp = _dtCtl.GetBearParam(id);

		labName.text = mBearObj.Name;
		ShowOneBear.getInstance();
	}
	public override void showView ()
	{
		base.showView ();

		spGrayBackground.ESetActive(true);

		tsContent.gameObject.SetActive(true);
		tsContent.PlayForward();
		mnIvokeBlock.InvokeBlock(tsContent.duration,()=>{
			showViewEnd();
		});
	}

	public override void showViewEnd ()
	{
		base.showViewEnd ();
		ShowOneBear.getInstance().ShowOneWithID(mBearObj.ID);
//		mnIvokeBlock.InvokeBlock(5,()=>{
//			hiddenView();
//		});
	}
	public override void hiddenView ()
	{
		if(willHidden) return;
		willHidden = true;
		ShowOneBear.DestroySelf();

		base.hiddenView ();
		tsContent.PlayReverse();
		mnIvokeBlock.InvokeBlock(tsContent.duration,()=>{
			hiddenViewEnd();
		});
	}
	public override void hiddenViewEnd ()
	{
		willHidden = false;
		spGrayBackground.ESetActive(false);
		base.hiddenViewEnd ();


	}
	static Object mObj = null;
	public static WGShowUnlockBear CreateUnlockBear()
	{

		if(mObj == null)
		{
			mObj = Resources.Load("pbWGShowUnlockBear");
		}
		if(mObj != null)
		{
			GameObject go = Instantiate(mObj) as GameObject;

			WGShowUnlockBear sub = go.GetComponent<WGShowUnlockBear>();

			return sub;
		}


		return null;
	}
}
