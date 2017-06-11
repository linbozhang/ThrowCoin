using UnityEngine;
using System.Collections;
//Create RampDownParticle by Song
// Copy Right 2014®
public class RampDownParticle : MonoBehaviour {
	
	public float delayTime=0;
	public float delayPlusTime=0;
	public float rampDownTime=1;
	public float origMinEmission;
	public float origMaxEmission;

	void Start () {

		origMinEmission=GetComponent<ParticleEmitter>().minEmission;
		origMaxEmission=GetComponent<ParticleEmitter>().maxEmission;
		GetComponent<ParticleEmitter>().emit=false;

	}

	void Update () {
		if((delayTime+delayPlusTime)>0) delayTime-=Time.deltaTime;


		if(delayTime<=0 && GetComponent<ParticleEmitter>().emit==false) GetComponent<ParticleEmitter>().emit=true;


		if((delayTime+delayPlusTime)<=0){
			GetComponent<ParticleEmitter>().minEmission=origMinEmission*rampDownTime;
			GetComponent<ParticleEmitter>().maxEmission=origMaxEmission*rampDownTime;
			rampDownTime-=Time.deltaTime;
			if(rampDownTime<0){ rampDownTime=0;}
		}

	}
}