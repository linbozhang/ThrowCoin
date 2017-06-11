using UnityEngine;
using System.Collections;

public class WGController : MonoBehaviour {

    public UIRoot _mainRoot = null;
    public Camera _mainCamera = null;
    public float heigth {
        get{
            if(_mainRoot != null)
            {
                return _mainRoot.activeHeight*_mainCamera.orthographicSize;
            }
            return Screen.height;
        }
    }
    public float width{
        get{
            if(_mainRoot != null)
            {
                return Screen.width*_mainRoot.activeHeight*_mainCamera.orthographicSize/Screen.height;
            }
            return Screen.width;
        }
    }
    /// <summary>
    /// 单例,最简单的单例
    /// </summary>
    public static WGController Instance=null;
    void Awake()
    {
        Instance = this;


//        float size = _mainCamera.orthographicSize;
//
//        SDK.Log("width===="+Screen.width+";height===="+Screen.height);
//
//        float factor = Screen.width*1.0f/Screen.height;
//        if(factor<0.6667f)
//        {
//            _mainCamera.orthographicSize = size*1f;
//        }
//        else{
//            _mainCamera.orthographicSize = size*0.85f;
//        }
//
//        SDK.Log("width===="+Screen.width+";height===="+Screen.height);

    }

}
