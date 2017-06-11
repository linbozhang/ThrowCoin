using UnityEngine;
using System.Collections;

public class WGTigerSkillEffect : MonoBehaviour {

	public GameObject goContent;
	public UISprite spSkillIcon;
	public UILabel labSkillDes;
	public TweenAlpha[] szTaLeft;
	public TweenAlpha[] szTaRight;


	static WGTigerSkillEffect _Self = null;
	public static WGTigerSkillEffect Self {
		get{
			if(_Self == null)
			{
				_Self = CreateTigerSkillEffect();
				SDK.AddChild(_Self.gameObject,WGRootManager.Self.goRootGameUI);
			}
			return _Self;
		}
	}

	int mRightCount = 3;
	int mMax = 3;
	int mLeftCount = 3;


	public void ShowSkillEffectWithSKid(int skid)
	{
		goContent.ESetActive(true);
		MDSkill sk = WGDataController.Instance.getSkill(skid);

		spSkillIcon.spriteName = sk.icon;
		labSkillDes.text =WGStrings.getFormate(1600,sk.name);

		CancelInvoke("LeftEffect");
		CancelInvoke("HiddenSkillEffect");
		mLeftCount = 3;
		mRightCount = 3;
		InvokeRepeating("LeftEffect",0.1f,0.3f);
		Invoke("HiddenSkillEffect",2.5f);
	}
	void HiddenSkillEffect()
	{
		goContent.ESetActive(false);
		CancelInvoke("LeftEffect");
		_Self = null;
		Destroy(gameObject);
	}

	void LeftEffect()
	{
		if(mLeftCount<mMax)
		{
			szTaLeft[mLeftCount].enabled = true;
			szTaLeft[mLeftCount].PlayForward();
			mLeftCount++;
		}
		else if(mLeftCount== mMax)
		{
			for(int i=0;i<szTaLeft.Length;i++)
			{
				szTaLeft[i].ResetToBeginning();
				szTaLeft[i].enabled = false;
			}
			mLeftCount = 0;
		}
		RightEffect();
	}

	void RightEffect()
	{
		if(mRightCount<mMax)
		{
			szTaRight[mRightCount].enabled = true;
			szTaRight[mRightCount].PlayForward();
			mRightCount++;
		}
		else if(mRightCount== mMax)
		{
			for(int i=0;i<szTaRight.Length;i++)
			{
				szTaRight[i].ResetToBeginning();
				szTaRight[i].enabled = false;
			}
			mRightCount = 0;
		}

	}

	static Object mObj=null;
	static WGTigerSkillEffect CreateTigerSkillEffect()
	{
		if(mObj == null)
		{
			mObj = Resources.Load("pbWGTigerSkillReleaseEffect");
		}

		if(mObj != null)
		{
			GameObject go = Instantiate(mObj) as GameObject;
			WGTigerSkillEffect tse = go.GetComponent<WGTigerSkillEffect>();
			return tse;
		}
		return null;

	}
	



}
