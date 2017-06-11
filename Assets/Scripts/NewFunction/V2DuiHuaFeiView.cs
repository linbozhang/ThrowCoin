using UnityEngine;
using System.Collections;

public class V2DuiHuaFeiView : MDBaseAlertView {

	public TweenScale tsContent;
	public UILabel labTitle;
	public UILabel labDes;
	// Use this for initialization
	void Start () {
	
	}

	void FreshUI()
	{
		WGDataController dc = WGDataController.Instance;
		for(int i=0;i<dc.szHuaFei.Count;i++)
		{
			MDV2HuaFei hf = dc.szHuaFei[i];
			HuaFei_CellView hc = HuaFei_CellView.CreateHuaFeiView();
			SDK.AddChild(hc.gameObject,tsContent.gameObject);
			hc.transform.localPosition = new Vector3(0,149-i*96,0);
			hc.freshUI(hf);
		}
	}

	#region MDBaseAlertView
	public override void showView ()
	{
		base.showView ();
		Time.timeScale = 0;

		FreshUI();
		
		tsContent.transform.localScale =  tsContent.from;
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

	#endregion
	
	public static V2DuiHuaFeiView CreateDuiHuaFeiView()
	{
		Debug.Log("Error");
		Object obj = Resources.Load("pbV2DuiHuaFeiView");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			V2DuiHuaFeiView dh = go.GetComponent<V2DuiHuaFeiView>();
			return dh;
		}
		return null;
	}
	
}
