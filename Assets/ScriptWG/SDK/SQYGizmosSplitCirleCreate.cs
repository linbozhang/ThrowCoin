using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SQYGizmosSplitCirleCreate : MonoBehaviour {

	public Color boundsColor = Color.red;
	public float radiou = 6;
	public int splitNum = 6;
	public bool b_OnDrawGizmos = true;

	public GameObject goImage;

	public List<GameObject> szImages = new List<GameObject>();

	void OnDrawGizmos() {
		if(b_OnDrawGizmos)
		{
			Gizmos.color = boundsColor;

			Gizmos.DrawWireSphere(Pos(0,0),radiou);

			Gizmos.DrawLine(Pos(0,0),Pos(0,radiou*Mathf.Sin(Mathf.PI/2)));
			float rr=0;
			for(int i=1;i<splitNum;i++)
			{
				 rr= Mathf.PI/2+ i*2*Mathf.PI/splitNum;
				Gizmos.DrawLine(Pos(0,0),Pos(radiou*Mathf.Cos(rr),radiou*Mathf.Sin(rr)));

			}
		}
	}
	[ContextMenu("CreateImage")]
	void CreateImage()
	{
		szImages.Clear();
		for(int i=0;i<splitNum;i++)
		{
			float rr = Mathf.PI/2+ i*2*Mathf.PI/splitNum;

			GameObject go = Instantiate(goImage) as GameObject;
			go.transform.parent = this.transform;
			go.transform.position = Pos(radiou*Mathf.Cos(rr),radiou*Mathf.Sin(rr));
			go.transform.localEulerAngles = new Vector3(0,0,90+360f/splitNum*i);
			go.transform.localScale = Vector3.one;
			go.name = goImage.name+i;
			szImages.Add(go);
		}
	}
	[ContextMenu("FreshImage")]
	void FreshImage()
	{
		float rr=0;
		for(int i= 0,max = szImages.Count;i<max;i++)
		{
			 rr= Mathf.PI/2+ i*2*Mathf.PI/splitNum;
			szImages[i].transform.position = Pos(radiou*Mathf.Cos(rr),radiou*Mathf.Sin(rr));
		}
	}

	Vector3 Pos(float x,float y){
		return this.transform.position + x * Vector3.right + y * Vector3.up;
	}
}
