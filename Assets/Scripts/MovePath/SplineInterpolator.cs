﻿using UnityEngine;
using System.Collections;

public enum eEndPointsMode { AUTO, AUTOCLOSED, EXPLICIT }
public enum eWrapMode { ONCE, LOOP }
public delegate void OnEndCallback();

public class SplineInterpolator : MonoBehaviour
{

	[System.NonSerialized]
	public SplineController mSplineController;

	eEndPointsMode mEndPointsMode = eEndPointsMode.AUTO;

	internal class SplineNode
	{
		internal Vector3 Point;
		internal Quaternion Rot;
		internal float Time;
		internal Vector2 EaseIO;

		internal SplineNode(Vector3 p, Quaternion q, float t, Vector2 io) { Point = p; Rot = q; Time = t; EaseIO = io; }
		internal SplineNode(SplineNode o) { Point = o.Point; Rot = o.Rot; Time = o.Time; EaseIO = o.EaseIO; }
	}

	//List<SplineNode> mNodes = new List<SplineNode>();
	ArrayList mNodes = new ArrayList();
	string mState = "";

	OnEndCallback mOnEndCallback;



	void Awake()
	{
		Reset();
	}

	public void StartInterpolation(OnEndCallback endCallback, eWrapMode mode)
	{
		if (mState != "Reset")
			throw new System.Exception("First reset, add points and then call here");

		mState = mode == eWrapMode.ONCE ? "Once" : "Loop";
		mOnEndCallback = endCallback;

		SetInput();
	}

	public void Reset()
	{
		mNodes.Clear();
		mState = "Reset";
		mCurrentIdx = 1;
		mCurrentTime = 0;
		mEndPointsMode = eEndPointsMode.AUTO;
	}

	public void AddPoint(Vector3 pos, Quaternion quat, float timeInSeconds, Vector2 easeInOut)
	{
		if (mState != "Reset")
			throw new System.Exception("Cannot add points after start");

		mNodes.Add(new SplineNode(pos, quat, timeInSeconds, easeInOut));
	}


	void SetInput()
	{
		if (mNodes.Count < 2)
			throw new System.Exception("Invalid number of points");

		if (mEndPointsMode == eEndPointsMode.AUTO)
		{
			mNodes.Insert(0, mNodes[0]);
			mNodes.Add(mNodes[mNodes.Count - 1]);
		}
		else if (mEndPointsMode == eEndPointsMode.EXPLICIT && (mNodes.Count < 4))
			throw new System.Exception("Invalid number of points");
	}

	void SetExplicitMode()
	{
		if (mState != "Reset")
			throw new System.Exception("Cannot change mode after start");

		mEndPointsMode = eEndPointsMode.EXPLICIT;
	}

	public void SetAutoCloseMode(float joiningPointTime)
	{
		if (mState != "Reset")
			throw new System.Exception("Cannot change mode after start");

		mEndPointsMode = eEndPointsMode.AUTOCLOSED;

		mNodes.Add(new SplineNode(mNodes[0] as SplineNode));
		((SplineNode)mNodes[mNodes.Count - 1]).Time = joiningPointTime;

		Vector3 vInitDir = (((SplineNode)mNodes[1]).Point - ((SplineNode)mNodes[0]).Point).normalized;
		Vector3 vEndDir = (((SplineNode)mNodes[mNodes.Count - 2]).Point - ((SplineNode)mNodes[mNodes.Count - 1]).Point).normalized;
		float firstLength = (((SplineNode)mNodes[1]).Point - ((SplineNode)mNodes[0]).Point).magnitude;
		float lastLength = (((SplineNode)mNodes[mNodes.Count - 2]).Point - ((SplineNode)mNodes[mNodes.Count - 1]).Point).magnitude;

		SplineNode firstNode = new SplineNode(mNodes[0] as SplineNode);
		firstNode.Point = ((SplineNode)mNodes[0]).Point + vEndDir * firstLength;

		SplineNode lastNode = new SplineNode(mNodes[mNodes.Count - 1] as SplineNode);
		lastNode.Point = ((SplineNode)mNodes[0]).Point + vInitDir * lastLength;

		mNodes.Insert(0, firstNode);
		mNodes.Add(lastNode);
	}

	float mCurrentTime;
	int mCurrentIdx = 1;

	public Quaternion targetRotation;

