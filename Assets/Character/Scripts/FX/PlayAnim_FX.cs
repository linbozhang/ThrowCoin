using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayAnim_FX : MonoBehaviour
{


	//自身ID号
	[System.NonSerialized]
	public int i_Num = 0;

	//默认随机调用
	public string ______="默认行为控制";
	public bool b_UseRandAmin = true;
	public bool b_UseUnifySet = false;
	public const float F_MinFreeTime = 15;
	public const float F_MaxFreeTime = 20;
	public const int I_MaxRandActions = 3;


	//定义最小自由时间
	public float minFreeTime = 15;
	//定义最大自由时间
	public float maxFreeTime = 20;
	//定义最大随机动作数量
	public int MaxRandActions = 3;

	public string _______ = "特效总开关";
	//特效总开关
	public bool FX_Main_key = true;
	//开关状态
	[System.NonSerialized]
	public bool FX_Main_Now;

	public string ________= "生命特征特效";
	//生命特效对象数组
	public FxLife[] myFXLife;

	public string _________ = "随身特效开关";
	//生命呼吸特效
	public Anim_Fx_ONOFF[] myFx_OnOff;

	public string __________= "行为烘托特效";
	//生命呼吸特效
	public FxLifeAdvanced [] myFxLifeAdvanced;

	public string ____________ = "攻击效果特效";

	//动画特效类数组
	public AttackFX [] myAttackFX;

	public string ______________ = "角色位移控制";
	//物体移动
	public MoveGo [] myMove;

	public string _______________ = "摄像机震动特效";
	//摄像机震动
	public GameObject CameraFx_IN;
	public CamearFX [] myCamearFX;

	public string ________________ = "子弹时间特效";
	//时间变速
	public TimeGo [] myTimeFX;
			[System.NonSerialized]
	public float NowTimeSheep;

	//当前动画状态名
	private string str_CurAnim = "";
	
	public string GetCurAnim(){
		return str_CurAnim;
	}

	public string _________________ = "角色标尺";
	//绘制参考线参数
	public bool Scaleplate_Key = true;
	public float Scaleplate_Height = 5f;
	public float Scaleplate_Wide = 4f;
	public float Scaleplate_Long = 4f;
	public bool ScaleplateShowBox = false;
	public Vector3 ScaleplateOffset;
	public float OneShow_Height = 0f;
	public bool b_WangShuNeed = false;
	public float f_WangShuLength = 1;


	//灵魂
	public GameObject prefab_Soul;


	List<Shader>  szDefaultShader = new List<Shader>();


	//添加碰撞体
	public void GenerateCollider()
	{
		BoxCollider boxColldier = this.gameObject.AddComponent<BoxCollider>();
		boxColldier.center = new Vector3(-ScaleplateOffset.x / this.transform.localScale.x, -ScaleplateOffset.y / this.transform.localScale.y, -ScaleplateOffset.z / this.transform.localScale.z) +
			Vector3.up * Scaleplate_Height / 2 / this.transform.localScale.y;
		boxColldier.extents = new Vector3(Scaleplate_Wide / 2 / this.transform.localScale.x, Scaleplate_Height / 2 / this.transform.localScale.y, Scaleplate_Long / 2 / this.transform.localScale.z);
	}

	//人物标尺绘制
	void OnDrawGizmos() {
		if(Scaleplate_Key){
			Gizmos.color = new Color(1, 0, 1);
			Gizmos.DrawLine(this.transform.position + ScaleplateOffset, this.transform.position + ScaleplateOffset + Vector3.up * Scaleplate_Height);
			Gizmos.DrawLine(this.transform.position + ScaleplateOffset + Vector3.forward * Scaleplate_Long * 0.5f, this.transform.position + ScaleplateOffset - Vector3.forward * Scaleplate_Long * 0.5f);
			Gizmos.DrawLine(this.transform.position + ScaleplateOffset + Vector3.right * Scaleplate_Wide * 0.5f, this.transform.position + ScaleplateOffset - Vector3.right * Scaleplate_Wide * 0.5f);
			Gizmos.DrawLine(this.transform.position + ScaleplateOffset + Vector3.forward * Scaleplate_Long * 0.5f + Vector3.up * Scaleplate_Height, this.transform.position + ScaleplateOffset - Vector3.forward * Scaleplate_Long * 0.5f + Vector3.up * Scaleplate_Height);
			Gizmos.DrawLine(this.transform.position + ScaleplateOffset + Vector3.right * Scaleplate_Wide * 0.5f + Vector3.up * Scaleplate_Height, this.transform.position + ScaleplateOffset - Vector3.right * Scaleplate_Wide * 0.5f + Vector3.up * Scaleplate_Height);
			if (ScaleplateShowBox)
			{
				Gizmos.DrawLine(this.transform.position + ScaleplateOffset - Vector3.forward * Scaleplate_Long * 0.5f, this.transform.position + ScaleplateOffset - Vector3.forward * Scaleplate_Long * 0.5f + Vector3.up * Scaleplate_Height);
				Gizmos.DrawLine(this.transform.position + ScaleplateOffset + Vector3.forward * Scaleplate_Long * 0.5f, this.transform.position + ScaleplateOffset + Vector3.forward * Scaleplate_Long * 0.5f + Vector3.up * Scaleplate_Height);
				Gizmos.DrawLine(this.transform.position + ScaleplateOffset - Vector3.right * Scaleplate_Wide * 0.5f, this.transform.position + ScaleplateOffset - Vector3.right * Scaleplate_Wide * 0.5f + Vector3.up * Scaleplate_Height);
				Gizmos.DrawLine(this.transform.position + ScaleplateOffset + Vector3.right * Scaleplate_Wide * 0.5f, this.transform.position + ScaleplateOffset + Vector3.right * Scaleplate_Wide * 0.5f + Vector3.up * Scaleplate_Height);
			}
		}
		if(b_WangShuNeed){
			Gizmos.color = new Color(0,1,0);
			Gizmos.DrawCube( this.transform.position + Vector3.down * 0.05f + ScaleplateOffset ,new Vector3(f_WangShuLength,0.1f,0.001f) );
		}
	}

	//有几个哥哥
	[System.NonSerialized]
	public int i_brotherCount;

	//生成的间隙
	public const float F_GenerateGap = 0.5f;

	[System.NonSerialized]
	public bool b_StandAlone = false;


	// 程序预运行部分
	void Start()
	{
		////特效开关判断
		//if (FX_Main_key)
		//{
		//      FX_Main_key = false;
		//      foreach(FxLife aFL in myFXLife ){
		//            if(aFL.ON_OFF == true && aFL.Life_FX!= null ){
		//                  Transform [] childs = aFL.Life_FX.GetComponentsInChildren<Transform>();
		//                  foreach(Transform temp in childs){
		//                        temp.gameObject.SetActiveRecursively(false);
		//                  }
		//                  aFL.Life_FX.SetActiveRecursively(false);
		//            }
		//      }
		//}



		if (b_UseRandAmin)
		{

			//默认随机行为设定
			if (b_UseUnifySet)
			{
				minFreeTime = F_MinFreeTime;
				maxFreeTime = F_MaxFreeTime;
				MaxRandActions = I_MaxRandActions;
			}

		}

		//创建首次随机调用休闲动作函数
		//计时调用（随机自由函数，调用时间<minFreeTime~maxFreeTime随机时间>
		if (b_UseRandAmin)
		{
			Invoke("RandomFree", Random.Range(minFreeTime, maxFreeTime));

		}
		//设定生命特效
		LifeFX_ON_OFF(FX_Main_Now);

		//if (StageShow_key)
		//{
		#region 上台下台材质球效果初始化
		//创建新材质球
		//Material material = new Material(Shader.Find("Unlit/REDCommonModelDouble"));

		//如果在名称提示中有(Clone)
		if (this.gameObject.name.EndsWith("(Clone)"))
		{
			//如果有克隆名称减少名称字符长度
			this.gameObject.name = this.gameObject.name.Substring(0, this.gameObject.name.Length - 7);
		}
		if (targetShader == null)
		{			
			Debug.LogWarning(this.gameObject.name + "没有赋予闪白材质");
		}
			Material material = new Material(targetShader);


		//载入白色图片
		Texture2D texture = (Texture2D)Resources.Load("Pictures/white");

		//把图片赋予材质球贴图属性
		material.mainTexture = texture;

		//
		SkinnedMeshRenderer[] renders = GetComponentsInChildren<SkinnedMeshRenderer>();

		//
		foreach (SkinnedMeshRenderer aMeshRenderer in renders)
		{
			foreach (Material aMaterial in aMeshRenderer.materials)
			{
				//把材质球加入原始材质目录
				if(list_Exist.Contains(aMaterial))
				{
				}
				else{
					list_Exist.Add(aMaterial);
					szDefaultShader.Add(aMaterial.shader);
				}
			}



			Material[] result = AddNewToExist(aMeshRenderer.materials, material);

//			aMeshRenderer.materials = result;

			//把白色贴图材质球加入遮盖材质目录
			list.Add(result[result.Length - 1]);
		}

			//让自身变无
			foreach (Material aMater in list_Exist)
			{
//				aMater.SetFloat("_Cutoff", 1);

			}

			//让白片也变无
			foreach (Material aMater in list)
			{
//				aMater.SetColor("_Color", new Color(1, 1, 1, 0));
			}

		//}
		#endregion
		if(UpShowKey){
			StartCoroutine(DelayStart());
		}
		
		FX_Main_key = false;
		Invoke("Fx_main_Botton", F_ShowTime + i_brotherCount * F_GenerateGap);
		//让影子不显示
		try {
			ShadowObj.GetComponent<Renderer>().enabled = false;	
		}catch(System.Exception e){
			Debug.LogError("ShadowObj Name:" + ShadowObj.name);
		}
		

	}
	public void GrayEffect(bool gray)
	{
		if(list_Exist.Count==0)
		{
			SkinnedMeshRenderer[] renders = GetComponentsInChildren<SkinnedMeshRenderer>();

			//
			foreach (SkinnedMeshRenderer aMeshRenderer in renders)
			{
				foreach (Material aMaterial in aMeshRenderer.materials)
				{
					//把材质球加入原始材质目录
					if(list_Exist.Contains(aMaterial))
					{
					}
					else{
						list_Exist.Add(aMaterial);
						szDefaultShader.Add(aMaterial.shader);
					}
				}
			}
		}
		for(int i=0;i<list_Exist.Count;i++)
		{
			if(gray)
			{
				list_Exist[i].shader = Shader.Find("Transparent/Diffuse");;
			}
			else{
				list_Exist[i].shader = szDefaultShader[i];
			}
		}
	}
	
	[System.NonSerialized]
	public bool UpShowKey = true;

	public void MaterialReturnToExist(){
		SkinnedMeshRenderer [] SkinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach(SkinnedMeshRenderer aSkinnedMeshRenderer in SkinnedMeshRenderers){
			aSkinnedMeshRenderer.materials = MaterialRemoveLast(aSkinnedMeshRenderer.materials);
		}
	}

	public void MaterialAddWhite(){
		Material material = new Material(targetShader);
		Texture2D texture = (Texture2D)Resources.Load("Pictures/white");
		material.mainTexture = texture;
		SkinnedMeshRenderer[] renders = GetComponentsInChildren<SkinnedMeshRenderer>();
		foreach (SkinnedMeshRenderer aMeshRenderer in renders){
			Material[] result = AddNewToExist(aMeshRenderer.materials, material);
			aMeshRenderer.materials = result;
			list.Add(result[result.Length - 1]);
		}
		foreach (Material aMater in list){
			aMater.SetColor("_Color", new Color(1, 1, 1, 0));
		}
	}

	IEnumerator DelayStart() {
		yield return new WaitForSeconds(i_brotherCount * F_GenerateGap );
		//登台效果
		UpStage();

		if(Application.loadedLevelName == "Game" && b_StandAlone == false ){
			Invoke("BanRandomFree",0.4f);
		}

	}

	void BanRandomFree(){
		//随机播放动作
		if(Random.Range(0,2) == 0){
			Free1();
		}else{
			Free2();
		}
	}

	//public void main_Fx()
	//{


	//}
	//特效开关标记
	void Fx_main_Botton()
	{
		if( FX_Main_key ){
				FX_Main_key = false;
		}else{
			FX_Main_key = true;
		}
	}

	//每帧调用命令
	void Update()
	{
		if (FX_Main_Now != FX_Main_key)
		{
			FX_Main_Now = FX_Main_key;
			LifeFX_ON_OFF(FX_Main_Now);
		}

		//如果当前没有动画播放
		if (!GetComponent<Animation>().isPlaying){

			//如果不是死亡和结束状态
			if (str_CurAnim != "die" && str_CurAnim != "over")
			{
				//调用默认动作
				Idle();
			}
			else
			{
				if(str_CurAnim != "over")
				{
					//把生命特效给摘出来
					if(myFXLife != null && myFXLife.Length > 0 ){
						foreach (FxLife aFxLife in myFXLife) {
							if(aFxLife != null && aFxLife.Life_FX != null ){
								aFxLife.Life_FX.transform.parent = null;
								Destroy(aFxLife.Life_FX,5);
							}
						}
					}

					//建立当前子物体名称列表
					Transform [] children = transform.GetComponentsInChildren<Transform>();

					//把生命特征特效摘出
					//判断生命特征数组是否为空
					if (myFxLifeAdvanced != null && myFxLifeAdvanced.Length > 0)
					{
						//逐个提取生命特征变量
						foreach (FxLifeAdvanced aFxLifeAdvanced in myFxLifeAdvanced)
						{


							foreach (UseToType aUseToType in aFxLifeAdvanced.LifeUse)
							{

								//判断当前组中生命特征特效非空
								if (aFxLifeAdvanced.Life_FX != null && aUseToType.UseType == UseToType.Type.die)
								{
									//在子属性列表中查提取名称
									foreach (Transform aTransform in children)
									{
										//如果搜索到的名称中包含（特效名称）
										if (aTransform.name.StartsWith(aFxLifeAdvanced.Life_FX.name))
										{
											//设置特效父物体为空
											aTransform.parent = null;
										}
									}
								}
							}
						}
					}


					//把死亡特效摘出
					//判断攻击特组是否为空
					if (myAttackFX != null && myAttackFX.Length > 0)
					{
						//提取个个特效组中的元素
						foreach (AttackFX aAttackFX in myAttackFX)
						{
							//判断当前特效是否为死亡特效
							if (aAttackFX.FXtype == AttackFX.Type.die)
							{
								if (aAttackFX.myFxElement != null && aAttackFX.myFxElement.Length > 0)
								{
									//在子特效包中逐个提取
									foreach (FxElement aFxElement in aAttackFX.myFxElement)
									{
										if (aFxElement.Prefab_FX != null)
										{
											//在子物体名称中逐个提取
											foreach (Transform aTransform in children)
											{
												//如果提取模型.名称.存在（攻击子特效.特效.名称）
												if (aTransform.name.StartsWith(aFxElement.Prefab_FX.name))
												{
													//设置当前特效为空
													aTransform.parent = null;
												}
											}
										}
									}
								}
							}
						}
					}






					//如果是死亡状态，调用退场效?					DownStage();
					//Invoke("DownStage", F_ShowTime);
					Destroy(this.gameObject, F_ShowTime*2);
					str_CurAnim = "over";


				}
			}
		}

//		如果按 1 键
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			//调用默认动作函数
			Debug.Log("=====");
			Idle();
		}
		//如果按 2 键
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			//调用攻击动作函数
			Attack();
		}
		//如果按 3 键
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			//调用技能动作函数
			Skill();
		}
