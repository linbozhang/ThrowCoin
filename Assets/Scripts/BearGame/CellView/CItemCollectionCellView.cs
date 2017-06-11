using UnityEngine;
using System.Collections;

public class CItemCollectionCellView : MonoBehaviour {

	public UISprite spBackground;
	public UISprite spBottom;
	public UILabel labNum;
	public GameObject goCollectionPanel;

	GameObject goCollection;
	int mItemColID;

	static Object mObj=null;



	public void freshWithCollectionID(int id,Color color)
	{

		WGDataController _dataCtrl = WGDataController.Instance;
		mItemColID = id;
		labNum.text = _dataCtrl.GetCollectionOwnNum(id).ToString();

		BCObj obj = _dataCtrl.GetBCObj(id);
		if(obj !=null)
		{
			GameObject go = Instantiate(obj.goRes,Vector3.zero,Quaternion.Euler(270,0,0)) as GameObject;

			Vector3 scale = go.transform.localScale;
			go.transform.parent = this.transform;
			go.transform.localScale = Vector3.one*14.6f;
			go.transform.localPosition = goCollectionPanel.transform.localPosition;
			go.transform.parent = goCollectionPanel.transform;

			Destroy(go.GetComponent<Rigidbody>());
			Destroy(go.GetComponent<BoxCollider>());
			goCollectionPanel.GetComponent<TweenRotation>().delay = Random.Range(0,1);
		}

		spBackground.color = color;
		spBottom.color = color;

	}
	public void refreshView()
	{
		labNum.text = WGDataController.Instance.GetCollectionOwnNum(mItemColID).ToString();
	}

	public static CItemCollectionCellView CreateCollectionCellView()
	{
		if(mObj == null)
		{
			mObj = Resources.Load("pbCItemCollectionCellView");
		}
		GameObject go = Instantiate(mObj) as GameObject;

		CItemCollectionCellView cc = go.GetComponent<CItemCollectionCellView>();

		return cc;

	}



}
