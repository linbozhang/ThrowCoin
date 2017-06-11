using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WGProductController : WGMonoComptent,IAPDelegate {

	DataPlayer  _dataPlayer{
		get{
			return DataPlayerController.getInstance().data;
		}
	}

	List<MDShopData> szIAPData = new List<MDShopData>();

	static string sCurBuy = "";

	public static WGProductController instance;
	void Awake()
	{
		instance = this;
	}
	// Use this for initialization
	void Start () {

		int count = WGDataController.Instance.szShopData.Count;
		for(int i=0;i<count;i++)
		{
			MDShopData sd = WGDataController.Instance.szShopData[i];
			if(sd.type == MDShopData.JEWEL)
			{
				szIAPData.Add(sd);
			}
		}
		IOSInterface.self._iapDelegate = this;
		IAP.RequestProducts();
	}

	public void OnBuyWithMDShopData(MDShopData data)
	{

#if UNITY_ANDROID

		sCurBuy =""+data.proid2;

		YeHuoSDK.YHPay(sCurBuy,(float)data.cost_num,data.get_num,(success)=>{

			if(success)
			{
				for(int i=0;i<szIAPData.Count;i++)
				{
					if( sCurBuy.Equals(szIAPData[i].proid2))
					{
						_dataPlayer.Jewel +=szIAPData[i].get_num;

						DataPlayerController.getInstance().saveDataPlayer();
						WGGameUIView.Instance.freshPlayerUI(UI_FRESH.BCOIN);
						WGAlertViewController.Self.showTipView(1001);
					}
				}
			}
		});
#else
		WGAlertViewController.Self.showConnecting();
		#if TEST
		mnIvokeBlock.InvokeBlock(0.3f,()=>{
			didCompleteWithRecepit("",data.proid);
		});
		#elif TBSDK
		if(TBSDK.TBIsLogined())
		{
			string order = data.proid+"_"+Core.nData.sysTime+"_"+TBSDK.TBUserID();

			Debug.Log(order);

			ShopOrderManager.getInstance().SetOrderDes(order,data.proid);
			ShopOrderManager.getInstance().setOrderStatu(order,ShopOrderManager.BuyingStates);
			ShopOrderManager.getInstance().saveShopOrder();
			TBSDK.TBPayRMB(data.cost_num,order,data.proid);
		}
		else
		{
			TBSDK.TBLogin(0);
		}
		#else
		IAP.payForProIdentifier(data.proid);
		#endif
#endif

	}
	#region IAPDelegate
	public void didCompleteWithRecepit(string recepit,string productId)
	{
		//WG.SLog("didCompleteWithRecepit==="+productId);
		#if UNITY_ANDROID
#else
		WGAlertViewController.Self.hiddenConnecting();
		IAP.didFinishLastTransaction(productId);
		#endif
		for(int i=0;i<szIAPData.Count;i++)
		{
			if(productId.Equals(szIAPData[i].proid) || productId.Equals(szIAPData[i].proid2))
			{
				_dataPlayer.Jewel +=szIAPData[i].get_num;
#if Add_AD
				if(!szIAPData[i].proid.Equals("Coins_60"))
				{
					WGGameWorld.Instance.RemoveAd();
				}
#endif
#if Umeng
				Umeng.GA.Pay(szIAPData[i].cost_num,Umeng.GA.PaySource.AppStore,szIAPData[i].get_num);
#endif
				DataPlayerController.getInstance().saveDataPlayer();
				WGGameUIView.Instance.freshPlayerUI(UI_FRESH.BCOIN);
				WGAlertViewController.Self.showTipView(1001);
			}
		}
	}
	public void didFailedBuyProduct()
	{
		WGAlertViewController.Self.hiddenConnecting();
		WGAlertViewController.Self.showAlertView(1078).alertViewBehavriour =(ab,view) => {
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
	public void didReceivedProducts()
	{

	}
	#endregion
}
