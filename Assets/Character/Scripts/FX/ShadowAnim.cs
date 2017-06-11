using UnityEngine;
using System.Collections;

public class ShadowAnim : MonoBehaviour
{

	public Transform targetBone;

	private Vector3 v3_Offset;

	public bool ScaleKey = false;

	public float ScaleRatio = -0.5f;

	public bool OpacityKey = false;

	public float OpacityRatio = -0.5f;

	private Vector3 v3_OriginPos;

	private Vector3 v3_OriginScale;

	private Color C_OriginColor;

	[System.NonSerialized]
	public Vector3 Old_Position;

	[System.NonSerialized]
	public float ShowOpacityKey = 1;
	[System.NonSerialized]
	public float ShowOpacityKeyOld;
	[System.NonSerialized]
	public float temp_opacity;
	//private Vector3 v3_Old_OriginPos;


	void Start()
	{
		//如果没有参数直接销毁代码
		//if (targetBone == null)
		//{
		//      Destroy(this);
		//      return;
		//}


		if (targetBone != null)
		{
			v3_Offset = targetBone.position - this.transform.position;
			v3_OriginPos = this.transform.position;
			v3_OriginScale = this.transform.localScale;
			

		}
		C_OriginColor = GetComponent<Renderer>().material.GetColor("_TintColor");

	}

	void LateUpdate()
	{

		if (targetBone != null)
		{

				Vector3 targetPos = targetBone.position + v3_Offset;
				//targetPos.y = v3_OriginPos.y;
				targetPos.y = 0;
			//GGGGGGG
				this.transform.position = new Vector3 (targetPos.x,v3_OriginPos.y,targetPos.z);

				float offsetHeight = targetBone.position.y - (v3_OriginPos.y + v3_Offset.y);
			
			//===edit=====
			
				if (ScaleKey&&false)
				{
					Vector3 targetScale = v3_OriginScale + offsetHeight * ScaleRatio * Vector3.one;

					if (targetScale.x < 0)
					{
						targetScale = Vector3.zero;
					}



					this.transform.localScale = targetScale;

				}

						//===edit=====
				if (OpacityKey)
				{
					temp_opacity = (C_OriginColor.a + offsetHeight * OpacityRatio) * ShowOpacityKey;


				}
				else
				{
					temp_opacity = C_OriginColor.a * ShowOpacityKey;
				}


		}
		else
		{

			temp_opacity = C_OriginColor.a * ShowOpacityKey;
			//Vector3 targetScale = v3_OriginScale;
		}


		if (ShowOpacityKeyOld != ShowOpacityKey)
		{
			if (temp_opacity < 0)
			{
				temp_opacity = 0;
			}
			else if (temp_opacity > 1)
			{
				temp_opacity = 1;
			}
			GetComponent<Renderer>().material.SetColor("_TintColor", new Color(C_OriginColor.r, C_OriginColor.g, C_OriginColor.b, temp_opacity));
			ShowOpacityKeyOld = ShowOpacityKey;
		}


	}


}
