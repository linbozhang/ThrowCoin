using UnityEngine;
using System.Collections;

public class BCGameObj : MonoBehaviour {

	public int ID;//物品ID
		
	public int Type;//物品类型
	
	public int Exp;//物品经验
		
	public int Value;//物品的价值
	
	public int State;//

	public void freshWithData(BCObj obj,int st = 0)
	{
		this.ID = obj.ID;
		this.Type = obj.Type;
		this.Exp = obj.Exp;
		this.Value = obj.Value;
		this.State = st;
	}
}
