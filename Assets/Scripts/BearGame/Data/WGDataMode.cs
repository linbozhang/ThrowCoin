using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class BCObj
{
	public int ID;//物品ID

	public string Name;//物品游戏名称

	public int Type;//物品类型

	public string Res;//物品资源名称 金币类型为纹理名

//	public string Icon;//物品图标UI名称

	public int Exp;//物品经验

	public int Value;//物品价值

	public int Level;//物品的解锁等级



	public GameObject goRes = null;//后期的加载的数据

	public BCObj()
	{
//		Icon = "";
	}

	public BCGameObjectType BCType{
		get{
			return (BCGameObjectType)Type;
		}
	}
}

public class BCCollectionInfo
{
	public int ID;

	public string description;

	public string MaskUISprite;

	public int groupID;
	//出售的基数
	public int sell_num;
	//出售的倍数
	public float sell_factor;

	public string groupdes;


	public BCCollectionInfo(){}
	public BCCollectionInfo(int id)
	{
		ID = id;
		description = "";
		MaskUISprite = "";
		groupID =0;
	}
}
public class MDWeapon{
	public int id;
	public string name;
	public int hurt;
	public float reload;
	public int cost;
	public int oid;
}

public class WGBearReward{
	public int type;
	public int id;
	public int num;
}

public class MDShopData{
	public int idx;
	public int id;
	public int sid;
	public int cost_num;
	public int get_num;
	public string icon;
	public int type;
	public string proid;
	public string proid2;
	public string des;
	public int cost_type;//花费类型 0:coin 1:jewel 2:rmb
	public string pdes;

	public const int ITEM = 0;
	public const int COIN = 1;
	public const int JEWEL = 2;
}



public class WGBearParam{
	public int id;
	public int hp;
	public int unlock;//解锁等级
	public int weight;//怪物的随机权重
	/// <summary>
	/// 死亡之后的掉落
	/// </summary>
	public List<int[]>death_reward;
	/// <summary>
	/// 秒杀之后的额外奖励
	/// </summary>
	public List<int[]> additional;

	/// <summary>
	/// 如果是非0的,开始游戏之后的这个时间 出现,
	/// </summary>
	/// 
	public int reload_time;
	/// <summary>
	/// 消失时间.如果这个时间内,没有被打死,则消失,reload_time之后再次出现
	/// </summary>
	public int fresh_time;
	/// <summary>
	/// 能量值,打死他会积累他的能量值
	/// </summary>
	public int energy;

	public string des;
	//是否可以触发老虎机
	public int tiger;
}

public class MDSkill{
	public int id;
	public string name;
	public string des;
	public float time;
//	public int type;
	public string icon;
	public string paramater;
}

public class MDRewardPackage{
	public int  id;
	public string name;
	public List<int[]> reward;
}

public class MDReward{
	public int id;
	public int num;
	public MDReward(int[] list){
		if(list.Length>=2)
		{
			id = list[0];
			num = list[1];
		}
	}
}

public class YHDLevelReward{
	//0 0:免费领取所有奖励,只有领取按钮 
	//1:支付领取所有奖励,点击取消,不购买,并关闭,
	//2:免费领取前2个奖励,支付可以领取全部奖励,
	//点击领取->支付并领取全部奖励->关闭弹窗,
	//点击取消->领取前2个按钮,并关闭弹窗;
	public int type;
	public List<int[]>reward;
	public int level;
}

public class HKReward{
	public int id;


	public HKReward(int id)
	{
		this.id = id;
	}
}

public class MDDailyReward{
	public string icon;
	public int reward;
	public string day;
	public int got_num;
}


public class WGRandom{
	public List<int> szWeight;
	public int max;

	public WGRandom(){
		max =0;
		szWeight = new List<int>();
	}
	public void Add(int weight)
	{

		if(szWeight.Count == 0)
		{
			max = weight;
			szWeight.Add(weight);
		}
		else{
			max +=weight;
			szWeight.Add(max);
		}
	}

	public int getRandom()
	{
		int weight = UnityEngine.Random.Range(0,max);
		int count = szWeight.Count;
		for(int i=0;i<count;i++)
		{
			if(weight<szWeight[i])
			{
				return i;
			}
		}
		return 0;
	}

}
[System.Serializable]
public class WGPanelButton{
	public UIButton button;
	public UILabel title;
	public UISprite background;
	public UISprite frontground;
}

