using UnityEngine;
using System.Collections;

public class WGWeaponView : MonoBehaviour {

	public GameObject goContent;
	public Vector3 v3ADPos;
	public Vector3 v3Pos;

	WGBearManage _bearManage;

	// Use this for initialization
	void Start () {
		_bearManage = WGBearManage.Instance;
		if(DataPlayerController.getInstance().data.DelAD == 0)
		{
			#if Add_AD
			goContent.transform.localPosition = v3ADPos;
			#else
			goContent.transform.localPosition = v3Pos;
			#endif
		}
		else
		{
			goContent.transform.localPosition = v3Pos;
		}
	}

	public void RemoveAD()
	{
		goContent.transform.localPosition = v3Pos;
	}
	public void ResetAD()
	{
		#if Add_AD
		goContent.transform.localPosition = v3ADPos;
		#else
		goContent.transform.localPosition = v3Pos;
		#endif
	}
	void OnBtnLeft()
	{
		_bearManage.csThrow.ChangeWeaponAdd();
	}
	void OnBtnRight()
	{
		_bearManage.csThrow.ChangeWeaponSub();
	}

}
