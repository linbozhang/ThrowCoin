using UnityEngine;
using System.Collections;
//Create U3DSprite by Song
// Copy Right 2014®
public class U3DSprite : MonoBehaviour {
	

	public SpriteRenderer spRender = null;

	public Sprite[] szSprites;

	public Sprite[] szAnimFrames;

	float totalTime = 0;
	float averageTime = 0;
	int curFrame = 0;
	int currentIndex = 0;
	bool bAnimaLoop = false;
	public void SetSpriteID(int index)
	{
		if(currentIndex == index)return;
		if(spRender == null) spRender = this.GetComponent<SpriteRenderer>();
		if(index <szSprites.Length && spRender!= null)
		{
			currentIndex = index;
			spRender.sprite = szSprites[index];
		}
	}
	public void PlayFrameAnim(float time,bool loop)
	{
		if(szAnimFrames.Length>0)
		{
			totalTime = time;
			bAnimaLoop = loop;
			averageTime = time/szAnimFrames.Length;
			curFrame = 0;
			InvokeRepeating("playOneFrame",averageTime,averageTime);
		}
	}

	void playOneFrame()
	{

		curFrame ++;
		if(curFrame>=szAnimFrames.Length)
		{
			if(bAnimaLoop)
			{
				curFrame = 0;
			}
			else{
				CancelInvoke("playOneFrame");
				return;
			}
		}
		spRender.sprite = szAnimFrames[curFrame];

	}
}