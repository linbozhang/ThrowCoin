using UnityEngine;
using System.Collections;

public class MDOrder{
	public string order;
	public int act;
	public int error;
}

public class MySampleTest3 : WGMonoComptent {


	string myTest = "{\"order\":\"tongbu132\",\"act\":2,\"error\":0}";



	public int goid = 111;
	public GameObject go;

	public float _Coin = 50.123f;

	public int Coin
	{
		get
		{
			int coin = Mathf.FloorToInt(_Coin);

			return coin;
		}
		set
		{

			float p = UnityEngine.Random.Range(0.1f,0.5f);
			
			_Coin = value+p;


			int t = Mathf.FloorToInt(_Coin);
			if(t %10 >0)
			{
				//WG.SLogError("===t="+t+"==_Coin="+_Coin+"==p="+p+"==value="+value);
			}


		}
	}

	void Awake()
	{
		//WG.EnableLog = true;
	}
	// Use this for initialization
	void Start () {
		Time.timeScale = 0;
		InvokeBlock(1f,()=>{
			//WG.SLog("this is 1111111");
		});


		MDOrder od = SDK.Deserialize<MDOrder>(myTest);


		Debug.Log(SDK.Serialize(od));



		int max = 100000;
		float result = 1;
		for(int i=0;i<max;i++)
		{
			result = result*getx(max);
		}
		Debug.Log("======"+result);

		Debug.Log("===="+getH());

	}
	float getx(float x){
		return 1+1/x;
	}
	float getH()
	{
		float f = Mathf.Sqrt(5);
		return f;
	}

	int runCount = 1;
	void OnGUI()
	{
		if(GUILayout.Button("TEST"))
		{
			runCount ++;
			int rc =runCount;
			InvokeBlock(1f,()=>{
				//WG.SLog("this is ======="+rc);
			});
		}
		else if(GUILayout.Button("TEST1"))
		{
			for(int i=0;i<10000;i++)
			{
				Coin +=10;
			}
		}
	}

}
