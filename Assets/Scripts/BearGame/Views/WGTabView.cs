using UnityEngine;
using System.Collections;

public class WGTabView : MonoBehaviour {

	public TweenPosition tpTitleBack;

	public Color colorHight;
	public Color colorNomarl;

	GameObject goProView;
	GameObject goCurView;

	UILabel proTitle = null;
	UILabel curTitle = null;

	Vector3 proPostion=Vector3.zero;
	Vector3 curPostion=Vector3.zero;




	public void InitState(Vector3 pos,UILabel title,GameObject content)
	{

		title.color = colorHight;

		tpTitleBack.ResetToBeginning();
		tpTitleBack.to = pos;
		tpTitleBack.PlayForward();
		content.SetActive(true);

		curTitle = title;
		goCurView = content; 
		curPostion = pos;
	}

	public void ChangeState(Vector3 pos,UILabel title,GameObject content)
	{

		if(title != curTitle)
		{
			curTitle.color = colorNomarl;
		}
		title.color = colorHight;

		tpTitleBack.ResetToBeginning();
		tpTitleBack.from = curPostion;
		tpTitleBack.to = pos;
		tpTitleBack.PlayForward();

		if(content != goCurView)
		{
			goCurView.SetActive(false);
		}
		content.SetActive(true);


		curTitle = title;
		goCurView = content; 
		curPostion = pos;

	}
}
