using UnityEngine;
using System.Collections;
using System;

public enum MDAlertBehaviour{
	NONE,
    WILL_SHOW,
    DID_SHOW,

    WILL_HIDDEN,
    DID_HIDDEN,

    CLICK_OK,
    CLICK_CANCEL,
    CLICK_BACKGROUND,

}
public class MDBaseAlertView : WGMonoComptent {

	public const int CLICK_OK = 0;
	public const int CLICK_Cancel = 1;
	public const int CLICK_BackGround = 2;
	public const int CLICK_OK1 = 10;
	public const int CLICK_OK2 = 11;
	public const int CLICK_OK3=12;
    [HideInInspector]
    public int alertID;
    public Action<MDAlertBehaviour,MDBaseAlertView> alertViewBehavriour;
	[HideInInspector]
	public int clickIndex  = 0;

    public virtual void showView()
    {
        if(alertViewBehavriour!=null)
        {
            alertViewBehavriour(MDAlertBehaviour.WILL_SHOW,this);
        }
    }
    public virtual void showViewEnd()
    {
        if(alertViewBehavriour!=null)
        {
            alertViewBehavriour(MDAlertBehaviour.DID_SHOW,this);
        }
    }
    public virtual void hiddenView()
    {
        if(alertViewBehavriour!=null)
        {
            alertViewBehavriour(MDAlertBehaviour.WILL_HIDDEN,this);
        }
    }
    public virtual void hiddenViewEnd()
    {
        if(alertViewBehavriour!=null)
        {
            alertViewBehavriour(MDAlertBehaviour.DID_HIDDEN,this);
        }
    }
    public virtual void OnBtnCancel()
    {
        if(alertViewBehavriour!=null)
        {
			clickIndex = CLICK_Cancel;
            alertViewBehavriour(MDAlertBehaviour.CLICK_CANCEL,this);
        }
    }
    public virtual void OnBtnOk()
    {
        if(alertViewBehavriour!=null)
        {
			clickIndex = CLICK_OK;
            alertViewBehavriour(MDAlertBehaviour.CLICK_OK,this);
        }
    }
	public virtual void OnBtnOkWithIndex(int index)
	{
		if(alertViewBehavriour!=null)
		{
			clickIndex = index;
			alertViewBehavriour(MDAlertBehaviour.CLICK_OK,this);
		}
	}
    public virtual void OnBtnBackGround()
    {
        if(alertViewBehavriour!=null)
        {
			clickIndex = CLICK_BackGround;
            alertViewBehavriour(MDAlertBehaviour.CLICK_BACKGROUND,this);
        }
    }
}