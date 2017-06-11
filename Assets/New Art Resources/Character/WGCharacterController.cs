using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
[RequireComponent(typeof(MonoInvokeBlock))]
public class WGCharacterController : WGMonoComptent {

	public CAnimParticle capProvoke;

	public Animator _animator;

	public bool bJustIdle = false;

	int _mstate;
	int mState{
		set{
			_mstate = value;
			_animator.SetInteger("state",value);
		}
		get{
			return _mstate;
		}
	}
	int mCurrentState = 0;

	public CAnimWeight provokeWeight;
	List<Shader>  szDefaultShader = new List<Shader>();
	List<Material> list_Exist = new List<Material>();

	const string IdleState = "Base Layer.idle";
	const string HurtState = "Base Layer.hurt";
	const string ProvokeState="Base Layer.provoke";
	const string DeathState="Base Layer.death";
	const string CrazyState="Base Layer.crazy";

	void Awake()
	{
		if(_animator == null)
		{
			_animator = this.GetComponentInChildren<Animator>();
		}
	}

	bool isDeath = false;

	// Use this for initialization
	void Start () {
	
		if(bJustIdle) mState =2;

		SkinnedMeshRenderer[] renders = GetComponentsInChildren<SkinnedMeshRenderer>();

		//
		foreach (SkinnedMeshRenderer aMeshRenderer in renders)
		{
			foreach (Material aMaterial in aMeshRenderer.materials)
			{
				//把材质球加入原始材质目录
				if(list_Exist.Contains(aMaterial))
				{
				}
				else{
					list_Exist.Add(aMaterial);
					szDefaultShader.Add(aMaterial.shader);
				}
			}
		}

	}

	int idlePlayCount = 0;
	int hurtPlayCount = 0;
	int provokeCount = 0;
	int deathCount = 0;
	int crazyCount = 0;
	AnimatorStateInfo preState;
	void Update()
	{
		if(bJustIdle)return;

		AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);


		if(!isDeath)
		{
			if(stateInfo.IsName(IdleState))
			{
				provokeCount = 0;
				hurtPlayCount = 0;
				if(stateInfo.normalizedTime>(0.6f+idlePlayCount))
				{
					idlePlayCount++;
					if(Random.Range(0,provokeWeight.max)<=provokeWeight.level)
					{
						PlayProvokeAnim();
					}
				}
			}
			else if(stateInfo.IsName(HurtState))
			{
				idlePlayCount = 0;
				if(stateInfo.normalizedTime>(0.5f+hurtPlayCount))
				{
					hurtPlayCount ++;
					reset();
				}
			}
			else if(stateInfo.IsName(ProvokeState))
			{
				idlePlayCount = 0;
				if(stateInfo.normalizedTime>(0.5f+provokeCount))
				{
					provokeCount++;
					reset();
				}
			}
			else if(stateInfo.IsName(CrazyState))
			{
				if(stateInfo.normalizedTime>(0.5f+crazyCount))
				{
					crazyCount++;
					reset();
				}
			}
		}
		else{
			if(stateInfo.IsName(DeathState))
			{
				if(stateInfo.normalizedTime>(0.9f+deathCount))
				{
					deathCount++;
					DeathSelf();
				}
			}
		}
//		if(Input.GetKey(KeyCode.Alpha3))
//		{
//			mState = 3;
//		}
//		else if(Input.GetKey(KeyCode.Alpha2))
//		{
//			mState = 2;
//		}
//		else if(Input.GetKey(KeyCode.Alpha4))
//		{
//			mState = 4;
//		}


	}

	void reset()
	{
		mState = 0;
	}
	public void PlayProvokeAnim()
	{

		if(!isDeath)
		{
			provokeCount = 0;
			mState = 2;
			if(capProvoke != null)
			{
				if(capProvoke.goParticle != null)
				{
					GameObject go = Instantiate(capProvoke.goParticle)as GameObject;
					go.transform.parent=capProvoke.goParent.transform;
					go.transform.localPosition = Vector3.zero;
				}
			}
		}
	}
	public void PlayHurtAnim()
	{
		if(!isDeath)
		{
			AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
			if(stateInfo.IsName(HurtState))
			{
				_animator.Play(HurtState,0,0);
			}
			else{
				hurtPlayCount = 0;
				mState = 3;
			}
		}
	}
	public void PlayDeathAnim()
	{
		if(!isDeath)
		{

			isDeath = true;

			mState = 4;

		}
	}
	public void PlayCrazy()
	{
		if(!isDeath)
		{
			mState = 10;
		}
	}
	void DeathSelf()
	{
		reset();
		Destroy(this.transform.parent.gameObject);
	}

	public void GrayEffect(bool gray)
	{
		if(list_Exist.Count==0)
		{
			SkinnedMeshRenderer[] renders = GetComponentsInChildren<SkinnedMeshRenderer>();

			//
			foreach (SkinnedMeshRenderer aMeshRenderer in renders)
			{
				foreach (Material aMaterial in aMeshRenderer.materials)
				{
					//把材质球加入原始材质目录
					if(list_Exist.Contains(aMaterial))
					{
					}
					else{
						list_Exist.Add(aMaterial);
						szDefaultShader.Add(aMaterial.shader);
					}
				}
			}
		}
		for(int i=0;i<list_Exist.Count;i++)
		{
			if(gray)
			{
				list_Exist[i].shader = Shader.Find("Diffuse");;
			}
			else{
				list_Exist[i].shader = szDefaultShader[i];
			}
		}
	}

}
[System.Serializable]
public class CAnimWeight{
	public int max;
	public int level;
}
public enum CAnimType{
	NONE=0,
	idle,
	hurt,
	provoke,
	death,
	crazy,
}
[System.Serializable]
public class CAnimParticle{
	public GameObject goParent;
	public GameObject goParticle;
}


