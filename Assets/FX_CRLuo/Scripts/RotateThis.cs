using UnityEngine;
using System.Collections;
//Create RotateThis by Song
// Copy Right 2014®
public class RotateThis : MonoBehaviour {
	
	public float rotationSpeedX=90;
	public float  rotationSpeedY=0;
	public float  rotationSpeedZ=0;

	void Update () {
		transform.Rotate(new Vector3(rotationSpeedX,rotationSpeedY,rotationSpeedZ)*Time.deltaTime);

	}
}