using UnityEngine;
using System.Collections;

public class CRLuo_Materials_Flicker : MonoBehaviour
{
	
	private float Intensity = 0;
	private float State = 0;
	private float speed = 0;
	private float Min = 0; 
	private float Max = 0;
	private float ColorR = 0;
	private float ColorG = 0;
	private float ColorB = 0;
	private bool MainKeyTF;
	private float NowIntensity = 0;
	
	public string _ = "-=<多层级材质闪烁程序>=-";
	public string __ = "材质球ID";
	public int ElementID = 0;
	public string ___ = "闪烁开始时间";
	public float StartTime = 0;
	public string ____ = "每次闪烁时长";
	public float OneFlickerTime = 1;
	public string _____ = "最大亮度";
	public float IntensityMax = 1;
	public string ______ = "-------高级随机闪烁开关--------";
	public bool OnAdvancedFlicker;
	public string _______ = "关闭时间";
	public float OffTimeMin = 0;
	public float OffTimeMax = 10;
	public string ________ = "变亮时间";
	public float UpTimeMin = 0;
	public float UpTimeMax = 0.5f;
	public string _________ = "开启时间";
	public float OnTimeMin = 0;
	public float OnTimeMax = 1;
	public string __________ = "变暗时间";
	public float DownTimeMin = 0;
	public float DownTimeMax = 1;
	public string ___________ = "亮度随机开关";
	public bool IntensityMaxRand;
	public string ____________ = "外部灯光连接";
	public Light light;
	public string _____________ = "灯光亮度微调";
	public float lightIntensityScle = 1;
	public string ______________ = "闪烁主开关";
	public bool MainKey = true;
	
	void Start () {
		ColorR = this.GetComponent<Renderer>().materials[ElementID].color.r;
		ColorG = this.GetComponent<Renderer>().materials[ElementID].color.g;
		ColorB = this.GetComponent<Renderer>().materials[ElementID].color.b;
	}
	
	void Update () {
		
		if (MainKey)
		{
			if (StartTime > 0)
			{
				StartTime -= Time.deltaTime;
				return;
			}
			
			
			
			if (OnAdvancedFlicker)
			{
				StartCoroutine(AdvancedFlicker());
			}
			else
			{
				BasisFlicker();
			}
			
			MainKeyTF = true;
			
			NowIntensity = this.GetComponent<Renderer>().materials[ElementID].color.a;
		}
		else
		{ 
			if(MainKeyTF)
			{
				NowIntensity -= Time.deltaTime;
				
				Debug.Log("NowIntensity = " + NowIntensity + "    Intensity = " + Intensity + "    OneFlickerTime = " + OneFlickerTime);
				if (NowIntensity < 0.01f)
				{
					NowIntensity = 0;
					MainKeyTF = false;
					State = 0;
					Intensity = 0;
				}
				this.GetComponent<Renderer>().materials[ElementID].SetColor("_Color", new Color(ColorR, ColorG, ColorB, NowIntensity));	
				
			}
			
			
		}
		
	}
	void BasisFlicker()
	{
		speed = 1 / OneFlickerTime;
		
		Intensity += Time.deltaTime * speed;
		
		this.GetComponent<Renderer>().materials[ElementID].SetColor("_Color", new Color(ColorR, ColorG, ColorB, Mathf.PingPong(Intensity, IntensityMax)));	
		
	}
	
	IEnumerator AdvancedFlicker()
	{
		yield return null;
		if (State == 0)
		{
			
			yield return new WaitForSeconds(Random.Range(OffTimeMin, OffTimeMax));
			State = 1;
			speed = 1 / Random.Range(UpTimeMin, UpTimeMax);
			rand_MinMax();
			
		};
		if (State == 1)
		{
			if (Intensity <= Max)
			{
				Intensity += Time.deltaTime * speed;
				setLinght();
			}
			else
			{
				Intensity = Max;
				setLinght();
				yield return new WaitForSeconds(Random.Range(OnTimeMin, OnTimeMax));
				State = -1;
				speed = -1 / Random.Range(DownTimeMin, DownTimeMax);
				rand_MinMax();
			}
		};
		if (State == -1)
		{
			if (Intensity > Min)
			{
				Intensity += Time.deltaTime * speed;
				setLinght();
				
			}
			else
			{
				Intensity = Min;
				setLinght();
				yield return new WaitForSeconds(Random.Range(OffTimeMin, OffTimeMax));
				State = 1;
				speed = 1 / Random.Range(UpTimeMin, UpTimeMax);
				rand_MinMax();
			}
		};
		
		
	}
	
	void rand_MinMax()
	{ 
		
		if(IntensityMaxRand)
		{ Max = Random.Range(0, IntensityMax); }
		else
		{Max = IntensityMax;};
		
	}
	void setLinght()
	{
		
		
		float Alp = 0;
		if (Intensity > 1)
		{
			Alp = 1;
			
		}
		else
		{
			Alp = Intensity;
		}
		this.GetComponent<Renderer>().materials[ElementID].SetColor("_Color", new Color(ColorR, ColorG, ColorB, Mathf.PingPong(Alp, 1)));
		
		
		if (light != null)
		{
			light.intensity = Intensity * lightIntensityScle;
			
		}
		
	}
	
}
