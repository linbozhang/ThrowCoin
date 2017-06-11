using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public partial class WGGameWorld{
	[HideInInspector]
	public int mGotCoinNum = 0;
	 

	public void OnApplicationPause(bool pause)
	{
		if(pause)
		{
			#if Add_AD
			if(DataPlayerController.getInstance().data.DelAD == 0)
			{
				IOSAD.showInterstitialMoGo();
			}
			#endif

			DataPlayerController.getInstance().saveDataPlayer();

			#if UNITY_IPHONE || UNITY_IOS
			System.DateTime today = System.DateTime.Now;


			int canGotNum = Mathf.Max(WGConfig.AUTO_ADD_MAX-_DataPlayer.Coin,0)/10;
			if(canGotNum>0)
			{
				for(int i=0;i<canGotNum;i++)
				{
					LocalNotification ln = new LocalNotification();
					ln.applicationIconBadgeNumber = (i+1)*10;

					System.DateTime answer = today.AddSeconds((i+1)*60);

					ln.fireDate = answer;

					NotificationServices.ScheduleLocalNotification(ln);
				}
			}

			today = System.DateTime.Now;

			int hour = 24 - today.Hour;
			int min = 10+60 -today.Minute;


			LocalNotification ln1 = new LocalNotification();
			ln1.alertBody = WGStrings.getText(1036);
			ln1.alertAction = WGStrings.getText(1037);
			ln1.hasAction = true;
			ln1.fireDate = today.Add(new System.TimeSpan(hour,min,0));
			ln1.soundName = LocalNotification.defaultSoundName;

			NotificationServices.ScheduleLocalNotification(ln1);
			#endif

			MDDataCoin dc = DataCoinController.getInstance().data;

			dc.CoinID.Clear();
			dc.CoinPos.Clear();
			dc.CoinRoto.Clear();
			for( int i=0,max=cs_ObjManager._szLiveCoin.Count;i<max;i++)
			{
				BCGameObj go = cs_ObjManager._szLiveCoin[i].GetComponent<BCGameObj>();
				dc.CoinID.Add(go.ID);
				dc.CoinPos.Add(SDK.to3Float( go.transform.position));
				dc.CoinRoto.Add(SDK.to3Float(go.transform.localEulerAngles));
			}
			DataCoinController.getInstance().saveDataCoin();

#if TalkingData
//			int[] szDa = new int[]{10000,90000,80000,70000,60000,50000,40000,30000,20000,10000,8000,6000,4000,2000,};
//			Dictionary<string, object> dic = new Dictionary<string, object>(); 
//			if(mGotCoinNum>100000)
//			{
//				dic.Add("getCoinNum", "10w");
//
//			}
//			else if(mGotCoinNum>90000)
//			{
//				dic.Add("getCoinNum","9w_10w");
//			}
//			else if(mGotCoinNum>80000)
//			{
//				dic.Add("getCoinNum","8w_9w");
//			}
////			else if(mGotCoinNum>
//			TalkingDataGA.OnEvent(WGStrings.getText(9001), dic);
#endif





		}
		else{
			#if Add_AD
			if(DataPlayerController.getInstance().data.DelAD == 0)
			{
				IOSAD.readyInterstitialAds();
			}
			#endif
			mGotCoinNum = 0;
			CleanNotification();
			CheckDefenseTime();
		}
	}
	//清空所有本地消息
	void CleanNotification()
	{
		#if UNITY_IOS || UNITY_IPHONE
		LocalNotification l = new LocalNotification (); 
		l.applicationIconBadgeNumber = -1; 
		NotificationServices.PresentLocalNotificationNow (l); 
		NotificationServices.CancelAllLocalNotifications (); 
		NotificationServices.ClearLocalNotifications (); 

		#endif
	}

	void OnApplicationQuit () 
	{
		DataPlayerController.getInstance().saveDataPlayer();
	}
}
