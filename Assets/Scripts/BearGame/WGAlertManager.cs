using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WGAlertManager : MonoBehaviour {


	[HideInInspector]
	public bool _bAlertShow = false;
	[HideInInspector]
	public bool bPause = false;
	bool bWillShow = false;
	public bool bBuySKill = false;
	public bool bSuperTiger = false;

//	Queue<System.Action> qAlertAction = new Queue<System.Action>();
	List<System.Action> szAlertAction = new List<System.Action>();

	public static WGAlertManager Self = null;
	void Awake()
	{
		Self = this;
	}

	public void AddAction( System.Action act)
	{
		szAlertAction.Add(act);
	}

	public void ShowNext()
	{
		if(_bAlertShow==false&&!bPause)
		{
			bWillShow = false;
			if(szAlertAction.Count>0)
			{
				_bAlertShow = true;
				szAlertAction[0]();
			}
		}
		else
		{
			bWillShow = true;
		}
	}
	public void Update()
	{
		if(bWillShow)
		{
			ShowNext();
		}
	}
	public void RemoveHead()
	{
		if(szAlertAction.Count>0)
		{
			_bAlertShow = false;
			szAlertAction.RemoveAt(0);
		}
	}
	public void RemoveHeadAndShowNext()
	{
		RemoveHead();
		ShowNext();
	}
}

