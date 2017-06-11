using UnityEngine;
using System.Collections;

public class UISwitch : MonoBehaviour {

	public System.Action OnChangeValue;

	[SerializeField]
	UISprite spBackground;
	[SerializeField]
	UISprite spButton;
	[SerializeField]
	UILabel labTrue;
	[SerializeField]
	UILabel labFalse;
	[SerializeField]
	UIButton myButton;
	[SerializeField]
	TweenPosition tpButton;
	[SerializeField]
	bool _bSwitch = false;
	[SerializeField]
	string spriteName_false;
	[SerializeField]
	string spriteName_true;
	[SerializeField]
	Vector3 v3True;
	[SerializeField]
	Vector3 v3False;

	float fTotalTime = 0;
	bool bChanged = false;

	public bool value{
		get{
			return _bSwitch;
		}
		set
		{
			_bSwitch = value;
//			Vector3 v3 = tpButton.transform.localPosition;
//			float x = Mathf.Abs(v3.x);
//			v3.x = (_bSwitch?-1:1)*x;

			spButton.spriteName = _bSwitch?spriteName_true:spriteName_false;

			tpButton.transform.localPosition = _bSwitch?v3True:v3False;

		}
	}

	// Use this for initialization
	void Start () {
		labTrue.text = WGStrings.getText(1092);
		labFalse.text = WGStrings.getText(1093);
	}

	void Update()
	{
		if(bChanged)
		{
			fTotalTime -=RealTime.deltaTime;
			if(fTotalTime<=0)
			{
				bChanged = false;
				freshUI();
			}
		}
	}

	void OnBtnSwitch()
	{
		_bSwitch = !_bSwitch;

		tpButton.from = _bSwitch?v3False:v3True;
		tpButton.to = _bSwitch?v3True:v3False;
		tpButton.enabled = true;
		tpButton.ResetToBeginning();
		tpButton.PlayForward();

		fTotalTime = tpButton.duration;
		bChanged = true;
		CancelInvoke("fresh");
		Invoke("freshUI",tpButton.duration);
		if(OnChangeValue != null)
		{
			OnChangeValue();
		}
	}


	void freshUI()
	{
		spButton.spriteName = _bSwitch?spriteName_true:spriteName_false;

		//WG.SLog("====="+_bSwitch);
	}
}
