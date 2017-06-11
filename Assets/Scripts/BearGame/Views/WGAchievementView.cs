using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WGAchievementView : MDBaseAlertView,WGTableViewDelegate {

	static Object mObj=null;


	[SerializeField]
	protected TweenScale tsContent;
	[SerializeField]
	protected UISprite spGrayBackground;

	public WGTableView tabView;



	int mDataCount;

	List<MDAchievement> szHaveGot = new List<MDAchievement>();
	List<MDAchievement> szReached = new List<MDAchievement>();
	List<MDAchievement> szNoReached = new List<MDAchievement>();
	void viewDidLoad ()
	{
		mDataCount = WGDataController.Instance.szAchievement.Count;
		tabView.csDelegate = this;
		ReloadAchievement();
	}
	#region MDBaseAlertView
	public override void showView ()
	{
		viewDidLoad();
		base.showView ();
		Time.timeScale = 0;

		tsContent.transform.localScale =  tsContent.from;
		tsContent.ESetActive(true);
		tsContent.PlayForward();
		InvokeBlock(tsContent.duration,()=>{
			showViewEnd();
		});

	}
	public override void showViewEnd ()
	{
		base.showViewEnd ();
	}
	public override void hiddenView ()
	{
		base.hiddenView ();
		Time.timeScale = 1;
		tsContent.PlayReverse();
		InvokeBlock(tsContent.duration,()=>{
			hiddenViewEnd();
		});
	}
	public override void hiddenViewEnd ()
	{
		base.hiddenViewEnd ();
	}

	#endregion
	void freshData()
	{
		szHaveGot.Clear();
		szReached.Clear();
		szNoReached.Clear();
		DataPlayer _dp = DataPlayerController.getInstance().data;

		for(int i=0,max=WGDataController.Instance.szAchievement.Count;i<max;i++)
		{
			MDAchievement ach = WGDataController.Instance.szAchievement[i];
			int reachNum =WGAchievementManager.Self.getAchievementProgress(ach);

			if(_dp.dicGotAchReward.ContainsKey(ach.id))
			{

				if(_dp.dicGotAchReward[ach.id] == 1)
				{
					szReached.Add(ach);
				}
				else if(_dp.dicGotAchReward[ach.id] == -1)
				{
					szHaveGot.Add(ach);
				}
				else{
					if(reachNum>=ach.goal_num)
					{
						_dp.dicGotAchReward[ach.id] = 1;
						szReached.Add(ach);
					}
					else
					{
						_dp.dicGotAchReward[ach.id] = 0;
						szNoReached.Add(ach);
					}
				}
			}
			else
			{

				if(reachNum >=ach.goal_num)
				{
					_dp.dicGotAchReward.Add(ach.id,1);
					szReached.Add(ach);
				}
				else
				{
					_dp.dicGotAchReward.Add(ach.id,0);
					szNoReached.Add(ach);
				}
			}

		}


		WGAchievementManager.Self.bCompleteAchievement = szReached.Count>0;
	}
	public void ReloadAchievement()
	{
		freshData();
		tabView.AllReset();
		tabView.reloadData();
	}




	public WGTableViewCell WGTableViewCellWithIndex(WGTableView scrView, int index){

		WGTableViewCell cell = scrView.getCacheCellsWithIdentifier(1);

		CAchievementCell tCell ;
		if(cell == null)
		{
			tCell = CAchievementCell.CreateAchievementCell();
			cell = tCell as WGTableViewCell;
			cell.identifier = 1;
			tCell.curAchViewManager = this;
		}
		else{
			tCell = cell as CAchievementCell;
		}
		tCell.index = index;
		MDAchievement ach;
		if(index<szReached.Count)
		{
			ach =szReached[index]; 
		}
		else if(index<szReached.Count+szNoReached.Count)
		{
			ach = szNoReached[index-szReached.Count];
		}
		else{
			ach = szHaveGot[index-szReached.Count-szNoReached.Count];
		}
		tCell.freshUIWithData(ach);

		return cell;

	}
	//	获取cell的数目,应该是总行数*总列数
	public int NumberOfTableViewCells(){

		return mDataCount;
	}
	//	选择一个cell对象
	public void SelectTableViewCell(WGTableViewCell aCell,int index){
		Debug.Log("selectScrollViewCellAndIndex=="+index);
	}
	//	长按一个cell对象
	public void HoldTableViewCell(WGTableViewCell aCell,int index){
		Debug.Log("holdScrollViewCellAndIndex=="+index);
	}
	//	刷新当前在视图内能展示的Cell 需要调用SQYScrollView.UpdataCells()函数
	public void UpdateShowTableViewCells(WGTableViewCell aCell,int index){
	}

	public static WGAchievementView CreateAchievementView()
	{
		if(mObj == null)
		{
			mObj = Resources.Load("pbWGAchievementView");
		}

		if(mObj != null)
		{
			GameObject go = Instantiate(mObj) as GameObject;
			WGAchievementView av = go.GetComponent<WGAchievementView>();
			return av;
		}
		return null;
	}
}






























