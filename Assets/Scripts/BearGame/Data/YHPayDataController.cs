using UnityEngine;
using System.Collections;


public class YHMDPayData{
	public int id;
	public int payKey;
	public string name;
	public float payCost;
	public int repeat;
	public string showText; //玩家购买时显示的名字
}
public enum YHPayType{
	NEW_Player=1,//新手礼包
	LUCKY = 2,//幸运礼包
	RICK = 3,//土豪礼包
	CHEAP = 4,//超值礼包
	COIN = 5,//金币礼包
	ITEM = 6,//道具补充
	JEWEL60 = 7,//60钻石
	JEWEL100 = 8,
	JEWEL200 = 9,
	JEWEL300 = 10,
	JEWEL400 = 11,
	POWER = 12,//狂暴解锁
	TIGER10 = 13,//十连抽
	DOUBLE_reward = 14,//双倍奖励
	FINGER=15,
	ROLE3in1 = 16,//角色3合1
}

public class YHPayDataController{


	public static int getOkTextID(YHPayType type)
	{
		int okid = 1094;
	#if YES_OK
			okid = 1002;
	#elif YES_BUY
			okid = 1094;
	#elif YES_GET
			okid = 1077;
	#elif YES_QueRen
		okid = 1106;
	#endif
		return okid;
	}
	public static int getDoubleOkTextID(YHPayType type)
	{
		int okid = 1104;
	#if YES_OK
		okid = 1104;
	#elif YES_BUY
		okid = 1104;
#elif YES_GET
		okid = 1101;
		#elif YES_QueRen
		okid = 1106;
	#endif
		return okid;
	}


}



