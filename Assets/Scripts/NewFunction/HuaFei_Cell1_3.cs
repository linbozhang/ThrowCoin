using UnityEngine;
using System.Collections;

public class HuaFei_Cell1_3 : MonoBehaviour {
	public UILabel labColNum;
	public UISprite spColletion;

	public void freshUI(int id)
	{
		spColletion.spriteName = "s"+id.ToString();
		DataPlayerController dpc = DataPlayerController.getInstance();
		int num = dpc.getCollectionNum(id);
		labColNum.text = num+"/10";
	}

	public static HuaFei_Cell1_3 CreateHuafeiCell1_3()
	{
		Object obj = Resources.Load("pbHuaFei_Cell1_3");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			HuaFei_Cell1_3 hf = go.GetComponent<HuaFei_Cell1_3>();
			return hf;
		}
		return null;
	}
}
