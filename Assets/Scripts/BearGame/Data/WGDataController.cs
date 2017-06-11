using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


public class WGDataController : MonoBehaviour {
	/// <summary>
	/// 物品数据
	/// </summary>
	public Dictionary<int,BCObj> dicGameObj;
	/// <summary>
	/// 小熊的数据
	/// </summary>
	public Dictionary<int,WGBearParam> dicBearParam;
	/// <summary>
	/// 解锁的小熊的id列表
	/// </summary>
	public SortedDictionary<int,List<WGBearParam>> dicBearUnLock;
	/// <summary>
	/// 收集品信息
	/// </summary>
	public Dictionary<int,BCCollectionInfo> dicCollection;
	/// <summary>
	/// 升级奖励信息
	/// </summary>
	public Dictionary<int,LevelUPReward>  dicLevelUpReward = new Dictionary<int, LevelUPReward>();
	/// <summary>
	/// 当前解锁的小熊的权重
	/// </summary>
	private SortedDictionary<int, WGBearParam> dicBearWeight = new SortedDictionary<int, WGBearParam>();

	public List<BCObj> szCollectionObj = new List<BCObj>();
	public Dictionary<int,List<BCObj>> dicTypeGameObj = new Dictionary<int, List<BCObj>>();

	public List<BCObj> szItemsObj = new List<BCObj>();

	public List<MDWeapon> szWeapons = new List<MDWeapon>();

	public List<MDShopData> szShopData = new List<MDShopData>();

	public List<WGBearParam> szBearsData = new List<WGBearParam>();

	public Dictionary<int,MDRewardPackage> dicRewardPackage = new Dictionary<int, MDRewardPackage>();

	public Dictionary<int,MDSkill> dicSkills = new Dictionary<int, MDSkill>();
	public List<MDSkill> szSkills = new List<MDSkill>();

	public List<MDAchievement> szAchievement = new List<MDAchievement>();
	public Dictionary<int,List<MDAchievement>> dicGoalAchieve = new Dictionary<int, List<MDAchievement>>();

	public Dictionary<int,MDTigerInfo> dicTigerInfo = new Dictionary<int, MDTigerInfo>();

	public Dictionary<int,int> dicObjType = new Dictionary<int, int>();

	public List<MDV2HuaFei> szHuaFei = new List<MDV2HuaFei>();

	public List<MDTiger> szSuperTiger = new List<MDTiger>();
	[HideInInspector]
	public List<int>_szSuperTigerWeight = new List<int>();

	public Dictionary<int,YHMDPayData> dicYHPayData = new Dictionary<int, YHMDPayData>();

	public Dictionary <int,MFText> dicStrings = new Dictionary<int, MFText>();

	[HideInInspector]
	public int _AllTigerWeight = 0;
	public YHMDAllReward mAllReward;

	WGRandom mTurnRandom;

	[HideInInspector]
	public DataCoin mDataCoin;

	int _MaxRandomWeight=0;
	int _NextUnLockLV = 0;


	[HideInInspector]
	public bool bLoadDataSuccess = false;

	public static WGDataController Instance;
	void Awake()
	{
		DontDestroyOnLoad(this.gameObject);
		Instance = this;
		Core.Initialize();
		mTurnRandom = new WGRandom();

	}
	void Start()
	{

		StartCoroutine(loadData());

	}

	IEnumerator loadData()
	{
		yield return new WaitForEndOfFrame();
		StartCoroutine( loadConfig());
		yield return new WaitForEndOfFrame();
		StartCoroutine(loadString());
		yield return new WaitForEndOfFrame();
		StartCoroutine(loadYHPayData());
		yield return new WaitForEndOfFrame();
		StartCoroutine(loadObjectType());
		yield return new WaitForEndOfFrame();
		StartCoroutine(LoadBCGameObjConfig());
		yield return new WaitForEndOfFrame();
		StartCoroutine(LoadBearParam());
		yield return new WaitForEndOfFrame();
		StartCoroutine(LoadCollectionParam());
		yield return new WaitForEndOfFrame();
		StartCoroutine(InitLevelup());
		yield return new WaitForEndOfFrame();
		StartCoroutine(LoadCoinData());
		yield return new WaitForEndOfFrame();
		StartCoroutine(LoadWeaponData());
		yield return new WaitForEndOfFrame();
		StartCoroutine(LoadShopData());
		yield return new WaitForEndOfFrame();
		StartCoroutine(LoadRewardPackage());
		yield return new WaitForEndOfFrame();
		StartCoroutine(LoadSkill());
		yield return new WaitForEndOfFrame();
		StartCoroutine(loadTigerInfo());
		yield return new WaitForEndOfFrame();
		StartCoroutine(loadAchievement());
		yield return new WaitForEndOfFrame();
		StartCoroutine(loadAllReward());
		yield return new WaitForEndOfFrame();
		StartCoroutine(loadFuafei());
		yield return new WaitForEndOfFrame();
		bLoadDataSuccess = true;
	}

