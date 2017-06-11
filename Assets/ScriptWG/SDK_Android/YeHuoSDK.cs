using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Unicom     //开启是联通电信版，否则是移动版
//YHAbout		//关于按钮

public class YeHuoSDK : WGMonoComptent {


	Dictionary<string,System.Action<bool>> dicPayBlock = new Dictionary<string, System.Action<bool>>();
	Dictionary<string,string> dicPayOrder = new Dictionary<string, string>();   
	public static int mTipType=0;
	public const string KEYSOUND = "BSOUND";
	public const string KEYMUSIC = "BMUSIC";
	public string mUserID="";
	/// <summary>
	/// 4兑话费,超级老虎机:默认关闭
	/// </summary>
	public static bool bTiger_HuaFei = false;
	/// <summary>
	/// 32普通老虎机  :默认关闭
	/// </summary>

	///<summary>
	/// :礼包弹框，默认关闭
	/// </summary>
	public static bool bShowGift=true; 

	public static bool bShowNewhandGift=false;
	public static bool bShowLuckGift=false;
	public static bool bShowPoweGift=false;
	public static bool bShowDoubleGift=false;
	public static bool bShowRichGift=false;
	public static bool bShow3RoalGift=false;
	public static bool bShowFinger=false;


#if MM
	public static bool bCommonTiger = false;
#else
	public static bool bCommonTiger = true;
#endif
	/// <summary>
	/// 8客服控制:默认开启
	/// </summary>
	public static bool bKeFu = false;
	/// <summary>
	/// 16文案是否清楚:默认清楚
	/// </summary>
	public static bool bMsgClear = true;


	/// <summary>
	/// 是否使用第二套计费点代码，联通电信使用第二套代码
	/// </summary>
	public static bool bUsePayCode2{
		get{
			bool buse = false;
			if(Self.imType == IMSI.CT || Self.imType == IMSI.UNI)
			{
				buse = true;
			}
#if Unicom
			buse = true;
#endif
			buse=false;
			return buse;

		}
	}


	public static YeHuoSDK Self;


	public IMSI imType = IMSI.CM;
	public string mMessage = "Log:\n";
	void Awake()
	{
		Self = this;
		DontDestroyOnLoad(this.gameObject);
		//getDeviceIMSI();
		//getUserID ();
	}
	void Start()
	{
		if(bShowGift){
			 bShowNewhandGift=true;
			bShowLuckGift=true;
			bShowPoweGift=true;
			bShowDoubleGift=true;
			bShowRichGift=true;
			bShow3RoalGift=true;
			bShowFinger=false;
			bTiger_HuaFei=false;
			bCommonTiger=false;
		}else{
			getSpecificFunction(4);
		}
		//getSpecificFunction(4);
		//getTipType();
		//StartCoroutine(InitQYPayOther());

	}
	IEnumerator InitQYPayOther()
	{
		yield return new WaitForSeconds(0.05f);
		getSpecificFunction(4);//DuiHuaFei
//		yield return new WaitForSeconds(0.05f);
//		getSpecificFunction(8);//客服控制
//		yield return new WaitForSeconds(0.05f);
//		getSpecificFunction(16);//文案是否清楚
//		yield return new WaitForSeconds(0.05f);
//		getSpecificFunction(32);//SuperTiger

	}
	void getDeviceIMSI()
	{
		string mImsi = GetImis();
		if (string.IsNullOrEmpty(mImsi))
		{
			imType = IMSI.NONE;
		}
		else if (mImsi.StartsWith("46000") || mImsi.StartsWith("46002")|| mImsi.StartsWith("46007"))
		{// IMSI号前面3位460是国家，紧接着后面2位00 02 07是中国移动，01 06是中国联通，03 05是中国电信。
//			netOperatorid = "cm";
			imType = IMSI.CM;
		}
		else if (mImsi.StartsWith("46001") || mImsi.StartsWith("46006"))
		{
			imType = IMSI.UNI;
//			netOperatorid = "uni";
		}
		else if (mImsi.StartsWith("46003") || mImsi.StartsWith("46005"))
		{
			imType = IMSI.CT;
		}
		else
		{
			imType = IMSI.NONE;
		}

	}	


