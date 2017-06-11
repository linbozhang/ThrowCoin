using UnityEngine;
using System.Collections;

public class AllView:WGMonoComptent{

	public static AllView self;

	public static bool stShop = false;
	public static bool stItem = false;
	public static bool stAchievment = false;
	public static bool stHelp = false;
	public static bool stLuckReward = false;
	public static bool stRichReward = false;
	public static bool stNewPlayerReward = false;
	public static bool stDailyReward = false;
	public static bool stSelectRole = false;
	public static bool stPayforRole = false;
	public static bool stSuperTiger = false;

	public GameObject goPayforRole=null;
	public GameObject goSelectRole=null;




	void Awake()
	{
		self = this;
	}







}
