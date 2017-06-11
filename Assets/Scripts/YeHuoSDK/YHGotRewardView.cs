using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

public enum YHRewardType{
	Other = 1,
	Levelup = 2,
	Cheap =3,
	Lucky = 4,
	NewUser = 5,
	Rich = 6,
	Item = 7,
	Coin = 8,
	Jewel =9,
	SuperTiger=10,
	Finger=11,
}

public class YHGotRewardView : MDBaseAlertView {

	public TweenScale tsContent;

	public UILabel labTitle;

	public UILabel labContent;

	public UIButton btnCancel;

	public  UIGrid _grid;

	public UISprite spTitle;

	public UILabel labButtonOkText;
	public UILabel labButtonOk1;
	public UILabel labButtonOk2;

	public GameObject goBtnOk;
	public GameObject goBtnOk1;
	public GameObject goBtnOk2;

	public GameObject spriteFinger;
	public YHRewardType mRType = YHRewardType.Other; 
	public bool bDoubleReward=false;

	List<int[]> szAllReward;

	const float _scale5=0.8714489f;
	const float _scale4=1f;
	const float _scale2=1.199642f;
	const float _width5 = 110f;
	const float _width4 = 130f;
	const float _width2 = 220f;

	public void FreshWithMsg(string title,string content,bool showCancel = false,bool isBlue = false,bool notGift=false)
	{
		goBtnOk.SetActive(!bDoubleReward);
		goBtnOk1.SetActive(bDoubleReward);
		goBtnOk2.SetActive(bDoubleReward);
		if(notGift){
			goBtnOk2.transform.Find("Diamond").ESetActive(true);
		}else{
			goBtnOk2.transform.Find("Diamond").ESetActive(false);
		}
		labTitle.text = title;
		labContent.text = content;
		btnCancel.ESetActive(showCancel);
		if(mRType == YHRewardType.Cheap||
		   mRType == YHRewardType.Lucky||
		   mRType == YHRewardType.NewUser||
		   mRType == YHRewardType.Rich||
		   mRType == YHRewardType.Item||
		   mRType == YHRewardType.Coin||
		   mRType == YHRewardType.Jewel||
		   mRType == YHRewardType.SuperTiger)
		{
			if(YeHuoSDK.mTipType != 0)
			{
				labContent.text = "";
			}
		}

		if(isBlue)
		{
			spTitle.spriteName = "bg_rewardTitle_blue";
			labTitle.color = new Color(205f/255f,245f/255f,252f/255f);
		}
		else
		{
			spTitle.spriteName = "bg_rewardTitle";
		}
//		if(mRType == YHRewardType.Levelup)
//		{
//			labButtonOkText.text = WGStrings.getText(1023);
//		}
//		else
		{

			if(mRType == YHRewardType.Levelup || mRType == YHRewardType.SuperTiger)
			{
				labButtonOk1.text = WGStrings.getText(1007);
			}
			else
			{
				labButtonOk1.text = WGStrings.getText(1023);
			}
#if YES_OK
			labButtonOkText.text = WGStrings.getText(1002);
			labButtonOk2.text = WGStrings.getText(1104);
#elif YES_BUY
			labButtonOkText.text = WGStrings.getText(1094);
			labButtonOk2.text = WGStrings.getText(1104);
#elif YES_GET
			labButtonOkText.text = WGStrings.getText(1023);
			labButtonOk2.text = WGStrings.getText(1101);
#elif YES_QueRen
			labButtonOkText.text = WGStrings.getText(1106);
			labButtonOk2.text = WGStrings.getText(1104);
#else
			labButtonOkText.text = WGStrings.getText(1023);
			labButtonOk2.text = WGStrings.getText(1101);
#endif
		}
	}
	public void FreshRewardCell(List<int[]> lr,bool notGift=false)
	{
		szAllReward = lr;
		int id ;
		int num;
		float fscale = 1;
		float width = 110;
		if(lr.Count==2)
		{
			fscale = _scale2;
			width = _width2;
		}
		if(lr.Count == 4)
		{
			fscale = _scale4;
			width = _width4;
		}
		if(lr.Count ==5)
		{
			fscale= _scale5;
			width = _width5;
		}

		_grid.cellWidth = width;

		for(int i=0,max=lr.Count;i<max;i++)
		{
			if(lr[i].Length==2)
			{

				id = lr[i][0];
				num = lr[i][1];


				YHGotRewardCell cell = YHGotRewardCell.CreateCellView();
				_grid.AddChild(cell.transform);
				cell.transform.localScale = Vector3.one*fscale;
				cell.freshDailyCell(id,num,bDoubleReward);
				if(id==4001&&notGift){
					cell.transform.GetChild(0).ESetActive(false);
				}
			}
		}
		_grid.Reposition();
	}

