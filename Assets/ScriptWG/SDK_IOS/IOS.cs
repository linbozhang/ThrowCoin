using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
public partial class IOS{
	#if NATIVE_TEST
	public static string systemType(){return "Unity Simulator";}
	public static bool Debug(){return true;}
	public static void showShareView(string title,string appUrl){}
	public static SystemLanguage systemLanguage(){
			return Application.systemLanguage;	
	}
	public static bool readyInterstitialAds(){return false;}
	#elif UNITY_IPHONE

	[DllImport ("__Internal")]
	private static extern string _systemType();
	[DllImport ("__Internal")]
	private static extern bool _Debug();

    [DllImport ("__Internal")]
	private static extern void _showShareView(string title,string appUrl);
	[DllImport ("__Internal")]
	private static extern string _systemLanguage();


	//en代表英文、zh-Hant代表中文繁体、zh-Hans代表中文简体。
	public static SystemLanguage systemLanguage()
	{
	if(Application.platform == RuntimePlatform.IPhonePlayer||
	Application.platform == RuntimePlatform.OSXPlayer)
	{
	string lan = _systemLanguage();
	if(string.IsNullOrEmpty(lan)) return SystemLanguage.Unknown;
	if(lan.Equals("en")) return SystemLanguage.English;
	if(lan.Equals("zh-Hant"))return SystemLanguage.Chinese;
	if(lan.Equals("zh-Hans"))return SystemLanguage.Chinese;

	}
	return SystemLanguage.Chinese;
//	return Application.systemLanguage;
	}
	public static void showShareView(string title,string appUrl)
    {
        if(Application.platform == RuntimePlatform.IPhonePlayer||
            Application.platform == RuntimePlatform.OSXPlayer)
        {
			_showShareView(title,appUrl);
        }
    }

	public static string systemType()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer||
			Application.platform == RuntimePlatform.OSXPlayer)
		{
			return _systemType();
		}
		return "Unity Simulator";
	}
	public static bool Debug()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer||
			Application.platform == RuntimePlatform.OSXPlayer)
		{
			return _Debug();
		}
		return true;
	}

	#else
	public static string systemType(){return "Unity Simulator";}
	public static bool Debug(){return true;}
	public static void showShareView(string title,string appUrl){}
	public static SystemLanguage systemLanguage(){
		return Application.systemLanguage;	
	}

	public static bool readyInterstitialAds(){return false;}
	#endif


}