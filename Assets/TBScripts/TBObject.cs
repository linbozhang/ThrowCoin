using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TBData{
	public int act;
	public string order;
	public int status = -111;
	public int amount;
	public string key;
}
public class TBACT{
	public const int TBBuyGoodsDidSuccessWithOrder 			= 101;
	public const int TBBuyGoodsDidFailedWithOrder 			= 102;
	public const int TBBuyGoodsDidStartRechargeWithOrder	= 103;
	public const int TBBuyGoodsDidCancelByUser				= 104;

	public const int TBCheckOrderFinishedWithOrder			= 201;
	public const int TBCheckOrderDidFailed					= 202;

	public const int TBLoginResult							= 306;
	public const int TBDidLogout							= 307;
	public const int TBLeavedPlatform						= 308;
	public const int TBInitDidFinish						= 309;
}

public class TBObject : MonoBehaviour {
	DataPlayer  _dataPlayer{
		get{
			return DataPlayerController.getInstance().data;
		}
	}
	bool isUseOldMode = true;
	List<MDShopData> szIAPData = new List<MDShopData>();
	void Awake()
	{
		//WG.EnableLog = true;
		DontDestroyOnLoad(this.gameObject);
	}

	void Start()
	{
		int count = WGDataController.Instance.szShopData.Count;
		for(int i=0;i<count;i++)
		{
			MDShopData sd = WGDataController.Instance.szShopData[i];
			if(sd.type == MDShopData.JEWEL)
			{
				szIAPData.Add(sd);
			}
		}
		#if TEST

		#elif TBSDK
		WGAlertViewController.Self.showConnecting();
		TBSDK.TBSetUseOldLoadingMode(isUseOldMode);
		isUseOldMode = !isUseOldMode;
		TBSDK.TBInit();

		TBSDK.TBShowToolBar(1,true);
		#endif

	}

	void TBMessage(string msg)
	{

		Debug.Log(msg);
		TBData data = SDK.Deserialize<TBData>(msg);

		Debug.Log(SDK.Serialize(data));


		if(data.act == TBACT.TBInitDidFinish)
		{

		}
		else if(data.act == TBACT.TBLoginResult)
		{
			WGAlertViewController.Self.hiddenConnecting();
		}
		else if(data.act == TBACT.TBBuyGoodsDidSuccessWithOrder)
		{
			WGAlertViewController.Self.hiddenConnecting();
			BuySuccess(data);
		}
		else if(data.act == TBACT.TBBuyGoodsDidFailedWithOrder)
		{
			WGAlertViewController.Self.showAlertView(1800+data.status).alertViewBehavriour =(ab,view)=>{
				switch(ab)
				{
				case MDAlertBehaviour.CLICK_OK:
					view.hiddenView();
					break;
				case MDAlertBehaviour.DID_HIDDEN:
					WGAlertViewController.Self.hiddeAlertView(view.gameObject);
					break;
				}
			};
			WGAlertViewController.Self.hiddenConnecting();
			ShopOrderManager.getInstance().setOrderStatu(data.order,ShopOrderManager.BuyFail);
			ShopOrderManager.getInstance().saveShopOrder();
		}
		else if(data.act == TBACT.TBBuyGoodsDidStartRechargeWithOrder)
		{
//			TBSDK.TBCheckOrder(data.order);
//			WGAlertViewController.Self.showConnecting();
		}
		else if(data.act == TBACT.TBBuyGoodsDidCancelByUser)
		{
			WGAlertViewController.Self.showAlertView(1810).alertViewBehavriour =(ab,view)=>{
				switch(ab)
				{
				case MDAlertBehaviour.CLICK_OK:
					view.hiddenView();
					break;
				case MDAlertBehaviour.DID_HIDDEN:
					WGAlertViewController.Self.hiddeAlertView(view.gameObject);
					break;
				}
			};
			ShopOrderManager.getInstance().setOrderStatu(data.order,ShopOrderManager.BuyCancel);
			ShopOrderManager.getInstance().saveShopOrder();
			WGAlertViewController.Self.hiddenConnecting();
		}
		else if(data.act == TBACT.TBDidLogout)
		{
		}
		else if(data.act == TBACT.TBLeavedPlatform)
		{

			if(!string.IsNullOrEmpty( data.order ))
			{
#if TBSDK
				TBSDK.TBCheckOrder(data.order);
#endif
			}
			else
			{
				WGAlertViewController.Self.hiddenConnecting();
			}

		}
		else if(data.act == TBACT.TBCheckOrderFinishedWithOrder)
		{
			WGAlertViewController.Self.hiddenConnecting();
			if(data.status == 1 || data.status ==3)
			{
				BuySuccess(data);
			}
			else if(data.status == 0|| data.status == 2|| data.status == -1)
			{
				ShopOrderManager.getInstance().setOrderStatu(data.order,ShopOrderManager.BuyFail);
				WGAlertViewController.Self.showAlertView(1820+data.status).alertViewBehavriour =(ab,view)=>{
					switch(ab)
					{
					case MDAlertBehaviour.CLICK_OK:
						view.hiddenView();
						break;
					case MDAlertBehaviour.DID_HIDDEN:
						WGAlertViewController.Self.hiddeAlertView(view.gameObject);
						break;
					}
				};
			}
		}
		else if(data.act == TBACT.TBCheckOrderDidFailed)
		{
			WGAlertViewController.Self.showAlertView(1811).alertViewBehavriour =(ab,view)=>{
				switch(ab)
				{
				case MDAlertBehaviour.CLICK_OK:
					view.hiddenView();
					break;
				case MDAlertBehaviour.DID_HIDDEN:
					WGAlertViewController.Self.hiddeAlertView(view.gameObject);
					break;
				}
			};
			WGAlertViewController.Self.hiddenConnecting();
		}
	}

	void BuySuccess(TBData data)
	{

		string order = ShopOrderManager.getInstance().getOrderDes(data.order);

		for(int i=0;i<szIAPData.Count;i++)
		{
			if(order.Equals(szIAPData[i].proid))
			{

				ShopOrderManager.getInstance().setOrderStatu(order,ShopOrderManager.BuyEndStates);
				ShopOrderManager.getInstance().saveShopOrder();

				_dataPlayer.Jewel +=szIAPData[i].get_num;
				#if Add_AD
				if(!szIAPData[i].proid.Equals("Coins_60"))
				{
					WGGameWorld.Instance.RemoveAd();
				}
				#endif
#if Umeng
				Umeng.GA.Pay(szIAPData[i].cost_num,Umeng.GA.PaySource.TongBu,szIAPData[i].get_num);
#endif

				DataPlayerController.getInstance().saveDataPlayer();
				WGGameUIView.Instance.freshPlayerUI(UI_FRESH.BCOIN);
				WGAlertViewController.Self.showTipView(1001);

			}
		}
	}
}



