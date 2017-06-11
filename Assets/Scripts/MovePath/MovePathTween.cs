using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovePathTween : MonoBehaviour {

	public TweenTransform ttNiao;
	public List<MDMoveTween> szPaths;
	int mIndex =0 ;
	// Use this for initialization
	void Start () {
		resetTTNiao();
	}
	void OnMyFinish()
	{
		if(mIndex<szPaths.Count-1)
		{
			mIndex ++;
			resetTTNiao();
		}
	}
	void resetTTNiao()
	{

		MDMoveTween mt = szPaths[mIndex];

		if(mt.from != null)
		{
			ttNiao.from = mt.from;
		}
		ttNiao.to = mt.to;
		ttNiao.duration = mt.time;
		ttNiao.animationCurve = mt.animationCurve;
		ttNiao.ResetToBeginning();
		ttNiao.PlayForward();
	}
}
[System.Serializable]
public class MDMoveTween{
	public Transform from;
	public Transform to;
	public float time;
	public AnimationCurve animationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 1f), new Keyframe(1f, 1f, 1f, 0f));
}