	public void GetAllReward(bool bDouble = false,bool notgift=false)
	{
		DataPlayer _DataPlayer = DataPlayerController.getInstance().data;
		WGDataController _dataCtrl = WGDataController.Instance;
		int jewel = 0;
		for(int i=0;i<szAllReward.Count;i++)
		{
			BCGameObjectType type = _dataCtrl.GetBCObjType(szAllReward[i][0]);
			switch(type)
			{
			case BCGameObjectType.Coin:
				
				_DataPlayer.Coin +=szAllReward[i][1];
				if(bDouble)
				{
					_DataPlayer.Coin +=szAllReward[i][1];
				}
				break;
			case BCGameObjectType.Item:
				DataPlayerController.getInstance().AddSkillNum(szAllReward[i][0],szAllReward[i][1]);
				if(bDouble)
				{
					DataPlayerController.getInstance().AddSkillNum(szAllReward[i][0],szAllReward[i][1]);
				}
				break;
			case BCGameObjectType.Jewel:
				if(notgift){
					if(bDouble)
					{	_DataPlayer.Jewel +=szAllReward[i][1];
						jewel +=szAllReward[i][1];
						_DataPlayer.Jewel +=szAllReward[i][1];
						jewel +=szAllReward[i][1];
					}
				}else{
					_DataPlayer.Jewel +=szAllReward[i][1];
					jewel +=szAllReward[i][1];
					if(bDouble)
					{	
						_DataPlayer.Jewel +=szAllReward[i][1];
						jewel +=szAllReward[i][1];
					}
				}


				break;
			}
		}
		if(jewel>0)
		{
			if(mRType == YHRewardType.Levelup)//level up reward
			{
				//TDGAVirtualCurrency.OnReward(jewel,"LevelUp");
			}
		}
//		Dictionary<string, object> dic = new Dictionary<string, object>();
//		if(mRType != YHRewardType.Levelup && mRType != YHRewardType.Other)
//		{
//			dic.Add("result",WGStrings.getText(9002));
//			TalkingDataGA.OnEvent(labTitle.text,dic);
//		}
		
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
		base.OnBtnOk ();
		BCSoundPlayer.Play(MusicEnum.button);
		
	}
	public void OnBtnOk1()
	{
		BCSoundPlayer.Play(MusicEnum.button);
		base.OnBtnOkWithIndex(CLICK_OK1);
	}
	public void OnBtnOk2()
	{
		BCSoundPlayer.Play(MusicEnum.button);
		base.OnBtnOkWithIndex(CLICK_OK2);
	}

	public override void OnBtnCancel ()
	{
		base.OnBtnCancel ();
		BCSoundPlayer.Play(MusicEnum.button);
	}
	public static YHGotRewardView CreateGotRewardView()
	{

		Object obj = Resources.Load("pbYHGotRewardView");
		
		GameObject go = Instantiate(obj) as GameObject;
		
		YHGotRewardView drv = go.GetComponent<YHGotRewardView>();
		
		return drv;
		
	}
}
