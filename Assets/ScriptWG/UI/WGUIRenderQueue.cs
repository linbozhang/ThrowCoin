using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class WGUIRenderQueue : MonoBehaviour { 
	public int renderQueue = 3000; 
	public bool runOnlyOnce = false;  
	void Start()   
	{     
		Update();   
	}   
	void Update()   
	{  
		if (GetComponent<Renderer>() != null && GetComponent<Renderer>().sharedMaterial != null)   
		{          
			GetComponent<Renderer>().sharedMaterial.renderQueue = renderQueue;     
		} 
		if (runOnlyOnce && Application.isPlaying)  
		{          
			this.enabled = false;       
		}  
	}
}  