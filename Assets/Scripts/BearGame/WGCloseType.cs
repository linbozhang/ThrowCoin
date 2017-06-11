using UnityEngine;
using System.Collections;

public class WGCloseType : MonoBehaviour {

	// Use this for initialization
	void Start () {

		UISprite close = this.GetComponent<UISprite>();
		if(close != null)
		{
			#if Close
			close.color = new Color(1,1,1,1);
			#else
			close.color = new Color(1,1,1,70.0f/255);
			#endif
		}

	}

}
