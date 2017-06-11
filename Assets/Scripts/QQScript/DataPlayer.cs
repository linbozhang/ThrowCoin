using System;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

[Serializable]
public class DataPlayer
{




	//ID、名称、等级、经验、在线时间、金币数、金币花费、金币恢复时间、钻石数、钻石花费、主线任务进度、每日任务进度、随机任务进度
	public bool IsFirstLogin = true;

	public int Level = 1;
	
	public int Exp = 0;
	/// <summary>
	/// 是否购买删除广告的功能
	/// </summary>
	public int DelAD = 0;

	public int CoinUsed = 5;
	/// <summary>
	/// 玩家拥有的收集品的数量
	/// </summary>
	public Dictionary<int,int> dicCollectionNum= new Dictionary<int, int>();

	public List<int> szPayObjID=new List<int>();
	/// <summary>
	/// 能量值,超过一个特定的数,就会触发 狂暴技能(发射子弹不花钱)
	/// </summary>
	public int mEnergy =0;
	/// <summary>
	/// 日期
	/// </summary>
	public DateTime MyData;
	/// <summary>
	/// 防御技能的时间
	/// </summary>
	public int defenseTime =0;
	/// <summary>
	/// 老虎机77的概率变大
	/// </summary>
	public int up777Time = 0;
	/// <summary>
	/// 购买之后为一个时间
	/// </summary>
	public int guDingTime = 0;
	/// <summary>
	/// 购买之后为1
	/// </summary>
	public int guDingForever =0;
	/// <summary>
	/// 是否释放固定防御技能
	/// </summary>
	public int releaseGuding=0;

	public DateTime lastDailyRewardTime = DateTime.MinValue;

	public int ContinuousDay =0;
	/// <summary>
	/// 1是结束,0是没有结束
	/// </summary>
	public int mHelpEnd = 0;
	/// <summary>
	/// 是否购买了角色系统的 0:是没有购买   1:购买了.
	/// </summary>
	public int mR=0;
	/// <summary>
	/// 角色系统.购买的时候选择的那个角色 0,1,2
	/// </summary>
	public int r=1;
	/// <summary>
	/// 超级抽奖的金币购买次数,超级抽奖的金币会随着购买变大,花费也越来越多!
	/// </summary>
	public int cj=0;
	/// <summary>
	/// 是否购买了狂暴技能
	/// </summary>
	public int a1=0;

	//1:new user da li bao
	//2:luck da li bao
	//3:tuhao da li bao
	//4:chaozhi da li bao
	public List<int> szBigReward=new List<int>();

	public Dictionary<int,int> dicHelp = new Dictionary<int, int>();
    //震动1,长舌50,长舌100,防御1,防御2,轮盘,冰雪,地震2,减速,海啸 
//	public int[] szSkills = new int[Consts.SKILLNUM];
	public Dictionary<string,int> dicSkills = new Dictionary<string, int>();
	/// <summary>
	/// key:AchievementID
	/// value:完成的数量
	/// </summary>
	public Dictionary<int,int> dicReachAchs = new Dictionary<int, int>();
	/// <summary>
	/// 只用来记录 不能重复的物品的ID的
	/// </summary>
	public Dictionary<int,int> dicAchsNums = new Dictionary<int, int>();
	/// <summary>
	/// 已经领取的成就的奖励! 1:reached 0:noReach -1:have got
	/// </summary>
	public Dictionary<int,int> dicGotAchReward = new Dictionary<int, int>();
	public Dictionary<int,int> dicLrState = new Dictionary<int, int>();
	public DataPlayer ()
	{
		//AchievementCom = new List<int>();
		dicSkills.Add(WGDefine.SK_ChangShe100.ToString(),5);
		dicSkills.Add(WGDefine.SK_FangYu4.ToString(),5);
		dicSkills.Add(WGDefine.SK_DiZhen.ToString(),5);
		dicSkills.Add(WGDefine.SK_JianSu.ToString(),5);
	}

	public float _jewel = 0.03f;
    public int Jewel
    {
        get
        {
			return Mathf.FloorToInt(_jewel);;
        }
        set
        {
			_jewel = value+UnityEngine.Random.Range(0.1f,0.99f);
        }
    }

	public float _Coin = 5000.123f;
	public int Coin
	{
		get
		{
			int coin = Mathf.FloorToInt(_Coin);

			return coin;
		}
		set
		{

			float p = UnityEngine.Random.Range(0.01f,0.5f);

			_Coin = value+p;

		}
	}

}

public class DataPlayerController{
	
	private static DataPlayerController dataCtrl;
	private DataPlayerController(){
	}
	public static DataPlayerController getInstance(){
		if(dataCtrl == null)
			dataCtrl = new DataPlayerController();
		return dataCtrl;
	}

	private DataPlayer _data;
	public DataPlayer data{
		get{
			if(_data == null){
				_data = this.loadDataPlayer();

				if(_data == null||_data.IsFirstLogin){

					SharedPrefs prefs = SharedPrefs.getInstance();
					prefs.createAccountFolder();
					_data = new DataPlayer();
					_data.IsFirstLogin = false;
					_data.MyData = DateTime.Now;
#if TalkingData
					TDGAVirtualCurrency.OnReward(_data.Jewel,"Init");
#endif
					this.saveDataPlayer();
				}
			}

			return _data;
		}
	}
	/// <summary>
	/// 之前的功能,充值玩家数据,
	/// </summary>
	public void resetDataPlayer()
	{
		_data = new DataPlayer();
		_data.IsFirstLogin = false;

		this.saveDataPlayer();
	}
	public void saveDataPlayer(){
		SharedPrefs prefs = SharedPrefs.getInstance();
		data.MyData = Core.now;

//		//WG.SLog("saveDataPlayer"+SDK.Serialize(data));
		prefs.saveValue(data ,SharedPrefs.TYPE_Player,Consts.AES_ENCRYPT);
	}

	private DataPlayer loadDataPlayer(){
		SharedPrefs prefs = SharedPrefs.getInstance ();
		DataPlayer data = prefs.loadPlayer(Consts.AES_ENCRYPT);
//		//WG.SLog("loadDataPlayer"+SDK.Serialize(data));
		return data;
	}

	#region DataPlayer
	public int getCollectionNum(int id)
	{
		int num = 0;
		if(data.dicCollectionNum.TryGetValue(id,out num))
		{

		}
		else{
			data.dicCollectionNum.Add(id,0);
		}
		return num;
	}
	public void addCollectionNum(int id,int addNum)
	{
		int num = 0;
		if(data.dicCollectionNum.TryGetValue(id,out num))
		{

			data.dicCollectionNum[id] = num+addNum;

		}
		else{
			data.dicCollectionNum.Add(id,addNum);
		}
	}

	public int getSkillNum(int id)
	{
		int num = 0;
		if(data.dicSkills.ContainsKey(id.ToString()))
		{
			num = data.dicSkills[id.ToString()];
		}
		else{

			data.dicSkills.Add(id.ToString(),0);
		}
		return num;
	}
	public void setSkillNum(int id,int num)
	{
		if(data.dicSkills.ContainsKey(id.ToString()))
		{
			data.dicSkills[id.ToString()]=num;
		}
		else{
			data.dicSkills.Add(id.ToString(),num);
		}
	}
	public void AddSkillNum(int id,int addNum)
	{
		if(data.dicSkills.ContainsKey(id.ToString()))
		{
			data.dicSkills[id.ToString()] = data.dicSkills[id.ToString()]+addNum;
		}
		else{
			data.dicSkills.Add(id.ToString(),addNum);
		}
	}
	#endregion
}