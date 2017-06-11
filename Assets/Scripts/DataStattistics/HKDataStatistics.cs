using UnityEngine;
using System.Collections;

public class HKDataStatistics : MonoBehaviour {

	public static HKDataStatistics Self;
	void Awake()
	{
		Self = this;
		DontDestroyOnLoad(this.gameObject);
	}

	// Use this for initialization
	void Start () {

//#if TalkingData
		#if TDTest
		TalkingDataGA.OnStart("466D7B26A63316C8D6BFBFDD56346E33", "Yehuo1");
		#else
		//TalkingDataGA.OnStart("29D221273FF10472155C1465314F4133", "Yehuo1");
		#endif

		// TDGAAccount account = TDGAAccount.SetAccount(TalkingDataGA.GetDeviceId());
//		TDGAAccount account = TDGAAccount.SetAccount(TalkingDataGA.GetDeviceId());
//		account.SetAccountType(AccountType.ANONYMOUS);
//		account.SetLevel(DataPlayerController.getInstance().data.Level);
//		account.SetGameServer("野火渠道1");
		
//#endif

	}



	public void OnApplicationPause(bool isPause){

		if(isPause){
			//Debug.Log("pause"+isPause);
			//TalkingDataGA.OnEnd();
		}else{
			//Debug.Log("pause"+isPause);
			//TalkingDataGA.OnStart("29D221273FF10472155C1465314F4133", "Yehuo1");
		}

	}
	void OnDestroy(){
		//TalkingDataGA.OnKill();
		//TalkingDataGA.OnEnd();
	}
	void OnApplicationFocus(bool isFocus){

//		if(isFocus){
//			//Debug.Log("focus"+isFocus);
//			TalkingDataGA.OnStart("29D221273FF10472155C1465314F4133", "Yehuo1");
//		}else{
//			//Debug.Log("focus"+isFocus);
//			TalkingDataGA.OnEnd();
//		}

	}
	void OnApplicationQuit () 
	{
		//TalkingDataGA.OnEnd();
	}

}