	IEnumerator loadYHPayData()
	{
		TextAsset ta = null;
		if(YeHuoSDK.bUsePayCode2)
		{
//			 ta = Resources.Load(WGConfig.Path_YHPayDataExt) as TextAsset;
			ResourceRequest rr = Resources.LoadAsync(WGConfig.Path_YHPayDataExt);
			yield return rr;
			ta = rr.asset as TextAsset;
		}
		else
		{
//		 	ta = Resources.Load(WGConfig.Path_YHPayData) as TextAsset;
			ResourceRequest rr = Resources.LoadAsync(WGConfig.Path_YHPayData);
			yield return rr;
			ta = rr.asset as TextAsset;
		}

		YHMDPayData tem;
		using(StreamReader sr = new StreamReader(new MemoryStream(ta.bytes)))
		{
			string line;
			while(!string.IsNullOrEmpty(line = sr.ReadLine()))
			{
				tem = SDK.Deserialize<YHMDPayData>(line);
				dicYHPayData.Add(tem.id,tem);
			}
		}
		Resources.UnloadAsset(ta);
		
	}
	public void loadSuperTiger()
	{
		if(szSuperTiger.Count<=0)
		{
			TextAsset s =  Resources.Load(WGConfig.Path_SuperTiger)as TextAsset;
			
			MDTiger tem;
			using(StreamReader sr = new StreamReader(new MemoryStream(s.bytes)))
			{
				string line;
				while(!string.IsNullOrEmpty(line = sr.ReadLine()))
				{
					//Debug.Log(line);
					tem = SDK.Deserialize<MDTiger>(line);
					szSuperTiger.Add(tem);
				}
			}

			int num = szSuperTiger.Count;
			
			_szSuperTigerWeight = new List<int>();
			
			_szSuperTigerWeight.Add(szSuperTiger[0].Weight);
			
			for(int i = 1; i <num; i++)
			{
				_szSuperTigerWeight.Add(_szSuperTigerWeight[i-1]+szSuperTiger[i].Weight);
			}
			_AllTigerWeight = _szSuperTigerWeight[num-1];
			
			Resources.UnloadAsset(s);

		}


	}
	IEnumerator loadFuafei()
	{
//		TextAsset ta = Resources.Load(WGConfig.Path_Huafei) as TextAsset;
		ResourceRequest rr = Resources.LoadAsync(WGConfig.Path_Huafei);
		yield return rr;
		TextAsset ta = rr.asset as TextAsset;

		using(StreamReader sr = new StreamReader(new MemoryStream(ta.bytes)))
		{
			string line ;
			MDV2HuaFei hf;
			while(!string.IsNullOrEmpty(line = sr.ReadLine()))
			{
				hf = SDK.Deserialize<MDV2HuaFei>(line);
				szHuaFei.Add(hf);
			}
		}
		Resources.UnloadAsset(ta);

	}
	IEnumerator loadAllReward()
	{
//		TextAsset ta = Resources.Load(WGConfig.Path_Reward) as TextAsset;
		ResourceRequest rr = Resources.LoadAsync(WGConfig.Path_Reward);
		yield return rr;
		TextAsset ta  =rr.asset as TextAsset;

		using(StreamReader sr = new StreamReader(new MemoryStream(ta.bytes)))
		{
			string line = sr.ReadToEnd() ;


			mAllReward = SDK.Deserialize<YHMDAllReward>(line);

		}
		Resources.UnloadAsset(ta);
	}
	IEnumerator loadObjectType()
	{
		TextAsset ta = Resources.Load(WGConfig.Path_Types) as TextAsset;

		ResourceRequest rr = Resources.LoadAsync(WGConfig.Path_Types);

		yield return rr;

		ta = rr.asset as TextAsset;

		using(StreamReader sr = new StreamReader(new MemoryStream(ta.bytes)))
		{
			string line ;
			MDType tp;
			while(!string.IsNullOrEmpty(line = sr.ReadLine()))
			{
				tp = SDK.Deserialize<MDType>(line);
				dicObjType.Add(tp.ID,tp.Type);
			}
		}
		Resources.UnloadAsset(ta);
	}
	IEnumerator loadString()
	{

		Dictionary<int,MFText> uniDic = new Dictionary<int, MFText>();
		if(YeHuoSDK.bUsePayCode2)
		{
//			TextAsset tt= Resources.Load(WGConfig.Path_StringExt) as TextAsset;
			ResourceRequest rr = Resources.LoadAsync(WGConfig.Path_StringExt);
			yield return rr;

			TextAsset tt = rr.asset as TextAsset;

			using(StreamReader sr = new StreamReader(new MemoryStream(tt.bytes)))
			{
				string line;
				while ((line = sr.ReadLine()) != null) {
					
					if (line.StartsWith ("#") || line.Trim ().Equals (string.Empty)) {
						//skip this line
						continue;
					}
					try{
						
						MFText text = SDK.Deserialize<MFText>(line);
						uniDic.Add(text.ID,text);
						
					}
					catch(System.Exception e){
						Debug.LogError(line+"####"+e.ToString());
					}
					
				}
			}
			Resources.UnloadAsset(tt);
		}
		
//		TextAsset ta = Resources.Load(WGConfig.Path_String) as TextAsset;
		ResourceRequest rr1 = Resources.LoadAsync(WGConfig.Path_String);
		yield return rr1;
		
		TextAsset ta = rr1.asset as TextAsset;

		using(StreamReader sr = new StreamReader(new MemoryStream(ta.bytes)))
		{
			string line;
			while ((line = sr.ReadLine()) != null) {
				
				if (line.StartsWith ("#") || line.Trim ().Equals (string.Empty)) {
					//skip this line
					continue;
				}
				try{
					
					MFText text = SDK.Deserialize<MFText>(line);
					if(YeHuoSDK.bUsePayCode2)
					{
						if(uniDic.ContainsKey(text.ID))
						{
							text = uniDic[text.ID];
							uniDic.Remove(text.ID);
						}
					}
					
					dicStrings.Add(text.ID,text);
					
				}
				catch(System.Exception e){
					//WG.LogError(line+"####"+e.ToString());
				}
				
			}
		}

		WGStrings.instance.InitDataWithAsset(dicStrings);
		Resources.UnloadAsset(ta);
	}
	IEnumerator loadTigerInfo()
	{
//		TextAsset ta = Resources.Load(WGConfig.Path_TigerInfo) as TextAsset;
		ResourceRequest rr = Resources.LoadAsync(WGConfig.Path_TigerInfo);
		yield return rr;
		TextAsset ta = rr.asset as TextAsset;
		using(StreamReader sr = new StreamReader(new MemoryStream(ta.bytes)))
		{
			string line ;
			MDTigerInfo ti;
			while(!string.IsNullOrEmpty(line = sr.ReadLine()))
			{
				ti = SDK.Deserialize<MDTigerInfo>(line);
				dicTigerInfo.Add(ti.id,ti);
			}
		}
		Resources.UnloadAsset(ta);
	}
	IEnumerator loadAchievement()
	{
//		TextAsset ta = Resources.Load(WGConfig.Path_Achievement) as TextAsset;
		ResourceRequest rr = Resources.LoadAsync(WGConfig.Path_Achievement);
		yield return  rr;
		TextAsset ta = rr.asset as TextAsset;

		using(StreamReader sr = new StreamReader(new MemoryStream(ta.bytes)))
		{
			string line;
			MDAchievement ach;
			while(!string.IsNullOrEmpty(line = sr.ReadLine()))
			{
				ach = SDK.Deserialize<MDAchievement>(line);

				if(!YeHuoSDK.bCommonTiger&&ach.goals.Contains(4112))
				{

				}
				else{

					szAchievement.Add(ach);

					for(int i=0,max=ach.goals.Count;i<max;i++)
					{
						if(dicGoalAchieve.ContainsKey(ach.goals[i]))
						{
							dicGoalAchieve[ach.goals[i]].Add(ach);
						}
						else{
							List<MDAchievement> temp = new List<MDAchievement>();
							temp.Add(ach);
							dicGoalAchieve.Add(ach.goals[i],temp);
						}
					}
					processAchievementDescription(ach);
				}
			}
		}
		Resources.UnloadAsset(ta);
	}
	void processAchievementDescription(MDAchievement ach)
	{
		string objName =  "";
		if(ach.type == DTAchievementType.TIGER_ID)
		{
			MDTigerInfo ti = GetTigerInfo(ach.goals[0]);
			objName = ti.name;
		}
		else
		{
			BCObj mObj = GetBCObj(ach.goals[0]);
			objName = mObj.Name;
		}

		string desstr = "";
		string[] des = ach.des.Split('#');
		for(int i=0,max =des.Length;i<max;i++)
		{
			desstr +=des[i];
			if(i<ach.pramater.Count)
			{
				if(ach.pramater[i]==1)
				{
					desstr += objName;
				}
				else if(ach.pramater[i]==2)
				{
					desstr +=ach.goal_num.ToString();
				}
			}
		}
		ach.des = desstr;
	}
	IEnumerator loadConfig()
	{
		ResourceRequest rr = Resources.LoadAsync(WGConfig.Path_Config);
		yield return rr;
		TextAsset ta = rr.asset as TextAsset;
//		TextAsset ta = Resources.Load(WGConfig.Path_Config) as TextAsset;
		using(StreamReader sr = new StreamReader(new MemoryStream(ta.bytes)))
		{
			string line = sr.ReadToEnd();
			if(!string.IsNullOrEmpty(line))
			{
				Core.cfg = SDK.Deserialize<Config>(line);
				Core.fc = new FinalConfig(Core.cfg);
			}
//			//WG.SLog(line);
		}
		Resources.UnloadAsset(ta);
	}
	IEnumerator LoadSkill()
	{
//		TextAsset ta = Resources.Load(WGConfig.Path_Skill) as TextAsset;
		ResourceRequest rr = Resources.LoadAsync(WGConfig.Path_Skill);
		yield return rr;
		TextAsset ta = rr.asset as TextAsset;
		using(StreamReader sr = new StreamReader(new MemoryStream(ta.bytes)))
		{
			string line;
			MDSkill sd;
			while(!string.IsNullOrEmpty(line = sr.ReadLine()))
			{
				sd = SDK.Deserialize<MDSkill>(line);
				szSkills.Add(sd);
				dicSkills.Add(sd.id,sd);
			}
//			//WG.SLog(JsonFx.Json.JsonWriter.Serialize(dicSkills));
		}
		Resources.UnloadAsset(ta);
	}
	IEnumerator LoadRewardPackage()
	{
//		TextAsset ta = Resources.Load(WGConfig.Path_RewardPackage) as TextAsset;
		ResourceRequest rr = Resources.LoadAsync(WGConfig.Path_RewardPackage);

		yield return rr;

		TextAsset ta = rr.asset as TextAsset;

		using(StreamReader sr = new StreamReader(new MemoryStream(ta.bytes)))
		{
			string line;
			MDRewardPackage sd;
			while(!string.IsNullOrEmpty(line = sr.ReadLine()))
			{
				sd = SDK.Deserialize<MDRewardPackage>(line);
				dicRewardPackage.Add(sd.id,sd);
			}
		}
		Resources.UnloadAsset(ta);
	}
	IEnumerator LoadShopData()
	{
	
		Dictionary<int,MDShopData> uniTemp = new Dictionary<int, MDShopData>();
		if(YeHuoSDK.bUsePayCode2)
		{
//			TextAsset tt = Resources.Load(WGConfig.Path_ShopDataExt) as TextAsset;

			ResourceRequest rr = Resources.LoadAsync(WGConfig.Path_ShopDataExt);
			yield return rr;
			TextAsset tt = rr.asset as TextAsset;

			if(tt!=null)
			{
				using( StreamReader sr = new StreamReader(new MemoryStream(tt.bytes)))
				{
					string line ;
					MDShopData sd;
					bool inUni = false;
					while(!string.IsNullOrEmpty(line = sr.ReadLine()))
					{
						sd = SDK.Deserialize<MDShopData>(line);
						ProcessShopData(sd);
						uniTemp.Add(sd.idx,sd);
					}
				}
			}
			Resources.UnloadAsset(tt);
		}
		
//		TextAsset ta = Resources.Load(WGConfig.Path_ShopData) as TextAsset;

		ResourceRequest rr1 = Resources.LoadAsync(WGConfig.Path_ShopData);
		yield return rr1;
		TextAsset ta = rr1.asset as  TextAsset;
		using(StreamReader sr = new StreamReader(new MemoryStream(ta.bytes)))
		{
			string line;
			MDShopData sd;
			while(!string.IsNullOrEmpty(line = sr.ReadLine()))
			{
				sd = SDK.Deserialize<MDShopData>(line);
				ProcessShopData(sd);

				if(YeHuoSDK.bUsePayCode2 && uniTemp.ContainsKey(sd.idx))
				{
					sd = uniTemp[sd.idx];
					uniTemp.Remove(sd.idx);
				}
				szShopData.Add(sd);
			}
		}
		Resources.UnloadAsset(ta);
	}
	void ProcessShopData(MDShopData sd)
	{
		if(!string.IsNullOrEmpty(sd.proid))
		{
			string[] proid = sd.proid.Split('#');
			if(proid.Length>0)
			{
				sd.proid = proid[0];
			}
			if(proid.Length>1)
			{
				sd.proid2 = proid[1];
			}
		}
	}
	IEnumerator LoadWeaponData()
	{
//		TextAsset ta = Resources.Load(WGConfig.Path_Weapon) as TextAsset;
		ResourceRequest rr = Resources.LoadAsync(WGConfig.Path_Weapon);
		yield return rr;
		TextAsset ta = rr.asset as TextAsset;

		using(StreamReader sr = new StreamReader(new MemoryStream(ta.bytes)))
		{
			string line;
			MDWeapon weapon;
			while(!string.IsNullOrEmpty(line = sr.ReadLine()))
			{
				weapon = SDK.Deserialize<MDWeapon>(line);
				szWeapons.Add(weapon);
//				//WG.SLog(SDK.Serialize(weapon));
			}
		}
		Resources.UnloadAsset(ta);
	}
	/// <summary>
	/// 加载小熊的数据
	/// </summary>
	IEnumerator LoadBearParam()
	{
//		TextAsset BC =  Resources.Load(WGConfig.Path_BearParam)as TextAsset;
		ResourceRequest rr = Resources.LoadAsync(WGConfig.Path_BearParam);
		yield return rr;
		TextAsset BC = rr.asset as TextAsset;

		dicBearParam = new Dictionary<int, WGBearParam>();
		dicBearUnLock = new SortedDictionary<int, List<WGBearParam>>();
		WGBearParam tem;
		using(StreamReader sr = new StreamReader(new MemoryStream(BC.bytes)))
		{
			string line;

			while((line = sr.ReadLine())!=null)
			{
//				//WG.SLog(line);
				tem = SDK.Deserialize<WGBearParam>(line);

				dicBearParam.Add(tem.id,tem);
				szBearsData.Add(tem);
				List<WGBearParam> idlist = null;
				if(tem.unlock>=0)
				{
					if(dicBearUnLock.TryGetValue(tem.unlock,out idlist))
					{
						idlist.Add(tem);
					}
					else{
						idlist=new List<WGBearParam>();
						idlist.Add(tem);
						dicBearUnLock.Add(tem.unlock,idlist);
					}
				}

//				//WG.SLog(JsonFx.Json.JsonWriter.Serialize(tem));
			}
		}
		Resources.UnloadAsset(BC);
	}


