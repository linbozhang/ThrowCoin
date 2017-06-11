using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ShowOneBear : MonoBehaviour {

	public Transform goBearParent;
	public TweenScale tsDisappear;

	GameObject goBear=null;

	public Dictionary<int,GameObject> dicCacheBears = new Dictionary<int, GameObject>();

	public static ShowOneBear Self = null;

	static Object mObj=null;
	int mCurBearID = 0;

	WGBearParam mCurBearParam;

	public bool ShowCurBear{
		set{
			goBear.ESetActive(value);
		}
	}
	public bool IsGray{
		set{
			WGBear bear = goBear.GetComponent<WGBear>();
			if(bear != null)
			{
				bear.IsGray(value);
			}
		}
	}

	public void WillDisappear()
	{
		tsDisappear.enabled = true;
		tsDisappear.ResetToBeginning();
		tsDisappear.PlayForward();
	}
	public void DidDisappear()
	{
		tsDisappear.enabled = false;
		tsDisappear.gameObject.transform.localScale = Vector3.one;
		Self.ESetActive(false);
	}

	public void ShowOneWithID(int id)
	{
		if(goBear != null)
		{
			Destroy(goBear);
		}
		BCObj obj = WGDataController.Instance.GetBCObj(id);

		goBear = Instantiate(obj.goRes) as GameObject;

		goBear.transform.parent = goBearParent;
		goBear.transform.localPosition = Vector3.zero;
		goBear.transform.localEulerAngles = Vector3.zero;
		goBear.transform.localScale = Vector3.one;
		if(id == WGDefine.BossID)
		{
			goBear.transform.localScale = Vector3.one*0.6f;
		}

	}
	public static void DestroySelf()
	{
		if(Self != null)
		{
			Destroy(Self.gameObject);
			Self = null;
		}
	}

	public void ShowMonsterWithID(int id,bool isGray = false)
	{
		if(goBear != null)
		{
			goBear.SetActive(false);
			dicCacheBears.Add(mCurBearID,goBear);
		}

		
		GameObject go = null;
		if(dicCacheBears.TryGetValue(id,out go))
		{
			go.SetActive(true);
			dicCacheBears.Remove(id);
		}
		else{
			BCObj mObj = WGDataController.Instance.GetBCObj(id);

			go = Instantiate(mObj.goRes) as GameObject;
			go.transform.parent = goBearParent.transform;
			go.transform.localPosition = Vector3.zero;
			go.transform.localEulerAngles = Vector3.zero;
			go.transform.localScale = Vector3.one;
			if(mObj.ID == WGDefine.BossID)
			{
				go.transform.localScale = Vector3.one*0.6832443f;
			}
			else if(mObj.ID == WGDefine.PayBear1 || mObj.ID == WGDefine.PayBear2)
			{
				go.transform.localScale = Vector3.one*0.8f;
			}
			else
			{
				go.transform.localScale = Vector3.one;
			}
			go.transform.localEulerAngles = Vector3.zero;
		}
		WGBear bear = go.GetComponent<WGBear>();
		if(bear != null)
		{
			bear.IsGray(isGray);
		}
		
		
		goBear =go;
		mCurBearID = id;
		
	}

	public static ShowOneBear getInstance()
	{
		if(Self == null)
		{
			if(mObj == null)
			{
				mObj = Resources.Load("pbShowOneBear");
			}
			if(mObj != null)
			{
				GameObject go = Instantiate(mObj) as GameObject;
				go.transform.position = new Vector3(2000,0,0);
				Self = go.GetComponent<ShowOneBear>();
			}
		}
		return Self;
	}

}
