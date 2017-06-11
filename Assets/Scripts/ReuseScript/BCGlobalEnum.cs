using UnityEngine;
using System.Collections;
using System.Collections.Generic;




	
public enum BCGameObjectType
{
	UnKnow,
	Coin=1,
	Item=2,
	Collection=3,
	Jewel=4,
	Bear=5,
	ExpCoin=6,
	Pack=7,
}

public enum AchievementType
{
	Coin = 1,
	Collecton,
	CollectionLis,
	FailCoin,
	ItemCoin,
	Combo,
	CoinMiddle,
	CoinHight,
	KillBear2,
	KillBear3,
	KillBear4,
}


public enum ComboType
{
	Nice =1,
	Good,
	Cool,
	Great,
	Perfect,
}


public class CoinComboParameter
{
	public int ItemID;
	public int Num;
	public int Weight;
	public CoinComboParameter(){}
}
public class Combo
{
	public int ID;
	public string Name;
	public List<CoinComboParameter>  ParaArray;
	public Combo(ComboJson c)
	{
		ID = c.ID;
		Name = c.Name;
		ParaArray = new List<CoinComboParameter>();
		string tem = "";
		bool needChange = false;
		CoinComboParameter cc = new CoinComboParameter();
		int num = c.Para.Length;
		for(int i = 0; i<num;i++)
		{
			string str = c.Para.Substring(i,1);
			if(str.Equals(";"))
			{
				needChange = true;
				cc.ItemID = int.Parse(tem);
			}
			if(str.Equals("@"))
			{
				needChange = true;
				cc.Num = int.Parse(tem);
			}
			if(str.Equals("|"))
			{
				needChange = true;
				cc.Weight = int.Parse(tem);
				ParaArray.Add(cc);
//				Debug.Log("++++++++++ID"+cc.ItemID+"++++++++++++++++Num"+cc.Num+"+++++++++++++++Weight"+cc.Weight);
				cc = new CoinComboParameter();
			}
			if(needChange)
			{
				tem = "";
				needChange = false;
			}
			else
			{
				tem+=str;
			}
			if(i == num-1)
			{
				needChange = true;
				cc.Weight = int.Parse(tem);
				ParaArray.Add(cc);
//				Debug.Log("++++++++++ID"+cc.ItemID+"++++++++++++++++Num"+cc.Num+"+++++++++++++++Weight"+cc.Weight);
			}
		}
	}
		
}
public class ComboJson
{
	public int ID;
	public string Name;
	public string Para;
}



public class LevelUPReward
{
	public int level;//等级
	public int exp;//到达这个等级的经验
	public int[] reward;//获得奖励的个数
	public int[] weight;//升级奖励类别权重 0:金币 1:卡片 2:收集品
	public List<int[]> special;//特殊奖励 0:物品id 1:数量

	public List<int[]> drop_reward;

	public LevelUPReward(){}

	public void refreshWeight()
	{
		if(weight!=null && weight.Length>1)
		{
			for(int i=0;i<weight.Length-1;i++)
			{
				weight[i+1] +=weight[i]; 
			}
		}
	}
	public int randomRewardType()
	{
		if(weight != null&&weight.Length>0)
		{
			int w = Random.Range(1,weight[weight.Length-1]);

			for(int i=0;i<weight.Length;i++)
			{
				if(w<=weight[i])return i+1;
			}
		}
		return -1;
	}
}
public class TigerInfo
{
	public int ID;
	
	public int Weight;
	
	public List<int> ShowIndex;
	
	public int RewardType;
	
	public int Parameter1;
	
	public int Parameter2;

}
public class MDTiger
{
	public int ID;
	
	public int Weight;
	
	public List<int> ShowIndex;
	
	public int RewardType;

	public List<int[]> reward;
}


public class Achievementinfo
{
	public int ID;//
	public int PreID;//
	public int NextID;//
	public int Type;//
	public string Name;//
	public string Description;//
	public int Goal;//
	public int Num;//
	public int Reward;//
	public int Rewardnum;//
}

