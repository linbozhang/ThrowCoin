using UnityEngine;
using System.Collections;

public class WGWeapon : WGMonoComptent {

	public float fReloadTime = 0.15f;

	public UILabel labCostCoin;
	public UILabel labCostCoin1;
	public Transform tRote;
	public Transform tBullet;

	public TweenRotation trBullet;

	public TweenPosition tp_fireEffect1;
	public TweenRotation tr_fireEffect2;
	public TweenRotation tr_fireEffect3;
	public TweenPosition tr_fireEffect4;
	public TweenPosition tpChangeEffect0_1;
	public TweenRotation[] szChangeEffect1_2;
	public TweenScale tsChangeEffect2_3;
	public TweenPosition tpChangeEffect3_4;
	public GameObject goNum;
	public TweenAlpha taChangeNum;
	public TweenScale tsChangeNum;

	public GameObject pbChangeEffect;

	int curWeaponIndex = 0;
	bool bCanChange = true;


	// Use this for initialization
	void Start () {
		tpChangeEffect0_1.ESetActive(true);
		for(int i=0;i<szChangeEffect1_2.Length;i++)
		{
			szChangeEffect1_2[i].ESetActive(false);
		}
		tsChangeEffect2_3.ESetActive(false);
		tpChangeEffect3_4.ESetActive(false);
		tpChangeEffect0_1.PlayForward();
		tsChangeNum.enabled = false;
		taChangeNum.enabled = false;

	}
	void Update()
	{
//		if(Input.GetKeyUp(KeyCode.Alpha1))
//		{
//			if(curWeaponIndex>0)
//			{
//				ChangeWeapon(curWeaponIndex-1,100);
//			}
//		}
//		else if(Input.GetKeyUp(KeyCode.Alpha2))
//		{
//			if(curWeaponIndex<3)
//			{
//				ChangeWeapon(curWeaponIndex+1,1500);
//			}
//		}
//		else if(Input.GetMouseButtonUp(0))
//		{
//			FireEffect();
//		}
	}