//		如果按 4 键
		else if (Input.GetKeyDown(KeyCode.Alpha7))
		{
			//调用受伤动作函数
			Injure();
		}
		//如果按 5 键
		else if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			//调用自由动骱??			
			Free1();
		}
		//如果按 5 键
		else if (Input.GetKeyDown(KeyCode.Alpha5))
		{
			//调用自由2动作函数
			Free2();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha6))
		{
			//调用触屏动作函数
			Touch();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha8))
		{
			//调用死亡动作函数
			Die();
		}
		else if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			//特效开关
			if (FX_Main_key)
			{
				FX_Main_key = false;
			}
			else
			{
				FX_Main_key = true;
			}
		}
		#region 上台下台程序集Update

//		//从无到有
//		if (Input.GetKeyDown(KeyCode.A))
//		{
//
//			UpStage();
//		}
//		//从有到无
//		else if (Input.GetKeyDown(KeyCode.S))
//		{
//
//			DownStage();
//		}

		//上台下台显示代码
		if (fadeParam != 0)
		{
			StageShow();
		}
		#endregion

	}

	//返回眼睛的高度
	public Vector3 GetEyePos() {
		return this.transform.position + Scaleplate_Height * Vector3.up;
	}

	//物体销毁运行部分


	//定义开启生命特效程序集（接收布尔型变量）
	void LifeFX_ON_OFF(bool Key)
	{
		//判断生命特效数据数组是否为空或长度为0
		if (myFXLife.Length != 0)
		{
			//循环枚举特效数据，上限为数组长度
			for (int i = 0; i < myFXLife.Length; i++)
			{
				//判断特效内容是否为空
				if (myFXLife[i].Life_FX != null)
				{



					ParticleSystem particle = myFXLife[i].Life_FX.GetComponent<ParticleSystem>();
					particle.enableEmission = Key;


					//if (myFXLife[i].ON_OFF)
					//{


					//      //设置当前特效可见开关（true/false）

					//      //myFXLife[i].Life_FX.SetActiveRecursively(Key);
					//}
					//else
					//{
					//      //设置当前特效可见开关（true/false）
					//      myFXLife[i].Life_FX.SetActiveRecursively(false);
					//}

				}
			}

		}
	}

	//定义开启攻击特效程序集（接收布尔型变量）
	//void KillFX_ON_OFF(bool Key)
	//{
	//      //判断攻击特效数据数组是否为空或长度为0
	//      if (KillFX.Length != 0)
	//      {
	//            //循环枚举特效数据，上限为数组长度
	//            for (int i = 0; i < KillFX.Length; i++)
	//            {
	//                  //判断特效内容是否为空
	//                  if (KillFX[i] != null)
	//                  {
	//                        //设置当前特效可见开关（true/false）
	//                        KillFX[i].SetActiveRecursively(Key);

	//                  }
	//            }

	//      }
	//}



	//随机自由动作调用
	void RandomFree()
	{
		if (MaxRandActions != 0 )
		{
			//如果当前动作为默认
			if (GetComponent<Animation>().IsPlaying("idle"))
			{
				//调用自由动作函数

				switch (Random.Range(0, MaxRandActions))
				{
					case 0:
						break;
					case 1:
						Free1();
						break;
					case 2:
						Free2();
						break;
					case 3:
						Touch();
						break;
					case 4:
						Attack();
						break;
					case 5:
						Skill();
						break;
					case 6:
						Injure();
						break;
					case 7:
						Die();
						break;
				}

			}
			//计时调用（随机自由函数，调用时间<minFreeTime~maxFreeTime随机时间>
			Invoke("RandomFree", Random.Range(minFreeTime, maxFreeTime));
		}
	}

	#region Public Function

	//定义默认动作函数
	public void Idle() {

		//调用模型idle默认动作

		

		AnimationState aAnimationState = GetComponent<Animation>()["idle"];
		if (aAnimationState == null)
		{
			Debug.LogWarning(this.gameObject.name + "角色没有动作");
			return;
		}
		

		GetComponent<Animation>().CrossFade("idle");
		//设置当前动作名为idle(默认动作)
		str_CurAnim = "idle";

		if (FX_Main_Now) {
			//开启生命特效
			//LifeFX_ON_OFF(true);
			//KillFX_ON_OFF(false);

			StartCoroutine("AnimFx_ONOFF", Fx_ONOFF_UseToType.Type.idle);
			StartCoroutine("CreateMove", MoveGo.Type.idle);
			StartCoroutine("CreateLifeFx", UseToType.Type.idle);
			//从myAttackFX[]攻击特效类（数组）中提取创建 默认（idle） 的特效
			StartCoroutine("CreateFx", AttackFX.Type.idle);

			StartCoroutine("CreateCamearFx", UseToType.Type.idle);

		}
					
		StartCoroutine("CreateTime", TimeGo.Type.idle);
		StartCoroutine("CreateSounds", SoundsFx.Type.Idle);

	}

	//定义攻击特效函数
	public void Attack() {
		
//		Debug.Log("=====Attack====");

		//调用模型attack攻击动作
		GetComponent<Animation>().CrossFade("attack");
		//设置当前动作名为attack（攻击动作）
		str_CurAnim = "attack";

		if (FX_Main_Now) {

			//开启生命特ι			//		LifeFX_ON_OFF(true);
			//KillFX_ON_OFF(true);

			StartCoroutine("AnimFx_ONOFF", Fx_ONOFF_UseToType.Type.attack);
			StartCoroutine("CreateMove", MoveGo.Type.attack);
			StartCoroutine("CreateLifeFx", UseToType.Type.attack);
			//从myAttackFX[]攻击特效类（数组）中提取涧吖セ鳎╝ttack） 的特效
			StartCoroutine("CreateFx", AttackFX.Type.attack);

			StartCoroutine("CreateCamearFx", UseToType.Type.attack);

		}
					StartCoroutine("CreateTime", TimeGo.Type.attack);
		StartCoroutine("CreateSounds", SoundsFx.Type.Attack);

	}

	//定义技能特效函数
	public void Skill() {
//				Debug.Log("=====Skill====");

		//调用模型skill技能动作
		GetComponent<Animation>().CrossFade("skill");
		//设置当前动作名为skill（技能）
		str_CurAnim = "skill";

		if (FX_Main_Now) {
			//开启生命特效（使用技能开启生命特效，开起生命特效是的停顿不易被发现）
			//LifeFX_ON_OFF(true);
			//KillFX_ON_OFF(false);
			StartCoroutine("AnimFx_ONOFF", Fx_ONOFF_UseToType.Type.skill);
			StartCoroutine("CreateMove", MoveGo.Type.skill);
			StartCoroutine("CreateLifeFx", UseToType.Type.skill);
			//从myAttackFX[]攻击特效类（数组）中提取创建 技能（skill） 的特效
			StartCoroutine("CreateFx", AttackFX.Type.skill);

			StartCoroutine("CreateCamearFx", UseToType.Type.skill);

		}
					StartCoroutine("CreateTime", TimeGo.Type.skill);
		StartCoroutine("CreateSounds", SoundsFx.Type.Skill);
	}

	//定义受伤特效函数
	public void Injure() {
//			Debug.Log("=====Injure====");

		//调用模型injure受伤动作
		GetComponent<Animation>().CrossFade("injure");
		//设置当前动作名为injure(受伤)
		str_CurAnim = "injure";

		if (FX_Main_Now) {
			//开启生命特效
			//		LifeFX_ON_OFF(true);
			//KillFX_ON_OFF(false);
			StartCoroutine("AnimFx_ONOFF", Fx_ONOFF_UseToType.Type.injure);
			StartCoroutine("CreateMove", MoveGo.Type.injure);
			StartCoroutine("CreateLifeFx", UseToType.Type.injure);
			//从myAttackFX[]攻击特效类（数组）中提取创建 受伤（injure） 的特效
			StartCoroutine("CreateFx", AttackFX.Type.injure);

			StartCoroutine("CreateCamearFx", UseToType.Type.injure);

		}
					StartCoroutine("CreateTime", TimeGo.Type.injure);
		StartCoroutine("CreateSounds", SoundsFx.Type.Injure);
	}

	//定义自由动作函数
	public void Free1() {
//			Debug.Log("=====Free1====");
		if(b_NeedDie){
			return;
		}
		if (GetComponent<Animation>().IsPlaying("idle")) {

			//调用模型free1自由2动作
			GetComponent<Animation>().CrossFade("free1");
			//设置动作名为free(自由1)
			str_CurAnim = "free1";

			if (FX_Main_Now) {
				//开启生命特效
				//		LifeFX_ON_OFF(true);
				//KillFX_ON_OFF(false);
				StartCoroutine("AnimFx_ONOFF", Fx_ONOFF_UseToType.Type.free1);
				StartCoroutine("CreateMove", MoveGo.Type.free1);
				StartCoroutine("CreateLifeFx", UseToType.Type.free1);
				//从myAttackFX[]攻击特效类（数组）中提取创建 休闲1（free1） 的特效
				StartCoroutine("CreateFx", AttackFX.Type.free1);
				StartCoroutine("CreateCamearFx", UseToType.Type.free1);

			}
							StartCoroutine("CreateTime", TimeGo.Type.free1);
			StartCoroutine("CreateSounds", SoundsFx.Type.Free1);
		}
	}

	public void Free2() {
//		Debug.Log("=====Free2====");
		if(b_NeedDie){
			return;
		}
		if (GetComponent<Animation>().IsPlaying("idle")) {

			//调用模型free2自由2动作
			GetComponent<Animation>().CrossFade("free2");
			//设置动作名为free(自由2)
			str_CurAnim = "free2";
			if (FX_Main_Now) {
				//开启生命特效
				//		LifeFX_ON_OFF(true);
				//KillFX_ON_OFF(false);
				StartCoroutine("AnimFx_ONOFF", Fx_ONOFF_UseToType.Type.free2);
				StartCoroutine("CreateMove", MoveGo.Type.free2);
				StartCoroutine("CreateLifeFx", UseToType.Type.free2);
				//从myAttackFX[]攻击特效类（数组）中提取创建 休闲2（free2） 的特效
				StartCoroutine("CreateFx", AttackFX.Type.free2);
				StartCoroutine("CreateCamearFx", UseToType.Type.free2);

			}
							StartCoroutine("CreateTime", TimeGo.Type.free2);
			StartCoroutine("CreateSounds", SoundsFx.Type.Free2);
		}
	}

	public void Touch() {
//		Debug.Log("=====Touch====");
		if (GetComponent<Animation>().IsPlaying("idle") || GetComponent<Animation>().IsPlaying("free1") || GetComponent<Animation>().IsPlaying("free2")) {

			//调用模型free自由动作
			GetComponent<Animation>().CrossFade("touch");
			//设置动作名为free(自由)
			str_CurAnim = "touch";
			if (FX_Main_Now) {
				//开启生命特效
				//		LifeFX_ON_OFF(true);
				//KillFX_ON_OFF(false);
				StartCoroutine("AnimFx_ONOFF", Fx_ONOFF_UseToType.Type.touch);
				StartCoroutine("CreateMove", MoveGo.Type.touch);
				StartCoroutine("CreateLifeFx", UseToType.Type.touch);
				//从myAttackFX[]攻击特效类（数组）中提取创建 触屏（touch） 的特效
				StartCoroutine("CreateFx", AttackFX.Type.touch);
				StartCoroutine("CreateCamearFx", UseToType.Type.touch);

			}
							StartCoroutine("CreateTime", TimeGo.Type.touch);
			StartCoroutine("CreateSounds", SoundsFx.Type.Touch);
		}
	}

	private bool b_NeedDie = false;

	//定义死亡动作函数
	public void Die() {
		b_NeedDie = true;

		//调用模型die死亡动作
		GetComponent<Animation>().CrossFade("die");
		//设置当前动作名为die（死亡）
		str_CurAnim = "die";

		if (FX_Main_Now) {
			//关闭生命特效
			LifeFX_ON_OFF(false);
			//KillFX_ON_OFF(false);
			StartCoroutine("AnimFx_ONOFF", Fx_ONOFF_UseToType.Type.die);
			StartCoroutine("CreateMove", MoveGo.Type.die);
			StartCoroutine("CreateLifeFx", UseToType.Type.die);

			//从myAttackFX[]攻击特效类（数组）中提取创建 死亡（die） 的特效
			StartCoroutine("CreateFx", AttackFX.Type.die);

			StartCoroutine("CreateCamearFx", UseToType.Type.die);
		}
		StartCoroutine("CreateTime", TimeGo.Type.die);
		StartCoroutine("CreateSounds", SoundsFx.Type.Die);
		FX_Main_key = false;

	}

	#endregion
	public void DelayDestroy(float time){
		StartCoroutine(DestroyIE(time));
	}

	IEnumerator DestroyIE(float time){
		yield return new WaitForSeconds(time);
		Destroy(this.gameObject);
	}

	//随身特效开关
	IEnumerator AnimFx_ONOFF(Fx_ONOFF_UseToType.Type Type)
	{
		if (myFx_OnOff != null && myFx_OnOff.Length != 0)
		{
			for (int i = 0; i < myFx_OnOff.Length; i++)
			{
				if (myFx_OnOff[i].FX_obj != null)
				{
					if (myFx_OnOff[i].AnimFx_Use != null && myFx_OnOff[i].AnimFx_Use.Length != 0)
					{
						for (int j = 0; j < myFx_OnOff[i].AnimFx_Use.Length; j++)
						{
							if (myFx_OnOff[i].AnimFx_Use[j].TimeLong > 0)
							{
								if (myFx_OnOff[i].AnimFx_Use[j].UseType == Type)
								{
									//程序在此等待 WaitForSeconds(等待秒数） 游戏界面仍然运动。
									yield return new WaitForSeconds(myFx_OnOff[i].AnimFx_Use[j].StartTime);
									myFx_OnOff[i].FX_obj.SetActiveRecursively(true);
									yield return new WaitForSeconds(myFx_OnOff[i].AnimFx_Use[j].TimeLong);
									myFx_OnOff[i].FX_obj.SetActiveRecursively(false);

								}
							}
						}
					}
				
				
				}


			}

		}



	}


	//攻击特效  定义提取创建特效 的  有参函数 （类 变量）
	IEnumerator CreateFx(AttackFX.Type Type)
	{
		//----
		//判断当前   特效类数组不为空 并且 数组长度不为0
		if (myAttackFX != null && myAttackFX.Length != 0)
		{
			//循环枚举 类组的各个内容 上限为类组的长度
			for (int i = 0; i < myAttackFX.Length; i++)
			{
				//判断当前类组i中的 Type 是否与传入的动作名一致 并且 特效对象不为空  并且     绑定对象不为空
				if (myAttackFX[i].FXtype == Type && myAttackFX[i].go_TargetBones != null)
				{
					if (myAttackFX[i].myFxElement != null && myAttackFX[i].myFxElement.Length != 0) 
					{
					
					
					for (int j = 0; j < myAttackFX[i].myFxElement.Length; j++)
					{
						if (myAttackFX[i].myFxElement[j].Prefab_FX != null && myAttackFX[i].myFxElement[j].ON_OFF)
						{
							//程序在此等待 WaitForSeconds(等待秒数） 游戏界面仍然运动。
							yield return new WaitForSeconds(myAttackFX[i].myFxElement[j].FXtime);

							//创建粒子物体，temp继承GameObject类属性（认爹）   实例化特效。

							GameObject temp = (GameObject)GameObject.Instantiate(myAttackFX[i].myFxElement[j].Prefab_FX);

							if (!myAttackFX[i].myFxElement[j].UseNoParent)
							{
								//temp临时对象 继承绑定物体位置
								temp.transform.parent = myAttackFX[i].go_TargetBones.transform;
								//调整特效位置
								temp.transform.localPosition = myAttackFX[i].v3_FXPos + myAttackFX[i].myFxElement[j].v3_FXPos_offset;
							}
							else
							{
								temp.transform.position = myAttackFX[i].go_TargetBones.transform.position + myAttackFX[i].v3_FXPos + myAttackFX[i].myFxElement[j].v3_FXPos_offset;
							}




							//调整特效旋转
							temp.transform.localRotation = Quaternion.Euler(
															 myAttackFX[i].v3_FXRotation.x + myAttackFX[i].myFxElement[j].v3_FXRotation_offset.x,
															 myAttackFX[i].v3_FXRotation.y + myAttackFX[i].myFxElement[j].v3_FXRotation_offset.y,
															 myAttackFX[i].v3_FXRotation.z + myAttackFX[i].myFxElement[j].v3_FXRotation_offset.z
															 );
							//aaaaaaaaaaaaaaaaaaaaaaa

							//if()
							//{
							////temp临时对象 继承绑定物体位置
							//temp.transform.parent = null;
							//}

							
						}
					}

				}
				}


			}
		}

	}


	//生命特效
	IEnumerator CreateLifeFx(UseToType.Type Type)
	{
		//----
		//判断当前   特效类数组不为空 并且 数组长度不为0
		if (myFxLifeAdvanced != null && myFxLifeAdvanced.Length != 0)
		{
			//循环枚举 类组的各个内容 上限为类组的长度
			for (int i = 0; i < myFxLifeAdvanced.Length; i++)
			{
				//判断当前类组i中的 Type 是否与传入的动作名一致 并且 特效对象不为空  并且     绑定韵蟛晃?陨				if (myFxLifeAdvanced[i].go_TargetBones != null && myFxLifeAdvanced[i].Life_FX != null && myFxLifeAdvanced[i].ON_OFF)
				{
					if (myFxLifeAdvanced[i].LifeUse != null && myFxLifeAdvanced[i].LifeUse.Length != 0)
					{
						for (int j = 0; j < myFxLifeAdvanced[i].LifeUse.Length; j++)
						{

							if (myFxLifeAdvanced[i].LifeUse[j].UseType == Type)
							{


									//程序在此等待 WaitForSeconds(等待秒数） 游戏界面仍然运动。
									yield return new WaitForSeconds(myFxLifeAdvanced[i].LifeUse[j].StartTime);

									//创建粒子物体，temp继承GameObject类属性（认爹）   实例化特效。
									GameObject temp = (GameObject)GameObject.Instantiate(myFxLifeAdvanced[i].Life_FX);

									//temp临时对象 继承绑定物体位置
									temp.transform.parent = myFxLifeAdvanced[i].go_TargetBones.transform;

									//调整特效位置
									temp.transform.localPosition = myFxLifeAdvanced[i].v3_FXPos;

									//调整特效旋转
									temp.transform.localRotation = Quaternion.Euler(myFxLifeAdvanced[i].v3_FXRotation.x, myFxLifeAdvanced[i].v3_FXRotation.y, myFxLifeAdvanced[i].v3_FXRotation.z);

							}
						}

					}

				}
			}

		}
	}


	//摄像机震动效果
	IEnumerator CreateCamearFx(UseToType.Type Type)
	{

		if (CameraFx_IN != null && myCamearFX != null && myCamearFX.Length != 0)
		{
			for (int i = 0; i < myCamearFX.Length; i++)
			{
				if ( myCamearFX[i].CamearFx_Key && myCamearFX[i].CamearFx_Time != 0 && (myCamearFX[i].Use_X || myCamearFX[i].Use_Y || myCamearFX[i].Use_Z))
				{

					Vector3 shake_V3 = new Vector3(0, 0, 0);
					if (myCamearFX[i].Use_X)
					{
						shake_V3 += new Vector3(myCamearFX[i].Scale, 0, 0);
					}
					if (myCamearFX[i].Use_Y)
					{
						shake_V3 += new Vector3(0, myCamearFX[i].Scale, 0);
					}
					if (myCamearFX[i].Use_Z)
					{
						shake_V3 += new Vector3(0, 0, myCamearFX[i].Scale);
					}
					if (myCamearFX[i].CamearUse != null && myCamearFX[i].CamearUse.Length != 0)
					{
						for (int j = 0; j < myCamearFX[i].CamearUse.Length; j++)
						{
							if (myCamearFX[i].CamearUse[j].UseType == Type)
							{
								yield return new WaitForSeconds(myCamearFX[i].CamearUse[j].StartTime);
								MiniItween itween = MiniItween.Shake(CameraFx_IN, shake_V3, myCamearFX[i].CamearFx_Time, MiniItween.EasingType.EaseInOutQuart);
								itween.b_handleScaleTime = true;
							}
						}
					}
				}
			}
		}
	}

	//物体移位效果
	IEnumerator CreateMove(MoveGo.Type Type)
	{
		//判断当前   特效类数组不为空 并且 数组长度不为0
		if (myMove != null && myMove.Length != 0)
		{
			//循环枚举 类组的各个内容 上限为类组的长度
			for (int i = 0; i < myMove.Length; i++)
			{
				//判断当前类组i中的 Type 是否与传入的动作名一致 并且 特效对象不为空  并且     绑定对象不为空

				//try {
				if (myMove[i].ActionType == Type && myMove[i].Move_Key)
				{

					//MiniItween.MoveTo(this.gameObject, myAttackFX[i].MoveTo_Pos + this.gameObject.transform.position, myAttackFX[i].Move_Time, MiniItween.EasingType.AnimationCurve, true).myExtraData
					//      = new ForMiniItween.ExtraData(myAttackFX[i].curve);



					//MiniItween.RotateTo(this.gameObject, myAttackFX[i].MoveTo_Rot + this.gameObject.transform.eulerAngles, myAttackFX[i].Move_Time, MiniItween.EasingType.AnimationCurve, true).myExtraData
					//      = new ForMiniItween.ExtraData(myAttackFX[i].curve);


					if (myMove[i].Move_Time > 0 && myMove[i].curve != null)
					{
						yield return new WaitForSeconds(myMove[i].StartTime);

						if (myMove[i].MoveTo_Pos != Vector3.zero)
						{
							MiniItween itween = MiniItween.MoveTo(this.gameObject, myMove[i].MoveTo_Pos + this.gameObject.transform.position, myMove[i].Move_Time, MiniItween.EasingType.AnimationCurve, true);
							itween.myExtraData= new ForMiniItween.ExtraData(myMove[i].curve.curve);
							itween.b_handleScaleTime = true;
						}
						if (myMove[i].MoveTo_Rot != Vector3.zero)
						{
							MiniItween itween = MiniItween.RotateTo(this.gameObject, myMove[i].MoveTo_Rot + this.gameObject.transform.eulerAngles, myMove[i].Move_Time, MiniItween.EasingType.AnimationCurve, true);
							itween.myExtraData = new ForMiniItween.ExtraData(myMove[i].curve.curve);
							itween.b_handleScaleTime = true;
						}
					}


					//      }
					//}catch(System.Exception e){
					//      Debug.Log(i+" "+myMove.Length);
				}



			}
		}
	}

	//摄像机慢速效果
	IEnumerator CreateTime(TimeGo.Type Type)
	{


			
		
		
		//判断当前   特效类数组不为空 并且 数组长度不为0
		if (myTimeFX != null && myTimeFX.Length != 0)
		{
			//循环枚举 类组的各个内容 上限为类组的长度
			for (int i = 0; i < myTimeFX.Length; i++)
			{
				//判断当前类组i中的 Type 是否与传入的动作名一致 并且 特效对象不为空  并且     绑定对象不为空

				if (myTimeFX[i].ActionType == Type && myTimeFX[i].Time_Key)
				{

					if (myTimeFX[i].Time_Long > 0 && myTimeFX[i].curve != null)
					{
							
						//Debug.LogWarning("---mandongzuo---");
							
						yield return new WaitForSeconds(myTimeFX[i].Start_Time);

						MiniItween.TimeScale(myTimeFX[i].curve.curve, myTimeFX[i].Time_Long);
						
						Invoke("BakeTimeSheep",myTimeFX[i].Time_Long+0.5f );
						
					}

				}



			}
		}
	}
	
	void BakeTimeSheep()
	{
		 Time.timeScale = NowTimeSheep ;
		
	}


	#region 上台下台程序集
	//模型颜色变化标记：0、无材质变化 1、上场闪白阶段   2、上场闪白褪去显示颜色  3、下场闪白 4、闪白褪去物体消失
	private int fadeParam = 0;
	//当前透明度
	private float f_CurAlpha;
	//开始透明度
	private float f_FromAlpha;
	//目标透明度
	private float f_TargetAlpha;
	//开始时间
	private float f_FromTime;
	//变化过程时间
	public const float F_ShowTime = 0.25f;
	//变白材质列表
	[System.NonSerialized]
	public List<Material> list = new List<Material>();
	//原始材质列表
	[System.NonSerialized]
	public List<Material> list_Exist = new List<Material>();


	public string ________________________________________________ = "角色阴影连接";
	//阴影对象
	public GameObject ShadowObj;
	public string ___________________________________________________ = "材质球连接";
	public Shader targetShader;

	//在已知材质球上加一个
	Material[] AddNewToExist(Material[] exist, Material newMaterial)
	{
		Material[] result = new Material[exist.Length + 1];
		for (int i = 0; i < exist.Length; i++)
		{
			result[i] = exist[i];
		}
		result[exist.Length] = newMaterial;
		return result;
	}

	//材质球去掉最后一个
	Material[] MaterialRemoveLast(Material[] exist)
	{
		List<Material> list_Temp = new List<Material>();
		list_Temp.Add(exist[0]);
		for(int i = 1;i<exist.Length - 1 ;i++){
			list_Temp.Add(exist[i]);
		}
		return list_Temp.ToArray();
	}

	//上台主命令
	public void UpStage()
	{
		if (ShadowObj == null)
		{
			//如果在名称提示中有(Clone)
			if (this.gameObject.name.EndsWith("(Clone)"))
			{
				//如果有克隆名称减少名称字符长度
				this.gameObject.name = this.gameObject.name.Substring(0, this.gameObject.name.Length - 7);
			}

			Debug.LogWarning(this.gameObject.name + "阴影没有连接");
		}
		ShadowObj.GetComponent<Renderer>().enabled = true;
		f_CurAlpha = 0;
		f_FromAlpha = 0;
		f_TargetAlpha = 1;
		f_FromTime = Time.time;
		//上台变化标记
		fadeParam = 1;

	}
	//下台主命令
	public void DownStage()
	{
		//增加白色材质球
		MaterialAddWhite();
		f_CurAlpha = 0;
		f_FromAlpha = 0;
		f_TargetAlpha = 1;
		f_FromTime = Time.time;
		//下台变化标记
		fadeParam = 3;
	}


	//人物显示与消失主命令
	void StageShow()
	{
		if (fadeParam == 1)
		{
			if (f_CurAlpha != f_TargetAlpha)
			{
				//透明度变化
				f_CurAlpha = Mathf.Lerp(f_FromAlpha, f_TargetAlpha, (Time.time - f_FromTime) / F_ShowTime);
				//监测当前透明度与目标透明度差距
				if (Mathf.Abs(f_CurAlpha - f_TargetAlpha) < 0.01f)
				{
					//设置无差距
					f_CurAlpha = f_TargetAlpha;
					//这里代表完成了一次目标，开始现身
					fadeParam = 2;
					//在list_exist 中 便利设置原始材质球显示
					foreach (Material aMater in list_Exist)
					{
						//设置材质球通道限制 0
						aMater.SetFloat("_Cutoff", 0.5f);
					}
					//当前透明度
					f_CurAlpha = 1;
					//开始透明度
					f_FromAlpha = 1;
					//目标透明度
					f_TargetAlpha = 0;
					//开始时间
					f_FromTime = Time.time;
				}
				//每一帧设置白色材质球的透明度
				foreach (Material aMater in list)
				{
					aMater.SetColor("_Color", new Color(1, 1, 1, f_CurAlpha));
					if (ShadowObj != null)
					{
						ShadowObj.GetComponent<ShadowAnim>().ShowOpacityKey = f_CurAlpha;
					}
				}
			}
		}
		else if (fadeParam == 2)
		{
			if (f_CurAlpha != f_TargetAlpha)
			{
				f_CurAlpha = Mathf.Lerp(f_FromAlpha, f_TargetAlpha, (Time.time - f_FromTime) / F_ShowTime);
				if (Mathf.Abs(f_CurAlpha - f_TargetAlpha) < 0.01f)
				{
					f_CurAlpha = f_TargetAlpha;
					//这里代表完成了二次目标
					fadeParam = 0;
					//材质球返回正常
					MaterialReturnToExist();

				}
				foreach (Material aMater in list)
				{
					aMater.SetColor("_Color", new Color(1, 1, 1, f_CurAlpha));
				}
			}
		}
		else if (fadeParam == 3)
		{
			if (f_CurAlpha != f_TargetAlpha)
			{
				f_CurAlpha = Mathf.Lerp(f_FromAlpha, f_TargetAlpha, (Time.time - f_FromTime) / F_ShowTime);
				if (Mathf.Abs(f_CurAlpha - f_TargetAlpha) < 0.01f)
				{

					f_CurAlpha = f_TargetAlpha;
					//这里代表完成了一次目标
					fadeParam = 4;
					foreach (Material aMater in list_Exist)
					{
						aMater.SetFloat("_Cutoff", 1);

					}
					//当前透明度
					f_CurAlpha = 1;
					//开始透明度
					f_FromAlpha = 1;
					//目标透明度
					f_TargetAlpha = 0;
					//开始时间
					f_FromTime = Time.time;
				}

				if(f_CurAlpha > 0.6f){
					if(prefab_Soul != null){
						SkinnedMeshRenderer[] renders = GetComponentsInChildren<SkinnedMeshRenderer>();
						if(renders != null && renders.Length > 0 ){
							Vector3 targetPos = renders[0].bounds.center + Vector3.down * renders[0].bounds.extents.y;
							GameObject.Instantiate(prefab_Soul,targetPos,Quaternion.identity);
						}
						prefab_Soul = null;
					}
				}

				foreach (Material aMater in list)
				{
					aMater.SetColor("_Color", new Color(1, 1, 1, f_CurAlpha));
				}
			}
		}
		else if (fadeParam == 4)
		{
			if (f_CurAlpha != f_TargetAlpha)
			{
				f_CurAlpha = Mathf.Lerp(f_FromAlpha, f_TargetAlpha, (Time.time - f_FromTime) / F_ShowTime);
				if (Mathf.Abs(f_CurAlpha - f_TargetAlpha) < 0.01f)
				{
					f_CurAlpha = f_TargetAlpha;
					//这里代表完成了二次目标
					fadeParam = 0;

				}
				foreach (Material aMater in list)
				{
					aMater.SetColor("_Color", new Color(1, 1, 1, f_CurAlpha));
					ShadowObj.GetComponent<ShadowAnim>().ShowOpacityKey = f_CurAlpha;
				}
			}
		}
	}




	#endregion

	public string _____________________________________________ = "角色音效";
	//播放音效
	public SoundsFx [] MySoundsFx;

	IEnumerator CreateSounds(SoundsFx.Type Type)
	{
		//判断当前   特效类数组不为空 并且 数组长度不为0
		if (MySoundsFx != null && MySoundsFx.Length != 0)
		{
			//循环枚举 类组的各个内容 上限为类组的长度
			for (int i = 0; i < MySoundsFx.Length; i++)
			{
				//判断当前类组i中的 Type 是否与传入的动作名一致 并且 特效对象不为空  并且     绑定对象不为空

				if (MySoundsFx[i].Sound_Type == Type && MySoundsFx[i].Sound_Key)
				{

					if (MySoundsFx[i].sounds != null)
					{
						yield return new WaitForSeconds(MySoundsFx[i].StartTime);

						//else {
						//      Debug.Log("场景没有声音接收");
						//}
						
					}

				}



			}
		}
	}




}