	/// <summary>
	/// 加载物品配置表
	/// </summary>
	IEnumerator LoadBCGameObjConfig()
	{
//		TextAsset BC =  Resources.Load(WGConfig.Path_BCObj)as TextAsset;
		ResourceRequest rr = Resources.LoadAsync(WGConfig.Path_BCObj);
		yield return rr;
		TextAsset BC = rr.asset as TextAsset;
		dicGameObj = new Dictionary<int,BCObj>(); 
		BCObj tem;
		using(StreamReader sr = new StreamReader(new MemoryStream(BC.bytes)))
		{
			string line;
			while((line = sr.ReadLine())!=null)
			{

				tem = SDK.Deserialize<BCObj>(line)as BCObj;
				dicGameObj.Add(tem.ID,tem);
				GameObject go =null;

				go = Resources.Load("items/res"+tem.Res) as GameObject;

				tem.goRes = go;

				List<BCObj> szType = null;
				dicTypeGameObj.TryGetValue(tem.Type,out szType);
				if(szType == null){
					szType = new List<BCObj>();
					dicTypeGameObj.Add(tem.Type,szType);
				}
				if(!szType.Contains(tem))
				{
					szType.Add(tem);
				}


				if(tem.BCType == BCGameObjectType.Item)
				{
					szItemsObj.Add(tem);
				}

			}
		}

		Resources.UnloadAsset(BC);
	}

