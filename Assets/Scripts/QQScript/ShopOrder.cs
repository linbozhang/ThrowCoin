using System;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
[Serializable]
public class ShopOrder  {
	public Dictionary<string,int> dicOrders= new Dictionary<string, int>();
	public Dictionary<string,string> dicShopData = new Dictionary<string, string>();
	public ShopOrder()
	{

	}
}


public class ShopOrderManager
{

	public const int BuyingStates 	=  	1;
	public const int BuyFail		=	2;
	public const int BuyCancel		=	3;
	public const int BuyLeave		=	4;
	public const int BuyEndStates 	= 	8;

	private static ShopOrderManager dataCtrl;
	private ShopOrderManager(){
	}
	public static ShopOrderManager getInstance(){
		if(dataCtrl == null)
			dataCtrl = new ShopOrderManager();
		return dataCtrl;
	}


	private ShopOrder _data;
	public ShopOrder data{
		get{
			if(_data == null){
				_data = this.loadShopOrder();

				if(_data == null)
				{

					SharedPrefs prefs = SharedPrefs.getInstance();
					prefs.createAccountFolder();
					_data = new ShopOrder();

					this.saveShopOrder();
				}
			}

			return _data;
		}
	}

	public void SetOrderDes(string order,string des)
	{
		if(!data.dicShopData.ContainsKey(order))
		{
			data.dicShopData.Add(order,des);
		}
	}
	public string getOrderDes(string order)
	{
		if(!string.IsNullOrEmpty(order) && data.dicShopData.ContainsKey(order))
		{
			return data.dicShopData[order];
		}
		return null;
	}

	public void saveShopOrder(){
		SharedPrefs prefs = SharedPrefs.getInstance();

		prefs.saveValue(_data ,SharedPrefs.TYPE_Shop,Consts.AES_ENCRYPT);
	}

	private ShopOrder loadShopOrder(){
		SharedPrefs prefs = SharedPrefs.getInstance ();
		ShopOrder data = prefs.loadShopOrder(Consts.AES_ENCRYPT);
		return data;
	}


	public string getUnfinishedOrder()
	{
		foreach(KeyValuePair<string,int> kp in data.dicOrders)
		{
			if(kp.Value == BuyingStates)
			{
				return kp.Key;
			}
		}
		return null;
	}
	public void setOrderStatu(string order,int statu)
	{
		if(data.dicOrders.ContainsKey(order))
		{
			data.dicOrders[order] = statu;
		}
		else
		{
			data.dicOrders.Add(order,statu);
		}
	}
	public void removeOrder(string order)
	{
		if(data.dicOrders.ContainsKey(order))
		{
			data.dicOrders.Remove(order);
		}
		if(data.dicShopData.ContainsKey(order))
		{
			data.dicShopData.Remove(order);
		}
	}


}
