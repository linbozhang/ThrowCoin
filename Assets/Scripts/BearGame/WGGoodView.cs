using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WGGoodView : MDBaseAlertView {

	public TweenPosition tpContent;

	public List<int> szCoinGotNum= new List<int>();

	public UILabel labGotCoin;


	int mCoinNum = 0;
	int mCount = 0;
	float time = 0.1f;
	int step = 1;

	public void AddCoinNum(int coin)
	{
		int cn = coin;
		szCoinGotNum.Add(cn);
	}

	public void freshUIWith()
	{

		labGotCoin.text = "0";
		if(szCoinGotNum.Count>0)
		{
			mCoinNum = szCoinGotNum[0];
			szCoinGotNum.RemoveAt(0);
		}
		else
		{
			mCoinNum = 68;
		}
		mCount = 0;
		step =10;


		if(mCoinNum<130)
		{
			time = 2f/100;
		}
		else if(mCoinNum<200)
		{
			time = 2f/mCoinNum;
		}
		else 
		{
			time = 0.01f;
			step = Mathf.CeilToInt(mCoinNum / 200f);

		}
	}
	void TimeCount()
	{
		mCount +=step;
		if(mCount<mCoinNum)
		{
			labGotCoin.text = mCount.ToString();
		}
		else
		{
			labGotCoin.text = mCoinNum.ToString();
			CancelInvoke("TimeCount");
			hiddenView();
		}
	}


	public override void showView ()
	{
		Time.timeScale =1;
		base.showView ();
		BCSoundPlayer.Play(MusicEnum.great);
		//Debug.Log("Great");
		tpContent.ESetActive(true);
		tpContent.ResetToBeginning();
		tpContent.PlayForward();
		InvokeBlock(tpContent.duration,()=>{
			showViewEnd();
		});

	}
	public override void showViewEnd ()
	{
		base.showViewEnd ();
		CancelInvoke("TimeCount");
//		//WG.SLog("asfsfadfsa============"+time);
		InvokeRepeating("TimeCount",time,time);
	}
	public override void hiddenView ()
	{
		base.hiddenView ();
		InvokeBlock(1.5f,()=>{
			hiddenViewEnd();
		});
	}
	public override void hiddenViewEnd ()
	{
		base.hiddenViewEnd ();
	}
	public static WGGoodView CreateGoodView()
	{
		Object obj = Resources.Load("pbWGGoodView");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			WGGoodView gv = go.GetComponent<WGGoodView>();
			SDK.AddChild(go,WGRootManager.Self.goRootGameUI);
			return gv;
		}
		return null;
	}
}
