	void Update()
	{
		if (mState == "Reset" || mState == "Stopped" || mNodes.Count < 4)
			return;

		mCurrentTime += Time.deltaTime;

		// We advance to next point in the path
		if (mCurrentTime >= ((SplineNode)mNodes[mCurrentIdx + 1]).Time)
		{
			if (mCurrentIdx < mNodes.Count - 3)
			{
				mCurrentIdx++;
				if (mSplineController.RotateEnable) {
					targetRotation = mSplineController.LookRotation(mCurrentIdx - 1, mCurrentIdx);
				}
			}
			else
			{
				if (mState != "Loop")
				{
					mState = "Stopped";

					// We stop right in the end point
					transform.position = ((SplineNode)mNodes[mNodes.Count - 2]).Point;

					// We call back to inform that we are ended
					if (mOnEndCallback != null)
						mOnEndCallback();
				}
				else
				{
					mCurrentIdx = 1;
					mCurrentTime = 0;
					if (mSplineController.RotateEnable) {
						transform.rotation = mSplineController.LookRotation(0, 1);
						targetRotation = this.transform.rotation;
					}
				}
			}
		}

		if (mState != "Stopped")
		{
			// Calculates the t param between 0 and 1
			float param = (mCurrentTime - ((SplineNode)mNodes[mCurrentIdx]).Time) / (((SplineNode)mNodes[mCurrentIdx + 1]).Time - ((SplineNode)mNodes[mCurrentIdx]).Time);

			// Smooth the param
			param = MathUtils.Ease(param, ((SplineNode)mNodes[mCurrentIdx]).EaseIO.x, ((SplineNode)mNodes[mCurrentIdx]).EaseIO.y);

			transform.position = GetHermiteInternal(mCurrentIdx, param);

			//Rotate
			if (mSplineController.RotateEnable) {
				this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, mSplineController.RotateSpeed);
			}

		}
	}

	Quaternion GetSquad(int idxFirstPoint, float t)
	{
		Quaternion Q0 = ((SplineNode)mNodes[idxFirstPoint - 1]).Rot;
		Quaternion Q1 = ((SplineNode)mNodes[idxFirstPoint]).Rot;
		Quaternion Q2 = ((SplineNode)mNodes[idxFirstPoint + 1]).Rot;
		Quaternion Q3 = ((SplineNode)mNodes[idxFirstPoint + 2]).Rot;

		Quaternion T1 = MathUtils.GetSquadIntermediate(Q0, Q1, Q2);
		Quaternion T2 = MathUtils.GetSquadIntermediate(Q1, Q2, Q3);

		return MathUtils.GetQuatSquad(t, Q1, Q2, T1, T2);
	}



	public Vector3 GetHermiteInternal(int idxFirstPoint, float t)
	{
		float t2 = t * t;
		float t3 = t2 * t;

		Vector3 P0 = ((SplineNode)mNodes[idxFirstPoint - 1]).Point;
		Vector3 P1 = ((SplineNode)mNodes[idxFirstPoint]).Point;
		Vector3 P2 = ((SplineNode)mNodes[idxFirstPoint + 1]).Point;
		Vector3 P3 = ((SplineNode)mNodes[idxFirstPoint + 2]).Point;

		float tension = 0.5f;	// 0.5 equivale a catmull-rom

		Vector3 T1 = tension * (P2 - P0);
		Vector3 T2 = tension * (P3 - P1);

		float Blend1 = 2 * t3 - 3 * t2 + 1;
		float Blend2 = -2 * t3 + 3 * t2;
		float Blend3 = t3 - 2 * t2 + t;
		float Blend4 = t3 - t2;

		return Blend1 * P1 + Blend2 * P2 + Blend3 * T1 + Blend4 * T2;
	}


	public Vector3 GetHermiteAtTime(float timeParam)
	{
		if (timeParam >= ((SplineNode)mNodes[mNodes.Count - 2]).Time)
			return ((SplineNode)mNodes[mNodes.Count - 2]).Point;

		int c;
		for (c = 1; c < mNodes.Count - 2; c++)
		{
			if (((SplineNode)mNodes[c]).Time > timeParam)
				break;
		}

		int idx = c - 1;
		float param = (timeParam - ((SplineNode)mNodes[idx]).Time) / (((SplineNode)mNodes[idx + 1]).Time - ((SplineNode)mNodes[idx]).Time);
		param = MathUtils.Ease(param, ((SplineNode)mNodes[idx]).EaseIO.x, ((SplineNode)mNodes[idx]).EaseIO.y);

		return GetHermiteInternal(idx, param);
	}
}