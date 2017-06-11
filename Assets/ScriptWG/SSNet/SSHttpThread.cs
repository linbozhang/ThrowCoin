using System;
using System.Collections.Generic;
using System.Threading;

public class SSHttpThread
{

	public static string UNABLE_GET_RESPONSE = "Http request is timeout or can't get response.";

	private readonly object _locker = new object ();
	private Queue<SSHttpTask> sendingQueue;
	private Thread httpThread;
	//if user wants to stop Network Engine , Loop should be false
	private bool Loop = true;
	private SSHttpClient httpClient;
	//keep singleton
	private static SSHttpThread _engine;

	private SSHttpThread ()
	{
		_locker = new object ();

		sendingQueue = new Queue<SSHttpTask> ();
		Loop = true;
		httpClient = SSHttpClient.getInstance ();

		httpThread = new Thread (new ThreadStart (Run));
		httpThread.Start ();
	}

	public static SSHttpThread getInstance ()
	{

		return _engine ?? (_engine = new SSHttpThread ());
	}

	public void sendHttpTask (SSHttpTask task)
	{
		lock (_locker) {
			sendingQueue.Enqueue (task);          // We must pulse because we're
			Monitor.Pulse (_locker);              // changing a blocking condition.
		}
	}

	private SSHttpTask getHttpTask ()
	{

		lock (_locker) {
			while (sendingQueue.Count == 0)
				Monitor.Wait (_locker);
			return sendingQueue.Dequeue ();    // This signals our exit.
		}
	}

	private void ResetRequet ()
	{
		lock (_locker) {
			sendingQueue.Clear ();
		}
	}

//	void http_onReceieve (SSHttpTask task, string acknowlege)
//	{
//		if (task.request is HttpRequest) {
//			if (Utils.checkJsonFormat (acknowlege)) {
//				HttpResponseFactory.createResponse (task, acknowlege);
//				AsyncTask.QueueOnMainThread( () => { task.handleMainThreadCompleted(); } );
//
//
//			} else
//			{
//				http_onException (task, acknowlege);
//			}
//
//		} else if (task.request is ThirdPartyHttpRequest) {
//			task.response = new ThirdPartyResponse (acknowlege);
//			AsyncTask.QueueOnMainThread( () => { task.handleMainThreadCompleted(); } );
//
//
//		} else if (task.request is HttpDownloadRequest) {
//						
//			task.response = new HttpDownloadResponse ();
//			AsyncTask.QueueOnMainThread( () => { task.handleMainThreadCompleted(); } );
//
//		}
//
//	}


	// All http error will go through this routine.
	// Handle exception
	void http_onException (SSHttpTask task, string message)
	{
		task.response = message;
		AsyncTask.QueueOnMainThread( () => { task.handleErrorOcurr(); } );

	}

	private void Run ()
	{
		// Keep consuming until told otherwise.
		while (Loop) {
			SSHttpTask task = getHttpTask ();
			if (task != null && task.request != null) {
				if (task.reqType == SSTaskType.Common)
					RunCommon (task);
				else if (task.reqType == SSTaskType.Download)
					RunDownloadTask (task);
			}
		}
	}

	//网络下载任务
	private void RunDownloadTask (SSHttpTask task)
	{
		bool failure = httpClient.doDownload (task.request,task.filePath);
		if (!failure)
			AsyncTask.QueueOnMainThread( () => { task.handleMainThreadCompleted(); } );
		else
			AsyncTask.QueueOnMainThread( () => { task.handleErrorOcurr(); } );
	}

	private void RunCommon (SSHttpTask task)
	{

		string acknowlege = httpClient.doRequest (task.request);
		//Check if we need response or not.


		if (!string.IsNullOrEmpty (acknowlege)) {
			task.response = acknowlege.Trim ();
			AsyncTask.QueueOnMainThread( () => { task.handleMainThreadCompleted(); } );
		} else {
			// error ocurr
			task.response = UNABLE_GET_RESPONSE;
			AsyncTask.QueueOnMainThread( () => { task.handleErrorOcurr(); } );
		}

	}


}






