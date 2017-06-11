using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HuaFei_CellView : MonoBehaviour {

	public UISprite spHuaFeiNum;
	public GameObject[] szPanels;
	public TweenAlpha[] szEffect;

	List<HuaFei_Cell1_3> szHFCells;

	bool bInit= false;
	void InitView()
	{
		if(bInit)return;
		bInit =true;

		szHFCells = new List<HuaFei_Cell1_3>();
		for(int i=0;i<3;i++)
		{
			HuaFei_Cell1_3 c3 = HuaFei_Cell1_3.CreateHuafeiCell1_3();
			SDK.AddChild(c3.gameObject,szPanels[i]);
			szHFCells.Add(c3);
		}
		for(int i=0;i<szEffect.Length;i++)
		{
			szEffect[i].duration = Random.Range(0.5f,1.2f);
			szEffect[i].delay = Random.Range(0,0.5f);
		}
	}

	void OnBtnDuiHuan()
	{
		HuaFeiAlertView alert = HuaFeiAlertView.CreateAlertView();
		alert.alertViewBehavriour = (ab,view)=>{
			switch(ab)
			{
			case MDAlertBehaviour.CLICK_OK:
				view.hiddenView();
				break;
			case MDAlertBehaviour.DID_HIDDEN:
				Destroy(view.gameObject);
				break;
			}
		};
		alert.showView();
	}

	public void freshUI(MDV2HuaFei hf)
	{
		spHuaFeiNum.spriteName = "huafei_"+hf.huafei.ToString();
		for(int i=0;i<3;i++)
		{
			szHFCells[i].freshUI(hf.cid[i]);
		}

	}



	public static HuaFei_CellView CreateHuaFeiView()
	{
		Object obj =Resources.Load("pbHuaFei_CellView");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			HuaFei_CellView hf = go.GetComponent<HuaFei_CellView>();
			hf.InitView();
			return hf;
		}
		return null;
	}
}


public class MDV2HuaFei{
	public int huafei;
	public int[] cid;
}