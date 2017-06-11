using UnityEngine;
using System.Collections;

public class CAchievementCell : WGTableViewCell {

	public WGAchievementView curAchViewManager;

	public UISprite spGoalIcon;
	public UILabel labName;
	public UISlider sdProgress;
	public UISprite spRewardIcon;
	public UILabel labRewardNum;
	public UILabel labProgress;
	public UIButton btnGetReward;
	public GameObject goHaveGot;
	MDAchievement preAchievementData = null;
	// Use this for initialization
	void Start () {

	}

	public void freshUIWithData(MDAchievement ach)
	{


		preAchievementData = ach;

		WGDataController _dataCtrl = WGDataController.Instance;
		DataPlayerController dpc = DataPlayerController.getInstance();


		BCObj rwObj = _dataCtrl.GetBCObj(ach.reward[0]);

		labName.text = ach.name+":"+ach.des;

		spGoalIcon.spriteName = ach.icon;
		switch(rwObj.BCType)
		{
		case BCGameObjectType.Coin:
			spRewardIcon.spriteName = "coin_105";
			break;
		case BCGameObjectType.Jewel:
			spRewardIcon.spriteName = "gem_104";
			break;
		}
		spRewardIcon.MakePixelPerfect();

		labRewardNum.text =  WGStrings.getFormate(1066,ach.reward[1]);

		int got = 0;
		dpc.data.dicGotAchReward.TryGetValue(ach.id,out got);

		if(got==1)//到达了
		{
			btnGetReward.gameObject.SetActive(true);
			sdProgress.gameObject.SetActive(false);
			goHaveGot.SetActive(false);

		}
		else if(got == 0)//还没有到达
		{
			goHaveGot.SetActive(false);
			btnGetReward.gameObject.SetActive(false);
			sdProgress.gameObject.SetActive(true);

			int reachNum =WGAchievementManager.Self.getAchievementProgress(ach);

			labProgress.text = reachNum.ToString()+"/"+ach.goal_num.ToString();
			sdProgress.value = reachNum*1f/ach.goal_num;

		}
		else if(got == -1)//已经领取
		{
			goHaveGot.SetActive(true);
			btnGetReward.gameObject.SetActive(false);
			sdProgress.gameObject.SetActive(false);

		}


	}
	void OnBtnGetReward()
	{

		WGDataController _dataCtrl = WGDataController.Instance;
		DataPlayerController dpc = DataPlayerController.getInstance();

		if(dpc.data.dicGotAchReward[preAchievementData.id] != 1)
		{
			WGAlertViewController.Self.showAlertView(1016).alertViewBehavriour =(ab,view)=>{
				switch(ab)
				{
				case MDAlertBehaviour.CLICK_OK:
					view.hiddenView();
					break;
				case MDAlertBehaviour.DID_HIDDEN:
					WGAlertViewController.Self.hiddeAlertView(view.gameObject);
					break;
				}
			};
			return;
		}

		int objID = preAchievementData.reward[0];
		int gotNum =preAchievementData.reward[1];

		BCObj obj = _dataCtrl.GetBCObj(objID);

		switch(obj.BCType)
		{
		case BCGameObjectType.Item:
			dpc.AddSkillNum(objID,gotNum);
			WGGameUIView.Instance.freshSkillNum();
			break;
		case BCGameObjectType.Collection:
			dpc.addCollectionNum(objID,gotNum);
			break;
		case BCGameObjectType.Coin:
			dpc.data.Coin +=gotNum;
			WGGameUIView.Instance.freshPlayerUI(UI_FRESH.COIN);
			break;
		case BCGameObjectType.Jewel:
			dpc.data.Jewel +=gotNum;
#if TalkingData
			TDGAVirtualCurrency.OnReward(gotNum,"AchievementReward");
#endif
#if Umeng
			Umeng.GA.Bonus(gotNum,Umeng.GA.BonusSource.Source4);
#endif
			WGGameUIView.Instance.freshPlayerUI(UI_FRESH.BCOIN);
			break;
		}

		dpc.data.dicGotAchReward[preAchievementData.id]=-1;

//		WGAlertViewController.Self.showAlertView(1015).alertViewBehavriour =(ab,view)=>{
//			switch(ab)
//			{
//			case MDAlertBehaviour.CLICK_OK:
//				view.hiddenView();
//				if(curAchViewManager != null)
//				{
//					curAchViewManager.ReloadAchievement();
//				}
//				break;
//			case MDAlertBehaviour.DID_HIDDEN:
//				WGAlertViewController.Self.hiddeAlertView(view.gameObject);
//				break;
//			}
//		};
		if(curAchViewManager != null)
		{
			curAchViewManager.ReloadAchievement();
		}
	}

	static Object mObj = null;
	public static CAchievementCell CreateAchievementCell()
	{
		if(mObj == null)
		{
			mObj = Resources.Load("pbAchievementCell");
		}
		if(mObj != null)
		{
			GameObject go = Instantiate(mObj) as GameObject;
			CAchievementCell ac = go.GetComponent<CAchievementCell>();
			return ac;
		}
		return null;
	}
}
