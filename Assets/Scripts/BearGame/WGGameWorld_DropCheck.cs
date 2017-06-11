using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public partial class WGGameWorld {

	private float _DropTime;
	private int _DropCount = 0;
	private List<int> _Combo1Weight;

	private List<int> _Combo2Weight;

	private List<int> _Combo3Weight;

	private List<int> _Combo4Weight;

	private List<int> _Combo5Weight;

	private BCGameObj _Tem;

	private float _LastDropTime;//

	private float _FristDropTime;//

	public float DeltaTime;//

	public GameObject goAddCoin;

	int _mAddCoinNum = 0;
	int _mAddCoinNum2 = 0;

	private Dictionary<int,Combo> _ComboDic = new Dictionary<int, Combo>();
	public void InitComboWeight()
	{
		_Combo1Weight = new List<int>();
		_Combo2Weight = new List<int>();
		_Combo3Weight = new List<int>();
		_Combo4Weight = new List<int>();
		_Combo5Weight = new List<int>();
		int num = _ComboDic[1].ParaArray.Count;
		int i = 0;
		_Combo1Weight.Add(_ComboDic[1].ParaArray[i].Weight);
		for(i = 1; i<num;i++)
		{
			//Debug.Log(_ComboDic[i].ParaArray[i].Weight);
			_Combo1Weight.Add(_Combo1Weight[i-1]+_ComboDic[1].ParaArray[i].Weight);
		}

		num = _ComboDic[2].ParaArray.Count;
		i = 0;
		_Combo2Weight.Add(_ComboDic[2].ParaArray[i].Weight);
		for(i = 1; i<num;i++)
		{
			_Combo2Weight.Add(_Combo2Weight[i-1]+_ComboDic[2].ParaArray[i].Weight);
		}

		num = _ComboDic[3].ParaArray.Count;
		i = 0;
		_Combo3Weight.Add(_ComboDic[3].ParaArray[i].Weight);
		for(i = 1; i<num;i++)
		{
			_Combo3Weight.Add(_Combo3Weight[i-1]+_ComboDic[3].ParaArray[i].Weight);
		}

		num = _ComboDic[4].ParaArray.Count;
		i = 0;
		_Combo4Weight.Add(_ComboDic[4].ParaArray[i].Weight);
		for(i = 1; i<num;i++)
		{
			_Combo4Weight.Add(_Combo4Weight[i-1]+_ComboDic[4].ParaArray[i].Weight);
		}

		num = _ComboDic[5].ParaArray.Count;
		i = 0;
		_Combo5Weight.Add(_ComboDic[5].ParaArray[i].Weight);
		for(i = 1; i<num;i++)
		{
			_Combo5Weight.Add(_Combo5Weight[i-1]+_ComboDic[5].ParaArray[i].Weight);
		}
	}
	public int dropCount
	{
		get{return _DropCount;}

		set{

			if(_DropCount!=value&& value==0)
			{

//				if(_DropCount==1)
//				{
//					RewardCombo(1);
//				}
//				else if(_DropCount == 2)
//				{
//					RewardCombo(2);
//				}
//				else if(_DropCount == 3)
//				{
//					RewardCombo(3);
//				}
//				else if(_DropCount == 4)
//				{
//					RewardCombo(4);
//				}
//				else if(_DropCount >= 5)
//				{
//					RewardCombo(5);
//				}

			}
			_DropCount = value;
		}
	}
	public void RewardCombo(int combo)
	{
		int rand  = Random.Range(0,100);
		int num = 0;
		if(combo == 1)
		{
			num = _Combo1Weight.Count;
			for(int i = 0; i<num; i++)
			{
				if(rand<_Combo1Weight[i])
				{
					GetComboReward(_ComboDic[1].ParaArray[i].ItemID,_ComboDic[1].ParaArray[i].Num);
					return;
				}
			}
		}
		else if(combo == 2)
		{
			num = _Combo2Weight.Count;
			for(int i = 0; i<num; i++)
			{
				if(rand<_Combo2Weight[i])
				{
					GetComboReward(_ComboDic[2].ParaArray[i].ItemID,_ComboDic[2].ParaArray[i].Num);
					return;
				}
			}
		}
		else if(combo == 3)
		{
			num = _Combo3Weight.Count;
			for(int i = 0; i<num; i++)
			{
				if(rand<_Combo3Weight[i])
				{
					GetComboReward(_ComboDic[3].ParaArray[i].ItemID,_ComboDic[3].ParaArray[i].Num);
					return;
				}
			}
		}
		else if(combo == 4)
		{
			num = _Combo4Weight.Count;
			for(int i = 0; i<num; i++)
			{
				if(rand<_Combo4Weight[i])
				{
					GetComboReward(_ComboDic[4].ParaArray[i].ItemID,_ComboDic[4].ParaArray[i].Num);
					return;
				}
			}
		}
		else if(combo == 5)
		{
			num = _Combo5Weight.Count;
			for(int i = 0; i<num; i++)
			{
				if(rand<_Combo5Weight[i])
				{
					GetComboReward(_ComboDic[5].ParaArray[i].ItemID,_ComboDic[5].ParaArray[i].Num);
					return;
				}
			}
		}		
	}
	void GetComboReward(int id, int num)
	{
		for(int i = 0; i<num; i++)
		{
			cs_ObjManager.BCGameObjFactory(id,new Vector3(UnityEngine.Random.Range(-6,6),UnityEngine.Random.Range(4,16),UnityEngine.Random.Range(-4,5)),Vector3.zero,Vector3.zero);
		}
	
	}

	void GotCoinEnd()
	{
		bPlayGotCoin = false;
		CancelInvoke("GotCoinSound");
	}
	void GotCoinSound()
	{
//		BCSoundPlayer.Play(MusicEnum.getCoin);
	}
	bool bPlayGotCoin = false;
	public void ProcessDropObj()
	{
		if(DropQueue == null)return;
		if(DropQueue.Count ==0)
		{
			return ;
		}
		GameObject go = DropQueue.Dequeue();

		if(go == null)return;
		BCGameObj tem = go.GetComponent<BCGameObj>();

		if(tem == null)
		{
			HideObj(go);
			return;
		}

		CancelInvoke("GotCoinEnd");
		Invoke("GotCoinEnd",0.2f);
		
		if(!bPlayGotCoin)
		{
			bPlayGotCoin = true;
			InvokeRepeating("GotCoinSound",0.1f,0.15f);
		}

		WGAchievementManager.Self.processAchievement(tem.ID,DTAchievementType.GOT_COIN,tem.Value);
//		PlayerGetCoin(tem.Value);

		if(tem.Value>0)
		{
			V2CoinDropEffect cd = V2CoinDropEffect.CreateDropEffect();
			cd.labCoinNum.text =  "+"+tem.Value.ToString();
			cd.transform.parent = cs_BearManage.go3DUIFrontPanel.transform;
			cd.transform.position = go.transform.position;



//			GameObject goAdd = Instantiate(goAddCoin) as GameObject;
//			UILabel lab = goAdd.GetComponent<UILabel>();
//			lab.text = "+"+tem.Value.ToString();
//			goAdd.transform.parent = cs_BearManage.go3DUIFrontPanel.transform;
//			goAdd.transform.position = go.transform.position;

			Vector3 v3temp = cd.transform.localPosition;// goAdd.transform.localPosition;

			v3temp.z = -10f;
			v3temp.y = -1.4f;

			cd.transform.localPosition = v3temp;
			TweenPosition tp = cd.tpContent;// goAdd.GetComponent<TweenPosition>();

			tp.from = v3temp;
			tp.to = new Vector3(v3temp.x,Random.Range(3,7),v3temp.z);

			_mAddCoinNum2 +=tem.Value;

			CancelInvoke("IGotCoin");
			Invoke("IGotCoin",0.5f);

		}
		WGAchievementManager.Self.processAchievement(tem.ID,DTAchievementType.DROP_OBJ);
		PlayGetExp(tem.Exp);



		if(tem.Type == 1)
		{
			if(_mAddCoinNum == 0)
			{
				_FristDropTime = Time.time;
				_LastDropTime = Time.time;
				dropCount++;
				_mAddCoinNum +=tem.Value;

			}
			else
			{
				if(_LastDropTime-_FristDropTime< DeltaTime)
				{
					_LastDropTime = Time.time;
					dropCount++;
					_mAddCoinNum +=tem.Value;
				}
			}
		}



		float delt = 0;
		switch(tem.Type)
		{
		case 1:      //金币
			if(WGHelpManager.Self != null)
			{
				bool show = false;
				if(!show && !WGHelpManager.Self.StatesIsEnd(EMHelpStates.Drop_Coin))
				{
					show = true;
					Time.timeScale = 0;
					WGHelpManager.Self.ShowHelpView(EMHelpStates.Drop_Coin);
				}
			}
			break;
		case 2:  //道具币
			qReleaseSkillQueue.Enqueue(tem.ID);

			break;
		case 3:  //收藏品
			qDropCollection.Enqueue(tem.ID);
			WGAchievementManager.Self.processAchievement(tem.ID,DTAchievementType.GOT_ITEM);
			WGAchievementManager.Self.processAchievement(tem.ID,DTAchievementType.GOT_ITEM_GROUP);
			PlayerGetCollection(tem.ID);
			delt = 2;
			break;
		}
		HideObj(go,delt);

	}

	bool bGoodViewShow = false;
	void IGotCoin()
	{
		if(_mAddCoinNum2>350)
		{
			if(bGoodViewShow)
			{
			}
			else
			{
				bGoodViewShow = true;
				WGGoodView gv = WGGoodView.CreateGoodView();
				SDK.AddChild(gv.gameObject,WGRootManager.Self.goRootGameUI);
				gv.AddCoinNum(_mAddCoinNum2);
	//			WGAlertManager.Self.AddAction(()=>{
				gv.alertViewBehavriour =(ab,view)=>{
					switch(ab)
					{
					case MDAlertBehaviour.DID_HIDDEN:
						Destroy(view.gameObject);
						bGoodViewShow = false;
	//						WGAlertManager.Self.RemoveHead();
	//						WGAlertManager.Self.ShowNext();
					break;
					}
				};
				gv.freshUIWith();
				gv.showView();

//			});
				
//			WGAlertManager.Self.ShowNext();
			}
		}

		PlayerGetCoin(_mAddCoinNum2);
		
//		WGGotCoinEffectView.Self.OneCoinDropAtPos(_mAddCoinNum2);
		_mAddCoinNum2 = 0;
	}

	void ProcessAddCoin()
	{
		if((Time.time-_FristDropTime >DeltaTime))
		{
			if(_mAddCoinNum>0)
			{
//				WGGameUIView.Instance.mPlayerInfoView.PlayAddCoinEffect(_mAddCoinNum);
				WGGameUIView.Instance.mPlayerInfoView.PlayAddCoinEffect1(_mAddCoinNum);
			}

			_FristDropTime = Time.time;
			_LastDropTime = Time.time;
			dropCount = 0;
			_mAddCoinNum = 0;

		}
	}
	void ProcessSpecialCoin()
	{
		if(qReleaseSkillQueue.Count>0)
		{
			SpecialCoinAction(qReleaseSkillQueue.Dequeue());
		}
	}



	//道具币事件
	public void SpecialCoinAction(int id)
	{

		WGSkillController.Instance.ReleaseSkillWithID(id);

	}

	public void HideObj(GameObject g,float time=0)
	{
		if(!DropQueue.Contains(g))
		{
			cs_ObjManager.HideObj(g,time);
		}
	}

	public void PlayerGetCollection(int id)
	{
		WGGameUIView.Instance.freshMenuButton(2);
		DataPlayerController.getInstance().addCollectionNum(id,1);

	}
}
