using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading;
using System.Linq;

public class AsyncTask : MonoBehaviour {


	private static AsyncTask _current;
	private int _count;
	public static AsyncTask Current {
		get {
			Initialize();
			return _current;
		}
	}

	void Awake()
	{
		_current = this;
		initialized = true;
	}

	private static bool initialized=false;
	static void Initialize() {
		if (initialized == false) {
			if(Application.isPlaying==false)return;
			initialized = true;
			var g = new GameObject("AsyncTask");
			_current = g.AddComponent<AsyncTask>();
			DontDestroyOnLoad(g);
		}

	}

	/*	
	 * Action will be excute immediately
	 */
	private List<Action> _actions = new List<Action>();
	List<Action> _currentActions = new List<Action>();


	public static void QueueOnMainThread(Action action) {

		lock (Current._actions)
			Current._actions.Add(action);
	}

	// Update is called once per frame
	void Update() {
		lock (_actions) {
			_currentActions.Clear();
			if(_actions.Count>0)
			{
				_currentActions.AddRange(_actions);
			}
			_actions.Clear();
		}
		if(_currentActions.Count>0)
		{
			foreach(var work in _currentActions)
			{
				work();
			}
		}
	}
}