[System.Serializable]

//ㄒ骞セ魈匦Ю哌
public class AttackFX
{
	//攻击特效

	//定义菜单卷栏 Type 与内容
	public enum Type { none, idle, attack, skill, free1, free2, touch, injure, die };

	// 定义 FXtype 为 Type 类型 
	public Type FXtype;


	//定义特效绑定骨骼位置
	public GameObject go_TargetBones;

	//定义绑定位置位置偏移
	public Vector3 v3_FXPos;

	//定义绑定位置渲染偏移
	public Vector3 v3_FXRotation;


	public FxElement[] myFxElement;

	//public bool Move_Key;

	//public Vector3 MoveTo_Pos;

	//public Vector3 MoveTo_Rot;

	////public AnimationCurve curve;

	//public Curve curve;

	//public float Move_Time;

}

[System.Serializable]
public class FxElement
{
	//攻击特效元素

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

[System.Serializable]
public class FxLife
{
	//生命特征特效

	public bool ON_OFF = true;

	//定义物体特Ф韵蟊淞旧	
	public GameObject Life_FX;

}

[System.Serializable]
public class FxLifeAdvanced
{
	//生命呼吸特效

	//定义物体特效对象变量
	public GameObject Life_FX;

	public bool ON_OFF = true;


	
	//定义特效绑定骨骼位置
	public GameObject go_TargetBones;

