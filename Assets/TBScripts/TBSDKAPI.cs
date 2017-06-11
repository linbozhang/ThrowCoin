using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class TBSDK {
#if TBSDK
	 [DllImport("__Internal")]  
     private static extern void XCTBInit();
	//init
	public static void TBInit()
	{
		if (Application.platform != RuntimePlatform.OSXEditor)   
        {  
            XCTBInit();  
        }  
	}
	
	[DllImport("__Internal")]  
     private static extern void XCShowToolBar(int place,bool isUsingOldPlace);
	 //ShowToolBar
     public static void TBShowToolBar (int place,bool isUsingOldPlace)  
     {  
          
        if (Application.platform != RuntimePlatform.OSXEditor)   
        {  
            XCShowToolBar(place,isUsingOldPlace);
        }  
     } 
	
	[DllImport("__Internal")]  
     private static extern void XCHideToolBar();
	 //HideToolBar
     public static void TBHideToolBar()  
     {  
          
        if (Application.platform != RuntimePlatform.OSXEditor)   
        {  
            XCHideToolBar();
        }  
     } 
	
	 [DllImport("__Internal")]  
	private static extern int XCLogin(int tag);
	 //Login
	public static int TBLogin (int tag)  
     {  
          
        if (Application.platform != RuntimePlatform.OSXEditor)   
        {  
			return XCLogin(tag);  
        }  
		return 0;
     }  
       
     [DllImport("__Internal")]  
	private static extern int XCLogout();  
     //Logout
	public static int TBLogout()  
     {  
        if (Application.platform != RuntimePlatform.OSXEditor)   
        {  
			return XCLogout();  
        }  
		return 0;
     }
	
	 [DllImport("__Internal")]  
     private static extern void XCSwitchAccount();
	 //SwitchAccount
     public static void TBSwitchAccount()  
     {  
        if (Application.platform != RuntimePlatform.OSXEditor)   
        {  
            XCSwitchAccount();  
        }  
     }
	
	 [DllImport("__Internal")]  
	private static extern bool XCIsLogined();
	 //IsLogined
	public static bool TBIsLogined()  
     {  
        if (Application.platform != RuntimePlatform.OSXEditor)   
        {  
			return XCIsLogined();  
        }  
		return false;
     }
	
     [DllImport("__Internal")]  
     private static extern void XCSetDebug(); 
	 public static void TBSetDebug()  
   	 //SetUpdateDebug
     {  
        if (Application.platform != RuntimePlatform.OSXEditor)   
        {  
            XCSetDebug();  
        }  
     }

	 [DllImport("__Internal")]  
	 private static extern void XCSetUseOldLoadingMode(bool isU); 
	 public static void TBSetUseOldLoadingMode(bool isUseOld)  
	 	//SetOldLoadingMode
	 {  
	 	if (Application.platform != RuntimePlatform.OSXEditor)   
   		{  
			XCSetUseOldLoadingMode(isUseOld);  
	 	}  
	 }
	
	 [DllImport("__Internal")] 
	private static extern string XCSessionID();
   	 //SessionID
	public static string TBSessionID()   
	 {  
        if (Application.platform != RuntimePlatform.OSXEditor)   
        {  
			return XCSessionID();  
        }  
		return "";
     }     
		
	 [DllImport("__Internal")]  
	private static extern string XCUserID(); 
   	 //UserID
	public static string TBUserID()   
	 {  
        if (Application.platform != RuntimePlatform.OSXEditor)   
        {  
			return XCUserID();  
        }  
		return "";
	 }
	
	 [DllImport("__Internal")]  
	private static extern string XCNickName(); 
   	 //NickName
	public static string TBNickName()   
	 {  
        if (Application.platform != RuntimePlatform.OSXEditor)   
        {  
			return XCNickName();  
        }  
		return "";
	 }
	
	 [DllImport("__Internal")]  
	private static extern int XCPayRMB(int amount,string order,string payDes);
   	 //pay rmb
	public static int TBPayRMB(int amount,string order,string payDes)   
	 {  
        if (Application.platform != RuntimePlatform.OSXEditor)   
        {  
			return XCPayRMB(amount,order,payDes);  
        }  
		return 0;
	 }
	
	 [DllImport("__Internal")]  
	private static extern int XCExchange(string order,string payDes);
	 //exchange 
	public static int TBExchange(string order,string payDes)   
	 {  
        if (Application.platform != RuntimePlatform.OSXEditor)   
        {  
			return XCExchange(order,payDes);  
        }  
		return 0;
	 }	 

	
	 [DllImport("__Internal")]  
	private static extern int XCCheckOrder(string order);
     //CheckOrder
	public static int TBCheckOrder(string order)   
	 {  
        if (Application.platform != RuntimePlatform.OSXEditor)   
        {  
            return XCCheckOrder(order);  
        }  
		return 0;
	 }	 
	
	 [DllImport("__Internal")]  
	private static extern void XCEnterUserCenter(int tag);
	 //UserCenter
	public static void TBEnterUserCenter(int tag)   
	 {  
        if (Application.platform != RuntimePlatform.OSXEditor)   
        {  
			XCEnterUserCenter(tag);  
        }  
	 }	
	
	 [DllImport("__Internal")]  
	private static extern void XCEnterAppCenter(int tag);
	 //GameRecommend
	public static void TBEnterAppCenter(int tag)   
	 {  
        if (Application.platform != RuntimePlatform.OSXEditor)   
        {  
			XCEnterAppCenter(tag);  
        }  
	 }		 
	 
	 [DllImport("__Internal")]  
	private static extern void XCEnterAppBBS(int tag);  
	 //BBS
	public static void TBEnterAppBBS(int tag)   
	 {  
        if (Application.platform != RuntimePlatform.OSXEditor)   
        {  
			XCEnterAppBBS(tag);  
        }  
	 }	
#else

	public static void TBInit()
	{
	}
	public static void TBShowToolBar (int place,bool isUsingOldPlace)  
	{  
	
	} 
	public static void TBHideToolBar()  
	{  
	
	} 
	public static int TBLogin (int tag)  
	{  
		return 0;
	} 
	public static int TBLogout()  
	{  

		return 0;
	}
	public static void TBSwitchAccount()  
	{  

	}
	public static bool TBIsLogined()  
	{  

		return false;
	}
	public static void TBSetDebug()  
	{  
	}
	public static void TBSetUseOldLoadingMode(bool isUseOld)  
	{  
	}
	public static string TBSessionID()   
	{
		return "";
	}     
	public static string TBUserID()   
	{  
		return "";
	}
	public static string TBNickName()   
	{  
		return "";
	}
	public static int TBPayRMB(int amount,string order,string payDes)   
	{  
		return 0;
	} 
	public static int TBExchange(string order,string payDes)   
	{  
		return 0;
	}
	public static int TBCheckOrder(string order)   
	{  
		return 0;
	}
	public static void TBEnterUserCenter(int tag)   
	{  
	}
	public static void TBEnterAppCenter(int tag)   
	{  
	}
	public static void TBEnterAppBBS(int tag)   
	{  
	}	
#endif
}
