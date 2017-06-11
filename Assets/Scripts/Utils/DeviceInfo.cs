using System;
using System.Text;
using UnityEngine;
using System.IO;

public class DeviceInfo
{	
	public static string MAC {
		get {
			return UniqueGUID.getInstance().getUniqueIdetify();
		}
	}
	
	public static string GetDeviceUniqueID(){
		return SystemInfo.deviceUniqueIdentifier;
	}

	public static int _deviceflag = -1;
	public static int DeviceFlag {
		get {
			if (_deviceflag == -1) {
				ASCIIEncoding asciiEncoding = new ASCIIEncoding ();
				int And = (int)asciiEncoding.GetBytes (MAC) [0];
				_deviceflag = And;
			}

			return _deviceflag;
		}
	}

	//Get Device Type
	public static int Device_Type = Consts.MacX;

	//return full path
	public static string getDocumentPath (string name)
	{
		string path = null;
		string activeDir = Directory.GetCurrentDirectory (); 
		switch(Application.platform){
			case RuntimePlatform.OSXEditor:
			case RuntimePlatform.WindowsEditor:
				path = System.IO.Path.Combine (activeDir, name);
				return path;
			case RuntimePlatform.OSXPlayer:
				path = System.IO.Path.Combine (activeDir, name);
				return path;
			case RuntimePlatform.IPhonePlayer:
			path = System.IO.Path.Combine (getDocumentDir(), name);
				return path;
			case RuntimePlatform.Android:

			path = System.IO.Path.Combine (getDocumentDir(), name);
				return path;
		}
		return Application.persistentDataPath;
	}

	//return Directory path without file name
	public static string getDocumentDir(){
		string path = null;
		string activeDir = Directory.GetCurrentDirectory (); 

		switch(Application.platform){
		case RuntimePlatform.OSXEditor:
		case RuntimePlatform.WindowsEditor:
		case RuntimePlatform.OSXPlayer:
			path = activeDir;
			return path;
		case RuntimePlatform.IPhonePlayer:
			path = Application.persistentDataPath;
			return path;
		case RuntimePlatform.Android:

			AndroidJavaObject activity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject cw = activity.Call<AndroidJavaObject>("getFilesDir");
			
			String abPath = cw.Call<String>("getAbsolutePath");
			return abPath;

		}
		return activeDir;
	}
	#if UNITY_ANDROID
	private static AndroidJavaClass _unityClass=null;
	private static AndroidJavaClass unityClass{
		get{
			if(_unityClass == null)
			{
				_unityClass =  new AndroidJavaClass(UNTIFY_CLASS); 
			}
			return _unityClass;
		}
	}
	private static string UNTIFY_CLASS = "com.unity3d.player.UnityPlayer";
#endif
}

