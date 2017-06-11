using UnityEngine;
using System.Collections;

public class HelpView7 : MonoBehaviour {


	public UILabel labDes2003;
	public UILabel labDes2005;
	public UILabel labDes2007;
	public UILabel labDes2008;

	// Use this for initialization
	void Start () {
		MDSkill sk2003 = WGDataController.Instance.getSkill(WGDefine.SK_ChangShe100);
		MDSkill sk2005 = WGDataController.Instance.getSkill(WGDefine.SK_FangYu4);
		MDSkill sk2007 = WGDataController.Instance.getSkill(WGDefine.SK_DiZhen);
		MDSkill sk2008 = WGDataController.Instance.getSkill(WGDefine.SK_JianSu);

		labDes2003.text = sk2003.des;
		labDes2005.text = sk2005.des;
		labDes2007.text = sk2007.des;
		labDes2008.text = sk2008.des;

	}
	

}