	void OnMessage(string msg)
	{

		mMessage += msg+"\n";


		YHSDKMessage sdMsg = SDK.Deserialize<YHSDKMessage>(msg);
		//WG.Log("OnMessage==="+msg);
		if (sdMsg.a == 1) {//isMusicOn
			if (sdMsg.k == 1) {//on

				if (BCSoundPlayer.Instance != null) {
					BCSoundPlayer.Instance.SwitchMusic = true;
					BCSoundPlayer.Instance.SwitchSound = true;
				} else {
					PlayerPrefs.SetInt (KEYMUSIC, 1);
					PlayerPrefs.SetInt (KEYSOUND, 1);
				}
			} else if (sdMsg.k == 0) {// off

				if (BCSoundPlayer.Instance != null) {
					BCSoundPlayer.Instance.SwitchMusic = false;
					BCSoundPlayer.Instance.SwitchSound = false;
				} else {
					PlayerPrefs.SetInt (KEYMUSIC, 0);
					PlayerPrefs.SetInt (KEYSOUND, 0);
				}
			}
		} else if (sdMsg.a == 2) {//getPayTipType  k=0 LONG_TIP; k= 1 SHORT_TIP;k=2 NO_TIP;
			//WG.Log ("getPayTipType===" + sdMsg.k);

			mTipType = sdMsg.k;
		} else if (sdMsg.a == 3) {//payResult
			//WG.Log ("payResult ====" + sdMsg.k + "payCode====" + sdMsg.s);

			if (dicPayBlock.ContainsKey (sdMsg.s)) {
				System.Action<bool> myAct = dicPayBlock [sdMsg.s];
				dicPayBlock.Remove (sdMsg.s);
				if (myAct != null) {
					myAct (sdMsg.k == 0);
				}
			}

			if (dicPayOrder.ContainsKey (sdMsg.s)) {
				string order = dicPayOrder [sdMsg.s];
				if (sdMsg.k == 0) {
					#if TalkingData
					TDGAVirtualCurrency.OnChargeSuccess (order);
					int payKey= System.Int32.Parse(sdMsg.s);
					payKey%=100;
					#endif
//					string payName=WGDataController.Instance.getYHMDPay((YHPayType)payKey).name;
//					Dictionary<string ,object> dic=new Dictionary<string,object>();
//					dic.Add("item",payName);
//					TalkingDataGA.OnEvent("购买成功",dic);
				}
				dicPayOrder.Remove (sdMsg.s);
			}
		} else if (sdMsg.a == 4) {
			WGGameWorld.Instance.keyEscape = 0;
			if (sdMsg.k == 0) { // exit game
				WGGameWorld.Instance.OnApplicationPause (true);
				HKDataStatistics.Self.OnApplicationPause(true);
				//TalkingDataGA.OnKill();
			} else { //cancel exit game
				//HKDataStatistics.Self.OnApplicationPause (false);
			}
		} else if (sdMsg.a == 5) {//getSpecificFunction callback
			if (sdMsg.s.Equals ("4")) {//对话费,超级老虎机,十连抽
				bTiger_HuaFei = sdMsg.k == 1;
				Debug.Log("superTiger:"+bTiger_HuaFei);
				//bTiger_HuaFei=false;
				getSpecificFunction (8);//客服控制
			} else if (sdMsg.s.Equals ("8")) {//客服控制默认关闭
				bKeFu = (sdMsg.k == 0?false:true);
				getSpecificFunction (16);//文案是否清楚
			} else if (sdMsg.s.Equals ("16")) {//文案是否清楚
				bMsgClear = sdMsg.k == 0;
				getSpecificFunction (32);//SuperTiger
			} else if (sdMsg.s.Equals ("32")) {//普通老虎机

				bCommonTiger = sdMsg.k == 1;
				getSpecificFunction(64);
			}else if(sdMsg.s.Equals("64")){//新手礼包
				bShowNewhandGift=sdMsg.k==1;
				getSpecificFunction(128);
			}else if(sdMsg.s.Equals("128")){//幸运礼包
				bShowLuckGift=sdMsg.k==1;
				getSpecificFunction(256);
			}else if (sdMsg.s.Equals("256")){//土豪礼包
				bShowRichGift=sdMsg.k==1;
				getSpecificFunction(512);
			}else if(sdMsg.s.Equals("512")){//狂暴礼包
				bShowPoweGift=sdMsg.k==1;
				getSpecificFunction(1024);
			}else if(sdMsg.s.Equals("1024")){//双倍奖励
				bShowDoubleGift=sdMsg.k==1;
				getSpecificFunction(2);
			}else if(sdMsg.s.Equals("2")){//角色3合一
				bShow3RoalGift=sdMsg.k==1;
				Debug.Log("show3RoalGift"+bShow3RoalGift);
				getSpecificFunction(1);
			}else if(sdMsg.s.Equals("1")){//指哪打哪

				bShowFinger=sdMsg.k==1;
				Debug.Log("showFinger"+bShowFinger);
			}
		} else if (sdMsg.a == 6) 
		{
			mUserID = sdMsg.s;
		}
	}
//	public void YHInitCallBack(int tipType)
//	{
//		mTipType = tipType;
//	}
//	public void YHPayCallBack(string payCode,int success)
//	{
//		if(dicPayBlock.ContainsKey(payCode))
//		{
//			System.Action<bool> myAct=dicPayBlock[payCode];
//			dicPayBlock.Remove(payCode);
//			if(myAct != null)
//			{
//				myAct(success == 0);
//			}
//			if(dicPayOrder.ContainsKey(payCode))
//			{
//				string order = dicPayOrder[payCode];
//				if(success == 0)
//				{
//#if TalkingData
//
//				TDGAVirtualCurrency.OnChargeSuccess(order);
//#endif
//				}
//				dicPayOrder.Remove(payCode);
//			}
//		}
//	}
	void OnException(string msg)
	{
		mMessage +=msg+"\n";
	}
	#if UNITY_ANDROID

