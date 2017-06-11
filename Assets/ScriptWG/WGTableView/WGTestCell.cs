using UnityEngine;
using System.Collections;

public class WGTestCell : WGTableViewCell {

	public UILabel labName;
	public UILabel labGold;
	public UIToggle togSelect;
	// Use this for initialization
	void Start () {
	
	}
	
	public void freshWithIndex(int i)
	{
		labName.text = "Cell ="+i;
	}

	void OnBtnCell()
	{
		labGold.color = togSelect.value?Color.green:Color.yellow;
	}
}
