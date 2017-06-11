using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
public class GetTransformToJson : MonoBehaviour {

	public TextAsset taCoinData;
	public GameObject goCoin;
	public Vector3 coinScale=Vector3.one;
	// Use this for initialization
	void Start () {
	
	}

	[ContextMenu("ToJson")]
	void GetChildToJson()
	{

		DataCoin dc = new DataCoin();
		dc.IsFirstLoad = false;

		StreamWriter sw = new StreamWriter(AssetDatabase.GetAssetPath(taCoinData));

		List<float[]> szPos = new List<float[]>();
		foreach (Transform child in transform) {
			Vector3 pos = child.position;
			szPos.Add(new float[]{pos.x,pos.y,pos.z});
		}
		dc.CoinPos = szPos;
		Debug.Log(SDK.Serialize(dc));
		sw.Write(SDK.Serialize(dc));
		sw.Close();
	}
	[ContextMenu("Create")]
	void Create()
	{
		string res="";
		DataCoin dc;
		using(StreamReader sr = new StreamReader(new MemoryStream(taCoinData.bytes)))
		{
			res = sr.ReadToEnd();
			dc = SDK.Deserialize<DataCoin>(res);
		}

		for(int i=0;i<dc.CoinPos.Count;i++)
		{
			GameObject go = Instantiate(goCoin) as GameObject;
			SDK.AddChild(go,this.gameObject);
			go.transform.position = SDK.toV3(dc.CoinPos[i]);
			go.transform.localScale = coinScale;
			go.transform.rotation = Quaternion.Euler(new Vector3(0,0,180));
		}
	}
}
