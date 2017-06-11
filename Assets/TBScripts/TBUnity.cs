using UnityEngine;
using System.Collections;

public class TBUnity : MonoBehaviour {
	bool isUseOldMode = true;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnGUI() {  
		//Kinds of Buttons
		if(GUI.Button(getRectByNo(0),"Init"))
		{
			TBSDK.TBSetUseOldLoadingMode(isUseOldMode);
			isUseOldMode = !isUseOldMode;
			TBSDK.TBInit();
		}
        //Login
        if(GUI.Button(getRectByNo(1),"Login"))  
        {
			TBSDK.TBLogin(0);
        }
		//Logout
		if(GUI.Button(getRectByNo(2),"Logout"))  
        {  
			TBSDK.TBLogout();
        }
		//SwitchAccount
		if(GUI.Button(getRectByNo(3),"SwitchAccount"))  
        {  
			TBSDK.TBSwitchAccount();
        }
		//isLogin
		if(GUI.Button(getRectByNo(4),"IsLogin"))  
        {  
			TBSDK.TBIsLogined();
        }
		//setDebug
		if(GUI.Button(getRectByNo(5),"SetDebug"))  
        {  
			TBSDK.TBSetDebug();
        }
		//SessionID
		if(GUI.Button(getRectByNo(6),"SessionID"))  
        {  
			TBSDK.TBSessionID();
        }
		//UserID
		if(GUI.Button(getRectByNo(7),"UserID"))  
        {  
			TBSDK.TBUserID();
        }
		//UserName
		if(GUI.Button(getRectByNo(8),"NickName"))  
        {  
			TBSDK.TBNickName();
        }
		//Pay50
		if(GUI.Button(getRectByNo(9),"Pay 50 rmb"))  
        {  
			TBSDK.TBPayRMB(50,"order here","payDescription");
        }
		//Exchange
		if(GUI.Button(getRectByNo(10),"Exchange"))  
        {  
			TBSDK.TBExchange("order here","payDescription");
        }
		//CheckOrder
		if(GUI.Button(getRectByNo(11),"CheckOrder"))  
        {  
			TBSDK.TBCheckOrder("orderhere");
        }
		//UserCenter
		if(GUI.Button(getRectByNo(12),"UserCenter"))  
        {  
			TBSDK.TBEnterUserCenter(0);
        }
		//GameRecommend
		if(GUI.Button(getRectByNo(13),"Recommendation"))  
        {  
			TBSDK.TBEnterAppCenter(0);
        }
		//BBS
		if(GUI.Button(getRectByNo(14),"BBS"))  
        {  
			TBSDK.TBEnterAppBBS(0);
        }
		//ShowToolBar
		if(GUI.Button(getRectByNo(15),"ShowToolBar"))
		{
			TBSDK.TBShowToolBar(3,true);
		}
		//HideToolBar
		if(GUI.Button(getRectByNo(16),"HideToolBar"))
		{
			TBSDK.TBHideToolBar();
		}
    }
	
	 Rect getRectByNo(int no){
		return new Rect((Screen.width/2- ((2-no%3)*180)+70),70*(no/3)+200,160,60);
	}
}
