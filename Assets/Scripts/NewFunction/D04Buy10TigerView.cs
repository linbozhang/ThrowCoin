using UnityEngine;
using System.Collections;

public class D04Buy10TigerView : MDBaseAlertView {
	public TweenScale tsContent;
	
	public UILabel labTitle;
	
	public UILabel labContent;

	public UILabel labButtonOkText;
	// Use this for initialization
	void Start () {

	}

	public void FreshUI(string title,string content,string ok)
	{
		labTitle.text = title;
		labContent.text = content;
		labButtonOkText.text=ok;

		if(YeHuoSDK.mTipType != 0 )
		{
			labContent.ESetActive(false);
		}
	}

	public override void showView ()
	{
		base.showView ();
		tsContent.gameObject.SetActive(true);
		tsContent.transform.localScale = Vector3.one*0.4f;
		tsContent.PlayForward();
		
		mnIvokeBlock.InvokeBlock(tsContent.duration,()=>{
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
	public static D04Buy10TigerView CreateBuy10TigerView()
	{
		Object obj = Resources.Load("pbD04Buy10TigerView");
		
		GameObject go = Instantiate(obj) as GameObject;

		SDK.AddChild(go,WGRootManager.Self.goRootTopUI);

		D04Buy10TigerView drv = go.GetComponent<D04Buy10TigerView>();
		
		return drv;
		
	}
}
