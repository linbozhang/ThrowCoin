using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WGAchievementManager : MonoBehaviour {


	Dictionary<int,List<MDAchievement>> dicAchievement;
	WGDataController _dataCtrl;
	DataPlayer _dp{
		get{
			return DataPlayerController.getInstance().data;
		}
	}

	bool _bCompleteAchievement = false;
	public bool bCompleteAchievement {
		get{return _bCompleteAchievement;}
		set{

			if(_bCompleteAchievement != value)
			{
				_bCompleteAchievement = value;
				WGGameUIView.Instance.freshMenuButton(1);
			}
			else
			{
				_bCompleteAchievement = value;
			}
		}
	}
	float lastChangeTime = 0;
	bool hasChanged = false;
	public static WGAchievementManager Self;
	void Awake()
	{
		Self = this;
	}
	// Use this for initialization
	void Start () {

		_dataCtrl = WGDataController.Instance;
		dicAchievement = _dataCtrl.dicGoalAchieve;
		lastChangeTime = Time.realtimeSinceStartup;
		InvokeRepeating("AutoSave",2,40);
	}

	void AutoSave()
	{
		if(hasChanged && Time.realtimeSinceStartup-lastChangeTime>15)
		{
			lastChangeTime = Time.realtimeSinceStartup;
			hasChanged = false;
			DataPlayerController.getInstance().saveDataPlayer();
		}
	}

	void AchievementComplete(MDAchievement ach)
	{
//		bCompleteAchievement = true;
		//WG.SLog("AchievementComplete==="+SDK.Serialize(ach));
		WGAlertViewController.Self.showArchivementTipView(ach.name,ach.icon);
		#if TalkingData
		TDGAMission.OnBegin(ach.name+ach.id);
		TDGAMission.OnCompleted(ach.name+ach.id);
		#endif
	}
	public void processAchievement(int goal,int type,int value =1)
	{
		if(dicAchievement.ContainsKey(goal))
		{
			List<MDAchievement> szAch = dicAchievement[goal];
			for(int i=0,max = szAch.Count;i<max;i++)
			{
				MDAchievement ach = szAch[i];
				if(ach.type == type)
				{
					if(getAchievementProgress(ach)>ach.goal_num)
					{

						if(_dp.dicGotAchReward.ContainsKey(ach.id))
						{

							if(_dp.dicGotAchReward[ach.id] != -1)
							{
								bCompleteAchievement = true;
								if(_dp.dicGotAchReward[ach.id] == 0)
								{
									AchievementComplete(ach);
									_dp.dicGotAchReward[ach.id] = 1;
								}
							}


						}
						else
						{
							//WG.SLog("====getAchievementProgress===>goal_num"+SDK.Serialize(ach));

							bCompleteAchievement = true;
							_dp.dicGotAchReward.Add(ach.id,1);
							AchievementComplete(ach);
						}

					}
					lastChangeTime = Time.realtimeSinceStartup;
					hasChanged = true;
					if(ach.copy == 1)
					{
						if(_dp.dicReachAchs.ContainsKey(ach.id))
						{
							_dp.dicReachAchs[ach.id]+=value;
						}
						else{
							_dp.dicReachAchs.Add(ach.id,value);
						}
					}
					else
					{
						if(_dp.dicAchsNums.ContainsKey(goal))
						{
							_dp.dicAchsNums[goal]+=value;
						}
						else{
							_dp.dicAchsNums.Add(goal,value);
						}
					}
				}
			}
		}
	}
	public int getAchievementProgress(MDAchievement ach)
	{
		int progress = 0;
		if(ach.copy == 1)
		{
			if(_dp.dicReachAchs.ContainsKey(ach.id))
			{
				progress = _dp.dicReachAchs[ach.id];
			}
		}
		else if(ach.copy == 0)
		{

			for(int i=0,max = ach.goals.Count;i<max;i++)
			{
				if(progress>0)
				{
					if(_dp.dicAchsNums.ContainsKey(ach.goals[i]))
					{
						progress = Mathf.Min(_dp.dicAchsNums[ach.goals[i]],progress);
					}
				}
				else
				{
					if(_dp.dicAchsNums.ContainsKey(ach.goals[i]))
					{
						progress = _dp.dicAchsNums[ach.goals[i]];
					}
				}

			}
		}

		return progress;
	}

}
