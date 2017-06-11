using UnityEngine;
using System.Collections;

public class V2NewUseRewardView : MDBaseAlertView {

	public TweenScale tsContent;
	public UILabel labTitle;
	public UILabel labContentDes;
	public UILabel labPayDes;
	public UILabel labBtnOk;
	public UILabel[] szLabNames;
	public UILabel[] szlabAttributes;

	GameObject goThreeRole;
	bool bInitView = false;
	// Use this for initialization
	void Start () {
		InitView();
	}

	void InitView()
	{
		if(bInitView)return;
		bInitView = true;


		labTitle.text = WGStrings.getText(8207);
		labContentDes.text = WGStrings.getText(1103);


	}
	public void freshUI(string payDes, string ok)
	{
		labPayDes.text = payDes;
		labBtnOk.text = ok;
	}
	void OnBtnKefu()
	{
		goThreeRole.ESetActive(false);
		D04CustomerServiceView cs = D04CustomerServiceView.CreateCSView();
		cs.alertViewBehavriour =(ab,view)=>{
			switch(ab)
			{
			case MDAlertBehaviour.CLICK_OK:
				view.hiddenView();
				break;
			case MDAlertBehaviour.DID_HIDDEN:
				Destroy(view.gameObject);
				goThreeRole.ESetActive(true);
				break;
			}
		};
		cs.showView();
	}

	#region MDBaseAlertView

	public override void showView ()
	{
		base.showView ();
		Time.timeScale = 0;
		tsContent.ESetActive(true);
		tsContent.PlayForward();

		InvokeBlock(tsContent.duration,()=>{
			showViewEnd();
		});
	}
	public override void showViewEnd ()
	{
		base.showViewEnd ();
		Object obj = Resources.Load("pbSelectRolePanel1");
		goThreeRole = Instantiate(obj) as GameObject;
		goThreeRole.transform.position = new Vector3(100,0,0);
	}
	public override void hiddenView ()
	{
		Time.timeScale = 1;
		base.hiddenView ();
		tsContent.PlayReverse();
		Destroy(goThreeRole);
		InvokeBlock(tsContent.duration,()=>{
			hiddenViewEnd();
		});
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
	

	public static V2NewUseRewardView CreateNewUseView()
	{
		Object obj = Resources.Load("pbV2NewUseReward");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			V2NewUseRewardView c = go.GetComponent<V2NewUseRewardView>();
			SDK.AddChild(go,WGRootManager.Self.goRootTopUI);
			c.InitView();
			return c;
		}
		return null;
	}
}
