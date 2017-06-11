using UnityEngine;
using System.Collections;
//Create NewBehaviourScript by Song
// Copy Right 2014®
public class SHMove: MonoBehaviour {

//	public Transform transBegin;
//	public Transform transEnd;
	public Transform[] szPaths;
//	public int[] szPathIDs;
	public int mBeginIndex = 0;
	public float speed=20;
	public bool bPathMove = false;
	int mNextIndex = 1;

	public AnimationCurve aCurve = AnimationCurve.Linear(0,0,1,1);

	
	Transform _trans;
	Transform _transform{
		get{
			if(_trans == null)_trans = transform;
			return _trans;
		}
	}
	float distance = 0;
	// Use this for initialization
	void Start () {
		mNextIndex = mBeginIndex+1;
		if(mNextIndex>=szPaths.Length)
		{
			mNextIndex = 0;
		}
		distance = Vector3.Distance(szPaths[mBeginIndex].localPosition,szPaths[mNextIndex].localPosition)/100;
		bPathMove = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(bPathMove)
		{
			if(szPaths.Length>1)
			{
				if(Vector3.Distance(_transform.localPosition,szPaths[mNextIndex].localPosition)>distance)
				{

//					aCurve.
					_transform.localPosition = Vector3.MoveTowards(_transform.localPosition,
						szPaths[mNextIndex].localPosition,aCurve.Evaluate(Time.deltaTime) * speed);
				}
				else{
					_transform.localPosition = szPaths[mNextIndex].localPosition;
					mBeginIndex++;
					if(mBeginIndex>=szPaths.Length)
					{
						mBeginIndex=0;
					}
					mNextIndex = mBeginIndex+1;
					if(mNextIndex>=szPaths.Length)
					{
						mNextIndex = 0;
					}
					distance = Vector3.Distance(szPaths[mBeginIndex].localPosition,szPaths[mNextIndex].localPosition)/100;
				}
			}
		}

	}
}