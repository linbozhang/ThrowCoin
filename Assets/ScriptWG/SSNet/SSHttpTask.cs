using System;
using System.Collections;

public enum SSTaskType{
	Common			= 0,
	Download		= 1,
}

public class SSHttpTask  {

	/* The code will be excute after we get response
	 */ 
	public Action<string, string> afterCompleted;

	/* When Error Occured, it will notify user
	 */ 
	public Action<string, string> ErrorOccured;

	public string request 	= null;
	public string response 	= null;
	public string errorInfo	= null;
	public string filePath 	= null;
	public SSTaskType reqType = SSTaskType.Common;



	public void handleErrorOcurr() {
		if(ErrorOccured != null ) {
			if(!string.IsNullOrEmpty(errorInfo))
				ErrorOccured(request, errorInfo);
			else {
				ErrorOccured(request, response);
			}
		} 
	}

	public void handleMainThreadCompleted() {

		if(afterCompleted != null)
			afterCompleted(request, response);
	}
}
