using UnityEngine;
using System.Collections;
public class LoadingGame : MonoBehaviour {



	//异步对象  
	AsyncOperation async;
	// Use this for initialization
	void Start () {

		StartCoroutine(LoadTexture());

	}

	IEnumerator LoadTexture() {

		async= UnityEngine.SceneManagement.SceneManager.LoadSceneAsync (1);
		
		//读取完毕后返回， 系统会自动进入C场景  
		yield return async;  


	}

}
