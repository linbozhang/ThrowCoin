using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WGItemView : MDBaseAlertView {

	static Object mObj = null;
	public static WGItemView curItemView = null;
	public static bool isMonster;
	enum VIEW_STATE{
		NONE,
		Collection,
		Monster,
	}

	[SerializeField]
	protected TweenScale tsContent;
	[SerializeField]
	protected UISprite spGrayBackground;

	public WGPanelButton wpBtnCollection;
	public WGPanelButton wpBtnMonster;

	public WGTabView mTabView;

	public WGItemViewTabBear mTabMonster;
	public WGItemViewTabCollection mTabCollection;

	WGDataController _dataCtrl;
	VIEW_STATE mState = VIEW_STATE.NONE;
	
	public void viewDidLoad ()
	{
		_dataCtrl = WGDataController.Instance;
		mTabCollection.ViewDidLoad();
		mTabMonster.ViewDidLoad();
	}
	#region MDBaseAlertView
	public override void showView ()
	{
		viewDidLoad();
		base.showView ();
		Time.timeScale = 0;


		if(isMonster){
//			mTabMonster.ESetActive(true);
			//mTabMonster.InitShowBearView();
//			
			mState = VIEW_STATE.Monster;
//			
			mTabView.InitState(wpBtnMonster.button.transform.localPosition,wpBtnMonster.title, mTabMonster.gameObject);
//			//mTabCollection.

			//mTabCollection.ViewDidHidden();

			mTabCollection.ESetActive(false);
			//OnBtnMonster();

		}else{
			
			mState = VIEW_STATE.Collection;
			
			mTabView.InitState(wpBtnCollection.button.transform.localPosition,wpBtnCollection.title,mTabCollection.gameObject);
			mTabMonster.ESetActive(false);
			mTabCollection.ESetActive(true);
			mTabCollection.ViewWillShow();
		}



		tsContent.transform.localScale = tsContent.from;
		tsContent.ESetActive(true);
		tsContent.PlayForward();
		InvokeBlock(tsContent.duration,()=>{
			showViewEnd();
		});
	}
	public override void showViewEnd ()
	{
		base.showViewEnd ();
		if(isMonster){
			mTabMonster.InitShowBearView();
		}else{
			mTabCollection.ViewDidShow();
		}

	}
	public override void hiddenView ()
	{
		base.hiddenView ();
	
		Time.timeScale = 1;

		if(mState == VIEW_STATE.Monster)
		{
			mTabMonster.ViewWillDiappear();
		}
		else if(mState == VIEW_STATE.Collection)
		{
			mTabCollection.ViewWillHidden();
		}
		tsContent.PlayReverse();
		InvokeBlock(tsContent.duration,()=>{
			hiddenViewEnd();
		});
	}
	public override void hiddenViewEnd ()
	{
		if(mState == VIEW_STATE.Monster)
		{
			mTabMonster.ViewDidHidden(true);

		}
		else if(mState == VIEW_STATE.Collection)
		{
			mTabCollection.ViewDidHidden();
		}
		base.hiddenViewEnd ();
	}

	#endregion
	void OnBtnCollection()
	{
		if(mState == VIEW_STATE.Collection)
		{
			return;
		}
		BCSoundPlayer.Play(MusicEnum.button);
		mState = VIEW_STATE.Collection;
		mTabView.ChangeState(wpBtnCollection.button.transform.localPosition,wpBtnCollection.title,mTabCollection.gameObject);


		mTabMonster.ViewWillDiappear();
		mTabMonster.ViewDidHidden();
		mTabMonster.gameObject.SetActive(false);
		mTabCollection.gameObject.SetActive(true);
		mTabCollection.ViewDidShow();
	}
	void OnBtnMonster()
	{
		if(mState == VIEW_STATE.Monster)
		{
			return;
		}
		BCSoundPlayer.Play(MusicEnum.button);
		mState = VIEW_STATE.Monster;
		mTabView.ChangeState(wpBtnMonster.button.transform.localPosition,wpBtnMonster.title,mTabMonster.gameObject);

		mTabCollection.ViewDidHidden();

		mTabCollection.gameObject.SetActive(false);
		mTabMonster.gameObject.SetActive(true);


		mTabMonster.InitShowBearView();
	}

	public static WGItemView CreateItemView(bool _isMonster=false)
	{
		if(mObj == null)
		{
			mObj = Resources.Load("pbWGItemView");
		}
		isMonster=_isMonster;
		if(mObj != null)
		{
			GameObject go = Instantiate(mObj) as GameObject;
			WGItemView iv = go.GetComponent<WGItemView>();
			curItemView = iv;
			return iv;
		}
//		if(isMonster){
//			this.OnBtnMonster();
//		}
		return null;
	}


}
