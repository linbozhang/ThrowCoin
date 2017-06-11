using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum UI_FRESH{
	NONE =0,
	COIN =0x1,
	BCOIN = 0x10,
	EXP = 0x100,
	LEVEL =0x1000,
}
public enum BTN_ACT{
	NONE =0,
	ITEM = 1,
	SHOP = 2,
	SOCIAL = 3,
	OTHER = 4,
	Achievement = 5,
	HuaFei = 6,
	Sign=7,
	MONSTER=8,
	CLOSE = 10,

}
public enum VIEW_STATE{
	NONE = 0,
	ITEM = 1,
	SHOP = 2,
	SOCIAL = 3,
	OTHER = 4,
	Achievement = 5,
	HuaFei = 6,
	Sign=7,
	HELP = 10,
	BIG_TURE = 15,

}
public enum WG_TW_Type{
	NONE =0,
	show = 1,
	hidden = 2,
}
public class WGGameUIView : MonoBehaviour {


	public WGPlayerInfoView mPlayerInfoView;


	public WGMainMenuView mMainMenuView;
	public ParticleSystem imageEffect1;
	public GameObject wuqi;
	public GameObject leftHandle;

	public static WGGameUIView Instance;

	VIEW_STATE mTempViewState = VIEW_STATE.NONE;
	[HideInInspector]
	public VIEW_STATE mViewState = VIEW_STATE.NONE;
	MDBaseAlertView currentViewCtrl;

	void Awake(){
		Instance=this;
	}

	void Start()
	{

	}
	public void freshImageEffect(int coinNum){
		if(coinNum>=10000){
			int costCoin=System.Int32.Parse( wuqi.GetComponentInChildren<WGWeapon>().labCostCoin.text);
			if(costCoin<=50){
				imageEffect1.Play();
			}else if(costCoin==100&&coinNum>=50000){
				imageEffect1.Play();
			}else{
				imageEffect1.Stop();
			}


		}else{
			imageEffect1.Stop();
		}
	}

	public void freshPlayerUI(UI_FRESH ui)
	{
		mPlayerInfoView.FreshPlayerUI(ui);
	}
	public void freshSkillNum()
	{
		mMainMenuView.freshSkillMenu();
	}
	public void freshMenuButton(int type=0)
	{
		if(type==0){
			mMainMenuView.freshAchievement();
			mMainMenuView.freshItem();
			mMainMenuView.freshSign();
		}else if(type==1){
			mMainMenuView.freshAchievement();
		}else if(type==2){
			mMainMenuView.freshItem();
		}else if(type==3){
			mMainMenuView.freshSign();
		}

	}
	public void ViewControllerDoAct(BTN_ACT act,System.Action<MDAlertBehaviour,VIEW_STATE> myCallBack = null)
	{
		switch(act)
		{
		case BTN_ACT.ITEM:
			mViewState = VIEW_STATE.ITEM;
			currentViewCtrl = WGItemView.CreateItemView();
			break;
		case BTN_ACT.MONSTER:
			mViewState = VIEW_STATE.ITEM;
			currentViewCtrl = WGItemView.CreateItemView(true);
			break;
		case BTN_ACT.SHOP:
			mViewState = VIEW_STATE.SHOP;
			currentViewCtrl = WGShopView.CreateShopView();
			break;
		case BTN_ACT.Achievement:
			mViewState = VIEW_STATE.Achievement;
			currentViewCtrl = WGAchievementView.CreateAchievementView();
			break;
		case BTN_ACT.OTHER:
			mViewState = VIEW_STATE.OTHER;
			currentViewCtrl = WGHelpView.CreateHelpView();
			break;
		case BTN_ACT.HuaFei:
			mViewState = VIEW_STATE.HuaFei;
			currentViewCtrl = V2DuiHuaFeiView.CreateDuiHuaFeiView();
			break;
//		case BTN_ACT.Sign:
//			mViewState = VIEW_STATE.Sign;
//			currentViewCtrl = WGDailyRewardView.CreateDailyView();
//			break;
		}

		mTempViewState = mViewState;


		SDK.AddChild(currentViewCtrl.gameObject,WGRootManager.Self.goRootGameUI);

		currentViewCtrl.showView();

		currentViewCtrl.alertViewBehavriour =(ab,view)=>{
			if(myCallBack != null)
			{
				myCallBack(ab,mViewState);
			}
			freshMenuButton();
			switch(ab)
			{
			case MDAlertBehaviour.DID_SHOW:

				break;
			case MDAlertBehaviour.DID_HIDDEN:

				if(mTempViewState != VIEW_STATE.HuaFei)
				{
					if(YeHuoSDK.bShowLuckGift){ //close window lucky
						WGGameWorld.Instance.ShowLuckyDaLiBao();
					}

				}
				if(currentViewCtrl == view)
				{
					currentViewCtrl = null;
				}
				Destroy(view.gameObject);
				mTempViewState = VIEW_STATE.NONE;
				break;
			case MDAlertBehaviour.CLICK_OK:
				mViewState = VIEW_STATE.NONE;
				view.hiddenView();
				break;
			}
		};

	}
	public void CloseCurrentView()
	{
		mViewState = VIEW_STATE.NONE;
		currentViewCtrl.hiddenView();
	}


}
