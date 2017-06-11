using UnityEngine;
using System.Collections;

public class CameraGravity_Fx : MonoBehaviour {

	public bool Gravity_key = false;

	public float fMin = -180f;
	public float fMax = 180f;

	public float RotScale = 90f;

	public bool bTestEffect = false;

	public static float GravityDefault_X = 0;
	public static float GravityDefault_Y = 0;
	public static float GravityDefault_Z = 0;


	 Vector3 RotationDefault;

	Quaternion target = Quaternion.Euler(Vector3.zero);

	void Start () {
		Input.gyro.enabled = true;
		GravityDefault_X = Input.gyro.attitude.eulerAngles.x;
		GravityDefault_Y = Input.gyro.attitude.eulerAngles.y;
		GravityDefault_Z = Input.gyro.attitude.eulerAngles.z;
		RotationDefault = transform.rotation.eulerAngles;

	}


	void Update()
	{

		if (Gravity_key)
		{
		
			float y = Input.gyro.attitude.eulerAngles.x - GravityDefault_X;

			if(y>fMax)y=fMax;
			if(y<fMin)y=fMin;

			target = Quaternion.Euler(RotationDefault.x, y, 0);
			
		}


		transform.rotation = Quaternion.Lerp(transform.rotation, target, Time.deltaTime * 100);
	}
}
