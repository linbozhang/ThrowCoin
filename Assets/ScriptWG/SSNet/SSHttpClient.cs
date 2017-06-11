using System;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;


public class SSHttpClient
{
	public const int TIME_OUT = 20000; // Milliseconds 
	private const int BIT_BUFFER_SIZE = 8192;
	private const int RETRY_TIMES = 2;

	private const string USERAGENT = "Mozilla/5.0";
	// used to build entire input
	private StringBuilder sb; 
	// used on each read operation
	private byte[] buf; 

	private SSHttpClient ()
	{
		sb = new StringBuilder ();
		buf = new byte[BIT_BUFFER_SIZE];
		ServicePointManager.DefaultConnectionLimit = 20;
	}
	private static SSHttpClient clientEnd;
	public static SSHttpClient getInstance ()
	{
		if (clientEnd == null)
			clientEnd = new SSHttpClient ();
		return clientEnd;
	}



	public string doRequest (string urlReq, int times = 1) {
		bool isExceOcurr = false;

		string strResponse = null;
		if (sb.Length >= 1)
			sb.Remove (0, sb.Length);


		//WG.SLog(urlReq);
//		ConsoleEx.Write("Http Request is going out : => " + urlReq);
		// prepare the web page we will be asking for
		HttpWebRequest request = (HttpWebRequest) WebRequest.Create (urlReq);

		request.UserAgent = USERAGENT;
		request.Method = "POST";
		// Set the 'Timeout' property in Milliseconds.
		request.Timeout = TIME_OUT;
		// execute the request 
		// This request will throw a WebException if it reaches the timeout limit before it is able to fetch the resource.
		Stream resStream = null;
		HttpWebResponse response = null;
		try {
			response = (HttpWebResponse)request.GetResponse ();

			// we will read data via the response stream
			resStream = response.GetResponseStream ();

			string tempString = null;
			int count = 0;

			do {
				// fill the buffer with data
				count = resStream.Read (buf, 0, buf.Length);


				// make sure we read some data
				if (count != 0) {
					// translate from bytes to ASCII text
					tempString = Encoding.UTF8.GetString (buf, 0, count);

					// continue building the string
					sb.Append (tempString);
				}
			} while (count > 0); // any more data to read?

		} catch (WebException ex) {
			isExceOcurr = true;
			//WG.SLog( "###### WebException = " + ex.ToString () + "\nex.Message = " + ex.Message + "\nEx.status = " + ex.Status.ToString());
		} catch (System.Exception ex){
			isExceOcurr = true;
			//WG.SLog ( "###### Exception = " + ex.ToString ());
		} finally {
			if (resStream != null) {resStream.Close ();	resStream = null;}
			if(response != null) {response.Close (); response = null;}
			if(request != null) {request.Abort(); request = null;}

			if(!isExceOcurr) {
				strResponse = sb.ToString ();

				System.GC.Collect();
			} else {
				System.GC.Collect();

				if(times < RETRY_TIMES) {
					//If exception is ocurr, we try again after a few second.
					Thread.Sleep(500);
					strResponse = doRequest(urlReq, ++ times);
				}
			}


		}

		return strResponse;
	}
	public bool doDownload (string urlReq,string path) {

		bool isExceOcurr = false;




		HttpWebRequest request = (HttpWebRequest) WebRequest.Create (urlReq);

		request.Method = "GET";
		// Set the 'Timeout' property in Milliseconds.
		request.Timeout = TIME_OUT;
		// execute the request 
		// This request will throw a WebException if it reaches the timeout limit before it is able to fetch the resource.
		Stream resStream = null;
		HttpWebResponse response = null;
		FileStream fs = null;
		try {
			response = (HttpWebResponse)request.GetResponse ();

			// we will read data via the response stream
			resStream = response.GetResponseStream ();
			// we will open the file stream whatever 
			if(File.Exists(path))
			{
				fs = File.OpenWrite(path);
			}
			fs = File.Create(path);

			int count = 0;
			long curPrgress = 0;

			do {
				// fill the buffer with data
				count = resStream.Read (buf, 0, buf.Length);
				curPrgress += count;

				// make sure we read some data
				if (count > 0) {
					fs.Write(buf, 0, count);
				}
			} while (count > 0); // any more data to read?

		} catch (WebException ex) {
			isExceOcurr = true;
			//WG.SLog( "###### WebException = " + ex.ToString ());
		} catch (System.Exception ex){
			isExceOcurr = true;
			//WG.SLog ( "###### Exception = " + ex.ToString ());
		} finally {
			if (resStream != null) {resStream.Close ();	resStream = null;}
			if(response != null) {response.Close (); response = null;}
			if(request != null) {request.Abort(); request = null;}
			if(fs != null) {fs.Flush(); fs.Close(); fs = null;}

			System.GC.Collect();
		}


		return isExceOcurr;
	}
}
