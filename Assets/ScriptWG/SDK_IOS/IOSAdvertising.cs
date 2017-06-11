using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
public class IOSAD{
	#if Add_AD
	[DllImport ("__Internal")]
	private static extern bool _readyInterstitialAds();
	[DllImport ("__Internal")]
	private static extern void _showInterstitialMoGo();
	[DllImport ("__Internal")]
	private static extern void _showAdView(bool show);

	public static void ShowAdView(bool show)
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer||
			Application.platform == RuntimePlatform.OSXPlayer)
		{
			_showAdView(show);
		}
	}

	public static bool readyInterstitialAds()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer||
			Application.platform == RuntimePlatform.OSXPlayer)
		{
			return _readyInterstitialAds();
		}
		return false;
	}
	public static void showInterstitialMoGo()
	{
		if(Application.platform == RuntimePlatform.IPhonePlayer||
			Application.platform == RuntimePlatform.OSXPlayer)
		{
			_showInterstitialMoGo();
		}
	}
	#else
	public static void ShowAdView(bool show)
	{

	}

	public static bool readyInterstitialAds()
	{

		return false;
	}
	public static void showInterstitialMoGo()
	{

	}
	#endif

}