public class MDAchievement
{
	public int id;
	public string name;
	public List<int> goals;
	public int type;
	public int goal_num;
	public List<int> reward;
	public string des;
	public List<int> pramater;
	public int copy;//是否需要goals中每一个都到达goal_num;
	public string icon;
}
public class DTAchievementType
{
//	1	总共获得多少金币数
//	2	掉落失去
//	3	掉落多少物品
//	4	击中所有的怪物次数
//	5	击中特定的怪物次数
//	6	击杀所有的怪物次数
//	7	击杀特定的怪物次数
//	8	老虎机转到特定的次数
//	9	使用技能次数
//	10	掉了收集品族的数量
//	11	获得多少个收集品
	public const int GOT_COIN	= 1;
	public const int LOST_OBJ 	= 2;
	public const int DROP_OBJ 	= 3;
	public const int HIT_ALL 	= 4;
	public const int HIT_ONE 	= 5;
	public const int KILL_ALL	= 6;
	public const int KILL_ONE = 7;
	public const int TIGER_ID = 8;
	public const int USE_SKILL = 9;
	public const int GOT_ITEM_GROUP = 10;
	public const int GOT_ITEM	=11;
}
public class MDReachAchs
{
	public int id;
	public bool got;
	public List<int> goals;

}
public class MDTigerInfo
{
	public int id;
	public string name;
	public int index;
}

public class MDType
{
	public int ID;
	public int Type;
}
public class MDRole
{
	public float attack;//攻击倍数(还需要再除100 即 攻击倍数*攻击/100)
	public int max;//最大扔出金币数量
	public int add;//额外增加多少
	public float attV;//攻击的显示进度条是多少,只是在选择角色的时候 显示用的
	public float maxV;//最大扔出金币的进度条是值,只是在选择角色的时候显示扔出金币的进度条用的.
	public float addV;//增加多少额外的金币数量.只是在选择角色的时候显示额外获得金币数量的用的
}
public class YHMDAllReward
{
	public List<int[]> newuser;
	public List<int[]> lucky;
	public List<int[]> rich;
	public List<int[]> cheap;
	public List<int[]> item;
	public List<int[]> coin;
	public List<int[]> jewel;
}


public class WGDefine
{
	public const int CommonCoin = 1001; //普通币
	public const int Value2Coin = 1002; //2倍金币
	public const int Value3Coin = 1003; //3倍金币
	public const int Value5Coin = 1004; //5倍金币
	public const int Value10Coin = 1005; //10倍金币
	public const int Value100Coin = 1006; //100倍大金币
	public const int Item_Exp15Coin = 1007; //15经验币
	public const int Item_Exp30Coin = 1008; //30经验币
	public const int Item_Exp50Coin = 1009; //50经验币

	public const int BossID = 52001;//boss
	public const int PayBear1 = 51001;
	public const int PayBear2 = 51002;


	public const int SK_YunShi			= 2001;
	public const int SK_ChangShe50 		= 2002;
	public const int SK_ChangShe100		= 2003;//4
	public const int SK_FangYu1			= 2004;
	public const int SK_FangYu4			= 2005;//3
	public const int SK_BingXue			= 2006;
	public const int SK_DiZhen			= 2007;//2
	public const int SK_JianSu			= 2008;//1
	public const int SK_HaiXiao			= 2009;
	public const int SK_Defense10M		= 2010;
	public const int SK_Defense30M		= 2011;
	public const int SK_777Up1			= 2012;
	public const int SK_777Up2			= 2013;
	public const int SK_GuDing30		= 2014;
	public const int SK_GuDing			= 2015;

	public const int SPIDER  = 2; //被蜘蛛放下的金币

	public const int LEVEL1 = 5; //收藏品等级1的数量

	public const int LEVEL2 = 15;//收藏品等级2的数量

	public const int LEVEL3 = 35;//收藏品等级3的数量

	public const int LEVEL4 = 75;//收藏品等级4的数量

	public const int LEVEL5 = 135;//收藏品等级5的数量


}