using UnityEngine;
using System.Collections;

public class YHGotRewardCell : MonoBehaviour {

	public UISprite spBackground;
	public UILabel labGotNum;
	public UISprite spRewardIcon;

	public UILabel labX2;




	public void freshDailyCell(int rewardID,int num,bool bDouble=false)
	{

		labX2.ESetActive(bDouble);

		BCGameObjectType type = WGDataController.Instance.GetBCObjType(rewardID);
		if(rewardID == 1001)
		{
			spRewardIcon.spriteName = "item1001";
//			spRewardIcon.transform.localScale = Vector3.one*0.85f;
		}
		else if(rewardID == 4001)
		{
			spRewardIcon.spriteName = "item4001";
//			spRewardIcon.MakePixelPerfect();
//			spRewardIcon.transform.localScale = Vector3.one*0.85f;
		}
		else if(type == BCGameObjectType.Item)
		{
			spRewardIcon.spriteName = "sb1"+rewardID;
//			spRewardIcon.transform.localScale = Vector3.one;
		}
		labGotNum.text = num.ToString();

	}


	static Object mObj = null;
	public static YHGotRewardCell CreateCellView()
	{
		if(mObj == null)
		{

			mObj = Resources.Load("pbYHGotRewardCell");

		}

		GameObject go = Instantiate(mObj) as GameObject;

		YHGotRewardCell cv = go.GetComponent<YHGotRewardCell>();

		return cv;

	}

}
