using UnityEngine;
using System.Collections;

public class WGTipForCollectionView : MDBaseAlertView {


	static Object mObj = null;
	int mRetainCount = 0;
	public TweenScale tsContent;

	public UITexture txCollection;
	public TweenRotation trContent;
	public bool bOneShowing = false;

	public int curCollectionID;
	static WGTipForCollectionView _Self = null;
	public static WGTipForCollectionView Self{
		get{
			if(_Self == null)
			{
				_Self = WGTipForCollectionView.CreateCollectionView();
				SDK.AddChild(_Self.gameObject,WGRootManager.Self.goRootGameUI);
			}
			return _Self;
		}
	}

	public void showTipCollection(int id)
	{
		trContent.ResetToBeginning();
		tsContent.transform.localPosition=new Vector3(-184.8f,-339.5f,0);
		curCollectionID = id;
		txCollection.mainTexture = Resources.Load("Collection/ci"+id) as Texture;

		showView();
	}
	public override void showView ()
	{
		mRetainCount ++;
		bOneShowing  = true;
		base.showView ();
		tsContent.gameObject.SetActive(true);
		tsContent.transform.localScale = tsContent.from;
		//showViewEnd();
		tsContent.PlayForward();
		mnIvokeBlock.InvokeBlock(tsContent.duration,()=>{
			showViewEnd();
		});
	}
	public override void showViewEnd ()
	{

		base.showViewEnd ();
		InvokeBlock(0.5f,()=>{
			hiddenView();
		});
	}
	public override void hiddenView ()
	{
		tsContent.PlayReverse();
		base.hiddenView ();
		mnIvokeBlock.InvokeBlock(tsContent.duration,()=>{
			hiddenViewEnd();
		});
	}
	public override void hiddenViewEnd ()
	{
		mRetainCount --;

		base.hiddenViewEnd ();
		if(WGGameUIView.Instance.leftHandle.activeInHierarchy){
			tsContent.gameObject.GetComponent<Animation>().Play("CollectionFly");
			//tsContent.gameObject.animation.Play();
		}else{
			//tsContent.gameObject.animation.name="CollectionFly2";
			tsContent.gameObject.GetComponent<Animation>().Play("CollectionFly2");
		}

		mnIvokeBlock.InvokeBlock(1,()=>{
			bOneShowing  = false;
			tsContent.gameObject.SetActive(false);
			tsContent.transform.localPosition=new Vector3(-184.8f,-339.5f,0);
			InvokeBlock(2,()=>{
				if(_Self.mRetainCount<=0)
				{
					_Self = null;
					Destroy(gameObject);
				}
			});
		});

	}

	static WGTipForCollectionView CreateCollectionView()
	{
		if(mObj == null)
		{
			mObj = Resources.Load("pbWGTipForCollectionView");
		}
		if(mObj != null)
		{
			GameObject go = Instantiate(mObj) as GameObject;
			WGTipForCollectionView tv = go.GetComponent<WGTipForCollectionView>();
			return tv;
		}
		return null;
	}


}
