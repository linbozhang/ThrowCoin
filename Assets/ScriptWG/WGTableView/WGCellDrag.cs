using UnityEngine;
using System.Collections;

public class WGCellDrag : MonoBehaviour {


	public WGTableView myTableView = null;

	Transform mTrans=null;

	bool bInit=false;

	void OnEnable ()
	{
		InitCell();
	}
	void Start ()
	{
		InitCell();
	}
	void InitCell ()
	{
		if(mTrans == null)
		{
			mTrans = transform;
		}
		if(myTableView == null)
		{
			myTableView = NGUITools.FindInParents<WGTableView>(mTrans);
		}
	}

	void OnPress (bool pressed)
	{
		if(pressed)
		{
			myTableView.OnRedMouseDown(Input.mousePosition);
		}
		else{
			myTableView.OnRedMouseUp(Input.mousePosition);
		}
	}
	void OnDrag (Vector2 delta)
	{
		myTableView.OnRedMouseMove(Input.mousePosition);
	}

	void OnScroll (float delta)
	{

	}

}
