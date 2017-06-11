using UnityEngine;
using System.Collections;

public class EmptyLoading : MonoBehaviour {


		//异步对象
	AsyncOperation globeScen;
	void Start()
	{
			

	}
	void OnBtnBegin()
	{
		BCSoundPlayer.Play(MusicEnum.button);
		//在这里开启一个异步任务，
		//进入loadScene方法。
		StartCoroutine(loadScene());
	}

	//注意这里返回值一定是 IEnumerator
	IEnumerator loadScene()
	{
		//异步读取场景。
		//Globe.loadName 就是A场景中需要读取的C场景名称。
		globeScen = Application.LoadLevelAsync("Game");

		//读取完毕后返回， 系统会自动进入C场景
		yield return globeScen;

	}
}
