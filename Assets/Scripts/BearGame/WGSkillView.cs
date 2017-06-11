using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WGSkillView : WGMonoComptent {

	public UISprite spIcon;
	public UISprite spBG;

	public UILabel labNum;
	public UISprite spIconEffect;
	public System.Action<int> myReleaseSkill;


	public void hightLight(){
		GetComponentInChildren<UIButton>().enabled=false;
		spIcon.depth+=100;
		spIcon.ESetActive(false);
		spIcon.ESetActive(true);
		spBG.depth+=100;
		spBG.ESetActive(false);
		spBG.ESetActive(true);
	}
	public void unlight(){
		GetComponentInChildren<UIButton>().enabled=true;
		spIcon.depth-=100;
		spBG.depth-=100;
		spIcon.ESetActive(false);
		spIcon.ESetActive(true);
		spBG.ESetActive(false);
		spBG.ESetActive(true);
	}
	DataPlayer _dataPlayer{
		get{
			return DataPlayerController.getInstance().data;
		}
	}
	WGDataController _dataCtrl;
	int mSkillNum = 0;
	int mSkillID = 0;
	bool bCanRelease = true;
	void initView()
	{
		_dataCtrl = WGDataController.Instance;
	}

	public void freshSkillView(int id)
	{


		mSkillID = id;
		reFreshSkill();

		spIcon.spriteName = "sb1"+id.ToString();

		spIconEffect.ESetActive(false);

	}
	public void reFreshSkill()
	{
		mSkillNum = DataPlayerController.getInstance().getSkillNum(mSkillID);
		labNum.text = mSkillNum.ToString();
		if(mSkillNum>0)labNum.color = Color.white;
		else labNum.color = Color.red;
	}
	float beginTime = 0;
	void OnBtnRelease()
	{

		if(mSkillNum>0)
		{
			if(_dataPlayer.defenseTime>0 && mSkillID == WGDefine.SK_FangYu4) return;
			if(bCanRelease)
			{
				beginTime = Time.realtimeSinceStartup;
				bCanRelease = false;
				spIconEffect.ESetActive(true);
				spIconEffect.fillAmount = 1;

				if(myReleaseSkill != null)
				{
#if Umeng
					Umeng.GA.Use(mSkillID.ToString(),1,10);
#endif
					#if TalkingData
					MDSkill sk = WGDataController.Instance.getSkill(mSkillID);
					TDGAItem.OnUse(sk.name,1);
					Dictionary<string, object> dic = new Dictionary<string, object>();
					dic.Add("name",sk.name);
					TalkingDataGA.OnEvent("使用道具",dic);
					#endif
					myReleaseSkill(mSkillID);
				}
				mSkillNum--;
				labNum.text = mSkillNum.ToString();
				if(mSkillNum>0)labNum.color = Color.white;
				else labNum.color = Color.red;

				DataPlayerController.getInstance().setSkillNum(mSkillID,mSkillNum);
			}
		}
		else
		{
			if(!WGAlertManager.Self.bBuySKill)
			{
				WGAlertManager.Self.bBuySKill = true;
				WGAlertManager.Self.AddAction(()=>{
					YHMDPayData payData=WGDataController.Instance.getYHMDPay(YHPayType.ITEM);
					float costMenoy=payData.payCost;
					string payKey=payData.payKey.ToString();
//					float costMenoy = 30;
//					string payKey = "106";
//					if(YeHuoSDK.bUsePayCode2)
//					{
//						costMenoy = 20;
//						payKey = "206";
//					}
					YHGotRewardView rdview = YHGotRewardView.CreateGotRewardView();
					rdview.mRType = YHRewardType.Item;
					SDK.AddChild(rdview.gameObject,WGRootManager.Self.goRootTopUI);
					rdview.FreshRewardCell(_dataCtrl.mAllReward.item);
					#if YES_OK
					string content = WGStrings.getFormateInt(1081,1002,1088,costMenoy.ToString());
					#elif YES_BUY
					string content = WGStrings.getFormateInt(1081,1094,1088,costMenoy.ToString());
#elif YES_GET
					string content = WGStrings.getFormateInt(1081,1077,1088,costMenoy.ToString());
#elif YES_QueRen
					string content = WGStrings.getFormateInt(1081,1106,payData.showText,costMenoy.ToString());
					#else
					string content = WGStrings.getFormateInt(1081,1077,payData.showText,costMenoy.ToString());
					#endif
					rdview.FreshWithMsg(WGStrings.getText(1088),content,true);
					rdview.alertViewBehavriour =(ab,view)=>{
						switch(ab)
						{
						case MDAlertBehaviour.CLICK_OK:
							YeHuoSDK.YHPay(payKey,costMenoy,0,(succecc)=>{
								view.hiddenView();
								if(succecc)
								{
									rdview.GetAllReward();
									WGGameUIView.Instance.freshSkillNum();
									WGGameUIView.Instance.freshPlayerUI(UI_FRESH.BCOIN | UI_FRESH.COIN);
								}
							});
							break;
						case MDAlertBehaviour.CLICK_CANCEL:
							view.hiddenView();
							break;
						case MDAlertBehaviour.DID_HIDDEN:
							Destroy(view.gameObject);
							WGAlertManager.Self.bBuySKill = false;
							WGAlertManager.Self.RemoveHead();
							WGAlertManager.Self.ShowNext();
							Time.timeScale=1;

							break;
						}
					};
					Time.timeScale=0;
					rdview.showView();
					BCSoundPlayer.Play(MusicEnum.showReward,1f);
				});
				
				WGAlertManager.Self.ShowNext();
			}


		}
	}

	void Update()
	{
		if(!bCanRelease)
		{
			float t = Time.realtimeSinceStartup - beginTime;
			if(t<2)
			{
				spIconEffect.fillAmount = (2-t)/2;
			}
			else
			{
				spIconEffect.ESetActive(false);
				bCanRelease = true;
			}
		}
	}

	static Object mObj = null;
	public static WGSkillView CreateSkillView()
	{
		if(mObj == null)
		{
			mObj = Resources.Load("pbWGSkillView");
		}
		GameObject go = Instantiate(mObj) as GameObject;

		WGSkillView sk = go.GetComponent<WGSkillView>();
		sk.initView();
		return sk;
	}
}
