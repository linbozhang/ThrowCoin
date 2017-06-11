using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DataCoin
{ 
	public DataCoin (){}

	public List<float[]> CoinPos ;

	public bool IsFirstLoad = true;
	
}
[Serializable]
public class MDDataCoin
{ 


	public List<float[]> CoinPos ;
	public List<int> CoinID;
	public List<float[]>CoinRoto;
	public MDDataCoin (){
		CoinPos = new List<float[]>();
		CoinID = new List<int>();
		CoinRoto = new List<float[]>();
	}
	public bool IsFirstLoad = true;

}

public class DataCoinController{

	private static DataCoinController dataCoinCtrl;
	private DataCoinController(){
	}
	public static DataCoinController getInstance(){
		if(dataCoinCtrl == null)
			dataCoinCtrl = new DataCoinController();
		return dataCoinCtrl;
	}

	private MDDataCoin _data;
	public MDDataCoin data{
		get{
			if(_data == null){
				_data = this.loadDataCoin();

				if(_data == null||_data.IsFirstLoad){

					SharedPrefs prefs = SharedPrefs.getInstance();
					prefs.createAccountFolder();
					_data = new MDDataCoin();
					this.saveDataCoin();
				}
			}

			return _data;
		}
	}
//	public void resetDataCoin()
//	{
//		_data = new MDDataCoin();
//		_data.IsFirstLoad = false;
//
//		this.saveDataCoin();
//	}
	public void saveDataCoin(){
		SharedPrefs prefs = SharedPrefs.getInstance();

		prefs.saveValue(data ,SharedPrefs.TYPE_Coin,Consts.AES_ENCRYPT);
	}

	private MDDataCoin loadDataCoin(){
		SharedPrefs prefs = SharedPrefs.getInstance ();
		MDDataCoin data = prefs.loadDataCoin(Consts.AES_ENCRYPT);
		return data;
	}

}