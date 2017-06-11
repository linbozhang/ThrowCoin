using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Create SQYGizmosPolygon by Song
// Copy Right 2014®
[ExecuteInEditMode]
public class SQYGizmosPolygon : MonoBehaviour {
	public float width = 1;


	List<Vector3> szAllPos = new List<Vector3>();
	public Color boundsColor = Color.green;
	public bool b_OnDrawGizmos = true;
	public bool b_NeedAddPos = true;
	public Transform transPos;
	void OnDrawGizmos() {

		if(b_OnDrawGizmos)
		{

			Gizmos.color = boundsColor;

			if(szAllPos.Count>1)
			{
				Vector3 pos1 = szAllPos[0];
				for(int i=1;i<szAllPos.Count;i++)
				{
					Gizmos.DrawLine(pos1+this.transform.position,szAllPos[i]+this.transform.position);
					pos1 = szAllPos[i];
				}
				if(szAllPos.Count>2)
				{
					Gizmos.DrawLine(szAllPos[0]+this.transform.position,szAllPos[szAllPos.Count-1]+this.transform.position);
				}
			}

			if(b_NeedAddPos)
			{
//				if(Input.GetKey(KeyCode.A))
//				{
//					Debug.Log("==============");
//					current = Input.mousePosition;
//				}

				Gizmos.DrawLine(Pos(-width/2,-width/2), Pos(width/2,-width/2));
				Gizmos.DrawLine(Pos(-width/2,width/2), Pos(width/2,width/2));
				Gizmos.DrawLine(Pos(-width/2,-width/2), Pos(-width/2,width/2));
				Gizmos.DrawLine(Pos(width/2,-width/2), Pos(width/2,width/2));
				Gizmos.DrawLine(Pos(-width/2,0),Pos(width/2,0));
				Gizmos.DrawLine(Pos(0,-width/2),Pos(0,width/2));
			}
		}
	}

	Vector3 Pos(float x,float y){
		if(transPos !=null)
		{
			return  x * Vector3.right + y * Vector3.up+transPos.position;
		}
		return Vector3.zero;
	}

	[ContextMenu("addPos")]
	void AddCurrentToPos()
	{
		if(transPos !=null)
		{
			szAllPos.Add(transPos.position-this.transform.position);
		}
	}
	[ContextMenu("remove")]
	void removePos()
	{
		if(transPos !=null)
		{
			if(szAllPos.Count>0)
			{
				float distance = Vector3.Distance(transPos.position-this.transform.position,szAllPos[0]);
				Vector3 prePos = szAllPos[0];
				int index = 0;
				float temp = 0;
				for(int i=1;i<szAllPos.Count;i++)
				{
					temp = Vector3.Distance(szAllPos[i],szAllPos[index]);
					if(distance>temp)
					{
						index = i;
						distance = temp;
					}
				}
				szAllPos.RemoveAt(index);
			}
		}
	}
}





























