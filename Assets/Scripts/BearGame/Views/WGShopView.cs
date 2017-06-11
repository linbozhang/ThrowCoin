using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum SHOP_TAB_VIEW{
	NONE,
	Coin_SHOP,
	JEWEL_SHOP,
	ITEM_SHOP,
}
public class WGShopView : MDBaseAlertView,WGTableViewDelegate {

	static Object mObj = null;

	[SerializeField]
	protected TweenScale tsContent;
	[SerializeField]
	protected UISprite spGrayBackground;

	public WGTableView mTableView;

	public WGPanelButton wpBtnItem;
	public WGPanelButton wpBtnJewel;
	public WGPanelButton wpBtnCoin;

	public WGTabView mTabView;

	public GameObject goItemView;
	public GameObject goCoinView;
	public GameObject goJewelView;
	

	public static WGShopView curShopView = null;

	DataPlayer dp{
		get{
			return DataPlayerController.getInstance().data;
		}
	}

	int mDataCount;
	List<MDShopData> szShopData ;
	int mIdentifier = 1;

	List<MDShopData> szShopDataForSellItems = new List<MDShopData>();
	List<MDShopData> szShopDataForSellCoin = new List<MDShopData>();
	List<MDShopData> szShopDataForSellJewel = new List<MDShopData>();

	SHOP_TAB_VIEW mState = SHOP_TAB_VIEW.NONE;

	void viewDidLoad ()
	{

		mTableView.csDelegate = this;
		mTableView.f_WorldInScreenRate =  0.001760563f;

		int count = WGDataController.Instance.szShopData.Count;
		for(int i=0;i<count;i++)
		{
			MDShopData sd = WGDataController.Instance.szShopData[i];
			if(sd.type == MDShopData.ITEM)
			{
				if(sd.id == WGDefine.SK_777Up1 || sd.id == WGDefine.SK_777Up2)
				{
					if(YeHuoSDK.bCommonTiger)
					{
						szShopDataForSellItems.Add(sd);
					}
				}
				else{
					szShopDataForSellItems.Add(sd);
				}
			}
			else if(sd.type == MDShopData.COIN)
			{
				szShopDataForSellCoin.Add(sd);
			}
			else if(sd.type == MDShopData.JEWEL)
			{
				szShopDataForSellJewel.Add(sd);
			}
		}

		wpBtnCoin.title.text = WGStrings.getText(1096);
		wpBtnItem.title.text = WGStrings.getText(1095);
		wpBtnJewel.title.text = WGStrings.getText(1097);

		mState = SHOP_TAB_VIEW.ITEM_SHOP;

		szShopData = szShopDataForSellItems;
		mDataCount = szShopData.Count;
		mTableView.I_Hang = 4;
		mTableView.f_TileHeight = 170;
		mTableView.fAddOffset = 110;
		mTableView.AllReset();
		mTableView.reloadData();
		mIdentifier = 1;

		goItemView.SetActive(true);
		goCoinView.SetActive(false);
		goJewelView.SetActive(false);

		mTabView.InitState(wpBtnItem.button.transform.localPosition,wpBtnItem.title,goItemView);

	}
	#region MDBaseAlertView ...
	public override void showView ()
	{
		Time.timeScale = 0;
		
		viewDidLoad();
		
		base.showView ();
		
		if(dp.Jewel<=0)
		{
			InitWillShowWithTabView(SHOP_TAB_VIEW.JEWEL_SHOP);
		}
		else if(dp.Coin<=0)
		{
			InitWillShowWithTabView(SHOP_TAB_VIEW.Coin_SHOP);
		}
		tsContent.transform.localScale = tsContent.from;
		tsContent.ESetActive(true);
		tsContent.PlayForward();
		InvokeBlock(tsContent.duration,()=>{
			showViewEnd();
		});
	}

