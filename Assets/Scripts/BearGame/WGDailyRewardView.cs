using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
public class WGDailyRewardView : MDBaseAlertView {

	public TweenScale tsContent;
	public UILabel labDoubleOkTitle;
	public UILabel labPayTip;
	public UILabel labOkTitle1;
	public UIButton buttonOK;
	public List<GameObject> szCellPanel;

	List<MDDailyReward> szDaily7Reward = new List<MDDailyReward>();

	MDDailyReward curReward = null;

	void InitDailyView(bool signed)
	{
		if(signed){
			buttonOK.isEnabled=false;
			buttonOK.GetComponentInChildren<UILabel>().text="已领取";
			buttonOK.GetComponentInChildren<UISprite>().spriteName="v3Button_gray";
			buttonOK.transform.parent.Find("Button_ok_1").GetComponentInChildren<UISprite>().spriteName="v2Button_yellowred";
		}else{
			buttonOK.GetComponentInChildren<UILabel>().text="领取";
			buttonOK.transform.parent.Find("Button_ok_1").GetComponentInChildren<UISprite>().spriteName="v3Button_gray";
			buttonOK.GetComponentInChildren<UISprite>().spriteName="v2Button_yellowred";
			buttonOK.isEnabled=true;
		}
		TextAsset ta = Resources.Load(WGConfig.Path_Daily)as TextAsset;
		using(StreamReader sr = new StreamReader(new MemoryStream(ta.bytes)))
		{
			MDDailyReward dr = null;
			string line ="";
			while(!string.IsNullOrEmpty(line = sr.ReadLine()))
			{
				dr = SDK.Deserialize<MDDailyReward>(line);
				szDaily7Reward.Add(dr);
			}
		}
		Resources.UnloadAsset(ta);
		labPayTip.ESetActive(false);
//		WGDataController _dataCtrl = WGDataController.Instance;
//		YHMDPayData payData=_dataCtrl.getYHMDPay(YHPayType.DOUBLE_reward);
//		float costMenoy=payData.payCost;
//		string payKey=payData.payKey.ToString();
////		float costMenoy = 30;
////		string payKey = "114";
////		if(YeHuoSDK.bUsePayCode2)
////		{
////			costMenoy = 20;
////			payKey = "214";
////		}
//		string okString = "ok";
//
//
//
//		#if YES_OK
//		string content = WGStrings.getFormateInt(1081,1104,"",costMenoy.ToString());
//		okString = WGStrings.getText(1104);
//		#elif YES_BUY
//		string content = WGStrings.getFormateInt(1081,1104,"",costMenoy.ToString());
//		okString = WGStrings.getText(1104);
//#elif YES_GET
//		string content = WGStrings.getFormateInt(1081,1101,"",costMenoy.ToString());
//		okString = WGStrings.getText(1101);
//		#elif YES_QueRen
//		string content = WGStrings.getFormateInt(1081,1104,"",costMenoy.ToString());
//		okString = WGStrings.getText(1104);
//		#else
//		string content = WGStrings.getFormateInt(1081,1101,"",costMenoy.ToString());
//		okString = WGStrings.getText(1101);
//		#endif
//		labPayTip.text = content;
//		labDoubleOkTitle.text = okString;
//		if(YeHuoSDK.mTipType != 0)
//		{
//			labPayTip.ESetActive(false);
//		}

	}

	public void freshWithDailyReward(int daily,bool signed)
	{
		for(int i=0,max=szDaily7Reward.Count;i<max;i++)
		{
			CDailyCellView cell = CDailyCellView.CreateCellView(i+1);

			cell.freshDailyCell(szDaily7Reward[i],i+1,daily+1);

			SDK.AddChild(cell.gameObject,szCellPanel[i]);

		}


		if(signed){
			curReward = szDaily7Reward[daily-1];
		}else{
			curReward = szDaily7Reward[daily];
		}


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


	public override void OnBtnOk ()
	{
		BCSoundPlayer.Play(MusicEnum.button);
		base.OnBtnOk ();

	}
	void GetReward(bool bDouble)
	{
		DataPlayerController dpc = DataPlayerController.getInstance();
		DataPlayer _dp = dpc.data;
		BCGameObjectType type = WGDataController.Instance.GetBCObjType(curReward.reward);
		BCObj obj = WGDataController.Instance.GetBCObj(curReward.reward);
		switch(type)
		{
		case BCGameObjectType.Coin:
			_dp.Coin =_dp.Coin +curReward.got_num;
			if(bDouble)
			{
				_dp.Coin =_dp.Coin +curReward.got_num;
			}
			break;
		case BCGameObjectType.Collection:
			dpc.addCollectionNum(curReward.reward,curReward.got_num);
			if(bDouble)
			{
				dpc.addCollectionNum(curReward.reward,curReward.got_num);
			}
			break;
		case BCGameObjectType.Item:
			dpc.AddSkillNum(curReward.reward,curReward.got_num);
			if(bDouble)
			{
				dpc.AddSkillNum(curReward.reward,curReward.got_num);
			}
			break;
		case BCGameObjectType.Jewel:
			_dp.Jewel += curReward.got_num;
			if(bDouble)
			{
				_dp.Jewel += curReward.got_num;
			}
			#if TalkingData
			TDGAVirtualCurrency.OnReward(curReward.got_num,"DailyReward");
			#endif
			#if Umeng
			Umeng.GA.Bonus(curReward.got_num,Umeng.GA.BonusSource.Source2);
			#endif
			break;
		}
		
		_dp.lastDailyRewardTime = Core.now;
		
		dpc.saveDataPlayer();
		
		WGGameUIView.Instance.freshPlayerUI(UI_FRESH.BCOIN|UI_FRESH.COIN);

	}
	public void OnBtnOk1()
	{

		BCSoundPlayer.Play(MusicEnum.button);
		base.hiddenViewEnd();
		if(YeHuoSDK.bShowFinger){ //close window lucky
			WGGameWorld.Instance.ShowFingerGift();
		}
		//base.OnBtnOkWithIndex(CLICK_OK1);
		//GetReward(false);
	}
	public void OnBtnOk2()
	{
		BCSoundPlayer.Play(MusicEnum.button);
		base.OnBtnOkWithIndex(CLICK_OK2);

		GetReward(false);
//		WGDataController _dataCtrl = WGDataController.Instance;
//		YHMDPayData payData=_dataCtrl.getYHMDPay(YHPayType.DOUBLE_reward);
//		float costMenoy=payData.payCost;
//		string payKey=payData.payKey.ToString();
////		float costMenoy = 30;
////		string payKey = "114";
////		if(YeHuoSDK.bUsePayCode2)
////		{
////			costMenoy = 20;
////			payKey = "214";
////		}
//		YeHuoSDK.YHPay(payKey,costMenoy,0,(success)=>{
//			if(success)
//			{
//				GetReward(true);
//			}
//			else
//			{
//				GetReward(false);
//			}
//		});

	}

	public static WGDailyRewardView CreateDailyView(bool signed=false)
	{
		Object obj = Resources.Load("pbWGDailyRewardView");

		GameObject go = Instantiate(obj) as GameObject;

		WGDailyRewardView drv = go.GetComponent<WGDailyRewardView>();

		SDK.AddChild(go,WGRootManager.Self.goRootTopUI);
		drv.InitDailyView(signed);

		return drv;

	}

}
