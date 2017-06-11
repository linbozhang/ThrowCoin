using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class WGBearManage : WGMonoBehaviour {

	MDSceneParamter _sp = new MDSceneParamter();//这个数据会重新读取配置表

	public int _Num=15;// 当前小熊的数量

	public int _MaxNum = 10;
	int _curBearNum = 0;

	private Transform _TurnPlateTrans;

	public GameObject BearRoot;

	public GameObject HP;

	public TweenPosition TreeTween;

	public GameObject BearCoinRoot;
	public Transform wuqiTran;
	public ThrowCoin csThrow;

	public GameObject goTableDesk;
	public GameObject goIceDesk;
	public GameObject TongueRoot;
	public GameObject WaveTongueBox;
	public GameObject goCommonTongueBox;

	public UISlider pgHaiXiao;

	public WGRoteObject mTurnPlate;
	public RoSelf csRoteTp;
//  public Vector3 FirstPos = new Vector3(0,10.5f,-16f);
    public Transform tTurnplateCenter;

	public GameObject go3DUIFrontPanel;

	public SQYGizmosBox coinDropBox;
	public SQYGizmosBox collectionDropBox;

	public Transform tDeskPanel;



	float pgHaiXiaoValue = 1;
	[HideInInspector]
	public bool bHaiXiaoEffect = false;

    float fRadiou = 19f;

	[HideInInspector]
	public List<int> szLiveBearID = new List<int>();

	int _MaxRandomWeight;


	bool _IsLongTongue = false;

	float Speed = 0;
	int nDirection = 0;
	float _TongueSide;

	bool _IsWave;

	bool _bDonotNeedCoin = false;


	bool _bTongueLong100 = false;


	int _NextUnLockLV;
	Vector3 _BearScale = new Vector3(0.06f,0.06f,0.06f);

	float fCreateBearTime = 3f;

	int _EnergyFull = 100;

	bool _bShowPowerTip = false;

	Queue<Transform> _DeadBearList;

	WGBear _bearBoss = null;

	WGGameWorld _ShareWorld;

	BCObjManager _ShareObjManager;

	WGDataController dataCtrl;
	DataPlayer _dataPlayer{
		get{
			return DataPlayerController.getInstance().data;
		}
	}
	List<WGBearPanel> szBearPanel = new List<WGBearPanel>();

	List<int> szBigBear ;

	public Vector3 ValidPostion{
		get{
			float x = 0;
			float y = 5;
			float z = -5;

			x = Random.Range(-coinDropBox.size.x/2,coinDropBox.size.x/2);
			y = Random.Range(-coinDropBox.size.y/2,coinDropBox.size.y/2);
			z = Random.Range(-coinDropBox.size.z/2,coinDropBox.size.z/2);

			return coinDropBox.transform.position+coinDropBox.center+(new Vector3(x,y,z));
		}
	}
	public Vector3 CollectionValidPos{
		get{
			float x = 0;
			float y = 5;
			float z = -5;

			x = Random.Range(-collectionDropBox.size.x/2,collectionDropBox.size.x/2);
			y = Random.Range(-collectionDropBox.size.y/2,collectionDropBox.size.y/2);
			z = Random.Range(-collectionDropBox.size.z/2,collectionDropBox.size.z/2);

			return collectionDropBox.transform.position+collectionDropBox.center+(new Vector3(x,y,z));
		}
	}

	public static WGBearManage Instance;

	void Awake()
	{
		Instance = this;
	}
	public void InitWithGameWorld(WGGameWorld World)
	{
		_sp = Core.fc.sp;
		szBigBear = new List<int>(){
			WGDefine.BossID,WGDefine.PayBear1,WGDefine.PayBear2
		};

		_EnergyFull = Core.fc.EnergyFull;

		Speed = _sp.SPEED_commen;
		if(_dataPlayer.DelAD == 0)
		{
			#if Add_AD
			wuqiTran.localPosition = Core.fc.weaponsPos1;
			#else
			wuqiTran.localPosition = Core.fc.weaponsPos;
			#endif
		}
		else
		{
			wuqiTran.localPosition = Core.fc.weaponsPos;
		}
		TongueRoot.ESetActive(true);

		mTurnPlate.v3Rotate = Core.fc.turnPlateComm;
		csRoteTp.V = Core.fc.turnPlateComm.z/40.0f;//result is 0.3
		tDeskPanel.localPosition = Core.fc.deskPos;

		coinDropBox.transform.localPosition = Core.fc.boxCoin.pos;
		coinDropBox.size = Core.fc.boxCoin.size;
		coinDropBox.center = Core.fc.boxCoin.center;

		collectionDropBox.transform.localPosition = Core.fc.boxColletion.pos;
		collectionDropBox.size = Core.fc.boxColletion.size;
		collectionDropBox.center = Core.fc.boxColletion.center;

		pgHaiXiao.value = _dataPlayer.mEnergy*1.0f/_EnergyFull;
		_ShareWorld = World;

		dataCtrl = WGDataController.Instance;
		_ShareObjManager = World.cs_ObjManager;

		_TongueSide = _sp.Backward;
		_TurnPlateTrans = BearRoot.transform;


		_DeadBearList = new Queue <Transform>();

		TreeTween.gameObject.SetActive(false);

		goIceDesk.SetActive(false);
		ResetAllBear();

	}
	public void RemoveAD()
	{

		wuqiTran.localPosition = Core.fc.weaponsPos;

	}
	public void ResetAD()
	{
		#if Add_AD
		wuqiTran.localPosition = Core.fc.weaponsPos1;
		#else
		wuqiTran.localPosition = Core.fc.weaponsPos;
		#endif
	}

	void ResetAllBear()
	{

		dataCtrl.freshUnLockBearWith(_dataPlayer.Level);

        float angleDelta = 360/_Num;

		GameObject tem ;
		WGBearPanel preBear = null;
        float rr=0;
		for(int i = 0; i<_Num;i++)
		{
			tem = new GameObject();
			tem.name = "bear"+i;
			WGBearPanel bp = tem.AddComponent<WGBearPanel>();

			szBearPanel.Add(bp);
			if(preBear != null)
			{
				preBear.right = bp;
				bp.left = preBear;
			}
			preBear = bp;
			if(i==_Num-1)
			{
				bp.right = szBearPanel[0];
				szBearPanel[0].left = bp;
			}
			bp.index = i;

            tem.transform.rotation = Quaternion.Euler(0,angleDelta*i+180,0);
			tem.transform.parent = BearRoot.transform;

            rr = Mathf.PI/2+ i*angleDelta*Mathf.PI/180f;

            tem.transform.localPosition = new Vector3(-fRadiou*Mathf.Cos(rr),-fRadiou*Mathf.Sin(rr),0)+tTurnplateCenter.localPosition;


			//int id  = 5001+i;
			int id = GetNextBearID(bp);

			bp.id = id;

			CreateBearWith(id,tem.transform);
			
		}

	}

	bool bInMiddel = false;
	void Update () {
		if(_IsLongTongue)
		{
			if(_TongueSide<_sp.LongTongue)
			{
				_TongueSide = _sp.LongTongue;
				_IsLongTongue = false;
			}

		}
		if(_IsWave)
		{
			_TongueSide = _sp.WaveSide;
			_IsWave = false;
		}

		if(TongueRoot.transform.localPosition.z>_TongueSide)
		{
			if(nDirection!=1)
			{
				nDirection = 1;
			}
			bInMiddel= true;

			if(_TongueSide!=_sp.Backward)
			{
				if(_TongueSide == _sp.WaveSide)
				{
					_bTongueLong100 = true;
				}
				WaveTongueBox.SetActive(false);
				goCommonTongueBox.ESetActive(true);
				_TongueSide = _sp.Backward;
			}

		}
		else if(TongueRoot.transform.localPosition.z<_sp.Forward)
		{
			if(nDirection!=0)
			{
				bInMiddel= true;
				nDirection = 0;
				if(_bTongueLong100)
				{
					_bTongueLong100 = false;

					Invoke("ResetDeskCoin",0.1f);
					SpeedDown();
				}
			}
		}


		if(nDirection==1)
		{
			TongueRoot.transform.Translate(Vector3.forward * (-Speed) * Time.deltaTime);
		}
		else
		{
			if(bInMiddel&&TongueRoot.transform.localPosition.z>-27.11721)
			{
				bInMiddel = false;
				if(_TongueSide == _sp.WaveSide)
				{
					WaveTongueBox.SetActive(true);
					goCommonTongueBox.ESetActive(false);
				}
			}
			TongueRoot.transform.Translate(Vector3.forward * (Speed) * Time.deltaTime);
		}
	}
	void ResetDeskCoin()
	{
//		WGGameWorld.Instance.ChangeAllBearCoin();
		WGGameWorld.Instance.AddCoin(WGDefine.CommonCoin,35);
	}

	public void WhenBearHurted(int id)
	{
		//Debug.Log("whenbear hurted");
		BCSoundPlayer.Play(MusicEnum.hitBear);
		if(WGHelpManager.Self!=null)
		{
			if(WGHelpManager.Self.enabled)
			{
				if(!WGHelpManager.Self.StatesIsEnd(EMHelpStates.Kill_Drop))
				{
					WGHelpManager.Self.ShowHelpView(EMHelpStates.Kill_Drop);
				}
				else if(!WGHelpManager.Self.StatesIsEnd(EMHelpStates.Kill_Fast))
				{
					WGHelpManager.Self.ShowHelpView(EMHelpStates.Kill_Fast);
				}else if(!WGHelpManager.Self.StatesIsEnd(EMHelpStates.LongTouch))
				{
					WGHelpManager.Self.ShowHelpView(EMHelpStates.LongTouch);
				}
				else if(!WGHelpManager.Self.StatesIsEnd(EMHelpStates.Use_Weapon))
				{
					WGHelpManager.Self.ShowHelpView(EMHelpStates.Use_Weapon);
				}
				else if(!WGHelpManager.Self.StatesIsEnd(EMHelpStates.Free10_Skill))
				{
					WGHelpManager.Self.ShowHelpView(EMHelpStates.Free10_Skill);
				}

			}
		}
		WGAchievementManager.Self.processAchievement(id,DTAchievementType.HIT_ALL);
		WGAchievementManager.Self.processAchievement(id,DTAchievementType.HIT_ONE);
		int addNum = Random.Range(2,5);
		int coinID = Random.Range(0,4);
		if(id == WGDefine.BossID || id == WGDefine.PayBear1 || id == WGDefine.PayBear2)
		{
			_ShareWorld.AddCoin(WGDefine.CommonCoin+coinID,addNum);
		}
	}

	public void WhenBearKilled(int id,Transform tran,bool bMiao = false)
	{
		BCSoundPlayer.Play(MusicEnum.hitBear);



		WGAchievementManager.Self.processAchievement(id,DTAchievementType.KILL_ALL);
		WGAchievementManager.Self.processAchievement(id,DTAchievementType.KILL_ONE);
		WGBearParam mBear = dataCtrl.GetBearParam(id);
		BCObj mOjb = dataCtrl.GetBCObj(id);
		_ShareWorld.PlayGetExp(mOjb.Exp);


		if(WGHelpManager.Self != null)
		{
			if(WGHelpManager.Self.enabled)
			{
				if(!WGHelpManager.Self.StatesIsEnd(EMHelpStates.Kill_Energy))
				{
					WGHelpManager.Self.ShowHelpView(EMHelpStates.Kill_Energy);
				}
				else if(!WGHelpManager.Self.StatesIsEnd(EMHelpStates.Kill_Tiger) && mBear.tiger==1)
				{
					WGHelpManager.Self.ShowHelpView(EMHelpStates.Kill_Tiger);
				}
				else if(!WGHelpManager.Self.StatesIsEnd(EMHelpStates.Use_Item))
				{
					WGHelpManager.Self.ShowHelpView(EMHelpStates.Use_Item);
				}
				else if(!WGHelpManager.Self.StatesIsEnd(EMHelpStates.RegetCoin))
				{
					WGHelpManager.Self.ShowHelpView(EMHelpStates.RegetCoin);
				}
			}
		}


		BearDead(tran.parent);
		szLiveBearID.Remove(id);
		_curBearNum--;
		if(id == WGDefine.BossID)
		{
			_bearBoss = null;
		}
		if(!bHaiXiaoEffect)
		{
			_dataPlayer.mEnergy += mBear.energy;
		}
		if(_dataPlayer.mEnergy>=_EnergyFull)
		{
			if(!_bShowPowerTip)
			{
				if(_dataPlayer.a1 == 0 )
				{
					if(YeHuoSDK.bShowPoweGift){
						_bShowPowerTip = true;
						D04PowerTipView tip = D04PowerTipView.CreatePowerPayView();
						Time.timeScale = 0;
						tip.alertViewBehavriour = (ab,view)=>{
							if(ab == MDAlertBehaviour.CLICK_OK)
							{
								Destroy(view.gameObject);
								showBuyPowerView();
							}
						};
					}else{
						_dataPlayer.mEnergy -=_EnergyFull;
						bHaiXiaoEffect = true;
						
						//DonNotNeedCoin(10+2);
//						WGTsunamiView tv = WGTsunamiView.CreateTsunamiView();
//						tv.alertViewBehavriour =(ab,view)=>{
//							switch(ab)
//							{
//							case MDAlertBehaviour.DID_HIDDEN:
//								Destroy(view.gameObject);
//								break;
//							}
//						};
//						SDK.AddChild(tv.gameObject,_ShareWorld.go2DUIBottom);
//						tv.showTsunamiView(10f);
						
						csThrow.showTsunamiEffect(true);
						pgHaiXiao.value = 1;
						pgHaiXiaoValue = 1;
						InvokeRepeating("HaiXiaoEffect",2f,10f/100);
					}

				}
				else if(_dataPlayer.a1 == 1)
				{
					_dataPlayer.mEnergy -=_EnergyFull;
					bHaiXiaoEffect = true;

					DonNotNeedCoin(10+2);
					WGTsunamiView tv = WGTsunamiView.CreateTsunamiView();
					tv.alertViewBehavriour =(ab,view)=>{
						switch(ab)
						{
						case MDAlertBehaviour.DID_HIDDEN:
							Destroy(view.gameObject);
							break;
						}
					};
					SDK.AddChild(tv.gameObject,_ShareWorld.go2DUIBottom);
					tv.showTsunamiView(10f);

					csThrow.showTsunamiEffect(true);
					pgHaiXiao.value = 1;
					pgHaiXiaoValue = 1;
					InvokeRepeating("HaiXiaoEffect",2f,10f/100);
				}
			}

		}

		if(!bHaiXiaoEffect)
		{
			pgHaiXiao.value = _dataPlayer.mEnergy*1.0f/_EnergyFull;
		}



//		//WG.SLog("WhenBearKilled======="+SDK.Serialize(mBear.death_reward));
		for(int i=0,max = mBear.death_reward.Count;i<max;i++)
		{
			_ShareWorld.AddReward(mBear.death_reward[i]);
		}
		if(bMiao && mBear.additional !=null &&mBear.additional.Count>0)
		{
			for(int i=0,max = mBear.additional.Count;i<max;i++)
			{
				_ShareWorld.AddReward(mBear.additional[i]);
			}
		}

		if(mBear.tiger == 1)
		{
			WGGameTiger.Instance.StartTiger();
		}
	}

	void showBuyPowerView()
	{
		WGDataController _dataCtrl = WGDataController.Instance;
		YHMDPayData payData=_dataCtrl.getYHMDPay(YHPayType.POWER);
		float costMenoy=payData.payCost;
		string payKey=payData.payKey.ToString();
//		string paykey = "112";
//		float costMenoy = 30f;
//		if(YeHuoSDK.bUsePayCode2)
//		{
//			paykey = "212";
//			costMenoy = 20f;
//		}

		string okString ="ok";
		
		#if YES_OK
		string content = WGStrings.getFormateInt(1081,1002,8209,costMenoy.ToString());
		okString =  WGStrings.getText(1002);
		#elif YES_BUY
		string content = WGStrings.getFormateInt(1081,1094,8209,costMenoy.ToString());
		okString =  WGStrings.getText(1094);
#elif YES_GET
		string content = WGStrings.getFormateInt(1081,1077,8209,costMenoy.ToString());
		okString =  WGStrings.getText(1077);
		#elif YES_QueRen
		string content = WGStrings.getFormateInt(1081,1106,payData.showText,costMenoy.ToString());
		okString =  WGStrings.getText(1106);
		#else
		string content = WGStrings.getFormateInt(1081,1077,payData.showText,costMenoy.ToString());
		okString =  WGStrings.getText(1077);
		#endif

		D04PowerBuyView bv = D04PowerBuyView.CreatePowerBuyView();
		bv.FreshUI(content,okString);
		bv.alertViewBehavriour =(ab,view)=>{
			switch(ab)
			{
			case MDAlertBehaviour.CLICK_OK:
				YeHuoSDK.YHPay(payKey,costMenoy,0,(success)=>{
					view.hiddenView();
					if(success)
					{
						_dataPlayer.a1 = 1;
						_bShowPowerTip = false;
					}
				});
				break;
			case MDAlertBehaviour.CLICK_CANCEL:
				_dataPlayer.mEnergy -=_EnergyFull;
				view.hiddenView();
				break;
			case MDAlertBehaviour.DID_HIDDEN:
				Destroy(view.gameObject);
				_bShowPowerTip = false;
				Time.timeScale = 1;
				break;
			}
		};
		bv.showView();
	}

	void HaiXiaoEffect()
	{
		if(pgHaiXiaoValue>0)
		{
			pgHaiXiaoValue -=0.01f;

			pgHaiXiao.value = pgHaiXiaoValue;
		}
		else
		{
			bHaiXiaoEffect = false;
			pgHaiXiao.value = _dataPlayer.mEnergy*1.0f/_EnergyFull;
			CancelInvoke("HaiXiaoEffect");
			csThrow.showTsunamiEffect(false);
		}
	}
	void BearDead(Transform tran)
	{
		_DeadBearList.Enqueue(tran);
		Invoke("CreateBear",fCreateBearTime);
	}

	void CreateBear()
	{

		WGDataController.Instance.freshUnLockBearWith(_dataPlayer.Level);

		Transform tran = _DeadBearList.Dequeue();

		CreateBearWith(GetNextBearID(tran.GetComponent<WGBearPanel>()),tran);
	}

	bool IsAdjacent(int num1,int num2,int cycle)
	{
		if(Mathf.Abs(num1-num2)==1)return true;
		if(num1 == 0 && num2 == cycle-1)return true;
		if(num1 == cycle-1 && num2 == 0)return true;
		return false;
	}

	int GetNextBearID(WGBearPanel bp)
	{
//		//WG.SLog("++"+_ShareWorld.bBossResurrection +"===level="+dataCtrl.GetBearParam(WGDefine.BossID).unlock);

//		//WG.SLog("GetNextBearID==="+_curBearNum+"===="+_MaxNum);

		if(_curBearNum>=_MaxNum)
		{
			BearDead(bp.transform);
			return -1;
		}

		if((bp.right != null && szBigBear.Contains(bp.right.id)) ||(bp.left != null &&szBigBear.Contains(bp.left.id)))
		{
			if(!_bDonotNeedCoin&&bp.index %2 == 0 && Random.Range(0,100)>50)
			{
				BearDead(bp.transform);
				return -1;
			}

			return dataCtrl.GetRandomBearID();
		}

	
		if(_ShareWorld.bBossResurrection&&_dataPlayer.Level>=dataCtrl.GetBearParam(WGDefine.BossID).unlock
			&& _bearBoss == null && !szLiveBearID.Contains(WGDefine.BossID))
		{

			_ShareWorld.bBossResurrection = false;

			BCSoundPlayer.Play(MusicEnum.bossLaugh);


			for(int i=0;i<5;i++)
			{
				StartCoroutine(showBoosTips(i*0.5f));
			}

			return WGDefine.BossID;
		}


		if(_dataPlayer.szPayObjID.Contains(WGDefine.PayBear1)&&
			!szLiveBearID.Contains(WGDefine.PayBear1))
		{
			return WGDefine.PayBear1;
		}
		if(_dataPlayer.szPayObjID.Contains(WGDefine.PayBear2)&&
			!szLiveBearID.Contains(WGDefine.PayBear2))
		{
			return WGDefine.PayBear2;
		}

		if(!_bDonotNeedCoin && bp.index %2 == 0 && Random.Range(0,100)>50)
		{
			BearDead(bp.transform);
			return -1;
		}

		return dataCtrl.GetRandomBearID();
	}
	IEnumerator showBoosTips(float time)
	{
		yield return new WaitForSeconds(time);
		WGAlertViewController.Self.showTipView(1102);
	}
	/// <summary>
	/// boss 是否消失成功,
	/// </summary>
	/// <returns><c>true</c>, if disappear was bossed, <c>false</c> otherwise.</returns>
	public bool BossDisappear()
	{
		if(szLiveBearID.Contains(WGDefine.BossID)&&_bearBoss !=null)
		{
			szLiveBearID.Remove(WGDefine.BossID);
			_bearBoss.Disappear();
			BearDead(_bearBoss.transform.parent);

			_bearBoss = null;

			return true;
		}
		return false;
	}

	void CreateBearWith(int id,Transform t)
	{
		WGBearParam bp = dataCtrl.GetBearParam(id);
//		return;
		if(bp != null)
		{
			_curBearNum++;
			//int a = 5001;
			GameObject bearobj = _ShareObjManager.BCGameObjFactory(id,t.position,t.rotation.eulerAngles,_BearScale);

			bearobj.transform.parent = t.transform;
			bearobj.transform.localPosition = Vector3.zero;

			GameObject hp = Instantiate(HP,t.transform.position,t.transform.rotation) as GameObject;
			WGBear bear = bearobj.GetComponent<WGBear>();


			bear.ID  = id;
			bear.Blood = bp.hp;
			hp.transform.parent = bearobj.transform;
			hp.transform.localPosition = new Vector3(0,-0.5f,3.5f);
			hp.transform.localEulerAngles = new Vector3(0,-180,0);

			bear.AddHP(hp);

			szLiveBearID.Add(id);
			if(id == WGDefine.BossID)
			{
				_bearBoss = bear;
			}
		}
	}

	/// <summary>
	/// 变冰雪效果
	/// </summary>
	public void ChangeTableMoca()
	{
		CancelInvoke("ResetTable");
		goTableDesk.SetActive(false);
		goIceDesk.SetActive(true);
		Invoke("ResetTable",10f);
	}
	void ResetTable()
	{
		goTableDesk.SetActive(true);
		goIceDesk.SetActive(false);
	}
	//不需要金币20秒
	public void DonNotNeedCoin(float time)
	{
		_bDonotNeedCoin = true;
		fCreateBearTime = 1.5f;
		_MaxNum=_Num;
		CancelInvoke("CreateBear");

//		//WG.SLog("DonNotNeedCoin=============");

		int max = _DeadBearList.Count;
		int i = 0;
		while(i<max)
		{
			i++;
			if(_DeadBearList.Count>0)
			{
				CreateBear();
			}
			else{
				break;
			}
		}


		InvokeBlock(time,()=>{
			fCreateBearTime = 3;
			_MaxNum = 10;
			_bDonotNeedCoin = false;
		});

//		csThrow.DonotNeedCoin(time);
	}

	/// <summary>
	/// 树墙,维持多长时间的
	/// </summary>
	bool IsReleaseSkill = false;
	public void BearTreeUp(float time,bool isSkill = false)
	{
		if(IsReleaseSkill && !isSkill) return;
		if(IsReleaseSkill && isSkill)
		{
			time = _dataPlayer.defenseTime;
		}
		if(!IsReleaseSkill && isSkill)
		{

		}
		if(!isSkill)
		{
			_IsLongTongue = true;
		}
		CancelInvoke("BearTreeDown");
		CancelInvoke("UnableTreeTween");
		TreeTween.gameObject.SetActive(true);
		TreeTween.enabled = true;
		TreeTween.PlayForward();
		BCSoundPlayer.Play(MusicEnum.tree);
		Invoke("BearTreeDown",time+1);
//		SpeedUP();
		IsReleaseSkill = isSkill;
	}

	void BearTreeDown()
	{
		IsReleaseSkill = false;
		TreeTween.enabled = true;
		TreeTween.PlayReverse();
		BCSoundPlayer.Play(MusicEnum.tree);
		Invoke("UnableTreeTween",1);
		
	}
	void UnableTreeTween()
	{
		TreeTween.gameObject.SetActive(false);
		TreeTween.enabled=false;
	}

	/// <summary>
	///长舌50%
	/// </summary>
	public void Long50()
	{

		_IsLongTongue = true;
//		SpeedUP();
	}
	public void Long100()
	{

		_IsWave = true;
		SpeedUP();
	}
	/// <summary>
	/// 加快吐舌速度5秒
	/// </summary>
	public void SpeedUP()
	{
		CancelInvoke("SpeedDown");
		Speed = _sp.SPEED_Add1;
		Invoke("SpeedDown",5f);
	}

	void SpeedDown()
	{
		Speed = _sp.SPEED_commen;
	}

	/// <summary>
	/// 转盘速度减慢
	/// </summary>
	public void DecelerationTurn(float time)
	{
		CancelInvoke("DecelerationEnd");
		mTurnPlate.v3Rotate = Core.fc.turnPlateSlow;
		csRoteTp.V = Core.fc.turnPlateSlow.z/40f;
		Invoke("DecelerationEnd",time);
	}
	void DecelerationEnd()
	{
		mTurnPlate.v3Rotate = Core.fc.turnPlateComm;
		csRoteTp.V = Core.fc.turnPlateComm.z/40f;
	}





}