	/// <summary>
	/// 加载收集品的数据
	/// </summary>
	IEnumerator LoadCollectionParam()
	{
//		TextAsset s = Resources.Load(WGConfig.Path_conllectioninfo) as TextAsset;

		ResourceRequest rr = Resources.LoadAsync(WGConfig.Path_conllectioninfo);
		yield return rr;
		TextAsset s = rr.asset as TextAsset;

		dicCollection = new Dictionary<int, BCCollectionInfo>();
		BCCollectionInfo tem;
		using(StreamReader sr = new StreamReader(new MemoryStream(s.bytes)))
		{
			string line;
			int index =0;
			while((line = sr.ReadLine())!=null)
			{
				tem = SDK.Deserialize<BCCollectionInfo>(line);
				dicCollection.Add(tem.ID,tem);
				szCollectionObj.Add(dicGameObj[tem.ID]);
				index++;
			}
		}

		Resources.UnloadAsset(s);
	}
	/// <summary>
	/// 加载升级奖励数据
	/// </summary>
	IEnumerator InitLevelup()
	{
//		TextAsset s = Resources.Load( WGConfig.Path_levelup)as TextAsset;
		ResourceRequest rr = Resources.LoadAsync( WGConfig.Path_levelup);
		yield return rr;
		TextAsset s = rr.asset as TextAsset;
		using(StreamReader sr = new StreamReader(new MemoryStream(s.bytes)))
		{
			string line;
			while((line = sr.ReadLine())!=null)
			{
				try{

					LevelUPReward lr = SDK.Deserialize<LevelUPReward>(line);
					lr.refreshWeight();
					dicLevelUpReward.Add(lr.level,lr);
				}
				catch(IOException e){
					Debug.Log(e.ToString());
				}
			}
		}
		Resources.UnloadAsset(s);
	}


