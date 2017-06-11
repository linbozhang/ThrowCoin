using UnityEngine;
using System.Collections;

public class WGPlayerInfoView : MonoBehaviour {

	public UISprite sp_Coin;
	public UISprite sp_Time;
	public UISprite spPlayerRole;
	public UISlider sld_Exp;

	public UILabel lab_Coin;
	public UILabel lab_BCoin;

	public UILabel lab_Level;
	public UILabel lab_Time;

	public TweenScale tsGetCoinEffect;

	public GameObject go_AddCoinPanel;

	public GameObject prefbAddCoin;

	public GameObject pbGoldAdd;

	DataPlayer _dp{
		get{
			return DataPlayerController.getInstance().data;
		}
	}

	int _Time=60;
	int _previouCoinNum = 0;
	bool _bCounting = true;

	void Start()
	{

		_previouCoinNum = _dp.Coin;
		lab_Coin.text = _previouCoinNum.ToString();
		_Time = WGConfig.ADD_TIME;

		FreshPlayerUI(UI_FRESH.BCOIN | UI_FRESH.COIN |UI_FRESH.EXP |UI_FRESH.LEVEL);

		InvokeRepeating("TimeCount",1,1);
		_bCounting = true;
		FreshTimeCount();

		tsGetCoinEffect.enabled = false;
	}


	void FreshTimeCount()
	{
		if(DataPlayerController.getInstance().data.Coin <WGConfig.AUTO_ADD_MAX)
		{
			if(!_bCounting)
			{
				lab_Time.gameObject.SetActive(true);
				sp_Time.gameObject.SetActive(true);
				_bCounting = true;
			}
		}
		else{
			if(_bCounting)
			{
				_bCounting = false;
				sp_Time.gameObject.SetActive(false);
				lab_Time.gameObject.SetActive(false);
			}
		}
	}


	void TimeCount()
	{
		if(!_bCounting)return;
		_Time--;
		if(_Time<0){
			_Time = WGConfig.ADD_TIME;
			if(_dp.Coin<WGConfig.AUTO_ADD_MAX)
			{
				_dp.Coin +=10;
				freshPlayerCoin();
			}
		}
		lab_Time.text = _Time.ToString("00");
	}


	public void FreshPlayerUI(UI_FRESH ui)
	{
		if((ui & UI_FRESH.COIN) == UI_FRESH.COIN)
		{
			freshPlayerCoin();
		}
		if((ui & UI_FRESH.BCOIN) == UI_FRESH.BCOIN)
		{
			freshPlayerBCoin();
		}
		if((ui & UI_FRESH.EXP) == UI_FRESH.EXP)
		{
			freshPlayerExp();
		}
		if((ui & UI_FRESH.LEVEL) == UI_FRESH.LEVEL)
		{
			freshPlayerLevel();
		}

	}
	public void FreshPlayerHead()
	{
		if(_dp.mR == 1)
		{
			spPlayerRole.spriteName = "aIcon_100"+_dp.r.ToString();
		}
		else
		{
			spPlayerRole.spriteName = "aIcon_1002";
		}
	}
	void freshPlayerCoin()
	{

		if(_previouCoinNum != _dp.Coin)
		{
//			if(_previouCoinNum<_dp.Coin)//增加金币了
//			{
//				int min = Mathf.Min(_dp.Coin-_previouCoinNum,5);
//
//				for(int i=0;i<=min;i++)
//				{
//					StartCoroutine(AddGold((i)*0.5f));
//				}
//			}
			FreshTimeCount();
			_previouCoinNum = _dp.Coin;
			lab_Coin.text = _previouCoinNum.ToString();
			WGGameUIView.Instance.freshImageEffect(_dp.Coin);
			                   
		}
	}

	void freshPlayerBCoin()
	{

		lab_BCoin.text = _dp.Jewel.ToString();
	}
	void freshPlayerExp()
	{

		int curLevelExp=WGDataController.Instance.GetLevelUpReward(_dp.Level).exp;
		int nextLevelExp = WGDataController.Instance.GetLevelUpReward(Mathf.Min(_dp.Level+1,WGConfig.MAX_LEVEL)).exp;

		sld_Exp.value = (_dp.Exp - curLevelExp)*1.0f/(nextLevelExp-curLevelExp);
	}
	void freshPlayerLevel()
	{
		lab_Level.text = _dp.Level.ToString();
//		if(_dp.Level==WGConfig.MAX_LEVEL){
//			lab_Level.text="MAX";
//		}
		//lab_Level.text = "Lv:"+ _dp.Level.ToString();
	}

	public void PlayAddCoinEffect1(int num)
	{
		StartCoroutine(AddGold(0.1f));
	}

	IEnumerator AddGold(float delta)
	{
		yield return new WaitForSeconds(delta);

		GameObject go = Instantiate(pbGoldAdd) as GameObject;
		go.transform.parent = sp_Coin.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
		Destroy(go,1);
		if(!tsGetCoinEffect.enabled)
		{
			tsGetCoinEffect.enabled = true;
			tsGetCoinEffect.ResetToBeginning();
			tsGetCoinEffect.PlayForward();
			yield return new WaitForSeconds(delta+0.4f);
			tsGetCoinEffect.enabled = false;
		}
	}
	public void PlayAddCoinEffect(int num)
	{
		GameObject go = Instantiate(prefbAddCoin) as GameObject;
		UILabel lab=go.GetComponent<UILabel>();
		lab.text = "+"+num.ToString();
		go.transform.parent = go_AddCoinPanel.transform;
		go.transform.localPosition = Vector3.zero;
		Destroy(go,0.5f);
	}


	void OnBtnCoin()
	{

		WGGameUIView.Instance.ViewControllerDoAct(BTN_ACT.SHOP);
		WGShopView.curShopView.InitWillShowWithTabView(SHOP_TAB_VIEW.Coin_SHOP);
	}
	void OnBtnJewel()
	{
		WGGameUIView.Instance.ViewControllerDoAct(BTN_ACT.SHOP);
		WGShopView.curShopView.InitWillShowWithTabView(SHOP_TAB_VIEW.JEWEL_SHOP);
	}
	void OnBtnChangeRole()
	{
		WGGameWorld.Instance.ShowSelectRoleAlert();
		if(YeHuoSDK.bShow3RoalGift){
			WGGameWorld.Instance.ShowBuyRoleAlert();
		}

	}

}
