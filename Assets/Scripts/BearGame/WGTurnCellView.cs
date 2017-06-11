using UnityEngine;
using System.Collections;

public class WGTurnCellView : MonoBehaviour {

	public UILabel labNum;
	public UISprite spIcon;

	static Object obj=null;


	public void freshCellView(MDReward tr)
	{
		labNum.text = tr.num.ToString();
		BCObj obj = WGDataController.Instance.GetBCObj(tr.id);
		if(obj !=null)
		{
//			spIcon.spriteName = obj.Icon;
		}
	}

	public static WGTurnCellView CreateCellView()
	{
		if(obj == null)
		{
			obj = Resources.Load("pbSlot");
		}
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			WGTurnCellView cv = go.GetComponent<WGTurnCellView>();
			return cv;
		}

		return null;
	}
}
