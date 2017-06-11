using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class WGMutualExclusionObject {

	[SerializeField]
	List<MDListObject> szGameObject1;
}
[System.Serializable]
public class MDListObject{
	public List<GameObject> szAll; 
}