	public override void hiddenView ()
	{
		base.hiddenView ();
		Time.timeScale = 1;
		tsContent.PlayReverse();
		InvokeBlock(tsContent.duration,()=>{
			hiddenViewEnd();
		});
	}
	public override void hiddenViewEnd ()
	{
		base.hiddenViewEnd ();
	}
	public void InitWillShowWithTabView(SHOP_TAB_VIEW state)
	{
		switch(state)
		{
		case SHOP_TAB_VIEW.Coin_SHOP:
			OnBtnCoinShop();
			break;
		case SHOP_TAB_VIEW.ITEM_SHOP:
			OnBtnItemShop();
			break;
		case SHOP_TAB_VIEW.JEWEL_SHOP:
			OnBtnJewelShop();
			break;
		}
	}
	#endregion
	void OnBtnJewelShop()
	{
		if(mState == SHOP_TAB_VIEW.JEWEL_SHOP)
		{
			return;
		}
		BCSoundPlayer.Play(MusicEnum.button);
		mState = SHOP_TAB_VIEW.JEWEL_SHOP;
		mTabView.ChangeState(wpBtnJewel.button.transform.localPosition,wpBtnJewel.title,goJewelView);
		mIdentifier = 3;
		szShopData = szShopDataForSellJewel;
		mDataCount = szShopData.Count;
		mTableView.I_Hang = 4;
		mTableView.f_TileHeight = 170;
		mTableView.fAddOffset = 120;
		mTableView.AllReset();
		mTableView.reloadData();
	}
	void OnBtnItemShop()
	{
		if(mState == SHOP_TAB_VIEW.ITEM_SHOP){
			return;
		}
		BCSoundPlayer.Play(MusicEnum.button);
		mState = SHOP_TAB_VIEW.ITEM_SHOP;
		mTabView.ChangeState(wpBtnItem.button.transform.localPosition,wpBtnItem.title,goItemView);
		mIdentifier = 1;
		szShopData = szShopDataForSellItems;
		mDataCount = szShopData.Count;
		mTableView.I_Hang = 4;
		mTableView.f_TileHeight = 170;
		mTableView.fAddOffset = 110;
		mTableView.AllReset();
		mTableView.reloadData();
	}
	void OnBtnCoinShop()
	{
		if(mState == SHOP_TAB_VIEW.Coin_SHOP)
		{
			return;
		}
		BCSoundPlayer.Play(MusicEnum.button);
		mState = SHOP_TAB_VIEW.Coin_SHOP;
		mTabView.ChangeState(wpBtnCoin.button.transform.localPosition,wpBtnCoin.title,goCoinView);
		mIdentifier = 2;
		szShopData = szShopDataForSellCoin;
		mDataCount = szShopData.Count;
		mTableView.I_Hang = 4;
		mTableView.f_TileHeight = 140;
		mTableView.fAddOffset = 10;
		mTableView.AllReset();
		mTableView.reloadData();
	}

	public WGTableViewCell WGTableViewCellWithIndex(WGTableView scrView, int index){

		WGTableViewCell cell = scrView.getCacheCellsWithIdentifier(mIdentifier);

		CShopScrollCell tCell ;
		if(cell == null)
		{
			tCell = CShopScrollCell.CreateShopScrollCell();
			cell = tCell as WGTableViewCell;
			cell.identifier = mIdentifier;
		}
		else{
			tCell = cell as CShopScrollCell;
		}
		tCell.index = index;

		tCell.freshShopItem(index,szShopData[index]);

		return cell;

	}
	//	获取cell的数目,应该是总行数*总列数
	public int NumberOfTableViewCells(){
		
		return mDataCount;
	}
	//	选择一个cell对象
	public void SelectTableViewCell(WGTableViewCell aCell,int index){
		Debug.Log("selectScrollViewCellAndIndex=="+index);
	}
	//	长按一个cell对象
	public void HoldTableViewCell(WGTableViewCell aCell,int index){
		Debug.Log("holdScrollViewCellAndIndex=="+index);
	}
	//	刷新当前在视图内能展示的Cell 需要调用SQYScrollView.UpdataCells()函数
	public void UpdateShowTableViewCells(WGTableViewCell aCell,int index){

		CShopScrollCell tCell=aCell as CShopScrollCell;
		tCell.freshShopItem(index,szShopData[index]);
	}

	public static WGShopView CreateShopView()
	{
		if(mObj == null)
		{
			mObj = Resources.Load("pbWGShopView");
		}
		if(mObj != null)
		{
			GameObject go = Instantiate(mObj) as GameObject;
			WGShopView sv = go.GetComponent<WGShopView>();
			curShopView = sv;
			return sv;
		}
		return null;
	}
}



















