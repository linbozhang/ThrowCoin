using UnityEngine;
using System.Collections;

public class FX_LookAt : MonoBehaviour {

	public GameObject ForObj;

	public GameObject ToObj;

	public ParticleSystem [] Part_G;

	public float Life_Scale;
	public float Speed_Scale;

	// Use this for initialization
	void Start () {
		if (Part_G.Length > 0)
		{
			for (int i = 0; i < Part_G.Length; i++)
			{
				if (Part_G[i] != null)
				{
					Part_G[i].startSpeed = Speed_Scale;
				}

			}


		}
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 relativePos = ForObj.transform.position - ToObj.transform.position;
		Quaternion rotation = Quaternion.LookRotation(relativePos);
		ForObj.transform.rotation = rotation;

		if (Part_G.Length > 0)
		{
			float Part_Distance = Vector3.Distance(ForObj.transform.position, ToObj.transform.position);

			for (int i = 0; i < Part_G.Length; i++)
			{
				if (Part_G[i] != null)
				{
					Part_G[i].startLifetime = Part_Distance * Life_Scale;
					Part_G[i].startSpeed = Speed_Scale;
				}
			
			}


		}


	}
}
