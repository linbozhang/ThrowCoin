using UnityEngine;
using System.Collections;

public class CRLuo_Move_LightningMove : MonoBehaviour
{
	public string _ = "-=<物体闪电形状位置程序>=-";
	public string __ = "吸附轴向";
	public bool X_Restrain = true;
	public bool Y_Restrain = false;
	public bool Z_Restrain = false;

	public string ___ = "向下一个位置移动时间";
	public float Time1 = 0.01f;
	public float Time2 = 0.1f;

	public string ____ = "目标点的随机位置范围";
	public float X_1 = -1;
	public float X_2 = 1;
	public float Y_1 = 0;
	public float Y_2 = 0;
	public float Z_1 = -1;
	public float Z_2 = 0;

	public string _____ = "停止时间";
	public float StopTime = 0;

	Vector3 NowPos;
	float MyTime;
	float My_X;
	float My_Y;
	float My_Z;
	float Old_X;
	float Old_Y;
	float Old_Z;
	float Now_X;
	float Now_Y;
	float Now_Z;


	// Use this for initialization
	void Start () {

		if (StopTime > 0)
		{

			Destroy(this, StopTime);
		}

		NowPos = this.transform.position;
		Old_X = Now_X = NowPos.x;
		Old_Y = Now_Y = NowPos.y;
		Old_Z = Now_Z = NowPos.z;
		RendMoveMain();


	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void RendMoveMain()
	{
		MyTime = Random.Range(Time1, Time2);
		My_X = Random.Range(X_1, X_2);
		My_Y = Random.Range(Y_1, Y_2);
		My_Z = Random.Range(Z_1, Z_2);


		if (X_Restrain)
		{
			if (((Now_X + My_X) > Old_X && My_X>0)&&((Now_X + My_X) < Old_X && My_X < 0))
			{
				Now_X -= My_X;
			}
			else  
			{
				Now_X += My_X;
			}


		}
		else
		{
			Now_X += My_X;
		
		}

		if (Y_Restrain)
		{
			if (((Now_Y + My_Y) > Old_Y && My_Y >0)&&((Now_Y + My_Y) < Old_Y && My_Y < 0))
			{
				Now_Y -= My_Y;
			}
			else  
			{
				Now_Y += My_Y;
			}


		}
		else
		{
			Now_Y += My_Y;

		}

		if (Z_Restrain)
		{
			if (((Now_Z + My_Z) > Old_Z && My_Z > 0) && ((Now_Z + My_Z) < Old_Z && My_Z < 0))
			{
				Now_Z -= My_Z;
			}else	
			{
				Now_Z += My_Z;
			}


		}
		else
		{
			Now_Z += My_Z;

		}


		MiniItween.MoveTo(this.gameObject, new Vector3(Now_X, Now_Y, Now_Z), MyTime,true);

		Invoke("RendMoveMain", MyTime);


	}
}
