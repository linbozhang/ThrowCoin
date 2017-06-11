using UnityEngine;
using System.Collections;

public class YHAccelerationCamera : MonoBehaviour {
	Transform _transform;


	void Start()
	{
		_transform = transform;
	}
	void Update()
	{
		Quaternion gyro = Input.gyro.attitude; 
		//回転の向きの调整 
		gyro.x *= -1.0f; 
		gyro.y *= -1.0f; 
		//自分の倾きとして适用 
		_transform.localRotation = gyro;
	}

}

