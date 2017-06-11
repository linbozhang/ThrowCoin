using UnityEngine;
using System.Collections;

public class WGAlertViewController : MonoBehaviour {

	GameObject goTopView;
	WGAlertView alertView=null;
	WGConnectingView conView = null;
	public static WGAlertViewController Self;
	void Awake()
	{
		Self = this;

	}
	public void InitAlertViewController()
	{
		goTopView = WGRootManager.Self.goRootTopUI;
	}

	string mString(int id){
		return WGStrings.getText(id);
	}

	public WGAlertView showAlertView(int msgID,int okID=1002,int cancelID=0){

		return showAlertView(mString(msgID),mString(okID),mString(cancelID));
	}
	public WGAlertView showAlertView(string msg,int okID=1002,int cancelID=0)
	{
		return showAlertView(msg,mString(okID),mString(cancelID));
	}
	public WGAlertView showAlertView(string msg,string ok,string cancel)
	{
		return showAlertView1(msg,ok,cancel,0);
	}
	public WGAlertView showAlertView1(int msgID,int type,int okID=1002,int cancelID=0){
		
		return showAlertView1(mString(msgID),mString(okID),mString(cancelID),type);
	}
	public WGAlertView showAlertView1(string msg,string ok,string cancel,int type)
	{
		if(alertView == null)
		{
			alertView = WGAlertView.CreateAlertView(type);
			SDK.AddChild(alertView.gameObject,goTopView);
		}

		alertView.ESetActive(true);
		alertView.freshViewWithTitle(msg,ok,cancel);
		alertView.showView();
		return alertView;
	}
	public void hiddeAlertView(GameObject goView)
	{
		goView.SetActive(false);
		alertView = null;
		Destroy(goView);
	}
	public void destroyAlertView(MDBaseAlertView alert)
	{
		if(alert == alertView)
		{
			alertView = null;
		}

		Destroy(alert.gameObject);
	}
	public WGConnectingView showConnecting()
	{
		if(conView == null)
		{
			conView =WGConnectingView.CreateConnectingView();
			SDK.AddChild(conView.gameObject,goTopView);
			conView.alertViewBehavriour = (ab,alert)=>{
				if(ab == MDAlertBehaviour.DID_HIDDEN)
				{
					conView.gameObject.SetActive(false);
				}
			};
		}
		conView.gameObject.SetActive(true);
		conView.showView();
		return conView;
	}
	public void hiddenConnecting()
	{
		if(conView != null)
		{
			conView.hiddenView();
		}
	}

	public WGTipAlertView showTipView(int msgID)
	{
		return showTipView(mString(msgID));
	}

//	public WGTipAlertView showArchivementTipView(int msgID){
//		return showArchivementTipView (mString (msgID));
//	}

	public WGTipAlertView showArchivementTipView(string msg,string icon)
	{
		WGTipAlertView tp = WGTipAlertView.CreateArchivementView();
		tp.Icon.GetComponent<UISprite>().spriteName=icon;
		tp.freshUI(msg);

		SDK.AddChild(tp.gameObject,goTopView);
		tp.showView();
		return tp;
	}
	public WGTipAlertView showTipView(string msg)
	{
		WGTipAlertView tp = WGTipAlertView.CreateTipView();
		tp.freshUI(msg);
		SDK.AddChild(tp.gameObject,goTopView);
		tp.showView();
		return tp;
	}
}
