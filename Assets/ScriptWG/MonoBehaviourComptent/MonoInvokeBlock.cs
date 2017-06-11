using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// Union
public class MonoInvokeBlock : MonoBehaviour {

	System.Action act;
	public Dictionary<int,System.Action> dicMyAction = new Dictionary<int, System.Action>();
	public Queue<System.Timers.Timer> myTimers = new Queue<System.Timers.Timer>();
	bool bExcute = false;
	void InvokeBlockWithTime(float time,System.Action a)
	{

		System.Timers.Timer t = new System.Timers.Timer(time*1000+Random.Range(0,100f));//实例化Timer类，设置间隔时间为10000毫秒； 
		t.Elapsed += new System.Timers.ElapsedEventHandler(ElapsedEvent);//到达时间的时候执行事件； 

		if(dicMyAction.ContainsKey(t.GetHashCode()))
		{
			dicMyAction.Remove(t.GetHashCode());
		}

		dicMyAction.Add(t.GetHashCode(),a);

		t.AutoReset = false;//设置是执行一次（false）还是一直执行(true)； 
		t.Enabled = true;//是否执行System.Timers.Timer.Elapsed事件；
	}
	void ElapsedEvent(object obj,System.Timers.ElapsedEventArgs args)
	{
		bExcute = true;
		System.Timers.Timer t = (System.Timers.Timer)obj;
		myTimers.Enqueue(t);
	}

	public virtual void InvokeBlock(float waitTime,System.Action a)
	{
//		if(Time.timeScale>0)
//		{
//			StartCoroutine(doBlock(a,waitTime));
//		}
//		else
		{
			InvokeBlockWithTime(waitTime,a);
		}
	}
	IEnumerator doBlock(System.Action a, float time)
	{
		yield return new WaitForSeconds(time);
		if(a!=null) a();
	}

	void Update()
	{

		if(myTimers.Count>0)
		{
			System.Timers.Timer t = myTimers.Dequeue();

			if(t != null)
			{
				System.Action act = null;
				dicMyAction.TryGetValue(t.GetHashCode(),out act);
				if(act != null)
				{
					act();
				}
				dicMyAction.Remove(t.GetHashCode());
			}
			else
			{
				Debug.LogWarning("myTimers is dequeue a null....");
			}
		}

	}
}