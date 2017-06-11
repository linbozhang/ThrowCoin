using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
public class D0330TigerPanel : MonoBehaviour {

	public System.Action<int,MDTiger> tigerCallBack;

	public int mRewardIndex;
	
	private List<MDTiger> _szTigerInfo;
	
	private List<int> _TigerWeightList;
	
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
			if(!_CanRotate)
			{
				_CanRotate = true;
			}
			_SetSpeed = false;
		}
	}
	

	private bool _CanRotate = false;
	
	private float _RoTime;
	
	//private float BIG
	
	int _777Index = 11;
	

	bool bInit=false;
	void InitSuperTiger()
	{
		if(bInit)return;
		bInit = true;

		_shareWorld = WGGameWorld.Instance;
		_CurrentShowID = new int[3];
		_WantShowID = new int[3];

		WGDataController.Instance.loadSuperTiger();

		_szTigerInfo = WGDataController.Instance.szSuperTiger;
		_TigerWeightList = WGDataController.Instance._szSuperTigerWeight;
		_AllTigerWeight = WGDataController.Instance._AllTigerWeight;
	}
	int GetRandomID()
	{
		//		return 9;
		
		DataPlayer _dp = DataPlayerController.getInstance().data;
		
		
		
		int maxWeight = _AllTigerWeight;

		
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
		
		MDTiger tiger = _szTigerInfo[id];
		
		WGAchievementManager.Self.processAchievement(tiger.ID,DTAchievementType.TIGER_ID);
		
		int _tempType = tiger.RewardType;
		//WG.SLog(SDK.Serialize(tiger));
		if(tiger.reward.Count>0)
		{
			if(tigerCallBack != null)
			{
				tigerCallBack(1,tiger);
			}
//			showReward(tiger);
		}
		else{
			if(tigerCallBack != null)
			{
				tigerCallBack(0,tiger);
			}
		}

			
	}

	public void StartTiger(int n = 1)
	{

			Num +=n;



	}
	
	// Update is called once per frame
	//int localId=1;
	void Update () {
		
		if(_Num>0&&_CanRotate)
		{
			_CanRotate = false;
			if(!_SetSpeed)
			{
				_CurrentID = GetRandomID();
				//_CurrentID=(localId++);
				//WG.SLog("===_CurrentID==="+_CurrentID);
//				_CurrentID = 11;
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

		_NeedRo = false;
	}
	

	public static D0330TigerPanel CreateTigerPanel()
	{
		Object obj = Resources.Load("pbD0330TigerPanel");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			D0330TigerPanel t = go.GetComponent<D0330TigerPanel>();
			t.InitSuperTiger();
			return t;
		}
		return null;
	}
	
	

}
