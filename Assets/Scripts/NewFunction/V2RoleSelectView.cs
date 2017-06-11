using UnityEngine;
using System.Collections;

public class V2RoleSelectView : MDBaseAlertView {

	public TweenScale tsContent;

	public Color cSelect;
	public Color cUnSelect;
	public UILabel labTitle;
	public UILabel labAttribute;
	public UILabel labBtnTitle;
	public UILabel[] szLabNames;
	public UILabel[] szLabPrecent;
	public UILabel labAttack;
	public UILabel labMaxCoin;
	public UILabel labAdditional;
	public UISlider[] sdAttPrecent;
	public UISprite[] szBottomQuan;
	public UISprite[] szHeadGuang;

	GameObject goThreeRole;

	int curSelectIndex = 1;

	bool  bInit=false;

	void Start()
	{
		InitView();
	}


	void InitView()
	{
		if(bInit)return;
		bInit = true;
		int index = DataPlayerController.getInstance().data.r;

		SelectWithIndex(index);
	}




	void OnBtnSelectGGW()
	{
		SelectWithIndex(0);
	}
	void OnBtnSelectHXZ()
	{
		SelectWithIndex(1);
	}
	void OnBtnSelectXMG()
	{
		SelectWithIndex(2);
	}
	void SelectWithIndex(int index)
	{
		DataPlayerController.getInstance().data.r = index;
		szBottomQuan[curSelectIndex].color = cUnSelect;
		szHeadGuang[curSelectIndex].color = cUnSelect;
		curSelectIndex = index;
		szBottomQuan[curSelectIndex].color = cSelect;
		szHeadGuang[curSelectIndex].color = cSelect;

		szLabPrecent[0].text = Core.fc.szRoles[curSelectIndex].attack.ToString()+"%";
		szLabPrecent[1].text = Core.fc.szRoles[curSelectIndex].max.ToString();
		szLabPrecent[2].text = Core.fc.szRoles[curSelectIndex].add.ToString();
		sdAttPrecent[0].value = Core.fc.szRoles[curSelectIndex].attV;
		sdAttPrecent[1].value = Core.fc.szRoles[curSelectIndex].maxV;
		sdAttPrecent[2].value = Core.fc.szRoles[curSelectIndex].addV;

	}

	#region MDBaseAlertView

	public override void showView ()
	{
		base.showView ();
		Time.timeScale = 0;
	
		tsContent.ESetActive(true);
		tsContent.PlayForward();

		InvokeBlock(tsContent.duration,()=>{
			showViewEnd();
		});
	}
	public override void showViewEnd ()
	{
		base.showViewEnd ();
		Object obj = Resources.Load("pbSelectRolePanel");
		goThreeRole = Instantiate(obj) as GameObject;
		goThreeRole.transform.position = new Vector3(100,0,0);
	}
	public override void hiddenView ()
	{
		Time.timeScale = 1;
		base.hiddenView ();
		tsContent.PlayReverse();
		Destroy(goThreeRole);
		InvokeBlock(tsContent.duration,()=>{
			hiddenViewEnd();
		});

	}
	public override void hiddenViewEnd ()
	{
		base.hiddenViewEnd ();
	}
	public override void OnBtnOk ()
	{
		base.OnBtnOk ();
	}

	#endregion


	public static V2RoleSelectView CreateRoleView()
	{
		Object obj = Resources.Load("pbV2RoleSelectView");
		if(obj != null)
		{
			GameObject go = Instantiate(obj) as GameObject;
			V2RoleSelectView v = go.GetComponent<V2RoleSelectView>();
			SDK.AddChild(go,WGRootManager.Self.goRootTopUI);
			v.InitView();
			return v;
		}
		return null;
	}
}