	public bool ChangeWeapon(int index,int coin)
	{
		if(!bCanChange)return false;
		bCanChange = false;

		tsChangeNum.enabled = true;
		taChangeNum.enabled = true;
		tsChangeNum.PlayForward();
		taChangeNum.PlayForward();

		InvokeBlock(tsChangeNum.duration,()=>{
			labCostCoin.text = coin.ToString();
//			labCostCoin1.text = coin.ToString();
			tsChangeNum.PlayReverse();
			taChangeNum.PlayReverse();
		});



		GameObject go = Instantiate(pbChangeEffect) as GameObject;
		SDK.AddChild(go,this.gameObject);
		InvokeBlock(1.0f,()=>{
			Destroy(go);
		});

		InvokeBlock(0.3f,()=>{
			curWeaponIndex = index;
			bCanChange=true;
		});
		if(index >curWeaponIndex)
		{
			switch(index)
			{
			case 1:
			{
				float dt = PlayReverse0_1();
				PlayForward1_2(dt*0.75f);
			}
				break;
			case 2:
			{
				float t1 = PlayReverse1_2();
				float t2 = PlayReverse0_1(t1);
				float t3 = PlayForward1_2(t1+t2*0.75f);
				float t4 = PlayForward2_3(t1+t2);

			}
				break;
			case 3:
			{

				float t1 = PlayReverse2_3();
				float t2 = PlayReverse1_2(t1*0.25f);
				float t3 = PlayReverse0_1(t2);
				float t4 = PlayForward1_2(t2+t3*0.75f);
				float t5 = PlayForward2_3(t2+t3);
				PlayForward3_4(t2+t3);
			}
				break;
			}

		}
		else if( index< curWeaponIndex)
		{
			switch(index)
			{
			case 0:
			{
				float t1 = PlayReverse1_2();
				float t2 = PlayForward0_1(t1*0.75f);
			}
				break;
			case 1:
			{
				float t1 = PlayReverse2_3();
				float t2 = PlayReverse1_2(t1);
				float t3 = PlayReverse0_1(t2+t1);
				float t4 = PlayForward1_2(t3+t1+t2);
			}
				break;
			case 2:
			{
				float t1 = PlayReverse3_4();
				float t2 = PlayReverse2_3(t1*0.5f);
				float t3 = PlayReverse1_2(t1*0.5f+t2*0.75f);
				float t4 = PlayReverse0_1(t1*0.5f+t2*0.75f+t3*0.75f);
				float t5 = PlayForward1_2(t1*0.5f+t2*0.75f+t3*0.75f+t4);
				float t6 = PlayForward2_3(t1*0.5f+t2*0.75f+t3*0.75f+t4+t5*0.5f);
			}
				break;
			}
		}
		return true;
	}
	public void FireEffect()
	{
		tp_fireEffect1.ResetToBeginning();
		tp_fireEffect1.PlayForward();

		tr_fireEffect2.ResetToBeginning();
		tr_fireEffect2.PlayForward();
		tr_fireEffect3.ResetToBeginning();
		tr_fireEffect3.PlayForward();

		tr_fireEffect4.ResetToBeginning();
		tr_fireEffect4.PlayForward();

	}
	float PlayForward0_1(float dt=0)
	{
		if(dt>0)
		{
			InvokeBlock(dt,()=>{
				tpChangeEffect0_1.ResetToBeginning();
				tpChangeEffect0_1.PlayForward();
			});
		}
		else
		{
			tpChangeEffect0_1.ResetToBeginning();
			tpChangeEffect0_1.PlayForward();
		}
		return tpChangeEffect0_1.duration;
	}
	float PlayReverse0_1(float dt=0)
	{
		return PlayForward0_1(dt);
	}
	float PlayForward1_2(float dt=0)
	{
		if(dt>0)
		{
			InvokeBlock(dt,()=>{
				for(int i=0;i<szChangeEffect1_2.Length;i++)
				{
					szChangeEffect1_2[i].ESetActive(true);
					szChangeEffect1_2[i].PlayForward();
				}
			});
		}
		else
		{
			for(int i=0;i<szChangeEffect1_2.Length;i++)
			{
				szChangeEffect1_2[i].ESetActive(true);
				szChangeEffect1_2[i].PlayForward();
			}
		}
		return szChangeEffect1_2[0].duration;
	}
	float PlayReverse1_2(float dt=0)
	{
		if(dt>0)
		{
			InvokeBlock(dt,()=>{
				for(int i=0;i<szChangeEffect1_2.Length;i++)
				{
					szChangeEffect1_2[i].PlayReverse();
					
				}
				InvokeBlock(szChangeEffect1_2[0].duration,()=>{
					for(int i=0;i<szChangeEffect1_2.Length;i++)
					{
						szChangeEffect1_2[i].ESetActive(false);
					}
				});
			});
		}
		else
		{
			for(int i=0;i<szChangeEffect1_2.Length;i++)
			{
				szChangeEffect1_2[i].PlayReverse();
				
			}
			InvokeBlock(szChangeEffect1_2[0].duration,()=>{
				for(int i=0;i<szChangeEffect1_2.Length;i++)
				{
					szChangeEffect1_2[i].ESetActive(false);
				}
			});
		}

		return szChangeEffect1_2[0].duration;
	}
	float PlayForward2_3(float dt=0)
	{
		if(dt>0)
		{
			InvokeBlock(dt,()=>{
				tsChangeEffect2_3.ESetActive(true);
				tsChangeEffect2_3.PlayForward();
			});
		}
		else
		{
			tsChangeEffect2_3.ESetActive(true);
			tsChangeEffect2_3.PlayForward();
		}

		return tsChangeEffect2_3.duration;
	}
	float PlayReverse2_3(float dt=0)
	{
		if(dt>0)
		{
			InvokeBlock(dt,()=>{
				tsChangeEffect2_3.PlayReverse();
				InvokeBlock(tsChangeEffect2_3.duration,()=>{
					tsChangeEffect2_3.ESetActive(false);
				});
			});
		}
		else
		{
			tsChangeEffect2_3.PlayReverse();
			InvokeBlock(tsChangeEffect2_3.duration,()=>{
				tsChangeEffect2_3.ESetActive(false);
			});
		}

		return tsChangeEffect2_3.duration;
	}
	float PlayForward3_4(float dt = 0)
	{
		if(dt>0)
		{
			InvokeBlock(dt,()=>{
				tpChangeEffect3_4.ESetActive(true);
				tpChangeEffect3_4.PlayForward();
			});
		}
		else
		{
			tpChangeEffect3_4.ESetActive(true);
			tpChangeEffect3_4.PlayForward();
		}

		return tpChangeEffect3_4.duration;
	}
	float PlayReverse3_4(float dt= 0)
	{
		if(dt>0)
		{
			InvokeBlock(dt,()=>{
				tpChangeEffect3_4.PlayReverse();
				InvokeBlock(tpChangeEffect3_4.duration,()=>{
					tpChangeEffect3_4.ESetActive(false);
				});
			});
		}
		else
		{
			tpChangeEffect3_4.PlayReverse();
			InvokeBlock(tpChangeEffect3_4.duration,()=>{
				tpChangeEffect3_4.ESetActive(false);
			});
		}

		return tpChangeEffect3_4.duration;
	}

	public static WGWeapon CreateWeapon()
	{
		Object obj = Resources.Load("pbWeapon");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			WGWeapon gun = go.GetComponent<WGWeapon>();

			return gun;
		}
		return null;
	}

}
