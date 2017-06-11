using UnityEngine;
using System.Collections;

public class WGGotCoinEffectView : MonoBehaviour {

	public GameObject pbCoin;

	public Transform tranGoal;

	GameObject _gameObject;

	public static WGGotCoinEffectView Self= null;
	void Awake()
	{
		Self = this;
		_gameObject = gameObject;
	}
	// Use this for initialization
	void Start () {
	
	}

	public void OneCoinDropAtPos(int coin)
	{
		int num = 5;
		if(coin >50)
		{
			num = 10;
		}
		else if(coin>20)
		{
			num = 7;
		}
		for(int i=0;i<num;i++)
		{
			GameObject go = Instantiate(pbCoin) as GameObject;
			SDK.AddChild(go,_gameObject);

			Vector3 v3pos = new Vector3(Random.Range(-300,300),-515,0);
			go.transform.localPosition = v3pos;

			v3pos.y = Random.Range(50,100);

			MiniItween it = MiniItween.MoveTo(go,v3pos,0.4f,MiniItween.EasingType.Bounce3);
			it.myDelegateFuncWithObject +=this.MoveEnd;
		}
	}
	void MoveEnd(GameObject go)
	{
		MiniItween it = MiniItween.MoveTo(go,tranGoal.localPosition,0.7f,MiniItween.EasingType.EaseInOutSine);
		it.myDelegateFuncWithObject +=this.MoveEnd2;
	}
	void MoveEnd2(GameObject go)
	{
		Destroy(go);
	}

}
