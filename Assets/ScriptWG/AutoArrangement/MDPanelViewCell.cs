using UnityEngine;
using System.Collections;

public class MDPanelViewCell : MonoBehaviour {

	public float _CellWidth = 1f;
	public float _CellHeight = 2f;

	public virtual float getCellWidth()
	{
		return _CellWidth;
	}
	public virtual float getCellHeight()
	{
		return _CellHeight;
	}
	public Color boundsColor = Color.green;
	public bool b_OnDrawGizmos = true;
	void OnDrawGizmos() {

		if(b_OnDrawGizmos)
		{
			Gizmos.color = boundsColor;
			Gizmos.DrawLine(Pos(-getCellWidth()/2,-getCellHeight()/2), Pos(getCellWidth()/2,-getCellHeight()/2));
			Gizmos.DrawLine(Pos(-getCellWidth()/2,getCellHeight()/2), Pos(getCellWidth()/2,getCellHeight()/2));
			Gizmos.DrawLine(Pos(-getCellWidth()/2,-getCellHeight()/2), Pos(-getCellWidth()/2,getCellHeight()/2));
			Gizmos.DrawLine(Pos(getCellWidth()/2,-getCellHeight()/2), Pos(getCellWidth()/2,getCellHeight()/2));
		}
	}
	Vector3 Pos(float x,float y){
		return this.transform.position + x * Vector3.right + y * Vector3.up;
	}
}

public interface MDPanelViewCellInterFace
{
	float getCellWidth();
	float getCellHeight();
	Transform getTransform();
}