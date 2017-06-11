using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class WGUIRenderGetQueue : MonoBehaviour { 

	void Start()   
	{

		Renderer[] all = this.GetComponentsInChildren<Renderer>();

		for(int i=0;i<all.Length;i++)
		{
			Debug.Log(all[i].gameObject.name+"...renderQueue = "+all[i].sharedMaterial.renderQueue );
		}

		if (GetComponent<Renderer>() != null && GetComponent<Renderer>().sharedMaterial != null)   
		{          
//			renderer.sharedMaterial.renderQueue = renderQueue;    
//			Debug.Log(gameObject.name+"this renderqueue = "+renderer.sharedMaterial.renderQueue);
		}  
	}   

}  