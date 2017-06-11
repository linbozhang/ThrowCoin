using UnityEngine;
using System.Collections;
[RequireComponent(typeof(UIButton))]
public class UIButtonEffect : MonoBehaviour {

	bool bEffect = false;

	public UISprite _sprite;
	public TweenAlpha _tweenAlpha;
	public TweenScale _tweenScale;



	// Use this for initialization
	void Start () {
		_tweenAlpha.enabled = false;
		_tweenScale.enabled = false;
		_sprite.gameObject.SetActive(false);
	}


	void OnClick()
	{
		if(bEffect)return;
		bEffect = true;

		_sprite.gameObject.SetActive(true);
		_sprite.alpha = 1;
		_sprite.transform.localScale = Vector3.one;

		_tweenAlpha.enabled = true;
		_tweenAlpha.ResetToBeginning();
		_tweenAlpha.PlayForward();

		_tweenScale.enabled = true;
		_tweenScale.ResetToBeginning();
		_tweenScale.PlayForward();

		Invoke("ClickEnd",Mathf.Max(_tweenAlpha.duration,_tweenScale.duration));

	}
	void ClickEnd()
	{
		bEffect = false;
		_tweenAlpha.enabled =false;
		_tweenScale.enabled =false;
		_sprite.gameObject.SetActive(false);

	}
}
