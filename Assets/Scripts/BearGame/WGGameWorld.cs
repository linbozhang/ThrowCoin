using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using  cn.sharesdk.unity3d;
//Create WgGameWorld by Song
// Copy Right 2014®
public partial class WGGameWorld : WGMonoComptent {


	public GameObject goFourthCamera;

	public GameObject go2DUIBottom;

	public GameObject CoinCamera;

	public WGBearManage cs_BearManage;

	public GameObject goPapapa;

	public GameObject goMiaoSha;

	public GameObject pbShakeStone;

	public GameObject pbEarthQuake;

	[HideInInspector]
	public BCObjManager cs_ObjManager;

	public Queue<GameObject>  DropQueue = new Queue<GameObject>();//用来记录掉落 集中处理 

	public Queue<int> qReleaseSkillQueue = new Queue<int>(); //用来记录事件 处理先后顺序

//	public Queue<HKReward> qRewardQueue=new Queue<HKReward>();//处理奖励的掉落;

	public List<HKReward> szRewardDrop = new List<HKReward>();//处理奖励的掉落;

	public Queue<int> qDropCollection = new Queue<int>();

//	public WGWeaponView csWeaponView;

	WGDataController mDataCtrl;
	int mFrameCount = 0;
	int mSecond=0;
	[HideInInspector]
	public bool bBossResurrection = false;//boss是否复活
	[HideInInspector]
	public bool bNoCoinTip = false;
	Vector3 v3InitCamera;

	int _bossReloadTime =0;
	int _bossDisapperTime = 0;
	private DataCoin _DataCoin;
	private DataPlayer _DataPlayer{
		get{
			return DataPlayerController.getInstance().data;
		}
	}

	public int _NextLevel = 2;

	WGDataController _dataCtrl;

	[HideInInspector]
	public int keyEscape = 0;





	public static WGGameWorld Instance;
	void Awake()
	{

		//WG.EnableLog = true;

		Application.targetFrameRate = 30;

		Instance = this;
		cs_ObjManager = this.gameObject.AddComponent<BCObjManager>();

	}
	void Start () {
#if Umeng
		#if TBSDK
		Umeng.GA.StartWithAppKeyAndChannelId("54a265b7fd98c539aa0000aa", "TongBu");
		#else
		Umeng.GA.StartWithAppKeyAndChannelId("54a265b7fd98c539aa0000aa", "App Store");
		#endif
		Umeng.GA.SetLogEnabled(false);
#endif
		#if Add_AD
		if(_DataPlayer.DelAD == 0)
		{
		IOSAD.ShowAdView(true);
		}
		else
		{
		IOSAD.ShowAdView(false);
		}
		#endif
		this.GetComponent<WGAlertViewController>().InitAlertViewController();
		CleanNotification();
		CoinCamera.transform.localPosition = Core.fc.coinCamPos;
		CoinCamera.transform.eulerAngles = Core.fc.coinCamRot;
		v3InitCamera = Core.fc.coinCamRot;

		_dataCtrl = WGDataController.Instance;


		goFourthCamera.SetActive(false);

		mDataCtrl = WGDataController.Instance;

		cs_ObjManager.InitWithGameWorld(this);

		cs_BearManage.InitWithGameWorld(this);

		cs_ObjManager.SetBearGameCoinRoot(cs_BearManage.BearCoinRoot);

		_DataCoin = WGDataController.Instance.mDataCoin;

		if(_DataPlayer.Level <WGConfig.MAX_LEVEL)
		{
			_NextLevel = _DataPlayer.Level+1;
		}
		else 
		{
			_NextLevel = WGConfig.MAX_LEVEL;
		}

		CheckDefenseTime();
		ChangeAllBearCoin();

		_bossReloadTime = mDataCtrl.GetBearParam(WGDefine.BossID).reload_time;
		_bossDisapperTime = mDataCtrl.GetBearParam(WGDefine.BossID).fresh_time;




		mnIvokeBlock.InvokeBlock(0.5f,()=>{
//			WGAlertManager.Self.AddAction(()=>{
//				if(YeHuoSDK.bTiger_HuaFei)
//				{
//					WGGameUIView.Instance.ViewControllerDoAct(BTN_ACT.HuaFei,(ab,st)=>{
//						if(ab== MDAlertBehaviour.DID_HIDDEN)
//						{
//							WGAlertManager.Self.RemoveHeadAndShowNext();
//						}
//					});
//				}
//				else
//				{
//					WGAlertManager.Self.RemoveHeadAndShowNext();
//				}
//			});

			if(YeHuoSDK.bShow3RoalGift){//init role
				if( _DataPlayer.mR == 0)
				{
					ShowSelectRoleAlert();
				}
				ShowBuyRoleAlert();
			}
			ShowOutLineCoinAlert();

			DailyRewardCheckAlert();

			if(YeHuoSDK.bUsePayCode2)
			{
			}
			else if(!_DataPlayer.szBigReward.Contains(1))
			{
				if(YeHuoSDK.bShowNewhandGift){
					XinShouLiBaoAlert();
				}

			}
			//WG.SLog("GameWorld Start......");
			WGAlertManager.Self.ShowNext();
		});


		InvokeRepeating("TimeCount",1,1);
		ShareSDK.open("b4453fc6bdb4");
		Hashtable wcConf = new Hashtable();
		wcConf.Add ("app_id", "wx011fff7c0672c7c1");
		ShareSDK.setPlatformConfig (PlatformType.WeChatSession, wcConf);
		ShareSDK.setPlatformConfig (PlatformType.WeChatTimeline, wcConf);
		ShareSDK.setPlatformConfig (PlatformType.WeChatFav, wcConf);

	}
	
