using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MDNetData {
	public double sysTime = 0;
	public MDNetData(){
		sysTime = WGTime.timeSince1970;
	}
}

public class HttpManager : MonoBehaviour {

	DataPlayer  _dp{
		get{
			return DataPlayerController.getInstance().data;
		}
	}

	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);

	}
	void ElapsedEvent(object obj,System.Timers.ElapsedEventArgs args)
	{
		Core.nData.sysTime++;
	}
	// Use this for initialization
	void Start () {
		//getSystemTime();

		System.Timers.Timer t = new System.Timers.Timer(1000);//实例化Timer类，设置间隔时间为10000毫秒； 
		t.Elapsed += new System.Timers.ElapsedEventHandler(ElapsedEvent);//到达时间的时候执行事件； 
		t.AutoReset = true;//设置是执行一次（false）还是一直执行(true)； 
		t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
	}
	void getSystemTime()
	{
		#if TEST
		OnMyReceive("",""+WGTime.timeSince1970);

//		SSHttpTask ht = new SSHttpTask();
//		ht.request = "http://localhost:8888/date.php";
//		ht.afterCompleted +=OnMyReceive;
//		ht.ErrorOccured +=OnMyException;
//		SSHttpThread.getInstance().sendHttpTask(ht);
//		
//		WGAlertViewController.Self.showConnecting();
		#else
//		SSHttpTask ht = new SSHttpTask();
//		ht.request = "http://zhangyuntao.com.cn/ytTest/time.php";
//		ht.afterCompleted +=OnMyReceive;
//		ht.ErrorOccured +=OnMyException;
//		SSHttpThread.getInstance().sendHttpTask(ht);
//
//		WGAlertViewController.Self.showConnecting();
		#endif
	}

	void OnMyReceive(string request,string response)
	{
		//WG.SLog(response);
		Core.nData.sysTime = double.Parse(response);
		WGAlertViewController.Self.hiddenConnecting();
	}
	void OnMyException(string request,string response)
	{
		WGAlertViewController.Self.hiddenConnecting();
		Core.nData.sysTime = WGTime.DateTime2Unix(_dp.MyData);
		//WG.SLogWarning(request+"\n"+response);
	}


}
