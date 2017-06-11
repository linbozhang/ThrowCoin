using UnityEngine;
using UnityEditor;
using System.Collections;

public class SMSpriteEditor : MonoBehaviour {

	
	[MenuItem("SQYAction/ParseCSV_NeedEmpty")]
	static void ParseCSV() {
		
		foreach (Object obj in Selection.objects) {

			string path = AssetDatabase.GetAssetPath(obj);

			Parse.ParseCSV.ParseCSV2JOSN(path,true);

		}
	}
	[MenuItem("SQYAction/ParseCSV_DeleteEmpty")]
	static void ParseCSVNotEmpty() {
		
		foreach (Object obj in Selection.objects) {
			
			string path = AssetDatabase.GetAssetPath(obj);
			
			Parse.ParseCSV.ParseCSV2JOSN(path,false);
			
		}
	}
	[MenuItem("SQYAction/ParseCSV_Format")]
	static void ParseCSVFormat() {

		foreach (Object obj in Selection.objects) {

			string path = AssetDatabase.GetAssetPath(obj);

			Parse.ParseCSV.ParseCSVFormat2JOSN(path);

		}
	}
	
	[MenuItem("SQYAction/JsonToCSV")]
	static void JsonToCSV ()
	{

		foreach (Object obj in Selection.objects) {

			string path = AssetDatabase.GetAssetPath (obj);

			Parse.ParseCSV.JsonToCSV (path);

		}
	}
}
