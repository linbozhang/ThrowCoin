using System.Collections;

public class Consts
{


	public const int THOUSAND = 1000;

	
	public const int MacX = 0x03;

	// password for AES
	public const string sharedSecret = "IWLLDOGOD";
	public const string shareShopSecret = "meiguohuanyinwo";
	#if TEST
	public const bool AES_ENCRYPT = false;
	#else
	// all local file with AES encryption
	public const bool AES_ENCRYPT = false;
	#endif
	//收集品的总数量
	public const int COLLECTION = 15;
    public const int SKILLNUM = 10;
	public const int WEAPON_MAX = 3;
	public const int WEAPON_MIN = 0;

	public const int BEAR_MONEY = 500;

	public const int COIN_Scale = 10;//1001


}
