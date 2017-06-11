using UnityEngine;
using System.Collections;

public class WGTsunamiView : MDBaseAlertView {

	public GameObject goContent;
	public UILabel labTipTitle;
	public TweenScale tsStep1;
	public TweenAlpha taStep1;
	public TweenPosition tpStep1;
	public TweenScale tsStep2;
	public GameObject goStep4;
	public TweenScale tsStep4;

	public UITexture texBianKuang;
	public GameObject goBaiKuang;

	public GameObject goKuangbaoBackEffect;

	float _topTime = 0;

	void InitView()
	{
		texBianKuang.SetAnchor(WGRootManager.Self.goRootMainUI.transform);
	}

	public void showTsunamiView(float time)
	{
		base.showView ();
//		Time.timeScale = 0.5f;
		_topTime = time+2;
		tpStep1.enabled = false;
		goContent.ESetActive(true);
		tsStep1.ESetActive(true);
		tsStep1.ResetToBeginning();
		taStep1.ResetToBeginning();
		tsStep1.PlayForward();
		taStep1.PlayForward();
		tsStep2.ESetActive(false);
		texBianKuang.ESetActive(false);
		goStep4.ESetActive(false);
		InvokeBlock(0.32f,()=>{
			tsStep2.style =UITweener.Style.PingPong;
			tsStep2.from = Vector3.one*1.25f;
			tsStep2.to = Vector3.one;
			tsStep2.ESetActive(true);
			tsStep2.duration = 0.4f;
			tsStep2.ResetToBeginning();
			tsStep2.PlayForward();
			goKuangbaoBackEffect.ESetActive(true);

			InvokeBlock(2.4f,()=>{
//				Time.timeScale = 1;
				Destroy(taStep1);

				tsStep1.to = Vector3.one*0.3438296f;
				tpStep1.enabled = false;
				tsStep1.PlayForward();
				tpStep1.PlayForward();

				goKuangbaoBackEffect.ESetActive(false);

				tsStep2.from = tsStep2.transform.localScale;
				tsStep2.to = Vector3.one*2;

				tsStep2.duration = 0.3f;
				tsStep2.style = UITweener.Style.Once;


					for(int i=0;i<40;i++)
					{
						InvokeBlock(0.32f+0.5f*(i+1),()=>{
						GameObject go = Instantiate(goBaiKuang)as GameObject;
						go.transform.parent = goContent.transform;
						go.transform.localPosition = goBaiKuang.transform.localPosition;
						go.ESetActive(true);
						});
					}

				

				texBianKuang.ESetActive(true);
				goStep4.ESetActive(true);
				tsStep4.PlayForward();
				labTipTitle.text = WGStrings.getText(1027);
			});
		});

		InvokeBlock(0.85f*_topTime,()=>{
			labTipTitle.text = WGStrings.getText(1028);
		});
		InvokeBlock(_topTime,()=>{
			hiddenView();
		});

	}




	public override void hiddenView ()
	{
		base.hiddenView ();
		goContent.ESetActive(false);
		hiddenViewEnd();
	}

	public static WGTsunamiView CreateTsunamiView()
	{
		Object obj = Resources.Load("pbWGTsunami");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			WGTsunamiView t = go.GetComponent<WGTsunamiView>();
			t.InitView();
			return t;
		}
		return null;
	}


}
