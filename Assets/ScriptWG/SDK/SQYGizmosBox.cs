using UnityEngine;
using System.Collections;

public class SQYGizmosBox : MonoBehaviour {

	public Vector3 center;
	public Vector3 size;

	public Color boundsColor = Color.green;
	public bool b_OnDrawGizmos = true;

	void OnDrawGizmos() {
		if(b_OnDrawGizmos)
		{
			Gizmos.color = boundsColor;

			Gizmos.DrawWireCube(this.transform.position+center,size);

		}
	}
	Vector3 Pos(float x,float y,float z){
		return this.transform.position +center+(new Vector3(x,y,z));
	}

}
