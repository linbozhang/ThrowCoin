using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WGShowBearController : MonoBehaviour {


	public GameObject goGemLock;
	public GameObject goLevelLock;

	public UILabel labNeedJewel;
	public UILabel labNeedLevel;
	public Dictionary<int,GameObject> dicCacheBears = new Dictionary<int, GameObject>();


	int mCurBearID = 0;
	WGBearParam mCurBearParam;
	static WGShowBearController _Self = null;
	public static WGShowBearController Instance{
		get{
			if(_Self == null)
			{

				_Self = CreateShowBear();
				SDK.AddChild(_Self.gameObject,WGRootManager.Self.goRootTopUI);
				_Self.transform.localPosition = new Vector3(0,0,-149);
			}
			return _Self;
		}
	}



	WGDataController _dataCtrl{
		get{
			return WGDataController.Instance;
		}
	}
	DataPlayer _dataPlayer{
		get{
			return DataPlayerController.getInstance().data;
		}
	}

	public void ShowMonsterWithID(int id,bool isGray = false)
	{


		mCurBearParam = _dataCtrl.GetBearParam(id);
		mCurBearID = id;

		int cost = -mCurBearParam.unlock;
		labNeedJewel.text = cost.ToString();

		labNeedLevel.text = WGStrings.getFormate(1004,mCurBearParam.unlock);

		ShowOneBear.getInstance().ShowMonsterWithID(id,isGray);
	}

	public void WillDisappear()
	{
		goGemLock.SetActive(false);
		goLevelLock.SetActive(false);
		ShowOneBear.getInstance().WillDisappear();
	}
	public void DidDisappear()
	{
		ShowOneBear.getInstance().DidDisappear();
	}
	public void ShowMode(int type)
	{
		if(type == 1)
		{
			goGemLock.SetActive(true);
			goLevelLock.SetActive(false);
		}
		else if(type ==2)
		{
			goGemLock.SetActive(false);
			goLevelLock.SetActive(true);
		}
		else if(type == 0)
		{
			goGemLock.SetActive(false);
			goLevelLock.SetActive(false);
		}
	}
	void OnBtnUnLock()
	{
		BCSoundPlayer.Play(MusicEnum.button);
		WGBearParam bear = _dataCtrl.GetBearParam(mCurBearID);
		if(!_dataPlayer.szPayObjID.Contains(mCurBearID))
		{
			if(_dataPlayer.Jewel>(-bear.unlock))
			{
				_dataPlayer.Jewel +=bear.unlock;
				//#if TalkingData
				BCObj ob = _dataCtrl.GetBCObj(mCurBearID);
				//TDGAItem.OnPurchase(ob.Name,1,Mathf.Abs(bear.unlock));
				//#endif
#if Umeng
				Umeng.GA.Buy(mCurBearID.ToString(),1,bear.unlock);
#endif
				_dataPlayer.szPayObjID.Add(mCurBearID);
				goGemLock.SetActive(false);
//				goCurBear.GetComponent<WGBear>().IsGray(false);

				ShowOneBear.getInstance().IsGray = false;

				WGGameUIView.Instance.freshPlayerUI(UI_FRESH.BCOIN);


//				goCurBear.SetActive(false);

				ShowOneBear.getInstance().ShowCurBear=false;
				goGemLock.SetActive(false);
				WGAlertViewController.Self.showAlertView(1001).alertViewBehavriour =(ab,view) => {
					switch(ab)
					{
					case MDAlertBehaviour.CLICK_OK:
						view.hiddenView();
						break;
					case MDAlertBehaviour.DID_HIDDEN:
//						goCurBear.SetActive(true);
						ShowOneBear.getInstance().ShowCurBear=true;
						WGAlertViewController.Self.hiddeAlertView(view.gameObject);
						break;
					}
				};
			}
			else{
//				goCurBear.SetActive(false);
				ShowOneBear.getInstance().ShowCurBear=false;
				goGemLock.SetActive(false);
				WGAlertViewController.Self.showAlertView(1003,1002,1007).alertViewBehavriour =(ab,view) => {
					switch(ab)
					{
					case MDAlertBehaviour.CLICK_OK:
						view.hiddenView();

						WGGameUIView.Instance.CloseCurrentView();
						break;
					case MDAlertBehaviour.CLICK_CANCEL:
						view.hiddenView();
						break;
					case MDAlertBehaviour.DID_HIDDEN:

						ShowOneBear.getInstance().ShowCurBear=true;
						WGAlertViewController.Self.hiddeAlertView(view.gameObject);
						if(view.clickIndex == MDBaseAlertView.CLICK_OK)
						{
							WGGameUIView.Instance.ViewControllerDoAct(BTN_ACT.SHOP);
							WGShopView.curShopView.InitWillShowWithTabView(SHOP_TAB_VIEW.JEWEL_SHOP);
						}
						else
						{
							goGemLock.SetActive(true);
						}
						break;
					}
				};
			}
		}
	}

	public static void DestroySelf()
	{
		if(_Self != null)
		{
			Destroy(_Self.gameObject);
			_Self = null;
		}
	}

	static WGShowBearController CreateShowBear()
	{
		Object obj = Resources.Load("pbShowBearView");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			WGShowBearController sb = go.GetComponent<WGShowBearController>();
			return sb;
		}
		return null;
	}









}
