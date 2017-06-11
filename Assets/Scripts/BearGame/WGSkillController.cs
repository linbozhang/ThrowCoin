using UnityEngine;
using System.Collections;

public class WGSkillController : MonoBehaviour {


	public static WGSkillController Instance;

	void Awake()
	{
		Instance = this;
	}



	public void ReleaseSkillWithID(int id)
	{
		DataPlayer _dp = DataPlayerController.getInstance().data;

		MDSkill sk = WGDataController.Instance.getSkill(id);
		//Debug.Log("SkillId:"+id);
		switch(id)
		{
		case WGDefine.SK_YunShi:
			WGGameWorld.Instance.BearCoinShaking(sk.time,WGDefine.SK_YunShi);
			break;
		case WGDefine.SK_ChangShe50:
			WGBearManage.Instance.Long50();
			break;
		case WGDefine.SK_ChangShe100:
			BCSoundPlayer.Play(MusicEnum.daoju4);
			WGBearManage.Instance.Long100();
			break;
		case WGDefine.SK_FangYu1:
			WGBearManage.Instance.BearTreeUp(sk.time,true);
			break;
		case WGDefine.SK_FangYu4:
			BCSoundPlayer.Play(MusicEnum.daoju3);
			WGBearManage.Instance.BearTreeUp(sk.time);
			break;
		case WGDefine.SK_BingXue:
			WGBearManage.Instance.ChangeTableMoca();
			break;
		case WGDefine.SK_DiZhen:
			BCSoundPlayer.Play(MusicEnum.daoju2);
			WGGameWorld.Instance.BearCoinShaking(sk.time,WGDefine.SK_DiZhen);
			break;
		case WGDefine.SK_JianSu:
			BCSoundPlayer.Play(MusicEnum.daoju1);
			WGBearManage.Instance.DecelerationTurn(sk.time);
			break;
		case WGDefine.SK_HaiXiao:
			WGBearManage.Instance.DonNotNeedCoin(sk.time);
			break;
		case WGDefine.SK_Defense10M:
		case WGDefine.SK_Defense30M:
			WGBearManage.Instance.BearTreeUp(sk.time,true);
			break;
		case WGDefine.SK_777Up1:

			break;
		case WGDefine.SK_777Up2:

			break;
		case WGDefine.SK_GuDing30:

			WGBearManage.Instance.csThrow.CheckStaticWeapon();
			break;
		case WGDefine.SK_GuDing:

			WGBearManage.Instance.csThrow.CheckStaticWeapon();
			break;
		}
	}

}
