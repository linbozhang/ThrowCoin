using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MusicEnum
{
	m_bg=0,
	Shot=1,
	tiger777=2,
	hitBear=3,
	noHit=4,
	getCoin,
	coinDrop,
	levelup,
	bossWarning,
	bossLaugh,
	button,
	tree,
	weapon1,
	weapon2,
	weapon3,
	weapon4,
	showReward,
	great,
	daoju1,
	daoju2,
	daoju3,
	daoju4,
}

public class BCSoundPlayer : MonoBehaviour {
	
	public AudioSource MainSoure;
	
	public AudioSource BGMSoure;
	
	public List<MDSound> GameAudio;

	public Dictionary<MusicEnum,AudioClip> dicSound = new Dictionary<MusicEnum, AudioClip>();


	bool bMusic = false;
	bool  bSound = false;
	
	public static BCSoundPlayer Instance = null;

	public const string KEYSOUND = "BSOUND";
	public const string KEYMUSIC = "BMUSIC";

	void Awake()
	{
		Instance = this;

		for(int i=0;i<GameAudio.Count;i++)
		{
			if(!dicSound.ContainsKey(GameAudio[i]._type))
			{
				dicSound.Add(GameAudio[i]._type,GameAudio[i]._clip);
			}
		}
		if(MainSoure != null)
		{
			MainSoure.GetComponent<AudioSource>().clip = dicSound[MusicEnum.m_bg];
		}

	}
	void Start()
	{

		bSound = PlayerPrefs.GetInt(KEYSOUND,0) == 1;
		bMusic = PlayerPrefs.GetInt(KEYMUSIC,0) == 1;
		
		SwitchMusic = bMusic;

		if(bMusic)
		{
			if(!MainSoure.isPlaying)
			{
				MainSoure.Play();
			}
		}
		else
		{
			if(MainSoure.isPlaying)
			{
				MainSoure.Stop();
			}
		}
	}
	
	//播放定制的音效
	static public void Play(MusicEnum sound) {
		if(Instance.BGMSoure.GetComponent<AudioSource>().volume > 0&& Instance.SwitchSound ){
			//Instance.BGMSoure.audio.clip = Instance.GameAudio[(int)sound];
			if(!Instance.BGMSoure.isPlaying)
			{
				Instance.BGMSoure.PlayOneShot(Instance.dicSound[sound],Instance.BGMSoure.GetComponent<AudioSource>().volume);
			}

		}else{
			//Debug.LogWarning("sound clip is null");
		}
		
	}
	static public void Play(MusicEnum sound,float volume) {
		if(Instance.BGMSoure.GetComponent<AudioSource>().volume > 0&& Instance.SwitchSound ){
			//Instance.BGMSoure.audio.clip = Instance.GameAudio[(int)sound];
			if(!Instance.BGMSoure.isPlaying)
			{
				Instance.BGMSoure.PlayOneShot(Instance.dicSound[sound],volume);
			}
			
		}else{
			//Debug.LogWarning("sound clip is null");
		}
		
	}
	static public bool IsPlay()
	{
		return Instance.BGMSoure.isPlaying;
	}
	public bool SwitchMusic
	{
		get{
			return bMusic;
		}
		set{
			bMusic = value;
			PlayerPrefs.SetInt(KEYMUSIC,bMusic?1:0);
			if(bMusic)
			{
				if(MainSoure != null)
				{
					MainSoure.Play();
				}

			}
			else{
				if(MainSoure != null)
				{
					MainSoure.Pause();
				}
			}
		}
	}
	public void PlayBackground(AudioClip clip)
	{
		if(MainSoure != null)
		{
			MainSoure.clip = clip;
			MainSoure.Play();
		}
	}
	public bool SwitchSound
	{
		get{
			return bSound;
		}
		set{
			bSound = value;
			PlayerPrefs.SetInt(KEYSOUND,bSound?1:0);
		}
	}


}
[System.Serializable]
public class MDSound
{
	public AudioClip _clip;
	public MusicEnum _type;
}