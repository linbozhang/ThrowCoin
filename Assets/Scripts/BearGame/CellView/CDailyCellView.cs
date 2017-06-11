using UnityEngine;
using System.Collections;

public class CDailyCellView : MonoBehaviour {
	public UISprite spBackground;
	public UILabel labGotNum;
	public UILabel labDaily;
	public UISprite spRewardIcon;
	public UISprite spCanGet; 

	// Use this for initialization
	void Start () {
	
	}

	public void freshDailyCell(MDDailyReward dr, int day,int curDay)
	{

		labGotNum.text = dr.got_num.ToString();
		spRewardIcon.spriteName = dr.icon;
		spRewardIcon.MakePixelPerfect();
		labDaily.text = dr.day;

		if(curDay>day)
		{
			spCanGet.ESetActive(true);
			spRewardIcon.color = Color.white;
		}
		else if(curDay == day)
		{
			spRewardIcon.color = Color.white;
			spCanGet.ESetActive(false);
		}
		else
		{
			spRewardIcon.color = Color.gray;
			spCanGet.ESetActive(false);
		}


		
	}


	static Object mObj = null;
	public static CDailyCellView CreateCellView(int day)
	{
		if(mObj == null)
		{

			mObj = Resources.Load("pbWGDailyCellView");
			
		}

		GameObject go = Instantiate(mObj) as GameObject;

		CDailyCellView cv = go.GetComponent<CDailyCellView>();

		return cv;

	}

}
