using UnityEngine;
using System.Collections;

public class D0330SuperTiger : MDBaseAlertView {

	public GameObject goBtnBuyCoin;
	public GameObject goBtnBuyJewel;
	public GameObject goBtnBuyRich;
	public UISprite spBackgroud;
	public UILabel labCostCoin;
	public UILabel labCostRMB;

	public TweenScale tsContent;

	public D0330TigerPanel mTiger;

	DataPlayer dp{
		get{
			return DataPlayerController.getInstance().data;
		}
	}

	// Use this for initialization
	void Start () {
	
	}
	bool bInit=false;
	void initView()
	{
		if(bInit)return;
		bInit = true;

		if(YeHuoSDK.bUsePayCode2)
		{
			spBackgroud.height = 700;
			goBtnBuyCoin.SetActive(false);
			goBtnBuyJewel.SetActive(false);
			goBtnBuyRich.SetActive(false);

			labCostRMB.text = "20 RMB";
		}
		else
		{
			labCostRMB.text = "30 RMB";
			spBackgroud.height = 830;
//			goBtnBuyCoin.SetActive(true);
//			goBtnBuyJewel.SetActive(true);
//			goBtnBuyRich.SetActive(true);
		}
		int coin1 = Mathf.Min(20000, dp.cj*1000+1000);
		labCostCoin.text = coin1.ToString();

		mTiger = D0330TigerPanel.CreateTigerPanel();
		mTiger.tigerCallBack  = myTigerCallBack;
		mTiger.ESetActive(false);
	}
	void myTigerCallBack(int st,MDTiger tiger)
	{
		if(st == 1)
		{
			mTiger.ESetActive(false);
			showReward(tiger);
		}
		else if(st == 0)
		{
			
		}
	}
	void showReward(MDTiger tiger)
	{
		YHMDPayData payData=WGDataController.Instance.getYHMDPay(YHPayType.DOUBLE_reward);
		float costMenoy=payData.payCost;
		string payKey=payData.payKey.ToString();
//		float costMenoy = 30f;
//		string payKey = "114";
//		if(YeHuoSDK.bUsePayCode2)
//		{
//			costMenoy = 20f;
//			payKey = "214";
//		}
		YHGotRewardView rdview = YHGotRewardView.CreateGotRewardView();
		rdview.mRType = YHRewardType.SuperTiger;
		rdview.bDoubleReward=true;

		SDK.AddChild(rdview.gameObject,WGRootManager.Self.goRootTopUI);

		rdview.FreshRewardCell(tiger.reward);
		#if YES_OK
		string content = WGStrings.getFormateInt(1081,1104,"",costMenoy.ToString());
		#elif YES_BUY
		string content = WGStrings.getFormateInt(1081,1104,"",costMenoy.ToString());
#elif YES_GET
		string content = WGStrings.getFormateInt(1081,1101,"",costMenoy.ToString());
#elif YES_QueRen
		string content = WGStrings.getFormateInt(1081,1104,"",costMenoy.ToString());
		#else
		string content = WGStrings.getFormateInt(1081,1101,"",costMenoy.ToString());
		#endif
		rdview.FreshWithMsg(WGStrings.getText(1100),content,false,false);
		rdview.alertViewBehavriour =(ab,view)=>{
		switch(ab)
		{
		case MDAlertBehaviour.CLICK_OK:
			if(view.clickIndex == MDBaseAlertView.CLICK_OK1)
			{
				view.hiddenView();
				rdview.GetAllReward(false);
				WGGameUIView.Instance.freshSkillNum();
				WGGameUIView.Instance.freshPlayerUI(UI_FRESH.BCOIN | UI_FRESH.COIN);
			}
			else if(view.clickIndex == MDBaseAlertView.CLICK_OK2)
			{
				YeHuoSDK.YHPay(payKey,costMenoy,0,(success)=>{
					view.hiddenView();
					if(success)
					{
						rdview.GetAllReward(true);
						WGGameUIView.Instance.freshSkillNum();
						WGGameUIView.Instance.freshPlayerUI(UI_FRESH.BCOIN | UI_FRESH.COIN);
					}
				});

			}
		break;
		case MDAlertBehaviour.DID_HIDDEN:
			mTiger.ESetActive(true);
			Destroy(view.gameObject);

		break;
		}
		};

		rdview.showView();
		BCSoundPlayer.Play(MusicEnum.showReward,1f);


	}

