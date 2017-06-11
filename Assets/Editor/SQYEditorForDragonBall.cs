using UnityEngine;
using System.Collections;
using UnityEditor;
public class SQYEditorForDragonBall{

	[MenuItem("GameObject/Create Select Empty &n")]
	static void CreatEmptyGOWithSelect()
	{
		GameObject go = new GameObject("GameObject");
		if(Selection.transforms.Length>0)
		{
			go.transform.parent = Selection.transforms[0];
			go.layer = Selection.transforms[0].gameObject.layer;
		}
		
		go.transform.localPosition = new Vector3(0,0,0);
		go.transform.localScale = Vector3.one;
		Selection.activeObject = go;
	}
    [MenuItem("GameObject/Set Active &a")]
    static void SetGOActive()
    {

        if(Selection.transforms.Length>0)
        {
            for(int i=0;i<Selection.transforms.Length;i++)
            {
                Selection.transforms[i].gameObject.SetActive(!Selection.transforms[i].gameObject.activeSelf);
            }
        }

    }

	[MenuItem("SQYAction/Disconnect")]
	static void DiscounnectPrefab() { 
		foreach(Transform aTransform in Selection.transforms){
			PrefabUtility.DisconnectPrefabInstance(aTransform);
		}
	}
	[MenuItem("SQYAction/GetCompent")]
	static void getComenpPath()
	{
		Debug.Log("====1111111111====="+Selection.transforms.Length.ToString());

		if(Selection.transforms.Length>0)
		{
			for(int i=0;i<Selection.transforms.Length;i++)
			{
				D04ShowOrHidden[] sh = Selection.transforms[i].GetComponentsInChildren<D04ShowOrHidden>();
				if(sh.Length>0)
				{
					for(int j=0;j<sh.Length;j++)
					{
//						if(sh[j].transform.parent.Equals(Selection.transforms[i]))
						{
							string path = "/"+sh[j].transform.name;
							Transform tran = sh[j].transform;
							while(tran.parent != null)
							{
								path = "/"+tran.name+path;
								tran = tran.parent;
							}
							Debug.Log("======"+path);
						}
					}
				}
				Debug.Log("=======2222222======="+sh.Length.ToString());
			}
		}
		
	}
}
