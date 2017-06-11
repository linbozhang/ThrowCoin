using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MySampleTest : WGMonoComptent {
	List<int> szNumber = new List<int>();

	string test = "[3011,1,10]#[3012,1,10]#[3013,1,10]#[3014,1,10]#[3015,1,10]#[3001,1,10]#[3002,1,10]#[3003,1,10]#[3004,1,10]#[3005,1,10]";

    public Vector3 FirstPos = new Vector3(0,10.5f,-16f);

	void Awake()
	{
		//WG.EnableLog = true;
		//WG.SLog("Awake do..");
	}

	// Use this for initialization
	void Start () {
//
//		szNumber = new List<int>{1,2,3,4,4,1,2,3};
//
//		szNumber.Remove(1);
//
//		//WG.SLog(SDK.Serialize(szNumber));
//
//		string rs = "[2,3,4,4,1,2,3]";
////
//		List<int> hh = SDK.Deserialize<List<int>>(rs);
 		
//		int math =  Mathf.FloorToInt(5.9f);
//		//WG.SLog(math.ToString());
//
//
//		List<List<int>> szIntTest = new List<List<int>>();
//		for(int i =0;i<4;i++)
//		{
//			List<int> cell = new List<int>();
//			for(int j =0;j<2;j++)
//			{
//				cell.Add(j+i*2);
//			}
//			szIntTest.Add(cell);
//		}
//
//
//        Vector3 v3=new Vector3(0,90,90);
//
//        Quaternion qt = Quaternion.Euler(v3);
//
//        Vector3 v33 = new Vector3(0,1,0);
//
//        v3 = qt*v33;
//
//        //WG.SLog(v3.ToString()+"=="+qt.ToString());
//

//		string[] szTestStr = test.Split('#');
//		for(int i=0;i<szTestStr.Length;i++)
//		{
//			//WG.SLog(szTestStr[i]);
//			List<int> sztest = SDK.Deserialize<List<int>>(szTestStr[i]);
//		}
//
//

		mnIvokeBlock.InvokeBlock(2f,()=>{
			Debug.Log("=======");
		});

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
