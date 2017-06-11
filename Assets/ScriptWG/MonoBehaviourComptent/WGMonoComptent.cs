using UnityEngine;
using System.Collections;
[RequireComponent(typeof(MonoInvokeBlock))]
public class WGMonoComptent : MonoBehaviour {

	MonoInvokeBlock _mnIvokeBlock = null;
	public MonoInvokeBlock mnIvokeBlock{
		get{
			if(_mnIvokeBlock == null)
			{
				_mnIvokeBlock = this.GetComponent<MonoInvokeBlock>();
			}
			return _mnIvokeBlock;
		}
	}
	public void InvokeBlock(float waitTime,System.Action a)
	{
		mnIvokeBlock.InvokeBlock(waitTime,a);
	}
}
