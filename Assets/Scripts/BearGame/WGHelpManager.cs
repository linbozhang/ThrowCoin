using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EMHelpStates{
	Kill_Drop = 0,//	第一次击中	杀死掉落金币
	Kill_Fast = 1,//	第二次击中	秒杀翻倍掉落
	Kill_Tiger = 2,//	第一次死亡	死亡激活老虎机
	Kill_Energy = 3,//	第二次死亡	死亡能量值提升
	Miss_Coin = 4,//	第一次未击中	未击中金币消失
	Drop_Coin = 5,//	第一次获取金币	推下金币获取
	Tiger_777 = 6,//	第一次777	777获取大量奖励
	Use_Weapon=7,//切换武器
	Use_Item = 8,// 使用道具
	Free10_Skill = 9,//免费赠送10钟的固定枪技能
	RegetCoin=10,
	LongTouch=11,//引导长按
	MAX = 12
}


public class WGHelpManager : MonoBehaviour {

	public GameObject go2DRoot;

	WGGuideHelpView curGuideView;

	int max = (int)EMHelpStates.MAX;

	DataPlayer dp {
		get{
			return DataPlayerController.getInstance().data;
		}
	}

	bool bViewShow=false;

	public static WGHelpManager Self = null;
	void Awake()
	{
		Self = this;

		if(dp.mHelpEnd == 0)
		{

		}
		else{
			this.enabled = false;

		}

	}
	void Start()
	{
		if(YeHuoSDK.bCommonTiger)
		{
		}
		else
		{
			StatesEnd(EMHelpStates.Kill_Tiger);
			StatesEnd(EMHelpStates.Tiger_777);
		}
	}

	public bool StatesIsEnd(EMHelpStates hs)
	{
		bool isEnd = true;

		int key = (int)hs;
		int val = 0;

		dp.dicHelp.TryGetValue(key,out val);


		if(hs == EMHelpStates.Free10_Skill)
		{
			int p=0;
			for(int i=0;i<max;i++)
			{
				if(dp.dicHelp.ContainsKey(i) &&dp.dicHelp[i]==1)
				{
					p++;
				}
			}
			if(p == max-1)
			{
				isEnd = false;
			}
		}
		else
		{
			isEnd = val == 1;
		}



		if(bViewShow)
		{
			isEnd =true;
		}
		if(WGAlertManager.Self._bAlertShow)
		{
			isEnd = true;
		}
		return isEnd;
	}
	public void StatesEnd(EMHelpStates hs)
	{
		int key = (int)hs;
		if(dp.dicHelp.ContainsKey(key))
		{
			dp.dicHelp[key]=1;
		}
		else
		{
			dp.dicHelp.Add(key,1);
		}
	}


	public void ShowHelpView(EMHelpStates hs)
	{

		bViewShow = true;

		curGuideView = WGGuideHelpView.CreateGuideView();
		curGuideView.showNextPrefabs((int)hs);
		StatesEnd(hs);

		int p=0;
		for(int i=0;i<max;i++)
		{
			if(dp.dicHelp.ContainsKey(i) &&dp.dicHelp[i]==1)
			{
				p++;
			}
		}
		if(p==max)
		{
			dp.mHelpEnd=1;
			this.enabled = false;
		}
		string missionName = WGStrings.getText(8100+(int)hs);
		//Debug.Log("==============="+missionName);
		#if TalkingData
		TDGAMission.OnBegin(missionName);
		#endif
#if Umeng
		var dict = new Dictionary<string,string>();
		dict.Add("states",hs.ToString());

		Umeng.GA.Event(UmengKey.UserHelp,dict);
#endif
		SDK.AddChild(curGuideView.gameObject,go2DRoot);
		curGuideView.ESetActive(true);
		curGuideView.showView();
		if(hs==EMHelpStates.Use_Item){
			curGuideView.hideBtnOK();
			UIEventListener.Get(WGGameUIView.Instance.mMainMenuView.btnGuide).onClick=(GameObject obj)=>curGuideView.OnBtnOk();
			WGGameUIView.Instance.mMainMenuView.hightLightBtnSkill4();
		}
		if(hs==EMHelpStates.Use_Weapon){
			curGuideView.hideBtnOK();
			UIEventListener.Get(WGGameUIView.Instance.mMainMenuView.btnGuide).onClick=(GameObject obj)=>curGuideView.OnBtnOk();
			//curGuideView.setBtnOKPos(WGGameUIView.Instance.mMainMenuView.btnAdd.transform.position);
			WGGameUIView.Instance.mMainMenuView.hightLightBtnAdd();
		}
		curGuideView.alertViewBehavriour =(ab,view)=>{
			switch(ab)
			{
			case MDAlertBehaviour.CLICK_OK:
				if(hs == EMHelpStates.Free10_Skill)
				{

					dp.guDingTime = 10*60;
					dp.releaseGuding = 1;
					WGSkillController.Instance.ReleaseSkillWithID(WGDefine.SK_GuDing30);
					WGAlertViewController.Self.showAlertView(1068).alertViewBehavriour = (ab1,view1)=>{
						switch(ab1)
						{
						case MDAlertBehaviour.CLICK_OK:
							view1.hiddenView();

							//TDGAMission.OnBegin(WGStrings.getText(8110));
							break;
						case MDAlertBehaviour.DID_HIDDEN:
							WGAlertViewController.Self.destroyAlertView(view1);

							//TDGAMission.OnCompleted(WGStrings.getText(8110));

							break;
						}
					};
				}
				if(hs==EMHelpStates.Use_Item){
					WGGameUIView.Instance.mMainMenuView.unLightBtnSkill4();
					WGSkillController.Instance.ReleaseSkillWithID(WGDefine.SK_FangYu4);
				}
				if(hs==EMHelpStates.Use_Weapon){
					WGGameUIView.Instance.mMainMenuView.unLightBtnAdd();
					WGBearManage.Instance.csThrow.ChangeWeaponAdd();
					//WGSkillController.Instance.ReleaseSkillWithID(WGDefine.SK_GuDing30);
				}
				view.hiddenView();
				//Debug.Log("Completed ========"+missionName);
				//TDGAMission.OnCompleted(missionName);
				break;
			case MDAlertBehaviour.DID_SHOW:
				Time.timeScale = 0;
				break;
			case MDAlertBehaviour.DID_HIDDEN:
				Time.timeScale = 1;
				Destroy(curGuideView.gameObject);
				curGuideView = null;
				StartCoroutine(resetViewShow());
				break;
			}
		};

	}
	IEnumerator resetViewShow()
	{
		yield return new WaitForSeconds(30);
		bViewShow = false;
	}

}
