using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
public class buttontest : MonoBehaviour {

	// Use this for initialization
	Button mButton;

	void Start () {
		mButton =GetComponent<Button>();
		Button.ButtonClickedEvent mClickevent=new Button.ButtonClickedEvent();
		mClickevent.AddListener(OnClick1);

		mButton.onClick=mClickevent;
		mClickevent.AddListener(OnClick1);
	}
	public void OnClick(GameObject obj ){
		Debug.Log(obj.name);
	}
	public void OnClick1(){
		Debug.Log("Onclick1");
	}
	// Update is called once per frame
	void Update () {
	
	}
}
