using UnityEngine;
using System.Collections;

public class V2CoinDropEffect : MonoBehaviour {

	public UILabel labCoinNum;
	public float time=1f	;
	public TweenPosition tpContent;
	void Start()
	{
		StartCoroutine(DeleteGameObject());
	}
	IEnumerator DeleteGameObject()
	{
		yield return new WaitForSeconds(time);
		Destroy(this.gameObject);
	}
	static Object mObj  = null;
	public static V2CoinDropEffect CreateDropEffect()
	{
		if(mObj == null)
		{
			mObj = Resources.Load("pbAddCoin");
		}
		if(mObj != null)
		{
			GameObject go = Instantiate(mObj) as GameObject;
			V2CoinDropEffect cd = go.GetComponent<V2CoinDropEffect>();
			return cd;
		}
		return null;
	}
}
