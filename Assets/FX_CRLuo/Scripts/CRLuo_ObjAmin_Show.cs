using UnityEngine;
using System.Collections;

public class CRLuo_ObjAmin_Show : MonoBehaviour {

	public float Value;

	string str_CurAnim;

	public AddFX[] MainAddFX;

	int NewValue;
	int OldValue = 0;

	public bool Try_key;
	// Use this for initialization
	void Start () {
	
	}
	void OnEnable()
	{
		NewValue = 0;
		GetComponent<Animation>().CrossFade("Take 001");
	}

	// Update is called once per frame
	void Update () {

		NewValue = (int)Mathf.Floor(Value * 10 / 2f);
		
		if (NewValue != OldValue)
		{

			switch (NewValue)
			{ 
			case 0 :
				if (str_CurAnim != "Take 001")
				GetComponent<Animation>().CrossFade("Take 001");
				str_CurAnim = "Take 001";
				StartCoroutine("ShowFX", 0);
				break;
			case 1:
				if (str_CurAnim != "Take 0010")
					GetComponent<Animation>().CrossFade("Take 0010");
				str_CurAnim = "Take 0010";
				StartCoroutine("ShowFX", 1);
				break;
			case 2:
				if (str_CurAnim != "Take 0011")
					GetComponent<Animation>().CrossFade("Take 0011");
				str_CurAnim = "Take 0011";
				StartCoroutine("ShowFX", 2);
				break;
			case 3:
				if (str_CurAnim != "Take 0012")
					GetComponent<Animation>().CrossFade("Take 0012");
				str_CurAnim = "Take 0012";
				StartCoroutine("ShowFX", 3);
				break;
			case 4:
				if (str_CurAnim != "Take 0013")
					GetComponent<Animation>().CrossFade("Take 0013");
				str_CurAnim = "Take 0013";
				StartCoroutine("ShowFX", 4);
				break;
			case 5:
				if (str_CurAnim != "Take 0014")
				{
					GetComponent<Animation>().CrossFade("Take 0014");
					//TODO
//					SmallGamePanel.GameWinnter();
				}
				str_CurAnim = "Take 0014";
				StartCoroutine("ShowFX", 5);
				break; 
			
			
			}


			OldValue = NewValue;
		}

		if (Try_key)
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
			{
				Value += 0.1f;
			}
			else if (Input.GetKeyDown(KeyCode.DownArrow))
			{
				Value -= 0.1f;
			}
		
		
		}
	}


	IEnumerator ShowFX( int ID)
	{
		if (MainAddFX != null && MainAddFX.Length != 0)
		{
			for (int i = 0; i < MainAddFX.Length; i++)
			{
				if ((int)Mathf.Floor(MainAddFX[i].AminID * 10 / 2f) == ID && MainAddFX[i].go_TargetBones != null)
				{

					for (int j = 0; j < MainAddFX[i].myAddFxElement.Length; j++)
					{

						if (MainAddFX[i].myAddFxElement[j].ON_OFF && MainAddFX[i].myAddFxElement[j].Prefab_FX != null)
						{
							yield return new WaitForSeconds(MainAddFX[i].myAddFxElement[j].FXtime);

							GameObject temp = (GameObject)Instantiate(MainAddFX[i].myAddFxElement[j].Prefab_FX);

							if (!MainAddFX[i].myAddFxElement[j].UseNoParent)
							{
								//temp临时对象 继承绑定物体位置
								temp.transform.parent =MainAddFX[i].go_TargetBones.transform;
								//调整特效位置
								temp.transform.localPosition = MainAddFX[i].myAddFxElement[j].v3_FXPos_offset;
							}
							else
							{
								temp.transform.position = MainAddFX[i].go_TargetBones.transform.position + MainAddFX[i].myAddFxElement[j].v3_FXPos_offset;
							}




							//调整特效旋转
							temp.transform.localRotation = Quaternion.Euler(
															 MainAddFX[i].myAddFxElement[j].v3_FXRotation_offset.x,
															 MainAddFX[i].myAddFxElement[j].v3_FXRotation_offset.y,
															 MainAddFX[i].myAddFxElement[j].v3_FXRotation_offset.z
															 );
						
						
						}


					}
				}
			}
		}
	
	}
}


[System.Serializable]
public class AddFX
{
	public float AminID;


	public GameObject go_TargetBones;

	public AddFxElement[] myAddFxElement;

}
[System.Serializable]
public class AddFxElement
{

	public bool ON_OFF = true;
	public bool UseNoParent = false;
	//定义物体特效对象变量
	public GameObject Prefab_FX;

	//定义攻击延迟之间变量
	public float FXtime;

	//定义绑定位置位置偏移
	public Vector3 v3_FXPos_offset;

	//定义绑定位置渲染偏移
	public Vector3 v3_FXRotation_offset;
}