	void OnBtnTigerWithCoin()
	{
		int coin = Mathf.Min(20000, dp.cj*1000+1000);
		if(dp.Coin<coin)
		{

		}
		else
		{
			WGGameWorld.Instance.PlayerGetCoin(-coin);
			dp.cj++;
			mTiger.StartTiger();
			int coin1 = Mathf.Min(20000, dp.cj*1000+1000);
			labCostCoin.text = coin1.ToString();
		}

	}
	void OnBtnTigerWithJewel()
	{
		if(dp.Jewel<12)
		{
			WGAlertViewController.Self.showTipView(WGStrings.getText(9010));
			//Debug.LogError("钻石不足");
		}
		else
		{
			dp.Jewel -=12;
			mTiger.StartTiger();
		}
	}
	void OnBtnTigerWithRMB()
	{
		YHMDPayData payData=WGDataController.Instance.getYHMDPay(YHPayType.TIGER10);
		float costMenoy=payData.payCost;
		string payKey=payData.payKey.ToString();
//		float costMenoy = 30;
//		string payKey = "113";
//		if(YeHuoSDK.bUsePayCode2)
//		{
//			costMenoy = 20;
//			payKey = "213";
//		}

		string okString ="ok";
		
		#if YES_OK
		string content = WGStrings.getFormateInt(1081,1002,8208,costMenoy.ToString());
		okString =  WGStrings.getText(1002);
		#elif YES_BUY
		string content = WGStrings.getFormateInt(1081,1094,8208,costMenoy.ToString());
		okString =  WGStrings.getText(1094);
#elif YES_GET
		string content = WGStrings.getFormateInt(1081,1077,8208,costMenoy.ToString());
		okString =  WGStrings.getText(1077);
#elif YES_QueRen
		string content = WGStrings.getFormateInt(1081,1106,payData.showText,costMenoy.ToString());
		okString =  WGStrings.getText(1106);
		#else
		string content = WGStrings.getFormateInt(1081,1077,payData.showText,costMenoy.ToString());
		okString =  WGStrings.getText(1077);
		#endif

		mTiger.ESetActive(false);
		D04Buy10TigerView tView = D04Buy10TigerView.CreateBuy10TigerView();
		tView.FreshUI(payData.showText,content,okString);

		tView.alertViewBehavriour = (ab,view) =>{
			switch(ab)
			{
			case MDAlertBehaviour.CLICK_OK:
				YeHuoSDK.YHPay(payKey,costMenoy,0,(success)=>{
					view.hiddenView();
					if(success)
					{
						mTiger.StartTiger(10);
					}
				});
				break;
			case MDAlertBehaviour.CLICK_CANCEL:
				view.hiddenView();
				break;
			case MDAlertBehaviour.DID_HIDDEN:
				Destroy(view.gameObject);
				mTiger.ESetActive(true);
				break;
			}
		};
		tView.showView();


	}
	void OnBtnBuyCoin()
	{
		mTiger.ESetActive(false);
		WGGameWorld.Instance.ShowCoinSupplementView((ab)=>{
			switch(ab)
			{
			case MDAlertBehaviour.DID_HIDDEN:
				mTiger.ESetActive(true);
				break;
			}
		});
	}
	void OnBtnBuyJewel()
	{
		mTiger.ESetActive(false);
		WGGameWorld.Instance.ShowJewelSupplementView((ab)=>{
			switch(ab)
			{
			case MDAlertBehaviour.DID_HIDDEN:
				mTiger.ESetActive(true);
				break;
			}
		});
	}
	void OnBtnBuyTuHao()
	{
		mTiger.ESetActive(false);
		WGGameWorld.Instance.ShowRichReward((ab)=>{
			switch(ab)
			{
			case MDAlertBehaviour.DID_HIDDEN:
				mTiger.ESetActive(true);
				break;
			}
		});
	}




	#region MDBaseAlertView

	public override void OnBtnOk ()
	{
		base.OnBtnOk ();
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
		mTiger.ESetActive(true);
	}
	public override void hiddenView ()
	{

		base.hiddenView ();
		Destroy(mTiger.gameObject);
		tsContent.PlayReverse();
		InvokeBlock(tsContent.duration,()=>{
			hiddenViewEnd();
		});
	}
	public override void hiddenViewEnd ()
	{
		base.hiddenViewEnd ();
	}
	#endregion


	public static D0330SuperTiger CreateSuperTiger()
	{
		Object obj = Resources.Load("pbD0330SuperTiger");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			D0330SuperTiger d = go.GetComponent<D0330SuperTiger>();
			d.initView();
			return d;
		}
		return null;
	}

}
