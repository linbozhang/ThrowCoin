using UnityEngine;
using System.Collections;

public class WGItemViewTabCollection : WGViewMonoBehaviour {


	public override void ViewDidLoad ()
	{
		base.ViewDidLoad ();
	}
	public override void ViewWillShow ()
	{
		base.ViewWillShow ();


	}
	public override void ViewDidShow ()
	{
		base.ViewDidShow ();
		WGGameWorld.Instance.goFourthCamera.SetActive(true);
		WGCameraScrollViewForCollection.Instance.InitScrollView();
		WGCameraScrollViewForCollection.Instance.freshScrollView();
	}
	public override void ViewWillHidden ()
	{
		base.ViewWillHidden ();

		WGGameWorld.Instance.goFourthCamera.SetActive(false);
	}
	public override void ViewDidHidden ()
	{
		base.ViewDidHidden ();
		WGGameWorld.Instance.goFourthCamera.SetActive(false);
	}
	

}
