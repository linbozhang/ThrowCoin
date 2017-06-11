using UnityEngine;
using System.Collections;

public class WGTipAlertView : MDBaseAlertView {

	public UILabel labTitle;
	public TweenAlpha[] szTweenAlpha;
	public TweenPosition tpContent;
	public TweenScale tsContent;
	public GameObject Icon;

	public void freshUI(string title)
	{
		labTitle.text = title;
	}

	public override void showView ()
	{
		tsContent.ResetToBeginning();
		tsContent.PlayForward();
		base.showView ();
		InvokeBlock(0.7f,()=>{
			tpContent.enabled = true;
			for(int i=0;i<szTweenAlpha.Length;i++)
			{
				szTweenAlpha[i].enabled = true;
			}
			this.showViewEnd();
			InvokeBlock(tpContent.duration,()=>{
				this.hiddenViewEnd();
			});
		});

	}
	public override void hiddenViewEnd ()
	{
		base.hiddenViewEnd ();
		Destroy(this.gameObject);
	}

	public static WGTipAlertView CreateTipView()
	{
		Object obj = Resources.Load("pbWGTipAlertView");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			WGTipAlertView tp = go.GetComponent<WGTipAlertView>();
			return tp;
		}
		return null;
	}
	public static WGTipAlertView CreateArchivementView(){
		Object obj = Resources.Load("pbWGArchivementTipAlertView");
		if(obj!= null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			WGTipAlertView tp = go.GetComponent<WGTipAlertView>();
			return tp;
		}
		return null;
	}
}
