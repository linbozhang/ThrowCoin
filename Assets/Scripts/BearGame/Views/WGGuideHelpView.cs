using UnityEngine;
using System.Collections;

public class WGGuideHelpView : MDBaseAlertView {

	public TweenScale tsContent;
	public UILabel labTitle;
	public UIWidget topWidget;
	public UIWidget butWidget;

	public UILabel labBtnTitle;
	public UISprite spBtnBackground;

	public GameObject[] szPrefabs;
	
	bool bCanOk = false;

	public override void OnBtnOk ()
	{
		if(bCanOk)
		{
			base.OnBtnOk ();
		}
	}

	public void hideBtnOK(  ){
		UIWidget btnClose=butWidget.transform.Find("Button_Close").GetComponent<UIWidget>();
		btnClose.ESetActive(false);
		//btnClose.width=74;
		//btnClose.height=74;
		//btnClose.transform.localPosition=pos;
//		for(int i=0;i<btnClose.transform.childCount;i++){
//			btnClose.transform.GetChild(i).ESetActive(false);
//		}
//		foreach(Transform child in btnClose.transform.childCount){
//			child.ESetActive(false);
//		}

	}
	public void resetBtnOKPos(){
		UIWidget btnClose=butWidget.GetComponentInChildren<UIWidget>();
		btnClose.width=74;
		btnClose.height=74;
		foreach(Transform child in btnClose.transform){
			child.ESetActive(true);
		}
	}

	public void showNextPrefabs(int index)
	{
		bCanOk = false;
		labTitle.text = WGStrings.getText(8000+index);

		GameObject go = Instantiate(szPrefabs[index]) as GameObject;
		UISprite sp = go.GetComponent<UISprite>();
		Vector3 pos = go.transform.localPosition;
		SDK.AddChild(go,tsContent.gameObject);
		go.transform.localPosition = pos;
		topWidget.SetAnchor(go);
		butWidget.SetAnchor(go);

		labBtnTitle.color = Color.gray;
		spBtnBackground.color = Color.gray;
	}

	public override void showView ()
	{
		base.showView ();

		tsContent.ESetActive(true);
		tsContent.transform.localScale = Vector3.zero;
		tsContent.PlayForward();

		mnIvokeBlock.InvokeBlock(1.2f,()=>{
			this.showViewEnd();
		});
	}
	public override void showViewEnd ()
	{
		labBtnTitle.color = Color.white;
		spBtnBackground.color = Color.white;
		bCanOk = true;
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


	public static WGGuideHelpView CreateGuideView()
	{
		Object obj = Resources.Load("pbWGGuideHelpView");
		if(obj !=null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			WGGuideHelpView ghv = go.GetComponent<WGGuideHelpView>();
			return ghv;
		}
		return null;
	}
}
