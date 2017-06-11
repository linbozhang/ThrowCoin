using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;




//游戏中物品的加载和生成
public class BCObjManager : MonoBehaviour {


	private GameObject _Obj;

	public List<GameObject> _szLiveCoin=new List<GameObject>();
	public int[] _szDeskCollection = new int[16];

	private BCGameObj _CoinProperty;//金币属性的临时变量
	
	private GameObject _BearCoinRoot;//
	
	public void InitWithGameWorld(WGGameWorld World)
	{

	}

	public void SetBearGameCoinRoot(GameObject root)
	{
		_BearCoinRoot = root;
	}
	//创建游戏物体工厂
	public GameObject BCGameObjFactory(int id,Vector3 po,Vector3 ro, Vector3 scale)
	{

		BCObj mObj = WGDataController.Instance.GetBCObj(id);

		switch(mObj.BCType)
		{
		case BCGameObjectType.Bear:	
			{
				_Obj = Instantiate(mObj.goRes,po,Quaternion.Euler(ro)) as GameObject;
				_Obj.transform.localScale = Vector3.one*0.8f;		
			}
				break;
		case BCGameObjectType.ExpCoin:
		case BCGameObjectType.Item:
		case BCGameObjectType.Coin:
			{

				_Obj = Instantiate(mObj.goRes) as GameObject;

				_Obj.transform.parent = _BearCoinRoot.transform;

				_Obj.transform.position = po;
				_Obj.transform.rotation = Quaternion.Euler(new Vector3(0,0,180));
				_Obj.SetActive(true);
			
				_Obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
				_Obj.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

				Vector3 ls = Vector3.one;
				if(Core.fc.dicCoinScale.ContainsKey(mObj.ID))
				{
					ls = Core.fc.dicCoinScale[mObj.ID];
				}
				else
				{
					switch(id)
					{
					case WGDefine.Value100Coin:
						ls = ls*0.7f;
						ls.y = 0.9f;
						break;
					case WGDefine.Value10Coin:
						ls = ls*0.55f;
						ls.y = 0.75f;
						break;
					case WGDefine.Value5Coin:
					case WGDefine.Value3Coin:
					case WGDefine.Value2Coin:
						ls = ls*0.434f;
						ls.y = 0.634f; 
						break;
					case WGDefine.CommonCoin:
						ls = ls*0.4f;
						ls.y = 0.6f;
						break;
						default:
						ls = ls*0.5f;
						ls.y = 0.6f;
						break;
					}
				}
			
				_Obj.transform.localScale = ls;

				_CoinProperty = _Obj.GetComponent<BCGameObj>();
				_CoinProperty.freshWithData(mObj);

				_szLiveCoin.Add(_Obj);

			}

			break;

		case BCGameObjectType.Jewel:// 宝石
			
			break;
		case BCGameObjectType.Collection:
			_Obj = Instantiate(mObj.goRes,po,Quaternion.Euler(new Vector3(270,0,0))) as GameObject;
			_Obj.transform.parent = _BearCoinRoot.transform;
//			_Obj.transform.localScale = Vector3.one*0.6f;

			_CoinProperty = _Obj.GetComponent<BCGameObj>();
			_CoinProperty.freshWithData(mObj);
			_szDeskCollection[id-3000]++;

			_szLiveCoin.Add(_Obj);

			break;
		}

		return _Obj;
	}
	public void AddLiveCoin(GameObject go)
	{
		_szLiveCoin.Add(go);
	}
	public void HideObj(GameObject g,float time)
	{
		if(_szLiveCoin.Contains(g))
		{
			_szLiveCoin.Remove(g);
			BCGameObj bcg = g.GetComponent<BCGameObj>();
			if(bcg != null)
			{
				BCGameObjectType tp = WGDataController.Instance.GetBCObjType(bcg.ID);
				if(tp == BCGameObjectType.Collection)
				{
					_szDeskCollection[bcg.ID-3000] --;
				}
			}
		}
		Destroy(g,time);
	}
	public void CoinShaking()
	{
		GameObject _CoinObj=null;

		int count = _szLiveCoin.Count;
		int index = 0;

		while(index<count)
		{
			_CoinObj = _szLiveCoin[index];
			index ++;
			if(_CoinObj == null)
			{			
				index --;
				_szLiveCoin.RemoveAt(index);
				count --;
			}
			else{
				if ( !_CoinObj.GetComponent<Rigidbody>().isKinematic)
				{
					_CoinObj.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(-10f,10f),Random.Range(12,17),Random.Range(-4,-2));
					_CoinObj.GetComponent<Rigidbody>().angularVelocity = new Vector3(Random.Range(-3,3),Random.Range(-3,3),Random.Range(-3,3));
				}
			}
		}


	}


}
