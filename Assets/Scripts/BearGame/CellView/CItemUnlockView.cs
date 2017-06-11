using UnityEngine;
using System.Collections;

public class CItemUnlockView : MonoBehaviour {
	public UILabel labName;
	public UILabel labDes;

	public void freshWithBearID(string name,string des)
	{
		labName.text = name;
		labDes.text = des;
	}

}
