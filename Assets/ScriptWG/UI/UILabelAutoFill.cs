using UnityEngine;
using System.Collections;


public class UILabelAutoFill : MonoBehaviour {

	public int textID;
	// Use this for initialization
	void Start () {
		UILabel lab = this.GetComponent<UILabel>();
		if(lab != null)
		{
			lab.text = WGStrings.getText(textID);
		}
	}

}
