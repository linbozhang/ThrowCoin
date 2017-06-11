using UnityEngine;
using System.Collections;

public class D0331SuperRewardCell : MonoBehaviour {
	
	public UILabel labName;
	public UISprite spRewardIcon;
	// Use this for initialization
	void Start () {
	
	}


	public static D0331SuperRewardCell CreateSuperTigerCellView()
	{
		Object obj = Resources.Load("pbD0331SuperRewardCell");
		if(obj != null)
		{
			GameObject go  = Instantiate(obj) as GameObject;
			D0331SuperRewardCell dc = go.GetComponent<D0331SuperRewardCell>();
			return dc;
		}

		return null;
	}

}
