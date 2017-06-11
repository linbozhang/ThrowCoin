using System;
using System.IO;
using System.Text;


public class SharedPrefs
{
	public const int TYPE_Player = 1;
	public const int TYPE_Coin = 2;
	public const int TYPE_Shop = 3;

	// const member
	private const string SharedCoinName = "Coin.bin";
	private const string SharedPlayerName = "Player.bin";
	private const string SharedShopName = "adsfasdf.bin";

	// shared preference path

	private string SharedCoinPath;
	private string SharedPlayerPath;
	private string ShardShopPath;

	private static SharedPrefs _sharedPrefs;
	//get salt  
	private byte[] _salt;
	
	//create by accountId
	private string accountFolder;
	
	private SharedPrefs ()
	{
		salt saltFac = new salt ();
		_salt = saltFac.getSalt ();
		Crypto._salt = _salt;
	}

	public static SharedPrefs getInstance ()
	{
		if (_sharedPrefs == null){
			_sharedPrefs = new SharedPrefs ();
			_sharedPrefs.generatePath();
		}
		return _sharedPrefs;
	}

	public void createAccountFolder() {
		accountFolder = DeviceInfo.MAC;
		string activeDir = DeviceInfo.getDocumentDir();
		FileUtility.createFold(activeDir,accountFolder);
	}
	public void generatePath(){

		//root directory. I will use MAC as default Account
		string actDir = null;
		actDir = System.IO.Path.Combine(DeviceInfo.getDocumentDir(),DeviceInfo.MAC); 
//		//WG.SLog(" #######  actDir === " + actDir);
		
		//final full path
		SharedCoinPath = System.IO.Path.Combine(actDir, SharedCoinName);
		SharedPlayerPath = System.IO.Path.Combine(actDir, SharedPlayerName);
		ShardShopPath = System.IO.Path.Combine(actDir,SharedShopName);
	}

	public void saveValue (Object save, int type ,  bool encrypt)
	{
		if (save != null) {
			string outdata = SDK.Serialize (save);
			if (outdata == null || outdata == string.Empty)
				return;
			if (encrypt) 
				outdata = Crypto.EncryptStringAES (outdata, Consts.sharedSecret);

			string path = null;
			switch(type){

			case TYPE_Coin:
				path = SharedCoinPath;
				break;
			case TYPE_Player:
				path = SharedPlayerPath;
				break;
			case TYPE_Shop:
				path = ShardShopPath;
				break;
			}
			
			using (StreamWriter sw = new StreamWriter(  File.Open(path , FileMode.Create) )) {
				sw.Write (outdata);
			}			
		}
	}


	
	public DataPlayer loadPlayer(bool decrypt){
		DataPlayer data = null;

		string encrpytStr = null;
		string decryptStr = null;
		try {
			// Create an instance of StreamReader to read from a file.
			// The using statement also closes the StreamReader.
			using (StreamReader sr = new StreamReader( SharedPlayerPath )) {
				// Read and display lines from the file until the end of the file is reached.
				encrpytStr = sr.ReadToEnd ();
			}

			if (decrypt) {
				decryptStr = Crypto.DecryptStringAES (encrpytStr, Consts.sharedSecret);
				data = SDK.Deserialize<DataPlayer> (decryptStr);
			} else {
				data = SDK.Deserialize<DataPlayer> (encrpytStr);
			}
			
		} catch (Exception e) {
			// Let the user know what went wrong.
			//WG.SLog (e.Message);
		}

		return data;
	}

	public ShopOrder loadShopOrder(bool decrypt){
		ShopOrder data = null;
		string encrpytStr = null;
		string decryptStr = null;
		try {
			// Create an instance of StreamReader to read from a file.
			// The using statement also closes the StreamReader.
			using (StreamReader sr = new StreamReader( ShardShopPath )) {
				// Read and display lines from the file until the end of the file is reached.
				encrpytStr = sr.ReadToEnd ();
			}

			if (decrypt) {
				decryptStr = Crypto.DecryptStringAES (encrpytStr, Consts.shareShopSecret);
				data = SDK.Deserialize<ShopOrder> (decryptStr);
			} else {
				data = SDK.Deserialize<ShopOrder> (encrpytStr);
			}

		} catch (Exception e) {
			// Let the user know what went wrong.
			//WG.SLog (e.Message);
		}

		return data;
	}
	public MDDataCoin loadDataCoin(bool decrypt)
	{
		MDDataCoin data = null;
		string encrpytStr = null;
		string decryptStr = null;
		try {
			// Create an instance of StreamReader to read from a file.
			// The using statement also closes the StreamReader.
			using (StreamReader sr = new StreamReader( SharedCoinPath )) {
				// Read and display lines from the file until the end of the file is reached.
				encrpytStr = sr.ReadToEnd ();
			}

			if (decrypt) {
				decryptStr = Crypto.DecryptStringAES (encrpytStr, Consts.sharedSecret);
				data = SDK.Deserialize<MDDataCoin> (decryptStr);
			} else {
				data = SDK.Deserialize<MDDataCoin> (encrpytStr);
			}

		} catch (Exception e) {
			// Let the user know what went wrong.
			//WG.SLog (e.Message);
		}

		return data;
	}

}