	public BCGameObjectType GetBCObjType(int id)
	{
		int type=0;
		dicObjType.TryGetValue(id,out type);
		return (BCGameObjectType)type;
	}
	/// <summary>
	/// 根据id 获得某个物品数据
	/// </summary>
	/// <returns>The BC object.</returns>
	/// <param name="id">Identifier.</param>
	public BCObj GetBCObj(int id)
	{
		BCObj o = null;
		dicGameObj.TryGetValue(id,out o);
		return o;
	}
	public MDTigerInfo GetTigerInfo(int id)
	{
		MDTigerInfo ti = null;
		dicTigerInfo.TryGetValue(id,out ti);
		return ti;
	}
	/// <summary>
	/// 根据id 获得某个收集品
	/// </summary>
	/// <returns>The collection info.</returns>
	/// <param name="id">Identifier.</param>
	public BCCollectionInfo GetCollectionInfo(int id)
	{
		BCCollectionInfo bc =null;
		dicCollection.TryGetValue(id,out bc);
		return bc;
	}
	public LevelUPReward GetLevelUpReward(int level)
	{
		LevelUPReward lr=null;
		dicLevelUpReward.TryGetValue(level,out lr);
		return lr;
	}
	public WGBearParam GetBearParam(int id)
	{
		WGBearParam bp = null;
		dicBearParam.TryGetValue(id,out bp);
		return bp;
	}
	public int  GetCollectionOwnNum(int id)
	{
		int num = DataPlayerController.getInstance().getCollectionNum(id);

		return num;
	}
	public MDSkill getSkill(int id)
	{
		MDSkill  sk = null;
		dicSkills.TryGetValue(id,out sk);
		return sk;
	}