	public static string mClassName = "com.unity3d.player.UnityPlayer";
	public static string mActivityName = "currentActivity";
	private static AndroidJavaClass _jc=null;
	private static AndroidJavaClass jc{
		get{
			if(_jc == null)
			{
				_jc = new AndroidJavaClass (mClassName);
			}
			return _jc;
		}
	}
	private static AndroidJavaObject _jo=null;
	private static AndroidJavaObject jo{
		get{
			if(_jo == null)
			{
				_jo = jc.GetStatic<AndroidJavaObject> (mActivityName);
			}

			return _jo;
		}
	}

	public string GetImis()
	{
		return "none";
		if(Application.platform == RuntimePlatform.Android)
		{
			return jo.Call<string>("myGetImsi");
		}
		else
		{
#if Unicom
			return "46001";
#elif CM
			return "46000";
#endif
		}
		return "none";
	}
	void getTipType()
	{
		return;
		if(Application.platform == RuntimePlatform.Android)
		{
			jo.Call("getTipType");
//			int type = _jo.Call<int>("getTipType1");
//			//WG.SLog(" ========================_jo.Call<int>(getTipType1);"+type);
		}

	}
	void getSpecificFunction(int key)
	{
		return; 
		if(Application.platform == RuntimePlatform.Android)
		{
			jo.Call("getSpecificFunction",key);
		}
	}
	/// <summary>
	///	int: TEL = 1;
	///		 SPE_1 = 2;
	///		 SPE_2 = 4;
	///		 SPE_3 = 8;
	///		 SPE_4 = 16;
	///		 SPE_5 = 32;
	///		 SPE_6 = 64;
	///		 SPE_7 = 128;
	///		 SPE_8 = 256;
	///		 SPE_9 = 512;
	///		 SPE_10 = 1024;
	/// </summary>
	/// <param name="key">Key.</param>
	void getUserID()
	{
		return;
		if(Application.platform == RuntimePlatform.Android)
		{
			jo.Call("getUserID");
		}
	}

	public static void YHPay (string key,float costRMB,int jewelCount,System.Action<bool> myBlock)
	{
		if(Self.dicPayBlock.ContainsKey(key))
		{
			Self.dicPayBlock.Remove(key);
		}
		Self.dicPayBlock.Add(key,myBlock);

		if(Self.dicPayOrder.ContainsKey(key))
		{
			Self.dicPayOrder.Remove(key);
		}
		string orderID = "Pay"+key+Core.nData.sysTime;
		Self.dicPayOrder.Add(key,orderID);
		//Debug.Log("paykey:"+key);
		int payKey= System.Int32.Parse(key);
		payKey%=100;
		string payName=WGDataController.Instance.getYHMDPay((YHPayType)payKey).name;
//		TDGAVirtualCurrency.OnChargeRequest(orderID,"Pay"+payName,costRMB,"CNY",jewelCount,"YeHuoSDK");
//		Dictionary <string ,object> dic=new Dictionary<string,object>();
//		dic.Add("item",payName);
//		TalkingDataGA.OnEvent("请求购买",dic);

//		Self.OnMessage("{\"a\":4,\"k\":0,\"s\":\""+key+"\"}");

//		SongSDK.Self.YHPay(key);

//		if(Application.platform == RuntimePlatform.Android)
//		{
//			jo.Call ("MyPay", key);
//		}
//		else // for test
		{
			Self.OnMessage("{\"a\":3,\"k\":0,\"s\":\""+key+"\"}");
		}



	}

	public static void ApplictionExit()
	{
//		SongSDK.Self.YHApplicationExit();
		
//		if(Application.platform == RuntimePlatform.Android)
//		{
//			HKDataStatistics.Self.OnApplicationPause(true);
//			jo.Call ("MyExit");
//		}
//		else
		{
			WGGameWorld.Instance.keyEscape = 0;
		}
	}

	// Update is called once per frame
//	void Update ()
//	{
//		if (Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.Home)) {
//			if(Application.platform == RuntimePlatform.Android)
//			{
//				jo.Call ("ApplicationExit");
//			}
//		}
//	}

	void OnGUI()
	{
//		if(GUILayout.Button("PayTest",GUILayout.Height(100)))
//		{
//			//注释1
//			try{
//				AndroidJavaClass mjc = new AndroidJavaClass(mClassName);
//				AndroidJavaObject mjo = mjc.GetStatic<AndroidJavaObject>(mActivityName);
//				mjo.Call("YHPay","Paytest1111111111111111111111");
//			}
//			catch(UnityException e)
//			{
//				mMessage +=e.Message;
//			}
//		}
//		else if(GUILayout.Button("PayTest2",GUILayout.Height(100)))
//		{
//			YHPay("Paytest22222222222222222222");
//		}
		
//		GUILayout.TextArea(mMessage, GUILayout.Width(320),GUILayout.Height(640));
	}

	

	#else 
	public static void YHPay (string key){}
	#endif

}
public enum IMSI{
	NONE = 0,
	CM,
	UNI,
	CT
}
public class YHSDKMessage{
	public int a;
	public int k;
	public string s;
}