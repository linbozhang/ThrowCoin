using UnityEngine;
using System.Collections;

public class SQYGizmosBoxCollider : MonoBehaviour {

	public Vector3 center;
	public Vector3 size;

	public Color boundsColor = Color.green;
	public bool b_OnDrawGizmos = true;

	BoxCollider _boxCol = null;

	void OnDrawGizmos() {
		if(b_OnDrawGizmos)
		{
			if(_boxCol == null)
			{
				_boxCol = this.GetComponent<BoxCollider>();
			}

			Gizmos.color = boundsColor;
			center = _boxCol.center;
			size = _boxCol.size;
			Gizmos.DrawWireCube(this.transform.position+center,size);

		}
	}
	Vector3 Pos(float x,float y,float z){
		return this.transform.position +center+(new Vector3(x,y,z));
	}

}
