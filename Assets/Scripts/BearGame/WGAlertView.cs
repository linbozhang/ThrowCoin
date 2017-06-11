using UnityEngine;
using System.Collections;


public class WGAlertView : MDBaseAlertView {

	public GameObject goContent;
	public UILabel labTitle;
	public WGPanelButton btnOK_Big;
	public WGPanelButton btnOK;
	public WGPanelButton btnCancel;
	public TweenScale tsContent;
	public int type = 0;

	public void freshViewWithTitle(string title,string ok ,string cancel = "")
	{
		labTitle.text = title;
		btnOK.title.text = ok;
		btnOK_Big.title.text = ok;
		if(string.IsNullOrEmpty(cancel))
		{
			btnOK.button.ESetActive(false);
			btnCancel.button.ESetActive(false);
			btnOK_Big.button.ESetActive(true);

		}
		else{
			btnOK_Big.button.ESetActive(false);
			btnOK.button.ESetActive(true);
			btnCancel.button.ESetActive(true);
			btnCancel.title.text = cancel;

		}
	}
	public override void OnBtnOk ()
	{
		base.OnBtnOk ();
		BCSoundPlayer.Play(MusicEnum.button);
	}
	public override void OnBtnCancel ()
	{
		BCSoundPlayer.Play(MusicEnum.button);
		base.OnBtnCancel ();
	}
	public override void showView ()
	{
		base.showView ();
		tsContent.gameObject.SetActive(true);

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

	static Object mObj0 = null;
	static Object mObj1 = null;
	public static WGAlertView CreateAlertView(int type = 0)
	{
		Object mObj = null;
		if(type == 0)
		{
			mObj = mObj0;
		}
		else if(type == 1)
		{
			mObj = mObj1;
		}

		if(mObj == null)
		{
			mObj = Resources.Load("pbWGAlertView"+type);
		}

		if(type == 0)
		{
			mObj0 = mObj;
		}
		else if(type == 1)
		{
			mObj1 = mObj;
		}

		if(mObj != null)
		{
			GameObject go = Instantiate(mObj) as GameObject;
			WGAlertView av = go.GetComponent<WGAlertView>();
			av.type = type;
			return av;
		}
		return null;
	}

}