	//定义绑定位置位置偏移
	public Vector3 v3_FXPos;

	//定义绑定位置渲染偏移
	public Vector3 v3_FXRotation;

	public UseToType[] LifeUse;




}

[System.Serializable]
public class UseToType
{
	//生命呼吸特效应用类型
	public enum Type { none, idle, attack, skill, free1, free2, touch, injure, die };

	// 定义 FXtype ┻Type 类型 
	public Type UseType;

	public float StartTime;
}


[System.Serializable]
public class Anim_Fx_ONOFF
{
	//特效开关

	//定义物体特效对象变量
	public GameObject FX_obj;

	public bool ON_OFF = true;

	public Fx_ONOFF_UseToType[] AnimFx_Use;




}

[System.Serializable]
public class Fx_ONOFF_UseToType
{
	//生命呼吸特效应用类型
	public enum Type { none, idle, attack, skill, free1, free2, touch, injure, die };

	// 定义 FXtype 为 Type 类型 
	public Type UseType;

	public float StartTime;

	public float TimeLong;
}




[System.Serializable]
public class MoveGo
{

	//物体移动
	public enum Type { none, idle, attack, skill, free1, free2, touch, injure, die };

	public Type ActionType;

	public bool Move_Key;

	public Vector3 MoveTo_Pos;