	public int getTurnRewardIndex(){
		return mTurnRandom.getRandom();
	}

	/// <summary>
	/// 当前等级
	/// </summary>
	/// <returns>数组0:奖励物品的ID,1:物品奖励数量</returns>
	/// <param name="level">Level.</param>
	public int[] GetLevelUPRewardID(int level)
	{

		int[] reward = new int[2];

		int type = 1;
		LevelUPReward lr = dicLevelUpReward[level];

		type = lr.randomRewardType();


		if(type !=-1 && lr.reward.Length>=type)
		{
			reward[1]=lr.reward[type-1];
		}
		else{
			Debug.LogWarning("SONG==>get level("+level+") reward type is -1");
			return null;
		}
			
		if(type == 1){
			reward[0] = WGDefine.CommonCoin;
		}
		else{
			List<BCObj> sztype = dicTypeGameObj[type];
			if(sztype != null && sztype.Count>0)
			{
				int idx = Random.Range(1,sztype.Count);
				reward[0] = sztype[idx].ID;
			}
			else{
				Debug.LogWarning("SONG==>get level=("+level+") reward with type=("+type+")is null");
				return null;
			}
		}

		return reward;
	}
	public void freshUnLockBearWith(int level)
	{

		if(_NextUnLockLV>level){
			
			return;
		}
		dicBearWeight.Clear();
		foreach(KeyValuePair<int,List<WGBearParam>> kv in dicBearUnLock)
		{
			if(level >= kv.Key)
			{
				int num = kv.Value.Count;
				for(int i = 0; i<num; i++)
				{
					if(kv.Value[i].reload_time==0)//需要刷新时间的则不在这里
					{
						if(dicBearWeight.Count == 0)
						{
							dicBearWeight.Add(kv.Value[i].weight,kv.Value[i]);
							_MaxRandomWeight = kv.Value[i].weight;
						}
						else 
						{
							_MaxRandomWeight +=kv.Value[i].weight;
							dicBearWeight.Add(_MaxRandomWeight,kv.Value[i]);
						}
					}
				}
			}
			else
			{
				_NextUnLockLV = kv.Key;
				return;
			}
//			//WG.SLog("------------"+JsonFx.Json.JsonWriter.Serialize(dicBearWeight));
		}
	}
	public List<WGBearParam> getCurrentUnloackBear(int level)
	{
		if(dicBearUnLock.ContainsKey(level))
		{
			return dicBearUnLock[level];
		}
		return null;
	}
	public int GetRandomBearID()
	{
		int weight = Random.Range(0,_MaxRandomWeight);
		foreach(KeyValuePair<int,WGBearParam> kv in dicBearWeight)
		{
			if(kv.Key>=weight)
			{
				return kv.Value.id;
			}
		}
		return 50001;
	}

