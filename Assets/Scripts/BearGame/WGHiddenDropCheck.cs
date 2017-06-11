using UnityEngine;
using System.Collections;

public class WGHiddenDropCheck : MonoBehaviour {

	public bool bChechHelpState = false;
	public bool bCheckAchievement=false;
	public bool bMissSound = false;
	void  OnTriggerEnter ( Collider other  )
	{
		if(other.tag .Equals( "Coin") || other.tag.Equals("COLLECTION"))
		{
			WGGameWorld.Instance.HideObj(other.gameObject);
			if(bMissSound)
			{
				BCSoundPlayer.Play(MusicEnum.noHit);
			}
			if(bCheckAchievement)
			{
				BCGameObj obj = other.GetComponent<BCGameObj>();
				WGAchievementManager.Self.processAchievement(obj.ID,DTAchievementType.LOST_OBJ);

			}
			if(bChechHelpState)
			{
				if(WGHelpManager.Self != null &&WGHelpManager.Self.enabled)
				{
					if(!WGHelpManager.Self.StatesIsEnd(EMHelpStates.Miss_Coin))
					{
						Time.timeScale = 0;
						WGHelpManager.Self.ShowHelpView(EMHelpStates.Miss_Coin);
					}
				}
			}
		}
	}
}
