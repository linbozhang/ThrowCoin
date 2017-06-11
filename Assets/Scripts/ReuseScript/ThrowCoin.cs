using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ThrowCoin : MonoBehaviour {
	
	private Rigidbody _Rig;
	
	private GameObject _Coin;
	

	public float Up;

	public float Up_Add=1.3f;

	public float Forward;
	
	public float Ro;

	public GameObject goEvent;

	private MDWeapon _weaponData;

	public WGWeapon _Weapon;

	private WGGameWorld _ShareWorld;

	public UILabel labCostNum;
	
	private Vector3 _Ro = new Vector3(270,270,0);

	WGDataController _dataCtrl;

	List<WGWeapon> szWeaponGO = new List<WGWeapon>();

	WGBullet _bullet;

	DataPlayer dp{
		get{
			return DataPlayerController.getInstance().data;
		}
	}

	int mWeaponIndex = 0;
//	bool bNeedCoin = true;

	bool bUseStaticWeapon = false;

//	public bool NeedCoin{
//		get{
//			return bNeedCoin;
//		}
//	}

	float upTemp = 0;
	void Awake()
	{
		upTemp = Up;
		#if Add_AD
		Up = Up_Add;
		#endif
	}
//	public void DonotNeedCoin(float time)
//	{
//		CancelInvoke("retNeedCoin");
//		bNeedCoin = false;
//		Invoke("retNeedCoin",time);
//	}
//	void retNeedCoin()
//	{
//		bNeedCoin = true;
//	}
	void Start()
	{
		if(dp.DelAD == 0)
		{
			Up = Up_Add;
		}
		else
		{
			Up = upTemp;
		}
		_ShareWorld = WGGameWorld.Instance;

		_dataCtrl = WGDataController.Instance;


//		for(int i=0;i<_dataCtrl.szWeapons.Count;i++)
//		{
//			WGWeapon wep = WGWeapon.CreateWeapon(i);
//			wep.gameObject.SetActive(false);
//			wep.transform.parent = this.transform;
//			wep.transform.localPosition = Vector3.zero;
//			wep.transform.localRotation = Quaternion.Euler(Vector3.zero);
//			wep.transform.localScale = Vector3.one;
//			szWeaponGO.Add(wep);
//		}
		mWeaponIndex =0;
//		_Weapon = szWeaponGO[0];

		_Weapon = WGWeapon.CreateWeapon();
		SDK.AddChild(_Weapon.gameObject,this.gameObject);
		_Weapon.transform.localPosition = new Vector3(0,0,-0.99f);
//		_Weapon.labCostCoin = labCostNum;
		_Weapon.ESetActive(true);
		_weaponData = _dataCtrl.szWeapons[0];

		UIEventListener.Get(goEvent).onPress += this.myOnPress;
		CheckStaticWeapon();
	}
	public void RemoveAD()
	{
		Up = upTemp;
	}
	public void ResetAD()
	{
		#if Add_AD
		Up = Up_Add;
		#endif
	}
	public void ChangeWeaponAdd()
	{
		int max = Consts.WEAPON_MAX;
//		if(dp.mR==1)//如果购买了角色系统
//		{
//			if(dp.r ==2)//如果选择的是最后的角色,则只能选择最大是100的金币,也就是Consts.WEAPON_MAX -1 这个.
//			{
////				max = Consts.WEAPON_MAX-1;
//			}
//		}
//		else
		if(dp.mR == 0)
		{
			max = Consts.WEAPON_MAX -1;
		}

		if(mWeaponIndex+1>max && max<Consts.WEAPON_MAX)
		{	if(YeHuoSDK.bShow3RoalGift){
				WGGameWorld.Instance.ShowBuyRoleAlert();
			}

		}

		if(mWeaponIndex<max)
		{
			if(changeWeapon(mWeaponIndex+1))
			{
				mWeaponIndex++;
				PlayWeaponSound(mWeaponIndex);
			}
		}


	}
	public void ChangeWeaponSub()
	{
		if(mWeaponIndex>Consts.WEAPON_MIN)
		{

			if( changeWeapon(mWeaponIndex -1))
			{
				mWeaponIndex--;
				PlayWeaponSound(mWeaponIndex);
			}
		}
	}
	void PlayWeaponSound(int index)
	{
		switch(index)
		{
		case 0:
			BCSoundPlayer.Play(MusicEnum.weapon1,1f);
			break;
		case 1:
			BCSoundPlayer.Play(MusicEnum.weapon2,1f);
			break;
		case 2:
			BCSoundPlayer.Play(MusicEnum.weapon3,1f);
			break;
		case 3:
			BCSoundPlayer.Play(MusicEnum.weapon4,1f);
			break;
		}
	}
	public void showTsunamiEffect(bool show)
	{
		for(int i=0;i<szWeaponGO.Count;i++)
		{
//			szWeaponGO[i].goTsunami.ESetActive(show);
		}
	}

	public bool changeWeapon(int index)
	{

		if(index>=0 && index<_dataCtrl.szWeapons.Count)
		{
//			_Weapon.gameObject.SetActive(false);
//			_Weapon = szWeaponGO[mWeaponIndex];
//			_Weapon.gameObject.SetActive(true);
			_weaponData = _dataCtrl.szWeapons[index];
//			_Weapon.trBullet.enabled = !bUseStaticWeapon;
			return _Weapon.ChangeWeapon(index,_weaponData.cost);

		}
		return false;

	}

	int GetCanThrowIndex()
	{
		for(int i=_dataCtrl.szWeapons.Count-1;i>-1;i--)
		{
			if(_dataCtrl.szWeapons[i].cost<dp.Coin)
			{
				return i;
			}
		}
		return 0;
	}

	void Throw()
	{
		



		if(!_ShareWorld.cs_BearManage.bHaiXiaoEffect)
		{
			_Weapon.FireEffect();
			int subNum = -10;
			if(_weaponData != null)
			{
				subNum = -_weaponData.cost;
			}
//			if(_weaponData.cost>dp.Coin)
//			{
//				if(mWeaponIndex>Consts.WEAPON_MIN)
//				{
//					mWeaponIndex = GetCanThrowIndex();
//					changeWeapon();
//					subNum = -_weaponData.cost;
//					return;
//				}
//
//			}
			if(!_ShareWorld.PlayerGetCoin(subNum))
			{
				if(!_ShareWorld.bNoCoinTip)
				{
//					_ShareWorld.bNoCoinTip = true;
					_ShareWorld.NoCoinTipCan();

					_ShareWorld.ShowCoinSupplementView();
				}
				return;
			}
		}
		BCSoundPlayer.Play(MusicEnum.Shot);
		if(bUseStaticWeapon)
		{

			Vector3 cur = Input.mousePosition - new Vector3(Screen.width/2,0,0);

			float mm = Vector3.Angle(Vector3.up,cur);

			if(mm>15)
			{
				mm = 15;
			}

			if(cur.x<=0)
			{
				mm = -mm;
			}

			Ro = mm;



			_Weapon.trBullet.enabled = false;
			Vector3 temp = _Weapon.tRote.localEulerAngles;
			temp.y = Ro;
			_Weapon.tRote.localEulerAngles = temp;


			if(Ro>300)
				Ro =Ro- 360;
			Ro /=15;
		}
		else{
			Ro = _Weapon.tRote.localEulerAngles.y;
//			Debug.Log(Ro);
			if(Ro>300)
				Ro =Ro- 360;
			Ro /=15;
		}
		_Coin = WGGameWorld.Instance.cs_ObjManager.BCGameObjFactory(_weaponData.oid,_Weapon.tBullet.position,Vector3.zero,Vector3.one);
//		_Coin =  Instantiate(_Weapon.goBullet,_Weapon.tBullet.position,Quaternion.Euler(Vector3.zero)) as GameObject;
//		_Coin.transform.parent = WGBearManage.Instance.ThrowCoinRoot.transform;



		_Rig = _Coin.GetComponent<Rigidbody>();

		Vector3 v3force = new Vector3(Ro,Up,Forward);

		_Rig.AddForce(v3force,ForceMode.Impulse);

		_bullet = _Coin.AddComponent<WGBullet>();

		_bullet.mAct = _weaponData.hurt;
		if(dp.mR==1)
		{
//			if(dp.r == 0)
//			{
//				_bullet.mAct = (int)(_weaponData.hurt *1.1f);
//			}
//			else if(dp.r == 1)
			{
				_bullet.mAct = (int)(_weaponData.hurt*1.3f);
			}
		}

		BCObj bco = WGDataController.Instance.GetBCObj(_weaponData.oid);

		BCGameObj bcgo = _Coin.GetComponent<BCGameObj>();

		bcgo.freshWithData(bco);



	}
	public void CheckStaticWeapon()
	{
		DataPlayer _dp = DataPlayerController.getInstance().data;


		if(_dp.guDingForever>0 && _dp.releaseGuding>0)
		{
			bUseStaticWeapon = true;
		}
		else if(_dp.guDingTime>0)
		{
			bUseStaticWeapon = true;
		}
		else
		{
			bUseStaticWeapon = false;
		}

		_Weapon.trBullet.enabled = !bUseStaticWeapon;
	}


	
	void myOnPress(GameObject go,bool b)
	{
		if(b)
		{
//			if(WGHelpManager.Self.enabled)
//			{
//				InvokeRepeating("Throw",0.05f,1);
//			}
//			else
			{
				InvokeRepeating("Throw",0.05f,_weaponData.reload);
			}
		}
		else
		{
			CancelInvoke("Throw");
		}
	}

}