	IEnumerator LoadCoinData()
	{
//		TextAsset s = Resources.Load(WGConfig.Path_SceneData) as TextAsset;

		ResourceRequest rr = Resources.LoadAsync(WGConfig.Path_SceneData);

		yield return rr;

		TextAsset s = rr.asset as TextAsset;

		string dc = "";
		using(StreamReader sr = new StreamReader(new MemoryStream(s.bytes)))
		{
			dc = sr.ReadToEnd();

			mDataCoin = SDK.Deserialize<DataCoin>(dc);
			
		}

		Resources.UnloadAsset(s);

	}
	public MDShopData getOneSell(int skid,int type)
	{
		for(int i=0,max=szShopData.Count;i<max;i++)
		{
			if((szShopData[i].id == skid)&&(szShopData[i].get_num==1)&&(szShopData[i].cost_type == type))
			{
				return szShopData[i];
			}
			else
			{
				//WG.SLog(SDK.Serialize(szShopData[i]));
			}
		}
		return null;
	}

	public int[] GetCollectionLevel(int num)
	{
		int[] info = new int[2];
		if(num ==0)
		{
			info[0] = 0;
			info[1] = WGDefine.LEVEL1;
		}
		else if(num<WGDefine.LEVEL1) {
			info[0] = 1;
			info[1] = WGDefine.LEVEL1;
		}
		else if(num<WGDefine.LEVEL2) {
			info[0] = 2;
			info[1] = WGDefine.LEVEL2;
		}
		else if(num<WGDefine.LEVEL3) {
			info[0] = 3;
			info[1] = WGDefine.LEVEL3;
		}
		else if(num<WGDefine.LEVEL4) {
			info[0] = 4;
			info[1] = WGDefine.LEVEL4;
		}
		else if(num<WGDefine.LEVEL5){
			info[0] = 5;
			info[1] = WGDefine.LEVEL5;
		}
		else{
			info[0] = 5;
			info[1] = WGDefine.LEVEL5;
		}
		return info;
	}

	public YHMDPayData getYHMDPay(YHPayType type)
	{

		int id = (int)type;
		if(dicYHPayData.ContainsKey(id))
		{
			return dicYHPayData[id];
		}
		return null;
	}

}
