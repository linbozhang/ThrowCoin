using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WGPanelView : MonoBehaviour {

	public enum ALIGNMENT{
		top_left,
		top_center,
		top_right,
		left_center,
		center,
		right_center,        
		bottom_left,
		bottom_center,
		bottom_right
	}
	float panelHeight=1;
	float panelWidth = 1;

	Vector3 topLeft =  Vector3.zero;
	Vector3 topright = Vector3.zero;
	Vector3 bottomLeft = Vector3.zero;
	Vector3 bottomRight = Vector3.zero;

	public float width = 1;
	public float height =1;

	public int rows = 1;

	public int columes = 4;


	public ALIGNMENT alignment = ALIGNMENT.center;

	public bool b_OnDrawGizmos = true;

	public bool b_OnDrawCenterLine = true;

	public Color boundsColor = Color.red;

	// Use this for initialization
	void Start () {
	
	}
	
	[ContextMenu("Reset")]
	void ResetObject()
	{
		List<Transform> szAll = new List<Transform>();

		for(int i=0,max=this.transform.childCount;i<max;i++)
		{
			szAll.Add(this.transform.GetChild(i));
		}
////
//		int count = this.transform.childCount;
//		List<List<Transform>> szAll = new List<List<Transform>>();
//		for(int i=0;i<rows;i++)
//		{
//			List<Transform> temp = new List<Transform>();
//			for(int j=0;j<columes;j++)
//			{
//				if(i*columes+j <count)
//				{
//					Transform tran = this.transform.GetChild(j+i*columes);
//					if(tran !=null)
//					{
//						temp.Add(tran);
//					}
//				}
//			}
//			szAll.Add(temp);
//		}


		jiSuanDingDian();


//		for(int i=0;i<rows;i++)
//		{
//			List<Transform> temp = szAll[i];
//			for(int
//		}
		     

		for(int i=0,max=szAll.Count;i<max;i++)
		{
			szAll[i].position =  Pos(topLeft.x+i%columes*width +width/2,topLeft.y-i/columes*height-height/2);
		}
	}

	void jiSuanDingDian()
	{
		panelHeight = height*rows;
		panelWidth = width*columes;

		topLeft =  V3(-panelWidth/2,panelHeight/2);
		topright = V3(panelWidth/2,panelHeight/2);
		bottomLeft = V3(-panelWidth/2,-panelHeight/2);
		bottomRight = V3(panelWidth/2,-panelHeight/2);

		switch(alignment)
		{
		case ALIGNMENT.top_left:
		case ALIGNMENT.left_center:
		case ALIGNMENT.bottom_left:
			topLeft.x +=panelWidth/2;
			topright.x +=panelWidth/2;
			bottomLeft.x +=panelWidth/2;
			bottomRight.x +=panelWidth/2;
			break;
		case ALIGNMENT.right_center:
		case ALIGNMENT.bottom_right:
		case ALIGNMENT.top_right:
			topLeft.x -=panelWidth/2;
			topright.x -=panelWidth/2;
			bottomLeft.x -=panelWidth/2;
			bottomRight.x -=panelWidth/2;
			break;
		}
		switch(alignment)
		{
		case ALIGNMENT.bottom_center:
		case ALIGNMENT.bottom_left:
		case ALIGNMENT.bottom_right:
			topLeft.y +=panelHeight/2;
			topright.y +=panelHeight/2;
			bottomLeft.y +=panelHeight/2;
			bottomRight.y +=panelHeight/2;
			break;
		case ALIGNMENT.top_center:
		case ALIGNMENT.top_left:
		case ALIGNMENT.top_right:
			topLeft.y -=panelHeight/2;
			topright.y -=panelHeight/2;
			bottomLeft.y -=panelHeight/2;
			bottomRight.y -=panelHeight/2;
			break;
		}
	}


	void OnDrawGizmos() {


		if(b_OnDrawGizmos)
		{


			Gizmos.color = boundsColor;

			jiSuanDingDian();

			Gizmos.DrawLine(Pos(topLeft),Pos(topright));
			Gizmos.DrawLine(Pos(bottomLeft),Pos(bottomRight));
			Gizmos.DrawLine(Pos(topLeft),Pos(bottomLeft));
			Gizmos.DrawLine(Pos(topright),Pos(bottomRight));


			if(b_OnDrawCenterLine)
			{
				Gizmos.color = boundsColor;
				Gizmos.DrawLine((Pos(topLeft)+Pos(bottomLeft))/2,(Pos(topright)+Pos(bottomRight))/2);
				Gizmos.DrawLine((Pos(topLeft)+Pos(topright))/2,(Pos(bottomLeft)+Pos(bottomRight))/2);
			}
		}

	}
	Vector3 V3(float x,float y)
	{
		return new Vector3(x,y,0);
	}
	Vector3 Pos(float x,float y){
		return this.transform.position + x * Vector3.right + y * Vector3.up;
	}
	Vector3 Pos(Vector3 v)
	{
		return this.transform.position+v;
	}

}
