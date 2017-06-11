using UnityEngine;
using System.Collections;

public class WGWindow : MonoBehaviour {

	public static float fViewWith;
	public static float fViewHeight;

	public static float fTop;
	public static float fBottom;
	public static float fLeft;
	public static float fRight;

	public Camera mMainCamera;

	public static WGWindow Instance;
	void Awake()
	{
		Instance = this;

		if(mMainCamera == null)
		{
			mMainCamera = FindObjectOfType<Camera>();
		}


		fViewHeight = mMainCamera.orthographicSize*2;
		fViewWith = Screen.width*mMainCamera.orthographicSize/Screen.height;

		fTop = fViewHeight/2;
		fBottom = - fTop;
		fRight = fViewWith/2;
		fLeft = -fRight;
	}
}
