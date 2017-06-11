using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CShopScrollCellView : MonoBehaviour {


	public System.Action<GameObject> myBuyAction;
	[HideInInspector]
	public int index;

	public MDShopData mData;

	public UISprite spCellIcon;
	public UILabel labCost;
	public UILabel labTitle;
	public UILabel labGotNum;
	public UILabel labHasBuy;
	public UILabel labTimeCount;
	public UILabel labProductDes;
	public UISprite spMoneyIcon;

	public UISprite spGrayBg1;
	public UISprite spGrayBg2;
	public List<Vector3> szPosition;
	public GameObject goCommon;
	public UISwitch mySwitch;

	public UISprite spBackground4;

	int mTimeCount = 0;
	float fTimeCount = 0;
	bool isBeginTimeCount = false;

	public Color coinColor = Color.yellow;
	public Color gemColor = Color.green;

	void Start()
	{
		if(mySwitch != null)
		{
			mySwitch.ESetActive(false);
			mySwitch.OnChangeValue +=this.SwitchChangeValue;
		}
	}

	public void refresh()
	{
		freshViewWithData(mData);
	}

	public void freshViewWithData(MDShopData data)
	{
		mData = data;

		if(labCost != null)
		{

			if(data.type == MDShopData.JEWEL)
			{
				labCost.text = WGStrings.getFormate(1044,data.cost_num);
				//WGStrings.getText(1044) + data.cost_num.ToString();
			}
			else
			{
				labCost.text = data.cost_num.ToString();
			}
		}
		if(spMoneyIcon != null)
		{
			if(data.cost_type == 0)
			{

				spMoneyIcon.spriteName = "coin_105";
				labCost.color = coinColor;
			}
			else if(data.cost_type == 1)
			{
				spMoneyIcon.spriteName = "gem_104";
				labCost.color = gemColor;
			}
			spMoneyIcon.MakePixelPerfect();
		}
		if(labProductDes != null && !string.IsNullOrEmpty(data.pdes))
		{
			labProductDes.text = data.pdes;
		}
		if(data.type == MDShopData.ITEM)
		{
			if(labGotNum != null)
			{
				labGotNum.text = data.des;
			}
			GetTimeCount(data);

		}
		else{
			if(labGotNum != null)
			{
				labGotNum.text = data.get_num.ToString();
			}
			mTimeCount = 0;
		}

		showTimeCount();

		spCellIcon.spriteName = data.icon;
		if(data.type == MDShopData.COIN || data.type == MDShopData.JEWEL)
		{
			if(data.icon.Equals("CoinUI49") || data.icon.Equals("CoinUI53"))
			{
				spCellIcon.transform.localPosition = szPosition[0];
				spGrayBg1.ESetActive(true);
				spGrayBg2.ESetActive(false);
			}
			else
			{
				spGrayBg1.ESetActive(false);
				spGrayBg2.ESetActive(true);
				spCellIcon.transform.localPosition = szPosition[1];
			}
		}
		else
		{
		}
		spCellIcon.MakePixelPerfect();

	}

	void Update()
	{
		if(isBeginTimeCount)
		{
			fTimeCount += RealTime.deltaTime;
			if(fTimeCount>1)
			{
				fTimeCount = fTimeCount-1;
				TimeCount();
			}

		}
	}

	void showTimeCount()
	{
		DataPlayerController dpc = DataPlayerController.getInstance();
		bool show = mTimeCount>0;

		goCommon.ESetActive(!show);
		labTimeCount.ESetActive(show);
		labHasBuy.ESetActive(show);
		if(mData.id == WGDefine.SK_GuDing)
		{
//			CancelInvoke("TimeCount");
			isBeginTimeCount = false;
			fTimeCount = 0;
			labTimeCount.ESetActive(false);

			if(dpc.data.guDingForever>0)
			{
				mySwitch.ESetActive(true);
				goCommon.ESetActive(false);
				spBackground4.ESetActive(false);
			}
			else
			{
				mySwitch.ESetActive(false);
				goCommon.ESetActive(true);
				spBackground4.ESetActive(true);
			}
		}
		else {
			mySwitch.ESetActive(false);
			spBackground4.ESetActive(true);
			if(show)
			{
//				CancelInvoke("TimeCount");
//				InvokeRepeating("TimeCount",1,1);
				isBeginTimeCount = true;
				fTimeCount = 0;

			}
			else
			{
//				CancelInvoke("TimeCount");
				isBeginTimeCount = false;
				fTimeCount = 0;
			}
		}
	}
	void TimeCount()
	{
		mTimeCount--;

		if(labTimeCount!= null)
		{
			if(mTimeCount<=0)
			{
				GetTimeCount(mData);
			}
			int mSecond = mTimeCount%60;
			int mMinute = mTimeCount/60;
			labTimeCount.text = WGStrings.getText(1046)+"00:"+mMinute.ToString("00")+":"+mSecond.ToString("00");
			if(mTimeCount<=0)
			{
				showTimeCount();
			}
		}

	}
	void SwitchChangeValue()
	{
		DataPlayerController dpc = DataPlayerController.getInstance();
		dpc.data.releaseGuding = mySwitch.value?1:0;
		WGSkillController.Instance.ReleaseSkillWithID(mData.id);
	}
	void GetTimeCount(MDShopData data){
		DataPlayerController dpc = DataPlayerController.getInstance();
		switch(data.id)
		{
		case WGDefine.SK_777Up1:
		case WGDefine.SK_777Up2:
			mTimeCount = dpc.data.up777Time;
			break;
		case WGDefine.SK_Defense10M:
		case WGDefine.SK_Defense30M:
			mTimeCount = dpc.data.defenseTime;

			break;
		case WGDefine.SK_GuDing30:
			mTimeCount = dpc.data.guDingTime;
			break;
		case WGDefine.SK_GuDing:
			mTimeCount = dpc.data.guDingForever;
			mySwitch.value = dpc.data.releaseGuding>0;
			break;
		default:
			mTimeCount = 0;
			break;
		}

		if(data.id !=WGDefine.SK_GuDing && mTimeCount>0 && labTimeCount != null)
		{
			int mSecond = mTimeCount%60;
			int mMinute = mTimeCount/60;
			labTimeCount.text = WGStrings.getText(1046)+"00:"+mMinute.ToString("00")+":"+mSecond.ToString("00");
		}
	}



	void OnBtnBuy()
	{

		char [] spliter={',','.','，','。'};
		BCSoundPlayer.Play(MusicEnum.button);
		DataPlayerController dpc = DataPlayerController.getInstance();
		DataPlayer _dp = dpc.data;

		if(mData.type == MDShopData.JEWEL)
		{
#if Umeng
			var dict = new Dictionary<string,string>();
			dict["costNum"]=mData.cost_num.ToString();
			dict["getNum"]=mData.get_num.ToString();
			Umeng.GA.Event(UmengKey.ClickPay,dict);
#endif

//			#if TalkingData
//			//TDGAItem.OnPurchase("钻石"+mData.get_num.ToString(),1,mData.cost_num);
//			Dictionary<string, object> dic = new Dictionary<string, object>();
//			dic.Add("JewelNum",mData.get_num.ToString());
//			TalkingDataGA.OnEvent("购买钻石",dic);
//			#endif

			WGProductController.instance.OnBuyWithMDShopData(mData);

		}
		else if(mData.type == MDShopData.COIN)
		{

			if(_dp.Jewel>= mData.cost_num)
			{

#if Umeng
				var dict = new Dictionary<string,string>();
				dict["coin"]=mData.sid.ToString();
				Umeng.GA.Event(UmengKey.ClickBuy,dict);
#endif
				string title = WGStrings.getFormate(1030,mData.get_num.ToString(),mData.cost_num.ToString());
				WGAlertViewController.Self.showAlertView(title,1002,1007).alertViewBehavriour =(ab,view) => {
					switch(ab)
					{
					case MDAlertBehaviour.CLICK_OK:
						{
#if Umeng
							Umeng.GA.Buy(mData.id.ToString(),mData.get_num, mData.cost_num*1.0f/mData.get_num);
							Umeng.GA.Event(UmengKey.BuySuccess,dict);
#endif
#if TalkingData
						TDGAItem.OnPurchase("金币_"+mData.get_num.ToString(),1,mData.cost_num);
						Dictionary<string, object> dic = new Dictionary<string, object>();
						dic.Add("CoinNum",mData.get_num.ToString());
						TalkingDataGA.OnEvent("购买金币",dic);
#endif
							_dp.Coin +=mData.get_num;
							_dp.Jewel -=mData.cost_num;
							DataPlayerController.getInstance().saveDataPlayer();
							WGGameUIView.Instance.freshPlayerUI(UI_FRESH.COIN|UI_FRESH.BCOIN);

							view.hiddenView();

							WGAlertViewController.Self.showTipView(1001);

//							WGAlertViewController.Self.showAlertView(1001).alertViewBehavriour =(ab1,view1) => {
//								switch(ab1)
//								{
//								case MDAlertBehaviour.CLICK_OK:
//									view.hiddenView();
//									break;
//								case MDAlertBehaviour.DID_HIDDEN:
//									WGAlertViewController.Self.hiddeAlertView(view1.gameObject);
//									break;
//								}
//							};
							break;
						}
					case MDAlertBehaviour.CLICK_CANCEL:
						view.hiddenView();
						break;
					case MDAlertBehaviour.DID_HIDDEN:
						WGAlertViewController.Self.hiddeAlertView(view.gameObject);
						break;
					}
				};
			}
			else
			{
				WGAlertViewController.Self.showAlertView(1003,1002,1007).alertViewBehavriour =(ab,view) => {
					switch(ab)
					{
					case MDAlertBehaviour.CLICK_OK:
						view.hiddenView();
						WGShopView.curShopView.InitWillShowWithTabView(SHOP_TAB_VIEW.JEWEL_SHOP);
						break;
					case MDAlertBehaviour.CLICK_CANCEL:
						view.hiddenView();
						break;
					case MDAlertBehaviour.DID_HIDDEN:
						WGAlertViewController.Self.hiddeAlertView(view.gameObject);
						break;
					}
				};
			}
		}
		else if(mData.type == MDShopData.ITEM)
		{


			if((mData.cost_type==1 && _dp.Jewel>=mData.cost_num) ||(mData.cost_type==0&&_dp.Coin>=mData.cost_num))
			{

				var dict = new Dictionary<string,string>();
				dict["item"]=mData.sid.ToString();
#if Umeng
				Umeng.GA.Event(UmengKey.ClickBuy,dict);
#endif
				if(mData.id == WGDefine.SK_Defense10M || mData.id == WGDefine.SK_Defense30M)
				{
					if(_dp.defenseTime>0)return;

					MDSkill sk = WGDataController.Instance.getSkill(mData.id);
					int titleID = mData.cost_type==0?7006:1006;
					string title = WGStrings.getFormate(titleID,sk.time.ToString(),mData.cost_num.ToString());
					WGAlertViewController.Self.showAlertView(title,1002,1007).alertViewBehavriour =(ab,view) => {
						switch(ab)
						{
						case MDAlertBehaviour.CLICK_OK:
							{
								string msg = "";
								if(_dp.defenseTime>0)
								{
									msg = WGStrings.getFormate(1008,_dp.defenseTime);
								}
								else{
									msg = WGStrings.getText(1001);
									if(mData.cost_type == 1)
									{
										_dp.Jewel -=mData.cost_num;
#if TalkingData				
									string name="道具_"+mData.pdes.Split(spliter)[0]+mData.des;
									TDGAItem.OnPurchase(name,1,mData.cost_num);
									
									TDGAItem.OnUse(name,1);
									Dictionary<string, object> dic = new Dictionary<string, object>();
									dic.Add("item",name);
									TalkingDataGA.OnEvent("购买道具",dic);
#endif
#if Umeng
										Umeng.GA.Buy(mData.id.ToString(),mData.get_num, mData.cost_num*1.0f/mData.get_num);
#endif
									}
									else if(mData.cost_type == 0)
									{
										#if TalkingData
									string name="道具_"+mData.pdes.Split(spliter)[0]+mData.des;
									Dictionary<string, object> dic1 = new Dictionary<string, object>();
									dic1.Add("item",name);
									TalkingDataGA.OnEvent("金币购买道具",dic1);
										#endif
										_dp.Coin -=mData.cost_num;

									}
#if Umeng
									Umeng.GA.Event(UmengKey.BuySuccess,dict);
#endif
									WGGameUIView.Instance.freshPlayerUI(UI_FRESH.COIN|UI_FRESH.BCOIN);
									WGGameUIView.Instance.freshSkillNum();

									_dp.defenseTime = _dp.defenseTime + (int)sk.time;
									WGSkillController.Instance.ReleaseSkillWithID(mData.id);
									GetTimeCount(mData);
									showTimeCount();
									
								WGShopView.curShopView.mTableView.UpdataCells();
//									node.refresh();
								}
								view.hiddenView();
								WGAlertViewController.Self.showTipView(msg);
//								WGAlertViewController.Self.showAlertView(msg).alertViewBehavriour =(ab1,view1) => {
//									switch(ab1)
//									{
//									case MDAlertBehaviour.CLICK_OK:
//										view1.hiddenView();
//										break;
//									case MDAlertBehaviour.DID_HIDDEN:
//										WGAlertViewController.Self.hiddeAlertView(view1.gameObject);
//										break;
//									}
//								};
							break;
							}
						case MDAlertBehaviour.CLICK_CANCEL:
							view.hiddenView();
							break;
						case MDAlertBehaviour.DID_HIDDEN:
							WGAlertViewController.Self.hiddeAlertView(view.gameObject);
							break;
						}
					};

				}
				else if(mData.id == WGDefine.SK_777Up1 || mData.id == WGDefine.SK_777Up2)
				{
					if(_dp.up777Time>0)return;
					MDSkill sk = WGDataController.Instance.getSkill(mData.id);
					int titleID=mData.cost_type==0?7009:1009;
					string title = WGStrings.getFormate(titleID,sk.time.ToString(),mData.cost_num.ToString());

					WGAlertViewController.Self.showAlertView(title,1002,1007).alertViewBehavriour =(ab,view) => {
						switch(ab)
						{
						case MDAlertBehaviour.CLICK_OK:
							{
								string msg = "";
								if(_dp.up777Time>0)
								{
									msg = WGStrings.getFormate(1010,_dp.defenseTime);
								}
								else{

									msg = WGStrings.getText(1001);

									if(mData.cost_type == 1)
									{
										_dp.Jewel -=mData.cost_num;

#if TalkingData

									TDGAItem.OnPurchase("道具_"+mData.pdes.Split(spliter)[0]+mData.des,1,mData.cost_num);
									
									TDGAItem.OnUse("道具_"+mData.pdes.Split(spliter)[0]+mData.des,1);

									Dictionary<string, object> dic = new Dictionary<string, object>();
									dic.Add("item",name);
									TalkingDataGA.OnEvent("购买道具",dic);
#endif
#if Umeng
										Umeng.GA.Buy(mData.id.ToString(),mData.get_num, mData.cost_num*1.0f/mData.get_num);
#endif
									}
									else if(mData.cost_type == 0)
									{
										#if TalkingData
									string name="道具_"+mData.pdes.Split(spliter)[0]+mData.des;
									Dictionary<string, object> dic1 = new Dictionary<string, object>();
									dic1.Add("item",name);
									TalkingDataGA.OnEvent("金币购买道具",dic1);
										#endif
										_dp.Coin -=mData.cost_num;
									}
#if Umeng
									Umeng.GA.Event(UmengKey.BuySuccess,dict);
#endif
									WGGameUIView.Instance.freshPlayerUI(UI_FRESH.COIN|UI_FRESH.BCOIN);
									WGGameUIView.Instance.freshSkillNum();

									_dp.up777Time = _dp.up777Time + (int)sk.time;
									WGSkillController.Instance.ReleaseSkillWithID(mData.id);

									GetTimeCount(mData);
									showTimeCount();
//									WGGameUIView.Instance.mShopView.mTableView.UpdataCells();
									WGShopView.curShopView.mTableView.UpdataCells();
//									node.refresh();
								}
								view.hiddenView();
								WGAlertViewController.Self.showTipView(msg);
//								WGAlertViewController.Self.showAlertView(msg).alertViewBehavriour =(ab1,view1) => {
//									switch(ab1)
//									{
//									case MDAlertBehaviour.CLICK_OK:
//										view1.hiddenView();
//										break;
//									case MDAlertBehaviour.DID_HIDDEN:
//										WGAlertViewController.Self.hiddeAlertView(view1.gameObject);
//										break;
//									}
//								};
								break;
							}
						case MDAlertBehaviour.CLICK_CANCEL:
							view.hiddenView();
							break;
						case MDAlertBehaviour.DID_HIDDEN:
							WGAlertViewController.Self.hiddeAlertView(view.gameObject);
							break;
						}
					};
				}
				else if(mData.id == WGDefine.SK_GuDing30)
				{
					if(_dp.guDingTime>0)
					{
						return;
					}
					if(_dp.guDingForever>0)
					{
						WGAlertViewController.Self.showAlertView(1033).alertViewBehavriour =(ab,view) => {
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
					MDSkill sk = WGDataController.Instance.getSkill(mData.id);
					string title = WGStrings.getFormate(1019,sk.time.ToString(),mData.cost_num.ToString());
					WGAlertViewController.Self.showAlertView(title,1002,1007).alertViewBehavriour =(ab,view) => {
						switch(ab)
						{
						case MDAlertBehaviour.CLICK_OK:
							{
								string msg = "";

								msg = WGStrings.getText(1001);

								if(mData.cost_type == 1)
								{
									_dp.Jewel -=mData.cost_num;
#if TalkingData
								string name="道具_"+mData.pdes.Split(spliter)[0]+mData.des;
								TDGAItem.OnPurchase(name,1,mData.cost_num);

								TDGAItem.OnUse(name,1);
								Dictionary<string, object> dic = new Dictionary<string, object>();
								dic.Add("item",name);
								TalkingDataGA.OnEvent("购买道具",dic);
#endif
#if Umeng
									Umeng.GA.Buy(mData.id.ToString(),mData.get_num, mData.cost_num*1.0f/mData.get_num);
#endif
								}
								else if(mData.cost_type == 0)
								{
									#if TalkingData
								string name="道具_"+mData.pdes.Split(spliter)[0]+mData.des;
								Dictionary<string, object> dic1 = new Dictionary<string, object>();
								dic1.Add("item",name);
								TalkingDataGA.OnEvent("金币购买道具",dic1);
									#endif
									_dp.Coin -=mData.cost_num;
								}
#if Umeng
								Umeng.GA.Event(UmengKey.BuySuccess,dict);
#endif
								_dp.guDingTime = (int)sk.time;

								WGGameUIView.Instance.freshPlayerUI(UI_FRESH.BCOIN);
								WGGameUIView.Instance.freshSkillNum();
								GetTimeCount(mData);
								showTimeCount();

								_dp.releaseGuding = 1;
								WGSkillController.Instance.ReleaseSkillWithID(WGDefine.SK_GuDing30);
								view.hiddenView();
								WGAlertViewController.Self.showTipView(1001);

								break;
							}
						case MDAlertBehaviour.CLICK_CANCEL:
							view.hiddenView();
							break;
						case MDAlertBehaviour.DID_HIDDEN:
							WGAlertViewController.Self.hiddeAlertView(view.gameObject);
							break;
						}
					};
				}
				else if(mData.id == WGDefine.SK_GuDing)
				{
					if(_dp.guDingForever>0)
					{
//						WGSkillController.Instance.ReleaseSkillWithID(mData.id);
//						GetTimeCount(mData);
					}
					else{
						MDSkill sk = WGDataController.Instance.getSkill(mData.id);
						string title = WGStrings.getFormate(1020,mData.cost_num.ToString());
						WGAlertViewController.Self.showAlertView(title,1002,1007).alertViewBehavriour =(ab,view) => {
							switch(ab)
							{
							case MDAlertBehaviour.CLICK_OK:
								{
									string msg = "";
									if(_dp.guDingForever>0)
									{
										msg = WGStrings.getText(1022);
									}
									else{

										msg = WGStrings.getText(1001);

										if(mData.cost_type == 1)
										{
											_dp.Jewel -=mData.cost_num;

#if TalkingData
										string name="道具_"+mData.pdes.Split(spliter)[0]+mData.des;
										TDGAItem.OnPurchase(name,1,mData.cost_num);
										TDGAItem.OnUse(name,1);
										Dictionary<string, object> dic = new Dictionary<string, object>();
										dic.Add("item",name);
										TalkingDataGA.OnEvent("购买道具",dic);
#endif

#if Umeng
											Umeng.GA.Buy(mData.id.ToString(),mData.get_num, mData.cost_num*1.0f/mData.get_num);
#endif
										}
										else if(mData.cost_type == 0)
										{
											#if TalkingData
										string name="道具_"+mData.pdes.Split(spliter)[0]+mData.des;
										Dictionary<string, object> dic1 = new Dictionary<string, object>();
										dic1.Add("item",name);
										TalkingDataGA.OnEvent("金币购买道具",dic1);
											#endif
											_dp.Coin -=mData.cost_num;
										}
#if Umeng
										Umeng.GA.Event(UmengKey.BuySuccess,dict);
#endif
										_dp.guDingForever = 1;

										WGGameUIView.Instance.freshPlayerUI(UI_FRESH.BCOIN);
										WGGameUIView.Instance.freshSkillNum();

										WGSkillController.Instance.ReleaseSkillWithID(mData.id);

										GetTimeCount(mData);
										showTimeCount();

									}
									view.hiddenView();
									WGAlertViewController.Self.showTipView(msg);

//									WGAlertViewController.Self.showAlertView(msg).alertViewBehavriour =(ab1,view1) => {
//										switch(ab1)
//										{
//										case MDAlertBehaviour.CLICK_OK:
//											view1.hiddenView();
//											break;
//										case MDAlertBehaviour.DID_HIDDEN:
//											WGAlertViewController.Self.hiddeAlertView(view1.gameObject);
//											break;
//										}
//									};
									break;
								}
							case MDAlertBehaviour.CLICK_CANCEL:
								view.hiddenView();
								break;
							case MDAlertBehaviour.DID_HIDDEN:
								WGAlertViewController.Self.hiddeAlertView(view.gameObject);
								break;
							}
						};
					}
				}
				else
				{
					MDSkill sk = WGDataController.Instance.getSkill(mData.id);
					int titleID = mData.cost_type==0?7029:1029;
					string title = WGStrings.getFormate(titleID,mData.get_num.ToString(),sk.name,mData.cost_num.ToString());
					WGAlertViewController.Self.showAlertView(title,1002,1007).alertViewBehavriour =(ab,view) => {
						switch(ab)
						{
						case MDAlertBehaviour.CLICK_OK:
							{
								dpc.AddSkillNum(mData.id,mData.get_num);
								if(mData.cost_type == 1)
								{
									_dp.Jewel -=mData.cost_num;
								#if TalkingData

								string name="道具_"+mData.pdes.Split(spliter)[0]+mData.des;
								TDGAItem.OnPurchase(name,1,mData.cost_num);
								Dictionary<string, object> dic = new Dictionary<string, object>();
								//dic.Add("name",sk.name);
								dic.Add("item",name);
								TalkingDataGA.OnEvent("购买道具", dic);
								#endif
#if Umeng
									Umeng.GA.Buy(mData.id.ToString(),mData.get_num, mData.cost_num*1.0f/mData.get_num);
#endif
								}
								else if(mData.cost_type == 0)
								{
									#if TalkingData
								string name="道具_"+mData.pdes.Split(spliter)[0]+mData.des;
								Dictionary<string, object> dic1 = new Dictionary<string, object>();
								dic1.Add("item",name);
								TalkingDataGA.OnEvent("金币购买道具",dic1);
									#endif
								_dp.Coin -=mData.cost_num;
								}
#if Umeng
								Umeng.GA.Event(UmengKey.BuySuccess,dict);
#endif
								WGGameUIView.Instance.freshPlayerUI(UI_FRESH.COIN|UI_FRESH.BCOIN);
								WGGameUIView.Instance.freshSkillNum();

								view.hiddenView();
								WGAlertViewController.Self.showTipView(1001);

//								WGAlertViewController.Self.showAlertView(1001).alertViewBehavriour =(ab1,view1) => {
//									switch(ab1)
//									{
//									case MDAlertBehaviour.CLICK_OK:
//										view1.hiddenView();
//										break;
//									case MDAlertBehaviour.DID_HIDDEN:
//										WGAlertViewController.Self.hiddeAlertView(view1.gameObject);
//										break;
//									}
//								};

								break;
							}
						case MDAlertBehaviour.CLICK_CANCEL:
							view.hiddenView();
							break;
						case MDAlertBehaviour.DID_HIDDEN:
							WGAlertViewController.Self.hiddeAlertView(view.gameObject);
							break;
						}
					};

				}
			}
			else
			{
				int msgID = mData.cost_type==0?7003:1003;
				WGAlertViewController.Self.showAlertView(msgID,1002,1007).alertViewBehavriour =(ab,view) => {
					switch(ab)
					{
					case MDAlertBehaviour.CLICK_OK:
						view.hiddenView();
						if(mData.cost_type == 1)
						{

							WGShopView.curShopView.InitWillShowWithTabView(SHOP_TAB_VIEW.JEWEL_SHOP);
						}
						else if(mData.cost_type == 0)
						{
							WGShopView.curShopView.InitWillShowWithTabView(SHOP_TAB_VIEW.Coin_SHOP);
						}
						break;
					case MDAlertBehaviour.CLICK_CANCEL:
						view.hiddenView();
						break;
					case MDAlertBehaviour.DID_HIDDEN:
						WGAlertViewController.Self.hiddeAlertView(view.gameObject);
						break;
					}
				};
			}
		}

	}
	public static CShopScrollCellView CreateItemCell(MDShopData sd)
	{
		Object mObj = null;
		if(sd.type == MDShopData.ITEM)
		{
			mObj = Resources.Load("shopItemCell") ;
		}
		else if(sd.type == MDShopData.COIN)
		{
			mObj = Resources.Load("shopCoinCell") ;
		}
		else if(sd.type == MDShopData.JEWEL)
		{
			mObj = Resources.Load("shopJewelCell");
		}
		GameObject go = Instantiate(mObj) as GameObject;
		CShopScrollCellView cell = go.GetComponent<CShopScrollCellView>();

		cell.freshViewWithData(sd);

		return cell;
	}

}
