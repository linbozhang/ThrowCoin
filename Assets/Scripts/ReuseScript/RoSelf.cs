using UnityEngine;
using System.Collections;

public class RoSelf : MonoBehaviour {
	
	private Transform _tran;
	// Use this for initialization
	
	public float V ;
	private Rigidbody _Rig;
	void Start () {
		
		_tran = transform;
		_Rig = gameObject.GetComponent<Rigidbody>();
		//_Rig.AddTorque(Vector3.up*V);
	}
	
	void FixedUpdate()
	{


		_Rig.angularVelocity = V*Vector3.up;
		_Rig.velocity = Vector3.zero;
	}

}