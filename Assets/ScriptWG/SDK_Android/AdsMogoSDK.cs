using UnityEngine;
using System.Collections;

public class AdsMogoSDK : MonoBehaviour
{


	#if UNITY_ANDROID
	private static string LOCATION_TOP = "isDown";
	private static string MOGO_ID = "68c457ef8d9142bb84aff91db3537c78";
	private static AndroidJavaClass _jc=null;
	private static AndroidJavaClass jc{
		get{
			if(_jc == null)
			{
				_jc = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
			}
			return _jc;
		}
	}
	private static AndroidJavaObject _jo=null;
	private static AndroidJavaObject jo{
		get{
			if(_jo == null)
			{
				_jo = jc.GetStatic<AndroidJavaObject> ("currentActivity");
			}
			return _jo;
		}
	}


	public static void ShowAdsMogoBanner ()
	{
		if(Application.platform == RuntimePlatform.Android)
		{
			string[] paramaddBannerAd = new string[2];
			//mogoID,must set
			paramaddBannerAd [0] = MOGO_ID;
			//set Advertising display position,default Displayed at the bottom,must set
			paramaddBannerAd [1] = LOCATION_TOP;

			jo.Call ("addBannerAd", paramaddBannerAd);
		}
	}

	public static void initInterstitialAd ()
	{
		if(Application.platform == RuntimePlatform.Android)
		{
			string[] paramaddInterstitialAd = new string[1];
			//mogoID,must set
			paramaddInterstitialAd [0] = MOGO_ID;
			jo.Call ("initInterstitialAd", paramaddInterstitialAd);
		}
	}

	public static void ShowInterstitialAd ()
	{
		if(Application.platform == RuntimePlatform.Android)
		{
			jo.Call ("showInterstitialAd");
		}
	}

	public static void CancelInterstitialAd ()
	{
		if(Application.platform == RuntimePlatform.Android)
		{
			jo.Call ("cancelInterstitialAd");
		}
	}


	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape) || Input.GetKeyDown (KeyCode.Home)) {
			if(Application.platform == RuntimePlatform.Android)
			{
				jo.Call ("removeInterstitial");
				Application.Quit (); //exit when key back
			}
		}
	}


	/////////////Banner  Callback///////////////////

	void onAdsMogoRequestAd ()
	{
		string[] parama = new string[1];
		parama [0] = "Uniy Demo onAdsMogoRequestAd";
		jo.Call ("showAndroidLog", parama);
	}

	void onAdsMogoReceiveAd ()
	{
		string[] parama = new string[1];
		parama [0] = "Uniy Demo onAdsMogoReceiveAd";
		jo.Call ("showAndroidLog", parama);
	}

	void onAdsMogoRealClickAd ()
	{
		string[] parama = new string[1];
		parama [0] = "Uniy Demo onAdsMogoRealClickAd";
		jo.Call ("showAndroidLog", parama);
	}

	void onAdsMogoFailedReceiveAd ()
	{
		string[] parama = new string[1];
		parama [0] = "Uniy Demo onAdsMogoFailedReceiveAd";
		jo.Call ("showAndroidLog", parama);
	}

	void onAdsMogoCloseMogoDialog ()
	{
		string[] parama = new string[1];
		parama [0] = "Uniy Demo onAdsMogoCloseMogoDialog";
		jo.Call ("showAndroidLog", parama);
	}

	void onAdsMogoCloseAd ()
	{
		string[] parama = new string[1];
		parama [0] = "Uniy Demo onAdsMogoCloseAd";
		jo.Call ("showAndroidLog", parama);
	}

	void onAdsMogoClickAd ()
	{
		string[] parama = new string[1];
		parama [0] = "Uniy Demo onAdsMogoClickAd";
		jo.Call ("showAndroidLog", parama);
	}

	void onAdsMogogetCustomEvemtPlatformAdapterClass ()
	{
		string[] parama = new string[1];
		parama [0] = "Uniy Demo onAdsMogogetCustomEvemtPlatformAdapterClass";
		jo.Call ("showAndroidLog", parama);
	}

	/////////////Interstitial  Callback///////////////////

	void onShowInterstitialScreen ()
	{
		string[] parama = new string[1];
		parama [0] = "Uniy Demo onShowInterstitialScreen";
		jo.Call ("showAndroidLog", parama);
	}

	void onInterstitialStaleDated ()
	{
		string[] parama = new string[1];
		parama [0] = "Uniy Demo onInterstitialStaleDated";
		jo.Call ("showAndroidLog", parama);
	}

	void onInterstitialGetView ()
	{
		string[] parama = new string[1];
		parama [0] = "Uniy Demo onInterstitialGetView";
		jo.Call ("showAndroidLog", parama);
	}

	void onInterstitialCloseAd ()
	{
		string[] parama = new string[1];
		parama [0] = "Uniy Demo onInterstitialCloseAd";
		jo.Call ("showAndroidLog", parama);
	}

	void onInterstitialClickCloseButtonAd ()
	{
		string[] parama = new string[1];
		parama [0] = "Uniy Demo onInterstitialClickCloseButtonAd";
		jo.Call ("showAndroidLog", parama);
	}

	void onInterstitialClickAd ()
	{
		string[] parama = new string[1];
		parama [0] = "Uniy Demo onInterstitialClickAd";
		jo.Call ("showAndroidLog", parama);
	}

	void onAdsMogogetInterstitialCustomEvemtPlatformAdapterClass ()
	{
		string[] parama = new string[1];
		parama [0] = "Uniy Demo onAdsMogogetInterstitialCustomEvemtPlatformAdapterClass";
		jo.Call ("showAndroidLog", parama);
	}
	#else 

	public static void ShowAdsMogoBanner (){}
	public static void initInterstitialAd (){}
	public static void ShowInterstitialAd (){}
	public static void CancelInterstitialAd (){}
	#endif
}