	void ShowOutLineCoinAlert()
	{
		int num = OutLineReward();
		if(num>0)
		{
			WGAlertManager.Self.AddAction(()=>{

				string msg = WGStrings.getFormate(1038,num);
				WGAlertViewController.Self.showAlertView(msg).alertViewBehavriour = (ab,view)=>{
					switch(ab)
					{
					case MDAlertBehaviour.CLICK_OK:
						view.hiddenView();
						break;
					case MDAlertBehaviour.DID_HIDDEN:
						WGAlertViewController.Self.hiddeAlertView(view.gameObject);
							WGAlertManager.Self.RemoveHeadAndShowNext();
						break;
					}
				};
			});
			WGAlertManager.Self.ShowNext();
		}

	}

//	void ShowOutLineCoin()
//	{
//		int num = OutLineReward();
//		if(num>0)
//		{
//			string msg = WGStrings.getFormate(1038,num);
//			WGAlertViewController.Self.showAlertView(msg).alertViewBehavriour = (ab,view)=>{
//				switch(ab)
//				{
//				case MDAlertBehaviour.CLICK_OK:
//					view.hiddenView();
//					break;
//				case MDAlertBehaviour.DID_HIDDEN:
//					mnIvokeBlock.InvokeBlock(0.5f,()=>{
//
//						DailyRewardCheck();
//
//					});
//					WGAlertViewController.Self.hiddeAlertView(view.gameObject);
//					break;
//				}
//			};
//		}
//		else
//		{
//			mnIvokeBlock.InvokeBlock(0.5f,()=>{
//				DailyRewardCheck();
//			});
//		}
//	}
	bool bShowSelectRoleAlert = false;
	public void ShowSelectRoleAlert()
	{
		if(bShowSelectRoleAlert)return;
		bShowSelectRoleAlert = true;
		WGAlertManager.Self.AddAction(()=>{
			V2RoleSelectView rs = V2RoleSelectView.CreateRoleView();
			AllView.stSelectRole = true;
			AllView.self.goSelectRole = rs.gameObject;
			rs.alertViewBehavriour = (ab,view)=>{
				switch(ab)
				{
				case MDAlertBehaviour.CLICK_OK:
					view.hiddenView();
					
					break;
				case MDAlertBehaviour.DID_HIDDEN:
					Time.timeScale = 1;
					Destroy(view.gameObject);
					AllView.stSelectRole = false;
					AllView.self.goSelectRole = null;
					bShowSelectRoleAlert = false;
					WGAlertManager.Self.RemoveHeadAndShowNext();
					break;
				}
			};
			rs.showView();
			Time.timeScale = 0;
		});
		WGAlertManager.Self.ShowNext();


		
	}
//	public void ShowSelectRole(bool needJudg)
//	{
//		if( _DataPlayer.mR == 0 || !needJudg)
//		{
//			WGAlertManager.Self.AddAction(()=>{
//				V2RoleSelectView rs = V2RoleSelectView.CreateRoleView();
//				AllView.stSelectRole = true;
//				AllView.self.goSelectRole = rs.gameObject;
//				rs.alertViewBehavriour = (ab,view)=>{
//					switch(ab)
//					{
//					case MDAlertBehaviour.CLICK_OK:
//						view.hiddenView();
//						
//						break;
//					case MDAlertBehaviour.DID_HIDDEN:
//						Destroy(view.gameObject);
//						AllView.stSelectRole  =false;
//						AllView.self.goSelectRole = null;
//						ShowBuyRole();
//						WGAlertManager.Self.RemoveHeadAndShowNext();
//						break;
//					}
//				};
//				rs.showView();
//			});
//			WGAlertManager.Self.ShowNext();
//		}
//		else
//		{
//			AllView.stSelectRole  =false;
//			ShowOutLineCoin();
//		}
//	
//	}
	bool bShowBuyRoleAlert = false;
	public void ShowBuyRoleAlert()
	{
		if(bShowBuyRoleAlert)return;
		bShowBuyRoleAlert =true;
		WGAlertManager.Self.AddAction(()=>{
			if(_DataPlayer.mR == 0)
			{
				WGDataController _dataCtrl = WGDataController.Instance;
				YHMDPayData payData=_dataCtrl.getYHMDPay(YHPayType.ROLE3in1);
				float costMenoy=payData.payCost;
				string payKey=payData.payKey.ToString();
//				float costMenoy = 30;
//				string payKey = "116";
//				if(YeHuoSDK.bUsePayCode2)
//				{
//					costMenoy = 20;
//					payKey = "216";
//				}
				string okString ="ok";
				
				#if YES_OK
				string content = WGStrings.getFormateInt(1081,1002,8207,costMenoy.ToString());
				okString =  WGStrings.getText(1002);
				#elif YES_BUY
				string content = WGStrings.getFormateInt(1081,1094,8207,costMenoy.ToString());
				okString =  WGStrings.getText(1094);
				#elif YES_GET
				string content = WGStrings.getFormateInt(1081,1077,8207,costMenoy.ToString());
				okString =  WGStrings.getText(1077);
				#elif YES_QueRen
				string content = WGStrings.getFormateInt(1081,1106,payData.showText,costMenoy.ToString());
				okString =  WGStrings.getText(1106);
				#else
				string content = WGStrings.getFormateInt(1081,1077,payData.showText,costMenoy.ToString());
				okString =  WGStrings.getText(1077);
				#endif
				
				V2NewUseRewardView nr = V2NewUseRewardView.CreateNewUseView();
				AllView.stPayforRole = true;
				AllView.self.goPayforRole = nr.gameObject;
				nr.freshUI(content,okString);
				nr.alertViewBehavriour = (ab,view) =>{
					switch(ab)
					{
					case MDAlertBehaviour.CLICK_OK:
						YeHuoSDK.YHPay(payKey,costMenoy,0,(success)=>{
							view.hiddenView();
							if(success)
							{
								_DataPlayer.mR = 1;
								WGGameWorld.Instance.PlayerGetCoin(100000);
							}
						});
						
						break;
					case MDAlertBehaviour.CLICK_CANCEL:
						view.hiddenView();
						
						break;
					case MDAlertBehaviour.DID_HIDDEN:
						Time.timeScale = 1;
						Destroy(view.gameObject);
						AllView.stPayforRole = false;
						AllView.self.goPayforRole = null;
						bShowBuyRoleAlert =false;
						WGGameUIView.Instance.mPlayerInfoView.FreshPlayerHead();
						WGAlertManager.Self.RemoveHeadAndShowNext();

						break;
					}
				};
				Time.timeScale = 0;
				nr.showView();
			
			}
			else
			{
				bShowBuyRoleAlert =false;
				WGGameUIView.Instance.mPlayerInfoView.FreshPlayerHead();
				WGAlertManager.Self.RemoveHeadAndShowNext();
			}
		});
		WGAlertManager.Self.ShowNext();
	}
//	public void ShowBuyRole(bool goon = true)
//	{
//		if(AllView.stPayforRole)return;
//		if(_DataPlayer.mR == 0 && !AllView.stRichReward)
//		{
//			float costMenoy = 30;
//			string payKey = "116";
//			if(YeHuoSDK.bUseExt)
//			{
//				costMenoy = 20;
//				payKey = "216";
//			}
//			string okString ="ok";
//
//#if YES_OK
//			string content = WGStrings.getFormateInt(1081,1002,8207,costMenoy.ToString());
//			okString =  WGStrings.getText(1002);
//#elif YES_BUY
//			string content = WGStrings.getFormateInt(1081,1094,8207,costMenoy.ToString());
//			okString =  WGStrings.getText(1094);
//#elif YES_GET
//			string content = WGStrings.getFormateInt(1081,1077,8207,costMenoy.ToString());
//			okString =  WGStrings.getText(1077);
//#else
//			string content = WGStrings.getFormateInt(1081,1077,8207,costMenoy.ToString());
//			okString =  WGStrings.getText(1077);
//#endif
//
//			V2NewUseRewardView nr = V2NewUseRewardView.CreateNewUseView();
//			AllView.stPayforRole = true;
//			AllView.self.goPayforRole = nr.gameObject;
//			nr.freshUI(content,okString);
//			nr.alertViewBehavriour = (ab,view) =>{
//				switch(ab)
//				{
//				case MDAlertBehaviour.CLICK_OK:
//					YeHuoSDK.YHPay(payKey,costMenoy,0,(success)=>{
//						view.hiddenView();
//						if(success)
//						{
//							_DataPlayer.mR = 1;
//							WGGameWorld.Instance.PlayerGetCoin(100000);
//						}
//					});
//
//					break;
//				case MDAlertBehaviour.CLICK_CANCEL:
//					view.hiddenView();
//
//					break;
//				case MDAlertBehaviour.DID_HIDDEN:
//					AllView.stPayforRole = false;
//					Destroy(view.gameObject);
//					AllView.self.goPayforRole = null;
//					if(goon)
//					{
//						ShowOutLineCoin();
//					}
//					WGGameUIView.Instance.mPlayerInfoView.FreshPlayerHead();
//					break;
//				}
//			};
//			nr.showView();
//		}
//		else
//		{
//			WGGameUIView.Instance.mPlayerInfoView.FreshPlayerHead();
//		}
//	}
	void ShowLuckyDaLiBaoAlert()
	{

		WGAlertManager.Self.AddAction(()=>{
			WGDataController _dataCtrl = WGDataController.Instance;
			YHMDPayData payData=_dataCtrl.getYHMDPay(YHPayType.LUCKY);
			float costMenoy=payData.payCost;
			string payKey=payData.payKey.ToString();
//			float costMenoy = 30;
//			string payKey = "102";
//			if(YeHuoSDK.bUsePayCode2)
//			{
//				costMenoy = 20;
//				payKey = "202";
//			}
			YHGotRewardView rdview = YHGotRewardView.CreateGotRewardView();
			rdview.mRType = YHRewardType.Lucky;
			
			SDK.AddChild(rdview.gameObject,WGRootManager.Self.goRootTopUI);
			
			rdview.FreshRewardCell(_dataCtrl.mAllReward.lucky);
			#if YES_OK
			string content = WGStrings.getFormateInt(1081,1002,1079,costMenoy.ToString());
			#elif YES_BUY
			string content = WGStrings.getFormateInt(1081,1094,1079,costMenoy.ToString());
			#elif YES_GET
			string content = WGStrings.getFormateInt(1081,1077,1079,costMenoy.ToString());
			#elif YES_QueRen
			string content = WGStrings.getFormateInt(1081,1106,payData.showText,costMenoy.ToString());
			#else
			string content = WGStrings.getFormateInt(1081,1077,payData.showText,costMenoy.ToString());
			#endif
			rdview.FreshWithMsg(payData.showText,content,true);
			rdview.alertViewBehavriour =(ab,view)=>{
				switch(ab)
				{
				case MDAlertBehaviour.CLICK_OK:
					
					YeHuoSDK.YHPay(payKey,costMenoy,0,(success)=>{
						view.hiddenView();
						if(success)
						{
							_DataPlayer.szBigReward.Add(2);
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
					Time.timeScale=1;
					
					WGAlertManager.Self.RemoveHeadAndShowNext();
					
					break;
				}
			};
			Time.timeScale=0;
			rdview.showView();
			BCSoundPlayer.Play(MusicEnum.showReward,1f);
		});
		WGAlertManager.Self.ShowNext();

		
		
	}
	public void ShowFingerGift(){
		if(_DataPlayer.guDingForever>0){
			return ;
		}else{
			//WGAlertManager.Self.AddAction(()=>{
				WGDataController _dataCtrl = WGDataController.Instance;

				YHMDPayData payData=_dataCtrl.getYHMDPay(YHPayType.FINGER);
				float costMenoy=payData.payCost;
				string payKey=payData.payKey.ToString();
				D04PowerBuyView bv=D04PowerBuyView.CreatePowerBuyView(true);
				//rdview.mRType = YHRewardType.Other;
				
				//SDK.AddChild(rdview.gameObject,WGRootManager.Self.goRootTopUI);
				//rdview.spriteFinger.gameObject.SetActive(true);
				//rdview.FreshRewardCell(_dataCtrl.mAllReward.lucky);
				#if YES_OK
				string content = WGStrings.getFormateInt(1081,1002,1079,costMenoy.ToString());
				#elif YES_BUY
				string content = WGStrings.getFormateInt(1081,1094,1079,costMenoy.ToString());
				#elif YES_GET
				string content = WGStrings.getFormateInt(1081,1077,1079,costMenoy.ToString());
				#elif YES_QueRen
				string content = WGStrings.getFormateInt(1081,1106,payData.showText,costMenoy.ToString());
				#else
				string content = WGStrings.getFormateInt(1081,1077,payData.showText,costMenoy.ToString());
				#endif
				string okString =  WGStrings.getText(1106);
				bv.FreshUI(content,okString,true);
				bv.alertViewBehavriour =(ab,view)=>{
					switch(ab)
					{
					case MDAlertBehaviour.CLICK_OK:
						YeHuoSDK.YHPay(payKey,costMenoy,0,(success)=>{
							view.hiddenView();
							if(success)
							{
								_DataPlayer.guDingForever=1;
								_DataPlayer.releaseGuding=1;
								WGSkillController.Instance.ReleaseSkillWithID(WGDefine.SK_GuDing);
							}
						});
						break;
					case MDAlertBehaviour.CLICK_CANCEL:
						view.hiddenView();
						break;
					case MDAlertBehaviour.DID_HIDDEN:
						Destroy(view.gameObject);
						Time.timeScale = 1;
						break;
					}
				};
				bv.showView();
				Time.timeScale=0;
				BCSoundPlayer.Play(MusicEnum.showReward,1f);
			//});
			//WGAlertManager.Self.ShowNext();

		}
	}
	public void ShowLuckyDaLiBao()
	{
		if(YeHuoSDK.bUsePayCode2)
		{
		}
		else if(!_DataPlayer.szBigReward.Contains(2))//luck da li bao
		{
			ShowLuckyDaLiBaoAlert();
//			WGAlertManager.Self.AddAction(()=>{
//				float costMenoy = 30;
//				string payKey = "102";
//				if(YeHuoSDK.bUseExt)
//				{
//					costMenoy = 20;
//					payKey = "202";
//				}
//				YHGotRewardView rdview = YHGotRewardView.CreateGotRewardView();
//				rdview.mRType = YHRewardType.Lucky;
//
//				SDK.AddChild(rdview.gameObject,WGRootManager.Self.goRootTopUI);
//				
//				rdview.FreshRewardCell(_dataCtrl.mAllReward.lucky);
//				#if YES_OK
//				string content = WGStrings.getFormateInt(1081,1002,1079,costMenoy.ToString());
//				#elif YES_BUY
//				string content = WGStrings.getFormateInt(1081,1094,1079,costMenoy.ToString());
//				#elif YES_GET
//				string content = WGStrings.getFormateInt(1081,1077,1079,costMenoy.ToString());
//				#else
//				string content = WGStrings.getFormateInt(1081,1077,1079,costMenoy.ToString());
//				#endif
//				rdview.FreshWithMsg(WGStrings.getText(1079),content,true);
//				rdview.alertViewBehavriour =(ab,view)=>{
//					switch(ab)
//					{
//					case MDAlertBehaviour.CLICK_OK:
//
//						YeHuoSDK.YHPay(payKey,costMenoy,0,(success)=>{
//							view.hiddenView();
//							if(success)
//							{
//								_DataPlayer.szBigReward.Add(2);
//								rdview.GetAllReward();
//								WGGameUIView.Instance.freshSkillNum();
//								WGGameUIView.Instance.freshPlayerUI(UI_FRESH.BCOIN | UI_FRESH.COIN);
//							}
//						});
//
//						break;
//					case MDAlertBehaviour.CLICK_CANCEL:
//						view.hiddenView();
//
//						break;
//					case MDAlertBehaviour.DID_HIDDEN:
//						Destroy(view.gameObject);
//						Time.timeScale=1;
//
//						WGAlertManager.Self.RemoveHeadAndShowNext();
//
//						break;
//					}
//				};
//				Time.timeScale=0;
//				rdview.showView();
//				BCSoundPlayer.Play(MusicEnum.showReward,1f);
//			});
//			WGAlertManager.Self.ShowNext();
		}


	}

	void CheckTBOrder()
	{
		string order = ShopOrderManager.getInstance().getUnfinishedOrder();
		if(!string.IsNullOrEmpty(order))
		{
			WGAlertViewController.Self.showConnecting();
			TBSDK.TBCheckOrder(order);
		}
	}


	// Update is called once per frame
	void Update () {
		mFrameCount++;
		if(Time.timeScale>0)
		{
			ProcessDropObj();
		}
		ProcessAddCoin();
		ProcessSpecialCoin();
		if(mFrameCount%2==0 && Time.timeScale>0)
		{
			ProcessRewardDrop();
		}

		if(mFrameCount>=1000)
		{
			mFrameCount = 0;
		}
		if(keyEscape ==0 &&( Input.GetKeyUp(KeyCode.Escape)||Input.GetKeyUp(KeyCode.Home)))
		{

			keyEscape = 1;

			if(!_DataPlayer.szBigReward.Contains(3))//tu hao da li bao
			{
				if(AllView.stSelectRole && AllView.self.goSelectRole!=null)
				{
					MDBaseAlertView view = AllView.self.goSelectRole.GetComponent<MDBaseAlertView>();
					view.hiddenView();
				}
				else if(AllView.stPayforRole && AllView.self.goPayforRole != null)
				{
					MDBaseAlertView view = AllView.self.goPayforRole.GetComponent<MDBaseAlertView>();
					view.hiddenView();
				}
				if(YeHuoSDK.bShowRichGift){//tuhao
					ShowRichReward((ab)=>{
						switch(ab)
						{
						case MDAlertBehaviour.WILL_SHOW:
							WGAlertManager.Self.bPause = true;
							break;
						case MDAlertBehaviour.DID_HIDDEN:
							WGAlertManager.Self.bPause = false;
							ApplicactionWillExit();
							break;
						}
					});
				}else{
					ApplicactionWillExit();
				}

			}
			else
			{
				ApplicactionWillExit();
			}
		}
	}
	/// <summary>
	/// Shows the rich reward.
	/// </summary>
	/// <param name="exit">If set to <c>true</c> exit.</param>
	public void ShowRichReward(System.Action<MDAlertBehaviour> myCallBack = null)
	{
		WGDataController _dataCtrl = WGDataController.Instance;
		YHMDPayData payData=_dataCtrl.getYHMDPay(YHPayType.RICK);
		float costMenoy=payData.payCost;
		string payKey=payData.payKey.ToString();
//		float costMenoy = 30;
//		string payKey = "103";
//		if(YeHuoSDK.bUsePayCode2)
//		{
//			costMenoy = 19;
//			payKey = "203";
//		}

		YHGotRewardView rdview = YHGotRewardView.CreateGotRewardView();
		AllView.stRichReward = true;
		rdview.mRType = YHRewardType.Rich;
		WGRootManager.Self.goRootTopUI1.ESetActive(true);
		SDK.AddChild(rdview.gameObject,WGRootManager.Self.goRootTopUI1);
		#if YES_OK
		string content = WGStrings.getFormateInt(1081,1002,1085,costMenoy.ToString());
		#elif YES_BUY
		string content = WGStrings.getFormateInt(1081,1094,1085,costMenoy.ToString());
#elif YES_GET
		string content = WGStrings.getFormateInt(1081,1077,1085,costMenoy.ToString());
		#elif YES_QueRen
		string content = WGStrings.getFormateInt(1081,1106,payData.showText,costMenoy.ToString());
		#else
		string content = WGStrings.getFormateInt(1081,1077,payData.showText,costMenoy.ToString());
		#endif
		rdview.FreshRewardCell(_dataCtrl.mAllReward.rich);
		rdview.FreshWithMsg(payData.showText,content,true);
		rdview.alertViewBehavriour =(ab,view)=>{
			if(myCallBack != null)
			{
				myCallBack(ab);
			}
			switch(ab)
			{
			case MDAlertBehaviour.CLICK_OK:
				
				YeHuoSDK.YHPay(payKey,costMenoy,0,(success)=>{
					view.hiddenView();
					if(success)
					{
						_DataPlayer.szBigReward.Add(3);
						
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
			{
				Destroy(view.gameObject);
				AllView.stRichReward = false;
			}
				break;
			}
		};
		rdview.showView();
		BCSoundPlayer.Play(MusicEnum.showReward,1f);
	}
	public void ShowJewelSupplementView(System.Action<MDAlertBehaviour> myCallBack = null)
	{
		WGDataController _dataCtrl = WGDataController.Instance;
		YHMDPayData payData=_dataCtrl.getYHMDPay(YHPayType.JEWEL400);
		float costMenoy=payData.payCost;
		string payKey=payData.payKey.ToString();
//		float costMenoy = 30;
//		string payKey = "111";
//		if(YeHuoSDK.bUsePayCode2)
//		{
//			costMenoy = 20;
//			payKey = "211";
//		}
		YHGotRewardView rdview = YHGotRewardView.CreateGotRewardView();
		rdview.mRType = YHRewardType.Jewel;
		
		SDK.AddChild(rdview.gameObject,WGRootManager.Self.goRootTopUI);
		rdview.FreshRewardCell(_dataCtrl.mAllReward.jewel);
		#if YES_OK
		string content = WGStrings.getFormateInt(1081,1002,1099,costMenoy.ToString());
		#elif YES_BUY
		string content = WGStrings.getFormateInt(1081,1094,1099,costMenoy.ToString());
#elif YES_GET
		string content = WGStrings.getFormateInt(1081,1077,1099,costMenoy.ToString());
#elif YES_QueRen
		string content = WGStrings.getFormateInt(1081,1106,payData.showText,costMenoy.ToString());
		#else
		string content = WGStrings.getFormateInt(1081,1077,payData.showText,costMenoy.ToString());
		#endif
		rdview.FreshWithMsg(payData.showText,content,true);
		rdview.alertViewBehavriour =(ab,view)=>{
			if(myCallBack != null)
			{
				myCallBack(ab);
			}
			switch(ab)
			{
			case MDAlertBehaviour.CLICK_OK:
				YeHuoSDK.YHPay(payKey,costMenoy,0,(success)=>{
					view.hiddenView();
					if(success)
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
				Time.timeScale=1;
				break;
			}
		};
		Time.timeScale=0;
		rdview.showView();
		BCSoundPlayer.Play(MusicEnum.showReward,1f);
	}
	 void ApplicactionWillExit()
	{
		YeHuoSDK.ApplictionExit();
		mnIvokeBlock.InvokeBlock (0.5f, () => {
			keyEscape = 0;
		});
	}
	public void RemoveAd()
	{
		if(_DataPlayer.DelAD == 0)
		{
			#if Add_AD
			IOSAD.ShowAdView(false);
			#endif
			_DataPlayer.DelAD = 1;

			cs_BearManage.RemoveAD();
			cs_BearManage.csThrow.RemoveAD();
		}

	}
	/// <summary>
	/// Shows the coin supplement view.
	/// </summary>
	public void ShowCoinSupplementView(System.Action<MDAlertBehaviour> myCallBack = null)
	{
		WGDataController _dataCtrl = WGDataController.Instance;
		YHMDPayData payData=_dataCtrl.getYHMDPay(YHPayType.COIN);
		float costMenoy=payData.payCost;
		string payKey=payData.payKey.ToString();
//		float costMenoy = 30;
//		string payKey = "105";
//		if(YeHuoSDK.bUsePayCode2)
//		{
//			costMenoy = 20;
//			payKey = "205";
//		}
		YHGotRewardView rdview = YHGotRewardView.CreateGotRewardView();
		rdview.mRType = YHRewardType.Coin;
		
		SDK.AddChild(rdview.gameObject,WGRootManager.Self.goRootTopUI);
		rdview.FreshRewardCell(_dataCtrl.mAllReward.coin);
#if YES_OK
		string content = WGStrings.getFormateInt(1081,1002,1087,costMenoy.ToString());
#elif YES_BUY
		string content = WGStrings.getFormateInt(1081,1094,1087,costMenoy.ToString());
#elif YES_GET
		string content = WGStrings.getFormateInt(1081,1077,1087,costMenoy.ToString());
#elif YES_QueRen
		string content = WGStrings.getFormateInt(1081,1106,payData.showText,costMenoy.ToString());
#else
		string content = WGStrings.getFormateInt(1081,1077,payData.showText,costMenoy.ToString());
#endif
		rdview.FreshWithMsg(payData.showText,content,true);
		rdview.alertViewBehavriour =(ab,view)=>{
			if(myCallBack != null)
			{
				myCallBack(ab);
			}
			switch(ab)
			{
			case MDAlertBehaviour.CLICK_OK:
				YeHuoSDK.YHPay(payKey,costMenoy,0,(success)=>{
					view.hiddenView();
					if(success)
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
				Time.timeScale=1;
				break;
			}
		};
		Time.timeScale=0;
		rdview.showView();
		BCSoundPlayer.Play(MusicEnum.showReward,1f);
	}
	void ResetAD()
	{
		#if Add_AD
		IOSAD.ShowAdView(true);
		#endif
		cs_BearManage.ResetAD();
		cs_BearManage.csThrow.ResetAD();
	}
	public void NoCoinTipCan()
	{
		InvokeBlock(30,()=>{
			bNoCoinTip = false;
		});
	}
	public void ResetGame()
	{
		ResetAD();

		int num = 0;

		for(int i=0;i<cs_ObjManager._szLiveCoin.Count;i++)
		{
			Destroy(cs_ObjManager._szLiveCoin[i]);
		}
		cs_ObjManager._szLiveCoin.Clear();


		num = _DataCoin.CoinPos.Count;
		for(int i = 0; i<num;i++)
		{
			cs_ObjManager.BCGameObjFactory(WGDefine.CommonCoin,SDK.toV3(_DataCoin.CoinPos[i]),new Vector3(0,0,180),Vector3.zero);
		}

	}
	void DailyRewardCheckAlert()
	{
		int dayOfYear = Core.now.DayOfYear;
		int lastDay = _DataPlayer.lastDailyRewardTime.DayOfYear;
		int CtDay = _DataPlayer.ContinuousDay;
//				#if TEST
//				dayOfYear = 10;
//				lastDay = 9;
//				#endif
		//
		if(dayOfYear >= lastDay+1)//连续登陆
		{

			WGAlertManager.Self.AddAction(()=>{
				if(dayOfYear >lastDay+1)
				{
					_DataPlayer.ContinuousDay = 0;
					DataPlayerController.getInstance().saveDataPlayer();
				}
				//_DataPlayer.ContinuousDay ++;
				
				//if(_DataPlayer.ContinuousDay>7)_DataPlayer.ContinuousDay = 1;

				WGDailyRewardView drview = WGDailyRewardView.CreateDailyView();
				if(_DataPlayer.ContinuousDay>=7){_DataPlayer.ContinuousDay=0;
					DataPlayerController.getInstance().saveDataPlayer();
				}
				drview.freshWithDailyReward(_DataPlayer.ContinuousDay,false);
				
				drview.alertViewBehavriour =(ab,view)=>{
					WGGameUIView.Instance.freshMenuButton(3);
					switch(ab)
					{
					case MDAlertBehaviour.CLICK_OK:
						view.hiddenView();
						_DataPlayer.ContinuousDay ++;
						//if(_DataPlayer.ContinuousDay>=7)_DataPlayer.ContinuousDay = 0;
						DataPlayerController.getInstance().saveDataPlayer();
						break;
					case MDAlertBehaviour.DID_HIDDEN:
						Destroy(view.gameObject);
						WGAlertManager.Self.RemoveHeadAndShowNext();
						break;
					}
				};
				BCSoundPlayer.Play(MusicEnum.showReward,1f);
				drview.showView();
			});
			WGAlertManager.Self.ShowNext();
		}
//		else
//		{
//			_DataPlayer.lastDailyRewardTime = Core.now;
//			_DataPlayer.ContinuousDay = 0;
//
//		}
	}
//	void DailyRewardCheck()
//	{
//		int dayOfYear = Core.now.DayOfYear;
//		int lastDay = _DataPlayer.lastDailyRewardTime.DayOfYear;
//		int CtDay = _DataPlayer.ContinuousDay;
////		#if TEST
////		dayOfYear = 10;
////		lastDay = 9;
////		#endif
////
//		if(dayOfYear >= lastDay+1)//连续登陆
//		{
//			if(dayOfYear >lastDay+1)
//			{
//				_DataPlayer.ContinuousDay = 0;
//			}
//			_DataPlayer.ContinuousDay ++;
//
//			if(_DataPlayer.ContinuousDay>7)_DataPlayer.ContinuousDay = 1;
//
//			WGDailyRewardView drview = WGDailyRewardView.CreateDailyView();
//
//			drview.freshWithDailyReward(_DataPlayer.ContinuousDay);
//
//			drview.alertViewBehavriour =(ab,view)=>{
//				switch(ab)
//				{
//				case MDAlertBehaviour.CLICK_OK:
//					view.hiddenView();
//					break;
//				case MDAlertBehaviour.DID_HIDDEN:
//					Destroy(view.gameObject);
////					XinShouLiBao();
//					break;
//				}
//			};
//			BCSoundPlayer.Play(MusicEnum.showReward,1f);
//			drview.showView();
//
//		}
//		else
//		{
//			_DataPlayer.lastDailyRewardTime = Core.now;
//			_DataPlayer.ContinuousDay = 0;
////			XinShouLiBao();
//		}
//	}
	void XinShouLiBaoAlert()
	{
		WGAlertManager.Self.AddAction(()=>{
			YHMDPayData payData=WGDataController.Instance.getYHMDPay(YHPayType.NEW_Player);
			float costMenoy=payData.payCost;
			string payKey=payData.payKey.ToString();
//			float costMenoy = 30;
//			string payKey = "101";
//			if(YeHuoSDK.bUsePayCode2)
//			{
//				costMenoy = 20;
//				payKey = "201";
//			}
			YHGotRewardView rdview = YHGotRewardView.CreateGotRewardView();
			rdview.mRType = YHRewardType.NewUser;
			AllView.stNewPlayerReward = true;
			SDK.AddChild(rdview.gameObject,WGRootManager.Self.goRootTopUI);
			
			rdview.FreshRewardCell(_dataCtrl.mAllReward.newuser);
			#if YES_OK
			string content = WGStrings.getFormateInt(1081,1002,1090,costMenoy.ToString());
			#elif YES_BUY
			string content = WGStrings.getFormateInt(1081,1094,1090,costMenoy.ToString());
			#elif YES_GET
			string content = WGStrings.getFormateInt(1081,1077,1090,costMenoy.ToString());
#elif YES_QueRen
			string content = WGStrings.getFormateInt(1081,1106,payData.showText,costMenoy.ToString());
			#else
			string content = WGStrings.getFormateInt(1081,1077,payData.showText,costMenoy.ToString());
			#endif
			rdview.FreshWithMsg(payData.showText,content,true);
			rdview.alertViewBehavriour =(ab,view)=>{
				switch(ab)
				{
				case MDAlertBehaviour.CLICK_OK:
					
					YeHuoSDK.YHPay(payKey,costMenoy,50,(success)=>{
						view.hiddenView();
						if(success)
						{
							_DataPlayer.szBigReward.Add(1);
							rdview.GetAllReward();
							WGGameUIView.Instance.freshSkillNum();
							WGGameUIView.Instance.freshPlayerUI(UI_FRESH.BCOIN | UI_FRESH.COIN);
						}
					});
					
					break;
				case MDAlertBehaviour.CLICK_CANCEL:
					view.hiddenView();
					break;
				case MDAlertBehaviour.WILL_HIDDEN:
					AllView.stNewPlayerReward = false;
					break;
				case MDAlertBehaviour.DID_HIDDEN:
					Time.timeScale = 1;
					Destroy(view.gameObject);
					WGAlertManager.Self.RemoveHeadAndShowNext();

					break;
				}
			};
			rdview.showView();
			Time.timeScale = 0;
			BCSoundPlayer.Play(MusicEnum.showReward,1f);
		});
		WGAlertManager.Self.ShowNext();

	}

	void CheckDefenseTime()
	{

		double time1 = WGTime.DateTime2Unix(_DataPlayer.MyData);

		double time2 = WGTime.DateTime2Unix(Core.now);
		int delat = (int)Mathf.Abs((float)(time2-time1));


		_DataPlayer.defenseTime = _DataPlayer.defenseTime-delat;
		if(_DataPlayer.defenseTime>0)
		{
			cs_BearManage.BearTreeUp(_DataPlayer.defenseTime,true);
		}
		else{
			_DataPlayer.defenseTime = 0;
		}
		_DataPlayer.up777Time -=delat;
		if(_DataPlayer.up777Time<0)_DataPlayer.up777Time = 0;

		_DataPlayer.guDingTime -=delat;

		if(_DataPlayer.guDingTime<=0)
		{
			_DataPlayer.guDingTime = 0;
		}
		else
		{
			if(_DataPlayer.guDingForever==0)
			{

				mnIvokeBlock.InvokeBlock(0.2f,()=>{
					_DataPlayer.releaseGuding = 0;
					WGSkillController.Instance.ReleaseSkillWithID(WGDefine.SK_GuDing30);
				});

			}
		}
	}

	int OutLineReward()
	{


		System.TimeSpan  ts=  Core.now - _DataPlayer.MyData.ToLocalTime();

		int minutes = ts.Minutes+ts.Hours*60+ts.Days*24*60;

		mSecond = minutes*60+ts.Seconds;

		if(minutes>0&&_DataPlayer.Coin<WGConfig.AUTO_ADD_MAX)
		{
			if(minutes*10+_DataPlayer.Coin>WGConfig.AUTO_ADD_MAX)
			{
				int num = WGConfig.AUTO_ADD_MAX - _DataPlayer.Coin;
				PlayerGetCoin(num);
				return num;
			}
			else if(minutes*10+_DataPlayer.Coin<=WGConfig.AUTO_ADD_MAX)
			{
				PlayerGetCoin(minutes*10);
				return minutes*10;
			}
		}
		return 0;
	}

	void TimeCount()
	{

		ProceesDropCollectionEffect();

		if(_DataPlayer.defenseTime>0)
		{
			_DataPlayer.defenseTime--;
		}
		if(_DataPlayer.up777Time>0)
		{
			_DataPlayer.up777Time--;
		}
		if(_DataPlayer.guDingTime>0)
		{
			_DataPlayer.guDingTime--;
		}
		if(_DataPlayer.guDingTime<=0)
		{
			cs_BearManage.csThrow.CheckStaticWeapon();
		}
		mSecond++;

		if(mSecond>=_bossDisapperTime)//disapperTime <reloadTime
		{
			if(cs_BearManage.BossDisappear())//如果之前存在,则消失成功
			{
				mSecond =0;
			}
		}

		if(mSecond >=_bossReloadTime)
		{
			//WG.SLog("TimeCount boss reload time");
			bBossResurrection = true;
			mSecond = 0;
			BCSoundPlayer.Play(MusicEnum.bossWarning);
		}

	}

	#region shaking
	public void BearCoinShaking(float time,int skid){

		InvokeRepeating("CameraShaking",0.12f,0.04f);
		InvokeRepeating("Shaking",0.12f,0.8f);
		if(skid == WGDefine.SK_YunShi)
		{
			InvokeRepeating("MeteoriteEffect",0.12f,0.8f);
		}
		else
		{
			GameObject go = Instantiate(pbEarthQuake) as GameObject;
		}
		Invoke("RsetCamera",time);
		//		cs_ObjManager.CoinShaking();
	}
	void CameraShaking()
	{

		CameraShakingAngel(0.6f);
	}

	void CameraShakingAngel(float shakingAngel)
	{
		float x = Random.Range(-shakingAngel,shakingAngel);
		float y = Random.Range(-shakingAngel,shakingAngel);
		float z = Random.Range(-shakingAngel,shakingAngel);

		CoinCamera.transform.eulerAngles = v3InitCamera+(new Vector3(x,y,z));
	}

	void RsetCamera()
	{
		CancelInvoke("Shaking");
		CancelInvoke("CameraShaking");
		CancelInvoke("MeteoriteEffect");
		CoinCamera.transform.eulerAngles = v3InitCamera;
	}
	public void Shaking()
	{
		cs_ObjManager.CoinShaking();
	}
	void MeteoriteEffect()
	{
		GameObject go = Instantiate(pbShakeStone) as GameObject;
		Vector3 v31= new Vector3(Random.Range(8f,20f),Random.Range(20f,35f),-0.5f);
		Vector3 v32= new Vector3(Random.Range(-13f,-21f),Random.Range(0,8f),-0.5f);
		go.transform.position = v31;
		TweenPosition tp = go.GetComponent<TweenPosition>();
		tp.from = v31;
		tp.to = v32;
	}
	#endregion shaking
	#if TEST
	void OnGUI()
	{
//		if(GUILayout.Button("Collection"))
//		{
//			for(int i=0;i<_dataCtrl.szCollectionObj.Count;i++)
//			{
//				AddCollection(_dataCtrl.szCollectionObj[i].ID,2);
//			}
//		}
//		else if(GUILayout.Button("Create"))
//		{
//			int[] myID = new int[]{
//				1003,1004,1005,1006,1007,1008,1009,2001,2002,2003,2004,2005,2007,2008,2009,2010,2011
//			};
//			for(int i=0;i<myID.Length;i++)
//			{
//				AddCoin(myID[i],2);
//			}
//		}
//		else if(GUILayout.Button("Create10*5"))
//		{
//			AddCoin(1004,10);
//		}
//		else if(GUILayout.Button("Create10*3"))
//		{
//			AddCoin(1003,10);
//		}
//		else if(GUILayout.Button("Create10*2"))
//		{
//			AddCoin(1002,10);
//		}
	}
	#endif

	public void AddCoin(int id)
	{
		cs_ObjManager.BCGameObjFactory(id,cs_BearManage.ValidPostion,Vector3.zero,Vector3.zero);
	}
	public void AddCoin(int id,Vector3 ro)
	{
		cs_ObjManager.BCGameObjFactory(id,cs_BearManage.ValidPostion,ro,Vector3.zero);
	}


	public void AddCoin(int id, int num,bool head=false)
	{
		for(int i = 0; i<num; i++)
		{
			if(head)
			{
				szRewardDrop.Insert(0,new HKReward(id));
			}
			else
			{
				szRewardDrop.Add(new HKReward(id));
			}
		}
	}
	public void ChangeAllBearCoin()
	{


		int num = 0;
		MDDataCoin dc = DataCoinController.getInstance().data;
		if(dc.IsFirstLoad)
		{
			dc.IsFirstLoad = false;
			num = _DataCoin.CoinPos.Count;
			for(int i = 0; i<num;i++)
			{
				cs_ObjManager.BCGameObjFactory(WGDefine.CommonCoin,SDK.toV3(_DataCoin.CoinPos[i]),new Vector3(0,0,180),Vector3.zero);
			}
		}
		else
		{
			num = dc.CoinID.Count;
			for(int i=0;i<num;i++)
			{
				GameObject go = cs_ObjManager.BCGameObjFactory(dc.CoinID[i],SDK.toV3(dc.CoinPos[i]),new Vector3(0,0,180),Vector3.zero);
				go.transform.localEulerAngles = SDK.toV3(dc.CoinRoto[i]);
			}
		}
	}

	public void CreatePapapa(Vector3 po)
	{
		GameObject tem = (GameObject)Instantiate(goPapapa,po,Quaternion.LookRotation(Vector3.forward));
		Destroy(tem,0.2f);
	}
	public void CreateMiaosha(Vector3 po)
	{
		GameObject tem = (GameObject)Instantiate(goMiaoSha,po,Quaternion.LookRotation(Vector3.forward));
		Destroy(tem,0.2f);
	}

	public bool PlayerGetCoin(int num)
	{
		if(num!=0)
		{

			if(_DataPlayer.Coin+num<0)
			{
				return false;
			}
			_DataPlayer.Coin+=num;

			WGGameUIView.Instance.freshPlayerUI(UI_FRESH.COIN);

			return true;
		}
		return true;
	}


	public int PlayGetExp(int exp)
	{

		_DataPlayer.Exp	+= exp;
		////WG.SLog("PlayGetExp======"+exp);
//		Debug.Log("nextLevel"+_NextLevel);
		if(_DataPlayer.Level>=WGConfig.MAX_LEVEL){
			return 0;
		}
		if(mDataCtrl.GetLevelUpReward(_NextLevel).exp<= _DataPlayer.Exp)
		{
			_DataPlayer.Level = _NextLevel;
			if(_NextLevel<WGConfig.MAX_LEVEL){
				_NextLevel+=1;
			}else{
				//_DataPlayer.Exp-=mDataCtrl.GetLevelUpReward(WGConfig.MAX_LEVEL).exp;
				_NextLevel=WGConfig.MAX_LEVEL;
			}

//			TDGAAccount account = TDGAAccount.SetAccount(TalkingDataGA.GetDeviceId());
//			account.SetLevel(_DataPlayer.Level);
			LevelUPReward lr = mDataCtrl.GetLevelUpReward(_DataPlayer.Level);
			
			if(lr!=null && lr.drop_reward!=null && lr.drop_reward.Count>0)
			{
				for(int i=0,max=lr.drop_reward.Count;i<max;i++)
				{
					AddReward(lr.drop_reward[i]);
				}
			}
			if(lr!=null && lr.special!=null && lr.special.Count>0)//level up
			{
				WGAlertManager.Self.AddAction(()=>{
					YHMDPayData payData=WGDataController.Instance.getYHMDPay(YHPayType.DOUBLE_reward);
					float costMenoy=payData.payCost;
					string payKey=payData.payKey.ToString();
//					float costMenoy = 30;
//					string payKey = "114";
//					if(YeHuoSDK.bUsePayCode2)
//					{
//						costMenoy = 20;
//						payKey = "214";
//					}
					#if YES_OK
					string content = WGStrings.getFormateInt(1081,1104,"",costMenoy.ToString());
					#elif YES_BUY
					string content = WGStrings.getFormateInt(1081,1104,"",costMenoy.ToString());
					#elif YES_GET
					string content = WGStrings.getFormateInt(1081,1101,"",costMenoy.ToString());
					#elif YES_QueRen
					string content = WGStrings.getFormateInt(1081,1104,"",costMenoy.ToString());
					#else
					string content = WGStrings.getFormateInt(1081,1104,"",costMenoy.ToString());
					#endif
					YHGotRewardView rdView = YHGotRewardView.CreateGotRewardView();
					rdView.mRType = YHRewardType.Levelup ;
					string showMsg="";
					if(YeHuoSDK.bShowDoubleGift){
						rdView.bDoubleReward = true;
					}else{
						content="";
						rdView.bDoubleReward = false;
					}

					SDK.AddChild(rdView.gameObject,WGRootManager.Self.goRootTopUI);
					rdView.FreshRewardCell(lr.special);
					rdView.FreshWithMsg(payData.showText,content,false,false);
					rdView.alertViewBehavriour =(ab2,view2)=>{
						switch(ab2)
						{
						case MDAlertBehaviour.CLICK_OK:
							if(view2.clickIndex == MDBaseAlertView.CLICK_OK1||view2.clickIndex == MDBaseAlertView.CLICK_OK)
							{
								view2.hiddenView();
								rdView.GetAllReward(false);
								WGGameUIView.Instance.freshSkillNum();
								WGGameUIView.Instance.freshPlayerUI(UI_FRESH.BCOIN | UI_FRESH.COIN);
							}
							else if(view2.clickIndex == MDBaseAlertView.CLICK_OK2)
							{
								YeHuoSDK.YHPay(payKey,costMenoy,0,(success)=>{
									view2.hiddenView();
									if(success)
									{
										rdView.GetAllReward(true);
										WGGameUIView.Instance.freshSkillNum();
										WGGameUIView.Instance.freshPlayerUI(UI_FRESH.BCOIN | UI_FRESH.COIN);
									}
								});
							}



							break;
						case MDAlertBehaviour.DID_HIDDEN:
							Destroy(view2.gameObject);
							Time.timeScale=1;
							WGAlertManager.Self.RemoveHead();
							WGAlertManager.Self.ShowNext();
							break;
						}
					};
					Time.timeScale=0;
					rdView.showView();
//					BCSoundPlayer.Play(MusicEnum.showReward);
				});
			}
			if(YeHuoSDK.bUsePayCode2)
			{
			}
			else if(!_DataPlayer.szBigReward.Contains(2) && _DataPlayer.Level%2==0)//luck da li bao
			{
				if(YeHuoSDK.bShowLuckGift){//uplevel lucky
					ShowLuckyDaLiBaoAlert();
				}

//				WGAlertManager.Self.AddAction(()=>{
//					float costMenoy = 30;
//					string payKey = "102";
////					if(YeHuoSDK.bUseExt)
////					{
////						costMenoy = 20;
////						payKey = "202";
////					}
//					YHGotRewardView rdview = YHGotRewardView.CreateGotRewardView();
//					rdview.mRType = YHRewardType.Lucky;
//					SDK.AddChild(rdview.gameObject,WGRootManager.Self.goRootTopUI);
//					
//					rdview.FreshRewardCell(_dataCtrl.mAllReward.lucky);
//					#if YES_OK
//					string content = WGStrings.getFormateInt(1081,1002,1079,costMenoy.ToString());
//#elif YES_BUY
//					string content = WGStrings.getFormateInt(1081,1094,1079,costMenoy.ToString());
//#elif YES_GET
//					string content = WGStrings.getFormateInt(1081,1077,1079,costMenoy.ToString());
//					#else
//					string content = WGStrings.getFormateInt(1081,1077,1079,costMenoy.ToString());
//					#endif
//					rdview.FreshWithMsg(WGStrings.getText(1079),content,true);
//					rdview.alertViewBehavriour =(ab,view)=>{
//						switch(ab)
//						{
//						case MDAlertBehaviour.CLICK_OK:
//
//							YeHuoSDK.YHPay(payKey,costMenoy,0,(success)=>{
//								view.hiddenView();
//								if(success)
//								{
//									_DataPlayer.szBigReward.Add(2);
//
//									rdview.GetAllReward();
//									WGGameUIView.Instance.freshSkillNum();
//									WGGameUIView.Instance.freshPlayerUI(UI_FRESH.BCOIN | UI_FRESH.COIN);
//								}
//							});
//
//							break;
//						case MDAlertBehaviour.CLICK_CANCEL:
//							view.hiddenView();
//							break;
//						case MDAlertBehaviour.DID_HIDDEN:
//							Destroy(view.gameObject);
//							Time.timeScale=1;
//							WGAlertManager.Self.RemoveHead();
//							WGAlertManager.Self.ShowNext();
//							break;
//						}
//					};
//					Time.timeScale=0;
//					rdview.showView();
//					BCSoundPlayer.Play(MusicEnum.showReward,1f);
//				});
			}
			if(YeHuoSDK.bUsePayCode2)
			{
			}
			else if(!_DataPlayer.szBigReward.Contains(1)&& _DataPlayer.Level%2==1)// new user da li bao
			{
				if(YeHuoSDK.bShowNewhandGift){//uplevel xinshou
					XinShouLiBaoAlert();
				}
			
			}

			BCSoundPlayer.Play(MusicEnum.levelup,1f);
			WGAlertManager.Self.AddAction(()=>{
				WGLevelUpViewController luvc = WGLevelUpViewController.CreaetViewController();


				luvc.alertViewBehavriour =(ab,view)=>{
					switch(ab)
					{
					case MDAlertBehaviour.DID_SHOW:
						mnIvokeBlock.InvokeBlock(2.5f,()=>{
							view.hiddenView();
						});
						break;
					case MDAlertBehaviour.DID_HIDDEN:
						{
							Destroy(view.gameObject);
							WGAlertManager.Self.RemoveHead();
							WGAlertManager.Self.ShowNext();
						}
						break;
					}
				};

				luvc.showView();
			});
			List<WGBearParam> szUnlockBear = mDataCtrl.getCurrentUnloackBear(_DataPlayer.Level);
			if(szUnlockBear!= null &&szUnlockBear.Count>0)
			{
				WGAlertManager.Self.AddAction(()=>{
					WGShowUnlockBear sub = WGShowUnlockBear.CreateUnlockBear();
					SDK.AddChild(sub.gameObject,WGRootManager.Self.goRootGameUI);
					sub.FreshUnlockBearView(szUnlockBear[0].id);
					sub.alertViewBehavriour = (ab1,view1)=>{
						switch(ab1)
						{
						case MDAlertBehaviour.CLICK_OK:
							view1.hiddenView();
							string contents="疯狂推金币";
							Hashtable content = new Hashtable();
							content["content"] = contents;
							content["image"] = "http://web.chuse123.com/EE04D07E0040.jpg";
							content["title"] = "疯狂推金币";
							content["description"] = "疯狂推金币";
							content["url"] = "http://mp.weixin.qq.com/s?__biz=MzAwMjY5NDUwNA==&mid=207913215&idx=1&sn=b53e73e4551269532cb90a81463f468e#rd";
							content["type"] = System.Convert.ToString((int)ContentType.News);
							content["siteUrl"] = "http://mp.weixin.qq.com/s?__biz=MzAwMjY5NDUwNA==&mid=207913215&idx=1&sn=b53e73e4551269532cb90a81463f468e#rd";
							content["site"] = "ShareSDK";
							content["musicUrl"] = "http://mp3.mwap8.com/destdir/Music/2009/20090601/ZuiXuanMinZuFeng20090601119.mp3";
							//开启，调用客户端授权
							content["disableSSOWhenAuthorize"] = false;
							content["shareTheme"] = "classic";//ShareTheme has only two value which are skyblue and classic
							
							ShareResultEvent evt = new ShareResultEvent(ShareResultHandler);
							ShareSDK.showShareMenu (null, content, 100, 100, MenuArrowDirection.Up, evt);
							break;
						case MDAlertBehaviour.CLICK_CANCEL:
							view1.hiddenView();
							break;
						case MDAlertBehaviour.DID_HIDDEN:
							Destroy(view1.gameObject);
							Time.timeScale=1;
							WGAlertManager.Self.RemoveHead();
							WGAlertManager.Self.ShowNext();
							break;
						}
					};
					Time.timeScale=0;
					sub.showView();
				});
			}


			WGAlertManager.Self.ShowNext();
		}
		if(exp !=0)
		{
			WGGameUIView.Instance.freshPlayerUI(UI_FRESH.EXP | UI_FRESH.LEVEL | UI_FRESH.BCOIN);
		}

		return _DataPlayer.Level;
	}

	void ShareResultHandler (ResponseState state, PlatformType type, Hashtable shareInfo, Hashtable error, bool end)
	{
		if (state == ResponseState.Success)
		{
			print ("share result :");
			print (MiniJSON.jsonEncode(shareInfo));
		}
		else if (state == ResponseState.Fail)
		{
			print ("fail! error code = " + error["error_code"] + "; error msg = " + error["error_msg"]);
		}
	}
	/// <summary>
	/// 在即将掉落的队列里面,增加一个掉落奖励!
	/// </summary>
	/// <param name="rd">长度为3的数组,rd[0]为掉落物品id,rd[1]为掉落数量,rd[2]为掉落物品的概率</param>
	public void AddReward(int[] rd,bool head = false)
	{
//		//WG.SLog("adReward====="+SDK.Serialize(rd));
		if(rd.Length>=3)
		{
			int rand = Random.Range(0,101);
			if(rand<=rd[2])
			{
				BCGameObjectType type = _dataCtrl.GetBCObjType(rd[0]);
				for(int i=0;i<rd[1];i++)
				{
					if(type == BCGameObjectType.Pack)
					{
						AddPackage(rd[0],head);
					}
					else
					{
//						qRewardQueue.Enqueue(new HKReward(rd[0]));
						if(head)
						{
							szRewardDrop.Insert(0,new HKReward(rd[0]));
						}
						else
						{
							szRewardDrop.Add(new HKReward(rd[0]));
						}
					}
				}
			}
		}
	}

	public void AddMorePackage(int pkID,int num)
	{
		for(int i=0;i<num;i++)
		{
			AddPackage(pkID);
		}
	}

	public void AddPackage(int id,bool head = false)
	{

		MDRewardPackage rp = null;
		if(_dataCtrl.dicRewardPackage.TryGetValue(id,out rp))
		{
//			//WG.SLog("addPackage====="+SDK.Serialize(rp));
			for(int i=0,max=rp.reward.Count;i<max;i++)
			{
				AddReward(rp.reward[i],head);
			}
		}
	}

	public GameObject AddCollection(int id)
	{
		if(id == 3003 || id== 3006 || id == 3009 || id == 3012 || id == 3015)
		{
			if(DataPlayerController.getInstance().getCollectionNum(id) >=8)
			{
				return null;
			}
			if(cs_ObjManager._szDeskCollection[id-3000]+DataPlayerController.getInstance().getCollectionNum(id)>=8)
			{
				return null;
			}
		}
		GameObject tem =  cs_ObjManager.BCGameObjFactory(id,cs_BearManage.CollectionValidPos,new Vector3(270,180,0),Vector3.zero);

		tem.transform.parent = cs_BearManage.BearCoinRoot.transform;

		return tem;
	}

	public void AddCollection(int id,int num,bool head=false)
	{
		for(int i=0;i<num;i++)
		{
			if(head)
			{
				szRewardDrop.Insert(0,new HKReward(id));
			}
			else
			{
				szRewardDrop.Add(new HKReward(id));
			}
		}
	}

	void ProcessRewardDrop()
	{
		if(szRewardDrop.Count>0)
		{

			HKReward rd= szRewardDrop[0];
			szRewardDrop.RemoveAt(0);


			BCGameObjectType type = WGDataController.Instance.GetBCObjType(rd.id);
			switch(type)
			{
			case BCGameObjectType.Coin:
			case BCGameObjectType.Item:
				AddCoin(rd.id,Quaternion.LookRotation(Vector3.left).eulerAngles);
				break;
			case BCGameObjectType.Collection:
				AddCollection(rd.id);
				break;
			case BCGameObjectType.ExpCoin:
				AddCoin(rd.id,Quaternion.LookRotation(Vector3.left).eulerAngles);
				break;
			case BCGameObjectType.Pack:
				AddPackage(rd.id);
				break;
			}
			CancelInvoke("OneCoinDropEnd");
			if(type == BCGameObjectType.Pack)
			{
				Invoke("OneCoinDropEnd",0.3f);
			}
			if(!bPlayCoinDrop)
			{
				bPlayCoinDrop = true;
//				InvokeRepeating("OneCoinDrop",0.1f,1.7f);
				Invoke("OneCoinDrop",0.02f);
				InvokeBlock(1.1f,()=>{
					bPlayCoinDrop = false;
				});
			}
		}
	}
	bool bPlayCoinDrop=false;
	void OneCoinDropEnd()
	{
		CancelInvoke("OneCoinDrop");
	}
	void OneCoinDrop()
	{
		BCSoundPlayer.Play(MusicEnum.coinDrop);
	}
	void ProceesDropCollectionEffect()
	{
		if(qDropCollection.Count>0 && !WGTipForCollectionView.Self.bOneShowing)
		{
			int id = qDropCollection.Dequeue();
			WGTipForCollectionView.Self.showTipCollection(id);
		}
	}
}