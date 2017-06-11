using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using WEMiniJSON;
//Create MiniJsonTest by Song
// Copy Right 2014®
public class MiniJsonTest : MonoBehaviour {
	
	//befor Start()
	void Awake()
	{

	}
	
    void Start () 
    {

        string jsonString = "{ \"array\": [1.44,22,33]" +
            "\"object\": {\"key1\":\"value1\", \"key2\":256}, " +
            "\"string\": \"The quick brown fox \\\"jumps\\\" over the lazy dog \", " +
            "\"unicode\": \"\\u3041 Men\\u00fa sesi\\u00f3n\", " +
            "\"int\": 65536, " +
            "\"float\": 3.1415926, " +
            "\"bool\": true, " +
            "\"null\": null }";
        string ArrayString = "[1,2,3,4,5,6,7]";

        //Dictionary<string, object> dict = MiniJSON.LC_MiniJson.Deserialize(jsonString) as Dictionary.<string. Object>;
        Dictionary<string, object> dict = Json.Deserialize(jsonString) as Dictionary<string, object>;
        Debug.Log("deserialized: " + dict.GetType());



        //double[][] arrayList  = MiniJSON.LC_MiniJson.Deserialize((dict["array"] as List<object>)[0].ToString()) as double[][];

        List<object> lst = (List<object>) dict["array"];

        Debug.Log("dict['array'][0]: "+ lst[2]);


        Debug.Log("dict['string']: " + dict["string"].ToString());
        Debug.Log("dict['float']: " + dict["float"]); // floats come out as doubles
        Debug.Log("dict['int']: " + dict["int"]); // ints come out as longs
        Debug.Log("dict['unicode']: " + dict["unicode"].ToString());

        Dictionary<string, object> dict2 = (dict["object"]) as Dictionary<string, object>;


        int[,] hh = new int[2,3]{
            {1,2,3},
            {4,5,6}
        };
        List<object> szTest = Json.Deserialize(ArrayString) as List<object>;
        long p = (long)szTest[0];
        Debug.Log(szTest+"==="+szTest.Count+"==="+p);

        string str = Json.Serialize(dict["array"]);

        Debug.Log("serialized: " + str);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}