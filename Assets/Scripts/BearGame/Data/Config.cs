using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Config{
	/// <summary>
	/// boss 可以掉落的技能
	/// </summary>
	public int[] szBossDropSkill;
	/// <summary>
	/// 可以释放的技能(游戏场景中的技能图标)
	/// </summary>
	public int[] szReleaseSkill;
	/// <summary>
	/// 触发大招无限金币的能量值
	/// </summary>
	public int EnergyFull ;

	public MDTran ct;//3D中的摄像机的位置和旋转

	public float[] wt;//武器的位置

	public float[] tv;//转盘速度

	public float[] dt;//桌子的位置

	public float[] wt1;

	public MDSceneParamter sp;//游戏中舌头的各种参数

	public MDBox box1;//金币的出生点(在box中随机某个位置)

	public MDBox box2;//收集品的出生点(在box中随机某个位置)

	public List<MDRole>role;
	/// <summary>
	/// 各种金币的大小
	/// </summary>
	public List<MDCoinScale> szCoinScale;

	public Config(){
		szBossDropSkill = new int[]{WGDefine.SK_YunShi,WGDefine.SK_ChangShe50,
						WGDefine.SK_FangYu4,WGDefine.SK_JianSu,WGDefine.SK_HaiXiao};
		szReleaseSkill = new int[]{WGDefine.SK_ChangShe100,WGDefine.SK_FangYu4,WGDefine.SK_DiZhen,WGDefine.SK_JianSu,WGDefine.SK_GuDing30};
		EnergyFull = 100;
		sp = new MDSceneParamter();
		szCoinScale = new List<MDCoinScale>();
		ct = new MDTran();
		wt = new float[]{0,0,0.82f};
	}

}

[System.Serializable]
public class MDSceneParamter{
	public float SPEED_commen = 8;
	public float SPEED_Add1 = 30;
	public float Forward = -36.53738f;
	public float Backward = -20.29767f;
	public float LongTongue = -10;
	public float WaveSide=-1.5f;
}
public class MDCoinScale{
	public int id;
	public List<float>s;
}
public class MDBox{
	public float[] p;
	public float[] size;
	public float[] c;
}

public class MDTran{
	public float[] p;
	public float[] r;
}


public class MDBoxVec{
	public Vector3 pos;
	public Vector3 size;
	public Vector3 center;
	public MDBoxVec(MDBox box)
	{
		this.pos = SDK.toV3(box.p);
		this.size = SDK.toV3(box.size);
		this.center = SDK.toV3(box.c);
	}
}

public class FinalConfig{
	/// <summary>
	/// boss 可以掉落的技能
	/// </summary>
	public int[] szBossDropSkill;
	/// <summary>
	/// 可以释放的技能(游戏场景中的技能图标)
	/// </summary>
	public int[] szReleaseSkill;
	/// <summary>
	/// 触发大招无限金币的能量值
	/// </summary>
	public int EnergyFull ;
	/// <summary>
	/// 游戏中舌头的各种参数
	/// </summary>
	public MDSceneParamter sp;

	public Vector3 coinCamPos ;
	public Vector3 coinCamRot;

	public Vector3 weaponsPos;
	public Vector3 weaponsPos1;

	public Vector3 turnPlateComm;
	public Vector3 turnPlateSlow;
	public Vector3 deskPos;

	public MDBoxVec boxCoin;
	public MDBoxVec boxColletion;

	public List<MDRole> szRoles;

	public Dictionary<int,Vector3> dicCoinScale ;

	public FinalConfig (Config cfg)
	{
		this.szBossDropSkill = cfg.szBossDropSkill;
		this.szReleaseSkill = cfg.szReleaseSkill;
		this.EnergyFull = cfg.EnergyFull;
		this.sp = cfg.sp;
		this.dicCoinScale = new Dictionary<int, Vector3>();
		for(int i=0,max = cfg.szCoinScale.Count;i<max;i++)
		{
			this.dicCoinScale.Add(cfg.szCoinScale[i].id,SDK.toV3(cfg.szCoinScale[i].s));
		}
		this.coinCamPos = SDK.toV3(cfg.ct.p);
		this.coinCamRot = SDK.toV3(cfg.ct.r);
		this.weaponsPos = SDK.toV3(cfg.wt);
		this.weaponsPos1 = SDK.toV3(cfg.wt1);
		this.turnPlateComm = new Vector3(0,0,cfg.tv[0]);
		this.turnPlateSlow = new Vector3(0,0,cfg.tv[1]);
		this.deskPos = SDK.toV3(cfg.dt);
		this.boxCoin = new MDBoxVec(cfg.box1);
		this.boxColletion = new MDBoxVec(cfg.box2);
		szRoles = cfg.role;
	}
}
