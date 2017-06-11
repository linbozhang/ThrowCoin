using UnityEngine;
using System.Collections;

public class WGHelpView : MDBaseAlertView {

	public TweenScale tsContent;
	public UILabel labTitle;
	public UIWidget topWidget;
	public UIWidget butWidget;

	public GameObject[] szPrefabs;

	GameObject goCurHelp = null;

	int showIndex = 0;

	public override void OnBtnOk ()
	{
		base.OnBtnOk ();
	}
	public override void OnBtnCancel ()
	{
		base.OnBtnCancel ();
		showIndex++;
		if(!YeHuoSDK.bCommonTiger)
		{
			if(showIndex == 2|| showIndex == 6)
			{
				showIndex ++;
			}
		}
		if(showIndex>=szPrefabs.Length)
		{
			showIndex -=szPrefabs.Length;
		}

		showNextPrefabs();
	}
	public override void OnBtnBackGround ()
	{
		base.OnBtnBackGround ();
		showIndex--;
		if(!YeHuoSDK.bCommonTiger)
		{
			if(showIndex == 2|| showIndex == 6)
			{
				showIndex --;
			}
		}
		if(showIndex<0)
		{
			showIndex +=szPrefabs.Length; 
		}

		showNextPrefabs();
	}

	void showNextPrefabs()
	{

		labTitle.text = WGStrings.getText(8000+showIndex);

		GameObject go = Instantiate(szPrefabs[showIndex]) as GameObject;
		UISprite sp = go.GetComponent<UISprite>();
		Vector3 pos = go.transform.localPosition;
		SDK.AddChild(go,tsContent.gameObject);
		go.transform.localPosition = pos;
		topWidget.SetAnchor(go);
		butWidget.SetAnchor(go);
		if(goCurHelp != null)
		{
			Destroy(goCurHelp);
		}
		goCurHelp = go;
	}

	public override void showView ()
	{
		base.showView ();
		showNextPrefabs();
		tsContent.ESetActive(true);
		tsContent.transform.localScale = Vector3.zero;
		tsContent.PlayForward();

		mnIvokeBlock.InvokeBlock(0.32f,()=>{
			this.showViewEnd();
		});
	}
	public override void showViewEnd ()
	{
		base.showViewEnd ();
	}
	public override void hiddenView ()
	{
		base.hiddenView ();
		tsContent.PlayReverse();
		mnIvokeBlock.InvokeBlock(0.3f,()=>{
			this.hiddenViewEnd();
		});
	}


	public static WGHelpView CreateHelpView()
	{
		Object obj = Resources.Load("pbWGHelpView");
		if(obj !=null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			WGHelpView ghv = go.GetComponent<WGHelpView>();
			return ghv;
		}
		return null;
	}
}
