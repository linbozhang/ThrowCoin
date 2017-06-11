using UnityEngine;
using System.Collections;

public class WGBear : MonoBehaviour {

	public int Blood;//总血量

	public int CurrentBlood;//当前血量

	public PlayAnim_FX BearAni;//

	public WGCharacterController _wgCC;

	UISlider _BearHPBar = null;
	UILabel _labHP = null;
	GameObject _goHp = null;
	public int ID;

	private bool _Dead;
	bool _bFirstHurt = true;

	void Start()
	{
		CurrentBlood = Blood;
		_bFirstHurt = true;

		_Dead = false;
		if(_labHP !=null)
		{
			_labHP.text = CurrentBlood.ToString()+"/"+Blood.ToString();
		}
	}

	public void AddHP(GameObject hp)
	{
		_BearHPBar = hp.GetComponentInChildren<UISlider>();
		_labHP = hp.GetComponentInChildren<UILabel>();
		_goHp = hp;
	}



	void OnCollisionEnter(Collision other)
	{
		if(_Dead){
			if(other.gameObject.tag.Equals("Coin"))
			{
				WGGameWorld.Instance.HideObj(other.gameObject);
			}
			return;
		}
		if(other.gameObject.tag.Equals("Coin"))
		{
			WGBullet bullet = other.gameObject.GetComponent<WGBullet>();
			if(bullet != null)
			{
				CurrentBlood -=bullet.mAct;


				_labHP.text = CurrentBlood.ToString()+"/"+Blood.ToString();

				


				if(CurrentBlood <= 0)
				{
					if(_bFirstHurt)
					{
						WhenBearKilled(true);
						WGGameWorld.Instance.CreateMiaosha(other.transform.position);
					}
					else{
						WhenBearKilled();
			WGGameWorld.Instance.CreatePapapa(other.transform.position);	
					}
				}
				else
				{
					WhenBearHurted();
		  WGGameWorld.Instance.CreatePapapa(other.transform.position);
				}
				_bFirstHurt = false;
				other.rigidbody.velocity = Vector3.back*5;
				Destroy(bullet);
			}
		}
	}

	void WhenBearHurted()
	{
		_BearHPBar.value = CurrentBlood*1f/Blood;


		WGBearManage.Instance.WhenBearHurted(ID);
		if(BearAni != null)
		{
			BearAni.Injure();
		}
		if(_wgCC != null)
		{
			_wgCC.PlayHurtAnim();
		}
	}

	public void Disappear()
	{
		_Dead = true;
		if(BearAni != null)
		{
			BearAni.Die();
		}
		if(_wgCC != null)
		{
			_wgCC.PlayDeathAnim();
		}
	}

	void WhenBearKilled(bool bMiao=false)
	{
		Destroy(_goHp);
		Collider[] allCol= this.GetComponents<Collider>();
		for(int i=0;i<allCol.Length;i++)
		{
			Destroy(allCol[i]);
		}
		Destroy(GetComponent<Rigidbody>());
		_Dead = true;
		if(BearAni != null)
		{
			BearAni.Die();
		}
		if(_wgCC != null)
		{
			_wgCC.PlayDeathAnim();
		}
		WGBearManage.Instance.WhenBearKilled(ID,transform,bMiao);
	}

	public void IsGray(bool gray)
	{
		if(BearAni != null)
		{
			BearAni.GrayEffect(gray);
		}
		if(_wgCC != null)
		{
			_wgCC.GrayEffect(gray);
		}
	}
}
