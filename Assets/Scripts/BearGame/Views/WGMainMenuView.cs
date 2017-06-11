using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WGMainMenuView : MonoBehaviour {

	public List<GameObject> szSkillsPanel;

	public UISprite spAchievementComplet;
	public UISprite spSignComplet;
	public UISprite spItemComplet;

	public UIButton btnChaoZhi;

	public GameObject goSuperTiger;

	public GameObject goHuaFei;

	public UISprite spBackground;


	List<WGSkillView> szSkills = new List<WGSkillView>();
	WGSkillView guDingSkView = null;

	private DataPlayer _DataPlayer{
		get{
			return DataPlayerController.getInstance().data;
		}
	}
	bool bShowButton = true;
	// Use this for initialization
	void Start () {
		DataPlayer _dataPlayer = DataPlayerController.getInstance().data;

		int skID=0;
		for(int i=0;i<Core.fc.szReleaseSkill.Length;i++)
		{
			skID = Core.fc.szReleaseSkill[i];
			WGSkillView sk = WGSkillView.CreateSkillView();
			sk.freshSkillView(skID);
			szSkills.Add(sk);
			SDK.AddChild(sk.gameObject,szSkillsPanel[i]);

			sk.myReleaseSkill +=this.myReleaseSkillWithID;

		}
		goSuperTiger.ESetActive(YeHuoSDK.bTiger_HuaFei);
		goHuaFei.ESetActive(false);

		//spBackground.height = YeHuoSDK.bTiger_HuaFei?536:437;

		if(YeHuoSDK.bUsePayCode2)
		{
			btnChaoZhi.ESetActive(false);
		}
		else
		{
			if(_DataPlayer.szBigReward.Contains(4))
			{
				btnChaoZhi.ESetActive(false);	
			}
			else
			{
				btnChaoZhi.ESetActive(true);
			}
		}
		WGGameUIView.Instance.freshMenuButton();

	}
	public void freshSkillMenu()
	{
		for(int i = 0;i<szSkills.Count;i++)
		{
			szSkills[i].reFreshSkill();
		}
	}
	public void freshAchievement()
	{
		spAchievementComplet.ESetActive(WGAchievementManager.Self.bCompleteAchievement);
	}
	public void freshSign(){
		int curday=Core.now.DayOfYear;
		int lastDay=_DataPlayer.lastDailyRewardTime.DayOfYear;
		if(curday==lastDay){
			spSignComplet.ESetActive(false);
			spSignComplet.transform.parent.Find("Background").GetComponent<Animation>().Stop();
		}else{
			spSignComplet.ESetActive(true);
		}

	}
	public void freshItem(){
		WGDataController _dataCtrl = WGDataController.Instance;
		
		
		Dictionary<int,List<int>> dicCollections= new Dictionary<int, List<int>>();
		
		for(int i=0;i<_dataCtrl.szCollectionObj.Count;i++)
		{
			
			BCObj obj = _dataCtrl.szCollectionObj[i];
			BCCollectionInfo col = _dataCtrl.GetCollectionInfo(obj.ID);
			List<int> szTemp;
			if(dicCollections.TryGetValue(col.groupID,out szTemp))
			{
				szTemp.Add(obj.ID);
			}
			else
			{
				szTemp = new List<int>();
				szTemp.Add(obj.ID);
				dicCollections.Add(col.groupID,szTemp);
			}
		}
		int index =0;
		bool showTips=false;
		foreach(KeyValuePair<int,List<int>> kvp in dicCollections)
		{

			int [] ids=kvp.Value.ToArray();
			if(ids.Length>=3){
				int left= _dataCtrl.GetCollectionOwnNum(ids[0]);
				int center= _dataCtrl.GetCollectionOwnNum(ids[1]);
				int right= _dataCtrl.GetCollectionOwnNum(ids[2]);
				if(Mathf.Min(left,center,right)>0){
					showTips=true;
					break;
				}
			}
		}
		spItemComplet.ESetActive(showTips);
	}

	void OnBtnItem()
	{
		WGGameUIView.Instance.ViewControllerDoAct(BTN_ACT.ITEM);
	}
	void OnBtnMonster(){
		WGGameUIView.Instance.ViewControllerDoAct(BTN_ACT.MONSTER);
	}
	void OnBtnShop()
	{
		WGGameUIView.Instance.ViewControllerDoAct(BTN_ACT.SHOP);
	}
	void OnBtnAchievement()
	{
		WGGameUIView.Instance.ViewControllerDoAct(BTN_ACT.Achievement);

	}
	void OnBtnSign(){
		//WGGameUIView.Instance.ViewControllerDoAct(BTN_ACT.Sign);


		int curday=Core.now.DayOfYear;
		int lastDay=_DataPlayer.lastDailyRewardTime.DayOfYear;
		bool signed=false;
		if(curday==lastDay){
			signed=true;
		}else{
			signed=false;
		}
		WGDailyRewardView rewardView= WGDailyRewardView.CreateDailyView(signed);
		if(curday >lastDay+1)
		{
			_DataPlayer.ContinuousDay = 0;
			DataPlayerController.getInstance().saveDataPlayer();
		}
		if(signed){
			rewardView.freshWithDailyReward(_DataPlayer.ContinuousDay,signed);
		}else{
			if(_DataPlayer.ContinuousDay>=7){
				_DataPlayer.ContinuousDay=0;
				DataPlayerController.getInstance().saveDataPlayer();
			}
			rewardView.freshWithDailyReward(_DataPlayer.ContinuousDay,signed);
		}


		rewardView.alertViewBehavriour =(ab,view)=>{
			freshSign();
			switch(ab)
			{
			case MDAlertBehaviour.CLICK_OK:
				view.hiddenView();
				_DataPlayer.ContinuousDay ++;
				//if(_DataPlayer.ContinuousDay>7)_DataPlayer.ContinuousDay = 0;
				DataPlayerController.getInstance().saveDataPlayer();
				break;
			case MDAlertBehaviour.DID_HIDDEN:
				Destroy(view.gameObject);
				WGAlertManager.Self.RemoveHeadAndShowNext();
				break;
			}
		};
		BCSoundPlayer.Play(MusicEnum.showReward,1f);
		rewardView.showView();
	}

	void OnBtnHelp()
	{
		WGGameUIView.Instance.ViewControllerDoAct(BTN_ACT.OTHER);
	}
	void OnBtnDuiHuaFei()
	{
		WGGameUIView.Instance.ViewControllerDoAct(BTN_ACT.HuaFei);
	}
	public GameObject btnGuide;
	public GameObject btnAdd;
	public void hightLightBtnAdd(){
		btnGuide.SetActive(true);
		btnGuide.transform.localPosition=btnAdd.transform.localPosition;
		btnGuide.GetComponent<BoxCollider>().size=btnAdd.GetComponent<BoxCollider>().size;
		btnGuide.GetComponent<BoxCollider>().center=btnAdd.GetComponent<BoxCollider>().center;
		btnAdd.GetComponent<UIButton>().enabled=false;
		foreach(UISprite child in btnAdd.GetComponentsInChildren<UISprite>()){
			child.depth+=100;
			child.ESetActive(false);
			child.ESetActive(true);
		}
	}
	public void unLightBtnAdd(){
		btnGuide.SetActive(false);
		btnAdd.GetComponent<UIButton>().enabled=true;
		foreach(UISprite child in btnAdd.GetComponentsInChildren<UISprite>()){
			child.depth-=100;
			child.ESetActive(false);
			child.ESetActive(true);
		}
	}
	public GameObject btnSkill4;
	public void hightLightBtnSkill4(){
		btnGuide.SetActive(true);
		btnGuide.transform.localPosition=btnSkill4.transform.localPosition+btnSkill4.transform.parent.localPosition;
		btnGuide.GetComponent<BoxCollider>().size=new Vector3(75,75,0);
		btnGuide.GetComponent<BoxCollider>().center=Vector3.zero;
		btnSkill4.GetComponentInChildren<WGSkillView>().hightLight();
	}
	public void unLightBtnSkill4(){
		btnGuide.SetActive(false);
		btnSkill4.GetComponentInChildren<WGSkillView>().unlight();
	}

	void OnBtnLeft()
	{
		WGBearManage.Instance.csThrow.ChangeWeaponAdd();
	}
	void OnBtnRight()
	{
		WGBearManage.Instance.csThrow.ChangeWeaponSub();
	}
	void OnBtnSuperTiger()
	{
		if(!WGAlertManager.Self.bSuperTiger)
		{
			WGAlertManager.Self.bSuperTiger = true;
			WGAlertManager.Self.AddAction(()=>{
				D0330SuperTiger st = D0330SuperTiger.CreateSuperTiger();
				SDK.AddChild(st.gameObject,WGRootManager.Self.goRootGameUI);
				st.alertViewBehavriour = (ab,view)=>{
					switch(ab)
					{
					case MDAlertBehaviour.CLICK_OK:
						view.hiddenView();
						break;
					case MDAlertBehaviour.DID_SHOW:
						WGAlertManager.Self.bSuperTiger = false;
						break;
					case MDAlertBehaviour.DID_HIDDEN:
						Destroy(view.gameObject);
						WGAlertManager.Self.bSuperTiger = false;
						WGAlertManager.Self.RemoveHead();
						WGAlertManager.Self.ShowNext();
						break;
					}
				};
				st.showView();
			});
			WGAlertManager.Self.ShowNext();
		}
	}
	void OnBtnChaoZhi()
	{
		if(!_DataPlayer.szBigReward.Contains(4))
		{

			WGAlertManager.Self.AddAction(()=>{
				WGDataController _dataCtrl = WGDataController.Instance;
				YHMDPayData payData=_dataCtrl.getYHMDPay(YHPayType.CHEAP);
				float costMenoy=payData.payCost;
				string payKey=payData.payKey.ToString();
//#if Unicom
//				float costMenoy = 1f;
//#else
//				float costMenoy = 0.1f;
//#endif

//				string payKey = "104";
//				if(YeHuoSDK.bUsePayCode2)
//				{
//					payKey = "204";
//				}

				YHGotRewardView rdview = YHGotRewardView.CreateGotRewardView();
				rdview.mRType = YHRewardType.Cheap;
				SDK.AddChild(rdview.gameObject,WGRootManager.Self.goRootTopUI);

				rdview.FreshRewardCell(_dataCtrl.mAllReward.cheap);
#if YES_OK
				string content = WGStrings.getFormateInt(1081,1002,1086,costMenoy.ToString());
#elif YES_BUY
				string content = WGStrings.getFormateInt(1081,1094,1086,costMenoy.ToString());
#elif YES_GET
				string content = WGStrings.getFormateInt(1081,1077,1086,costMenoy.ToString());
#elif YES_QueRen
				string content = WGStrings.getFormateInt(1081,1106,payData.showText,costMenoy.ToString());
#else
				string content = WGStrings.getFormateInt(1081,1077,payData.showText,costMenoy.ToString());
#endif
				rdview.FreshWithMsg(payData.showText,content,true,true);
				rdview.alertViewBehavriour =(ab,view)=>{
					switch(ab)
					{
					case MDAlertBehaviour.CLICK_OK:
					{
						YeHuoSDK.YHPay(payKey,costMenoy,0,(success)=>{
							view.hiddenView();
							if(success)
							{

								btnChaoZhi.ESetActive(false);

								_DataPlayer.szBigReward.Add(4);

								rdview.GetAllReward();
								WGGameUIView.Instance.freshSkillNum();
								WGGameUIView.Instance.freshPlayerUI(UI_FRESH.BCOIN | UI_FRESH.COIN);
							}
						});
					}
						break;
					case MDAlertBehaviour.CLICK_CANCEL:
						view.hiddenView();
						break;
					case MDAlertBehaviour.DID_HIDDEN:

						Destroy(view.gameObject);
						WGAlertManager.Self.RemoveHead();
						WGAlertManager.Self.ShowNext();
						break;
					}
				};
				rdview.showView();
				BCSoundPlayer.Play(MusicEnum.showReward,1f);
			});

			WGAlertManager.Self.ShowNext();
		}
	}
	void myReleaseSkillWithID(int id)
	{
		BCSoundPlayer.Play(MusicEnum.button);
		WGAchievementManager.Self.processAchievement(id,DTAchievementType.USE_SKILL);

		WGSkillController.Instance.ReleaseSkillWithID(id);
	}

}
