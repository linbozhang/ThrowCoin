using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WGSellGroupItemView : MDBaseAlertView {


	public TweenScale tsContent;

	public UILabel labSellNum;
	public UILabel labGetCoinNum;
	public UILabel labTotalNum;

	int itemID1,itemID2,itemID3;
	BCCollectionInfo col1,col2,col3;
	int maxCount=1;
	int mSellCount = 0;
	float mGotCoin = 1;

	DataPlayer _dp {
		get{
			return DataPlayerController.getInstance().data;
		}
	}

	// Use this for initialization
	void Start () {

	}
	public void FreshSellGroupWithID(int id1,int id2,int id3,int max)
	{
		maxCount = max;
		itemID1 = id1;
		itemID2 = id2;
		itemID3 = id3;
		WGDataController _dataCtrl = WGDataController.Instance;

		col1 = _dataCtrl.GetCollectionInfo(itemID1);
		col2 = _dataCtrl.GetCollectionInfo(itemID2);
		col3 = _dataCtrl.GetCollectionInfo(itemID3);
		mSellCount = max;
		freshSellView();
	}
	void freshSellView()
	{
		mGotCoin= 0;
		if(col1 != null) mGotCoin += col1.sell_num*col1.sell_factor*mSellCount;
		if(col2 != null) mGotCoin += col2.sell_num*col2.sell_factor*mSellCount;
		if(col3 != null) mGotCoin += col3.sell_num*col3.sell_factor*mSellCount;
		int got = Mathf.CeilToInt(mGotCoin+0.5f);
		labSellNum.text = mSellCount.ToString();
		labGetCoinNum.text = WGStrings.getFormate(1049,got);
		labTotalNum.text = WGStrings.getFormate(1048,maxCount);

	}
	void OnBtnAdd()
	{
		if(mSellCount<maxCount)
		{
			BCSoundPlayer.Play(MusicEnum.button);
			mSellCount++;
		}
		freshSellView();
	}
	void OnBtnSub()
	{
		if(mSellCount>1)
		{
			BCSoundPlayer.Play(MusicEnum.button);
			mSellCount--;
		}
		freshSellView();
	}
	public override void OnBtnOk ()
	{

		BCSoundPlayer.Play(MusicEnum.button);

		DataPlayerController dpc = DataPlayerController.getInstance();
		WGDataController _dataCtrl = WGDataController.Instance;
		int numleft = _dataCtrl.GetCollectionOwnNum(itemID1);
		int numMiddle = _dataCtrl.GetCollectionOwnNum(itemID2);
		int numRight = _dataCtrl.GetCollectionOwnNum(itemID3);

		if(Mathf.Min(numleft,numMiddle,numRight)>=mSellCount)
		{
			_dp.Coin += Mathf.CeilToInt(mGotCoin+0.5f);

			dpc.addCollectionNum(itemID1,-mSellCount);
			dpc.addCollectionNum(itemID2,-mSellCount);
			dpc.addCollectionNum(itemID3,-mSellCount);
//			Dictionary<string, object> dic = new Dictionary<string, object>();
//			dic.Add("SellNum",mSellCount.ToString());
//			//WG.SLog("=actionId="+WGStrings.getText(9005)+col1.groupdes+"==parameters="+SDK.Serialize(dic));
//
//			TalkingDataGA.OnEvent(WGStrings.getText(9005)+col1.groupdes, dic);
//#endif



			DataPlayerController.getInstance().saveDataPlayer();

			WGGameUIView.Instance.freshPlayerUI(UI_FRESH.COIN);
		}
		WGGameUIView.Instance.freshMenuButton(2);
		base.OnBtnOk ();
	}
	public override void OnBtnCancel ()
	{
		BCSoundPlayer.Play(MusicEnum.button);
		base.OnBtnCancel ();
	}

	public override void showView ()
	{
		base.showView ();
		tsContent.gameObject.SetActive(true);

		tsContent.transform.localScale = tsContent.from;

		tsContent.PlayForward();

		mnIvokeBlock.InvokeBlock(0.25f,()=>{
				showViewEnd();
		});
	}
	public override void showViewEnd ()
	{
		base.showViewEnd ();
	}
	public override void hiddenView ()
	{
		base.hiddenView ();
		tsContent.PlayReverse();
		mnIvokeBlock.InvokeBlock(tsContent.duration,()=>{
				hiddenViewEnd();
		});
	}
	public override void hiddenViewEnd ()
	{
		base.hiddenViewEnd ();
	}

	static Object mObj = null;
	public static WGSellGroupItemView  CreateSellGroupItemView()
	{
		if(mObj == null)
		{
			mObj = Resources.Load("pbWGSellGroupItemView");
		}
		GameObject go = Instantiate(mObj) as GameObject;
		WGSellGroupItemView sg = go.GetComponent<WGSellGroupItemView>();

		return sg;

	}





}
