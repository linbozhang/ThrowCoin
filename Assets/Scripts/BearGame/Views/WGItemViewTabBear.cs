using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WGItemViewTabBear : MonoBehaviour {

	public CItemUnlockView unLockView;
	public CItemUnlockView gemLockView;
	public CItemUnlockView levelLockView;

	int mCurIndex = 0;
	GameObject goCurBear=null;
	WGDataController _dataCtrl;

	public void ViewDidLoad()
	{
		_dataCtrl= WGDataController.Instance;
	}

	public void InitShowBearView()
	{
		mCurIndex = 0;

		WGShowBearController.Instance.ESetActive(true);
		ShowOneBear.getInstance().ESetActive(true);
		freshUIWithBearID(_dataCtrl.szBearsData[mCurIndex].id);

	}
	public void ViewWillDiappear()
	{
		WGShowBearController.Instance.WillDisappear();
		mCurIndex = 0;
	}
	public void ViewDidHidden(bool del = false)
	{
		if(del)
		{
			WGShowBearController.DestroySelf();
			ShowOneBear.DestroySelf();
		}
		else
		{
			WGShowBearController.Instance.DidDisappear();
			WGShowBearController.Instance.ESetActive(false);
		}
	}
	void OnBtnLeft()
	{
		mCurIndex --;
		BCSoundPlayer.Play(MusicEnum.button);
		if(mCurIndex<0)
		{

			mCurIndex = _dataCtrl.szBearsData.Count-1;
		}
		freshUIWithBearID(_dataCtrl.szBearsData[mCurIndex].id);
	}
	void OnBtnRight()
	{
		mCurIndex ++;
		BCSoundPlayer.Play(MusicEnum.button);
		if(mCurIndex>=_dataCtrl.szBearsData.Count)
		{

			mCurIndex = 0;
		}
		freshUIWithBearID(_dataCtrl.szBearsData[mCurIndex].id);
	}


	void freshUIWithBearID(int id)
	{

		BCObj obj = _dataCtrl.GetBCObj(id);
		WGBearParam bear = _dataCtrl.GetBearParam(id);

		DataPlayer _dp  = DataPlayerController.getInstance().data;

		int curLv = _dp.Level;

		bool isGray = false;


		if(bear.unlock>=0)
		{
			gemLockView.ESetActive(false);
			if(curLv>=bear.unlock)//解锁
			{
				isGray = false;
				unLockView.ESetActive(true);
				levelLockView.ESetActive(false);
				WGShowBearController.Instance.ShowMode(0);
			}
			else{//需要到达等级解锁
				isGray = true;
				unLockView.ESetActive(false);
				levelLockView.ESetActive(true);
				WGShowBearController.Instance.ShowMode(2);
			}
		}
		else{
			levelLockView.ESetActive(false);
			if(_dp.szPayObjID.Contains(id))//解锁
			{
				unLockView.ESetActive(true);
				gemLockView.ESetActive(false);
				isGray = false;
				WGShowBearController.Instance.ShowMode(0);
			}
			else{//需要花钻石解锁,没有解锁
				isGray =  true;
				unLockView.ESetActive(false);
				gemLockView.ESetActive(true);
				WGShowBearController.Instance.ShowMode(1);
			}
		}

		unLockView.freshWithBearID(obj.Name,bear.des);
		gemLockView.freshWithBearID(obj.Name,bear.des);
		levelLockView.freshWithBearID(obj.Name,bear.des);


		WGShowBearController.Instance.ShowMonsterWithID(id,isGray);


	}



}