	public Vector3 MoveTo_Rot;

	public Curve curve;

	public float Move_Time;

	public float StartTime;

}

[System.Serializable]
public class TimeGo
{

	//镜头变速
	public enum Type { none, idle, attack, skill, free1, free2, touch, injure, die };

	public Type ActionType;

	public bool Time_Key;

	public Curve curve;

	public float Time_Long;

	public float Start_Time;

}


[System.Serializable]
public class CamearFX
{

	//摄像机震动;

	public bool CamearFx_Key;

	public float CamearFx_Time;

	public float Scale = 1;

	public bool Use_X;

	public bool Use_Y;

	public bool Use_Z;

	public UseToType[] CamearUse;


}





[System.Serializable]
public class SoundsFx
{
	public enum Type { None, Idle, Attack, Skill, Free1, Free2, Touch, Injure, Die };
	public Type Sound_Type;
	public bool Sound_Key;
	public AudioClip sounds;
	public float StartTime = 0;
	//public float f_EndTime = 0;
}







/*
 * 组建多参数类
public class SmallItem{
	public int i;
	public AttackFX.Type type;
	public SmallItem(int i, AttackFX.Type type)
	{
		this.i = i;
		this.type = type;
	}
 * 
 * 组建调用函数（多参数）
 *IEnumerator CreateFx(SmallItem smallItem) {
			yield return new WaitForSeconds(myAttackFX[smallItem.i].FXtime);
			smallItem.type
 * 
 * 
 * 调用函数
 * StartCoroutine("CreateFx", new SmallItem(i, AttackFX.Type.die));
 */