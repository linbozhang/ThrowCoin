using UnityEngine;
using System.Collections;

public class YHAboutGameView : MDBaseAlertView {

	public TweenScale tsContent;
	public UILabel labContent;
	public UISprite spBackground;
	public UILabel labUserID;
	public static YHAboutGameView Self;
	void Awake()
	{
		Self = this;
	}

	// Use this for initialization
	void Start () {
//		labContent.text = WGDataController.Instance.sAbout;
	}


	public override void showView ()
	{
		base.showView ();
		if (string.IsNullOrEmpty (YeHuoSDK.Self.mUserID)) {
			labUserID.text = "";
		} else {
			labUserID.text = WGStrings.getText(1107)+YeHuoSDK.Self.mUserID;
		}

		spBackground.ESetActive(true);
		tsContent.ESetActive(true);
		tsContent.PlayForward();
		InvokeBlock(tsContent.duration,()=>{
			showViewEnd();
		});
	}
	public override void hiddenView ()
	{
		base.hiddenView ();
		tsContent.PlayReverse();
		InvokeBlock(tsContent.duration,()=>{
			hiddenViewEnd();
		});
	}

	public override void hiddenViewEnd ()
	{
		base.hiddenViewEnd ();
		tsContent.ESetActive(false);
		spBackground.ESetActive(false);
	}

	public override void OnBtnOk ()
	{
		BCSoundPlayer.Play(MusicEnum.button);
		base.OnBtnOk ();

	}

	public static YHAboutGameView CreateAboutView()
	{
		Object obj = Resources.Load("pbYHAboutGameView");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			YHAboutGameView agv = go.GetComponent<YHAboutGameView>();
			return agv;
		}
		return null;
	}
}
