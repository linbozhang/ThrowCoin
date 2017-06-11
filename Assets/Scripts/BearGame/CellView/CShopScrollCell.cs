using UnityEngine;
using System.Collections;

public class CShopScrollCell : WGTableViewCell {

	static Object mObj = null;
	[HideInInspector]
	public CShopScrollCellView leftCell = null;
	[HideInInspector]
	public CShopScrollCellView rightCell = null;

	int _index = 0;

	bool bInit=false;
	void initShopItem()
	{
		if(bInit)return;
		bInit = true;

	}



	public void freshShopItem(int index,MDShopData data)
	{
		_index = index;
		if(leftCell == null)
		{
			leftCell = CShopScrollCellView.CreateItemCell(data);
			leftCell.transform.parent = this.transform;
			leftCell.transform.localPosition = new Vector3(0,0,0);
			leftCell.transform.localScale = Vector3.one;

		}
		else{
			leftCell.freshViewWithData(data);
		}
	}

	public static CShopScrollCell CreateShopScrollCell()
	{
		if(mObj == null)
		{
			mObj = Resources.Load("shopcell");
		}
		GameObject go = Instantiate(mObj) as GameObject;
		CShopScrollCell ci = go.GetComponent<CShopScrollCell>();

		return ci;
	}
}


