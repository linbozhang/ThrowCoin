using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class CItemCollectionCell : MonoBehaviour {

	public GameObject goLeft;
	public GameObject goRight;
	public GameObject goMiddel;
	public UILabel labGroupNum;
	public UILabel labGroupName;
	public UISprite spTitleBackground;
	public UISprite spTip;

	public UIDragCamera[] szDragCamera;

	static Object mOjb;

	public List<CItemCollectionCellView> szItemColCell = new List<CItemCollectionCellView>();

	int mObjID1;
	int mObjID2;
	int mObjID3;
	int mCanSellNum;
	public void SetDragCamera(UIDraggableCamera cam)
	{
		for(int i=0,max=szDragCamera.Length;i<max;i++)
		{
			szDragCamera[i].draggableCamera = cam;
		}
	}
	public void freshWithCollections(int[] ids,Color color)
	{
		if(ids.Length >= 3)
		{
			freshWithThreeCollection(ids[0],ids[1],ids[2],color);
		}
	}
	void freshWithThreeCollection(int id1,int id2,int id3,Color color)
	{
		mObjID1 = id1;
		mObjID2 = id2;
		mObjID3 = id3;

		spTitleBackground.color = color;
		WGDataController _dataCtrl = WGDataController.Instance;
		int numleft = _dataCtrl.GetCollectionOwnNum(id1);
		int numMiddle = _dataCtrl.GetCollectionOwnNum(id2);
		int numRight = _dataCtrl.GetCollectionOwnNum(id3);

		mCanSellNum = Mathf.Min(numleft,numMiddle,numRight);

		BCCollectionInfo bci = WGDataController.Instance.GetCollectionInfo(id1);

		labGroupName.text = bci.groupdes;
		labGroupNum.text = mCanSellNum.ToString();

		CItemCollectionCellView leftCell = CItemCollectionCellView.CreateCollectionCellView();
		AddCell(leftCell.gameObject,goLeft);
		leftCell.freshWithCollectionID(id1,color);
		szItemColCell.Add(leftCell);

		CItemCollectionCellView middelCell = CItemCollectionCellView.CreateCollectionCellView();
		AddCell(middelCell.gameObject,goMiddel);
		middelCell.freshWithCollectionID(id2,color);
		szItemColCell.Add(middelCell);

		CItemCollectionCellView rightCell = CItemCollectionCellView.CreateCollectionCellView();
		AddCell(rightCell.gameObject,goRight);
		rightCell.freshWithCollectionID(id3,color);
		szItemColCell.Add(rightCell);

	}
	public void FreshUI()
	{

		WGDataController _dataCtrl = WGDataController.Instance;
		for(int i=0;i<szItemColCell.Count;i++)
		{
			szItemColCell[i].refreshView();
		}
		int numleft = _dataCtrl.GetCollectionOwnNum(mObjID1);
		int numMiddle = _dataCtrl.GetCollectionOwnNum(mObjID2);
		int numRight = _dataCtrl.GetCollectionOwnNum(mObjID3);

		mCanSellNum = Mathf.Min(numleft,numMiddle,numRight);

		labGroupNum.text = mCanSellNum.ToString();
		if(mCanSellNum>0){
			spTip.gameObject.SetActive(true);
		}else{
			spTip.gameObject.SetActive(false);
		}
	}
	void AddCell(GameObject go,GameObject super)
	{
		go.transform.parent=super.transform;
		go.transform.localPosition = Vector3.zero;
		go.transform.localScale = Vector3.one;
	}

	void OnBtnSell()
	{
		BCSoundPlayer.Play(MusicEnum.button);
		WGDataController _dataCtrl = WGDataController.Instance;
//		DataPlayerController dpc = DataPlayerController.getInstance();
		if(mCanSellNum>0)
		{

			WGSellGroupItemView sgv = WGSellGroupItemView.CreateSellGroupItemView();

			SDK.AddChild(sgv.gameObject,WGRootManager.Self.goRootTopUI);

			sgv.FreshSellGroupWithID(mObjID1,mObjID2,mObjID3,mCanSellNum);

			sgv.alertViewBehavriour =(ab,view) =>{
				switch(ab)
				{
				case MDAlertBehaviour.CLICK_OK:
					{
						view.hiddenView();
						FreshUI();
					}
					break;
				case MDAlertBehaviour.CLICK_CANCEL:
					view.hiddenView();
					break;
				case MDAlertBehaviour.DID_HIDDEN:

					Destroy(view.gameObject);
					break;
				}
			};

			return;
		}
		else
		{
			WGAlertViewController.Self.showAlertView(1011).alertViewBehavriour =(ab,view) => {
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


	public static CItemCollectionCell CreateCollectionCell()
	{
		if(mOjb == null)
		{
			mOjb = Resources.Load("pbCItemCollectionCell");
		}
		GameObject go = Instantiate(mOjb) as GameObject;
		CItemCollectionCell cc = go.GetComponent<CItemCollectionCell>();
		return cc;
	}
}
