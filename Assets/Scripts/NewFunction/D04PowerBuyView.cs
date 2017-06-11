using UnityEngine;
using System.Collections;

public class D04PowerBuyView : MDBaseAlertView {

	public TweenScale tsContent;
	public UILabel labTitle;
	public UILabel labPayTip;
	public UILabel labDes1;
	public UILabel labDes2;
	public UILabel labOkTitle;
	bool bInit=false;
	void initView()
	{
		if(bInit) return;
		bInit = true;


	}

	public void FreshUI(string payTip,string ok,bool isFinger=false)
	{
		if(isFinger){
			labTitle.text = WGStrings.getText(9009);
		}else{
			labTitle.text = WGStrings.getText(8209);
		}

		labPayTip.text = payTip;
		labOkTitle.text = ok;
	}

	void OnBtnKefu()
	{
		D04CustomerServiceView cs = D04CustomerServiceView.CreateCSView();
		cs.alertViewBehavriour =(ab,view)=>{
			switch(ab)
			{
			case MDAlertBehaviour.CLICK_OK:
				view.hiddenView();
				break;
			case MDAlertBehaviour.DID_HIDDEN:
				Destroy(view.gameObject);
				break;
			}
		};
		cs.showView();
	}

	#region MDBaseAlertView

	public override void showView ()
	{
		base.showView ();
		tsContent.ESetActive(true);
		tsContent.PlayForward();
		InvokeBlock(tsContent.duration,()=>{
			showViewEnd();
		});
	}
	public override void hiddenView ()
	{
		base.hiddenView ();
		tsContent.PlayReverse();
		InvokeBlock(tsContent.duration,()=>{
			hiddenViewEnd();
		});
	}
	public override void showViewEnd ()
	{
		base.showViewEnd ();
	}
	public override void hiddenViewEnd ()
	{
		base.hiddenViewEnd ();
	}
	public override void OnBtnOk ()
	{
		base.OnBtnOk ();
	}
	public override void OnBtnCancel ()
	{
		base.OnBtnCancel ();
	}

	#endregion


	public static D04PowerBuyView CreatePowerBuyView(bool isFinger=false)
	{
		string prefabName="pbD04PowerBuyView";
		if(isFinger){
			prefabName="pbD04FingerBuyView";
		}
		Object obj = Resources.Load(prefabName);
		if(obj != null)
		{
			GameObject go = Instantiate(obj)as GameObject;
			D04PowerBuyView v =go.GetComponent<D04PowerBuyView>();
			SDK.AddChild(go,WGRootManager.Self.goRootGameUI);
			return v;
		}
		return null;
	}
}
