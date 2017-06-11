using UnityEngine;
using System.Collections;

public class D04CustomerServiceView : MDBaseAlertView {

	public TweenScale tsContent;
	// Use this for initialization
	void Start () {
	
	}
	
	public override void showView ()
	{
		base.showView ();
		tsContent.gameObject.SetActive(true);
		tsContent.transform.localScale = Vector3.one*0.4f;
		tsContent.PlayForward();
		
		mnIvokeBlock.InvokeBlock(tsContent.duration,()=>{
			showViewEnd();
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
		mnIvokeBlock.InvokeBlock(tsContent.duration,()=>{
			hiddenViewEnd();
		});
	}
	public override void hiddenViewEnd ()
	{
		
		base.hiddenViewEnd ();
	}
	
	public override void OnBtnOk ()
	{
		base.OnBtnOk ();
		BCSoundPlayer.Play(MusicEnum.button);
	}

	public static D04CustomerServiceView CreateCSView()
	{
		Object obj = Resources.Load("pbD04CustomerServiceView");
		
		GameObject go = Instantiate(obj) as GameObject;

		SDK.AddChild(go,WGRootManager.Self.goRootTopUI);

		D04CustomerServiceView cs = go.GetComponent<D04CustomerServiceView>();
		
		return cs;
		
	}
}
