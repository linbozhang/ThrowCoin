

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
	


public class WGGameTiger : MonoBehaviour {


	public int mRewardIndex;

	private List<TigerInfo> _szTigerInfo = new List<TigerInfo>();

	private List<int> _TigerWeightList = new List<int>();
	
	private int _AllTigerWeight;
	
	private float[] _ShowAngle;
		
	public GameObject[] RoGameObj;
	
	private float _Angle;
	
	public float[] RoTime;
	
	public float[] ToAngle;
	
	public float[] _AngleV = new float[3];
	
	private bool _SetSpeed = false;
	
	public float[] FromRo;
	
	public float MaxTime;
	
	public bool _NeedRo = false;
	
	private int _Num = 0;
	
	private int _DeltaAngle;

	WGGameWorld _shareWorld;

	private int[] _CurrentShowID;
	
	private int[] _WantShowID;
	
	private int _CurrentID;
	
	public AnimationCurve Curve;
	
	public int Num
	{
		get{return _Num;}
		set{
			_Num = value;
			if(_Num==1&&!_CanRotate)
			{
				_CanRotate = true;
			}
			_SetSpeed = false;
		}
	}
	
	public static WGGameTiger Instance;
	
	private bool _CanRotate = false;
	
	private float _RoTime;

	//private float BIG

	int _777Index = 11;
	
	// Use this for initialization
	void Start () {


		//StartRotate();
		Instance = this;
		TextAsset s =  Resources.Load(WGConfig.Path_Tiger)as TextAsset;
	
		TigerInfo tem;
		using(StreamReader sr = new StreamReader(new MemoryStream(s.bytes)))
		{
			string line;
			while((line = sr.ReadLine())!=null)
			{
				tem = SDK.Deserialize<TigerInfo>(line);
				_szTigerInfo.Add(tem);
			}
		}

		Resources.UnloadAsset(s);


		int num = _szTigerInfo.Count;
		
		_TigerWeightList = new List<int>();
		
		_TigerWeightList.Add(_szTigerInfo[0].Weight);
		
		for(int i = 1; i <num; i++)
		{
			if(_szTigerInfo[i].ID == 4112)
			{
				_777Index = i;
			}
			_TigerWeightList.Add(_TigerWeightList[i-1]+_szTigerInfo[i].Weight);
		}
		_AllTigerWeight = _TigerWeightList[num-1];

		_shareWorld = WGGameWorld.Instance;
		_CurrentShowID = new int[3];
		_WantShowID = new int[3];
		
	}
	
	int GetRandomID()
	{
//		return 9;

		DataPlayer _dp = DataPlayerController.getInstance().data;



		int maxWeight = _AllTigerWeight;

		if(_dp.up777Time>0)
		{
			MDSkill sk = WGDataController.Instance.getSkill(WGDefine.SK_777Up1);
			maxWeight = _AllTigerWeight+ sk.paramater.toInt();
		}
		if(WGHelpManager.Self != null&&WGHelpManager.Self.enabled)
		{
			if(!WGHelpManager.Self.StatesIsEnd(EMHelpStates.Tiger_777))
			{
				maxWeight += 90000;
			}
		}

		int weight = Random.Range(0,maxWeight);

		for(int i = 0,max = _TigerWeightList.Count;i<max;i++)
		{
			if(_TigerWeightList[i]>=weight)
			{
				return i;
			}
		}

		return _777Index;//如果没有则返回[7,7,7]
	}
	
	void StartTiger(int id)
	{
		if(id == -1)
			return ;
		else
		{
			//_TigerInfoDic[id].ShowIndex
		}
	}
	//++++++++++++++++++++++++++++++++临时
	
	void ShowTiger(int id)
	{
		_WantShowID = _szTigerInfo[id].ShowIndex.ToArray();
	}
	
	void SetSpeed()
	{
		for(int i = 0; i<3; i++)
		{
			FromRo[i] = RoGameObj[i].transform.localEulerAngles.x;
			ToAngle[i] = 30*(12-_WantShowID[i])-1080;
			_AngleV[i]  = ToAngle[i]/RoTime[i];
			_CurrentShowID[i] = _WantShowID[i];
		}
	}
	
	int GetRandomCollectionID()
	{
		int idx = Random.Range(0,WGDataController.Instance.szCollectionObj.Count);

		BCObj obj = WGDataController.Instance.szCollectionObj[idx];

		return obj.ID;
	}

	IEnumerator StopCollectionAni(GameObject t)
	{
		yield return  new WaitForSeconds(0.5f);
		t.GetComponent<Rigidbody>().useGravity = true;
		t.GetComponent<Rigidbody>().isKinematic = false;
	}
	void Reward(int id)
	{

		if(_CurrentID == _777Index)
		{
			BCSoundPlayer.Play(MusicEnum.tiger777);
		}

		TigerInfo tiger = _szTigerInfo[id];

		WGAchievementManager.Self.processAchievement(tiger.ID,DTAchievementType.TIGER_ID);

		int _tempType = tiger.RewardType;
		int _tempParam1 = tiger.Parameter1;
		int _tempParam2 = tiger.Parameter2;
	
		switch(_tempType)
		{
		case 0:
			return;
		case 1:
			_shareWorld.AddCoin(_tempParam1,_tempParam2,true);
			break;
		case 4:
			{
				_shareWorld.AddPackage(_tempParam1,true);
			}
			break;
		case 3:
			WGTigerSkillEffect.Self.ShowSkillEffectWithSKid(_tempParam1);
			WGSkillController.Instance.ReleaseSkillWithID(_tempParam1);

			break;
		case 5:
			break;
		}		
	}
	
	public void StartTiger()
	{
		Num++;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(_Num>0&&_CanRotate)
		{
			_CanRotate = false;
			if(!_SetSpeed)
			{
				_CurrentID = GetRandomID();

				//WG.SLog("===_CurrentID==="+_CurrentID);
				//_CurrentID = 11;
				ShowTiger(_CurrentID);
				SetSpeed();
				_SetSpeed = true;
				_NeedRo = true;
				_RoTime = -Time.deltaTime;
				Invoke("StopRotate",MaxTime);
			}
			
		}

		if(_NeedRo)
		{
			RotateUpdate();
		}
	}
	
	void RotateUpdate()
	{
		_RoTime+=Time.deltaTime;
		for(int i = 0;i<RoGameObj.Length;i++)
		{
			_Angle = Mathf.Lerp(FromRo[i],ToAngle[i],Curve.Evaluate(_RoTime/RoTime[i]));

			RoGameObj[i].transform.localEulerAngles = new Vector3(_Angle,0,0);
			
		}
	}
	void StopRotate()
	{
		if(--_Num>0)
		{
			_CanRotate = true;
			_SetSpeed = false;
			
		}
		Reward(_CurrentID);
		if(YeHuoSDK.bCommonTiger)
		{
			if(_CurrentID == _777Index)
			{
				if(WGHelpManager.Self.enabled)
				{
					if(!WGHelpManager.Self.StatesIsEnd(EMHelpStates.Tiger_777))
					{
						WGHelpManager.Self.ShowHelpView(EMHelpStates.Tiger_777);
					}
					else
					{
						WGTiger777.Self.showView();
					}
				}
				else
				{
					WGTiger777.Self.showView();
				}
			}
			else
			{
	//			WGTiger777.Self.showView();
			}
		}
		_NeedRo = false;
	}














}
