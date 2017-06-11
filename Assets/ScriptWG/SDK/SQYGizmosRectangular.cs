using UnityEngine;
using System.Collections;

public class SQYGizmosRectangular : MonoBehaviour {
	public float factor = 1f;
	public float width = 13.0f;
	public float height = 13.0f;
	public Color boundsColor = Color.green;
	public bool b_OnDrawGizmos = true;
	public bool b_OnDrawCenterLine = true;
	public Transform tRoot = null;
	public bool bSizeToRoot= false;
	public UIWidget widget=null;

	void OnDrawGizmos() {
		if(bSizeToRoot)
		{
			if(widget == null)widget = this.GetComponent<UIWidget>();
			if(tRoot == null) tRoot = transform.parent;
			width = tRoot.localScale.x*widget.width;
			height=tRoot.localScale.y*widget.height;
		}

		if(b_OnDrawGizmos)
		{
			Gizmos.color = boundsColor;
			Gizmos.DrawLine(Pos(-width/2,-height/2), Pos(width/2,-height/2));
			Gizmos.DrawLine(Pos(-width/2,height/2), Pos(width/2,height/2));
			Gizmos.DrawLine(Pos(-width/2,-height/2), Pos(-width/2,height/2));
			Gizmos.DrawLine(Pos(width/2,-height/2), Pos(width/2,height/2));

		}
		if(b_OnDrawCenterLine)
		{
			Gizmos.color = boundsColor;
			Gizmos.DrawLine(Pos(-width/2,0),Pos(width/2,0));
			Gizmos.DrawLine(Pos(0,-height/2),Pos(0,height/2));
		}
	}
	Vector3 Pos(float x,float y){
		if(!bSizeToRoot)
		{
			return this.transform.position + x * Vector3.right*factor + y * Vector3.up*factor;
		}
		return this.transform.position + x * Vector3.right + y * Vector3.up;
	}
}
