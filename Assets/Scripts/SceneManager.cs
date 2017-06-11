using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour {

	public GameObject goLoading;

	public GameObject goAbout;

	public GameObject goRoot;

	public GameObject go3DRoot;

	public GameObject goLog;

	public GameObject goBtnBegin;


	//异步对象
	AsyncOperation globeScen;
	void Start()
	{
		#if Add_AD
			IOSAD.readyInterstitialAds();
		#endif

#if YHAbout
		goAbout.ESetActive(true);
#else
		goAbout.ESetActive(false);
#endif

		StartCoroutine(LoadTexture());
	}
	void OnBtnBegin()
	{
		if(WGDataController.Instance.bLoadDataSuccess)
		{
			goLoading.ESetActive(true);
			//Application.LoadLevel(2);
			StartCoroutine(loadScene());
		}
	}

	
	

	IEnumerator LoadTexture() {
		
		ResourceRequest request = Resources.LoadAsync("pbBearGameRoot");
		yield return request;
		GameObject obj = request.asset as GameObject;
		GameObject go = Instantiate(obj) as GameObject;
		go.transform.parent = go3DRoot.transform;
		go.transform.position = Vector3.zero;
		goLog.ESetActive(false);
		goBtnBegin.ESetActive(true);
	}

	void OnBtnSetting()
	{
		YHAboutGameView aboutView = YHAboutGameView.CreateAboutView();
		aboutView.alertViewBehavriour = (ab,view)=>{
			switch(ab)
			{
			case MDAlertBehaviour.CLICK_OK:
				view.hiddenView();
				break;
			case MDAlertBehaviour.DID_HIDDEN:

				break;
			}
		};
		SDK.AddChild(aboutView.gameObject,goRoot);
		aboutView.showView();
	}
	//注意这里返回值一定是 IEnumerator
	IEnumerator loadScene()
	{
		yield return new WaitForSeconds(0.1f);
		//异步读取场景。
		//Globe.loadName 就是A场景中需要读取的C场景名称。
		globeScen= UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(2);


		//读取完毕后返回， 系统会自动进入C场景
		yield return globeScen;

	}
}
