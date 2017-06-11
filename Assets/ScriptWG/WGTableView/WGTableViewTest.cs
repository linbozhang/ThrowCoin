using UnityEngine;
using System.Collections;

public class WGTableViewTest : MonoBehaviour,WGTableViewDelegate {

	public GameObject goTestCell;

	public WGTableView myScrollView;

	// Use this for initialization
	void Start () {
		myScrollView.csDelegate = this;

		myScrollView.AllReset();

		myScrollView.reloadData();
	}
	
	//	根据index获得一个cell,在实现的时候,需要通过 scrView.getCacheCellsWithIdentifier(identifier)获取缓存里面的Cell如果没有,在生成一个新的.
	public WGTableViewCell WGTableViewCellWithIndex(WGTableView scrView, int index){

		WGTableViewCell cell = scrView.getCacheCellsWithIdentifier(1);

		WGTestCell tCell ;
		if(cell == null)
		{
			GameObject go = createCell();
			tCell = go.GetComponent<WGTestCell>();
			cell = tCell as WGTableViewCell;
			cell.identifier = 1;
		}
		else{
			tCell = cell as WGTestCell;
		}
		tCell.index = index;

		tCell.freshWithIndex(index);

		return cell;

	}
	//	获取cell的数目,应该是总行数*总列数
	public int NumberOfTableViewCells(){
		return 30;
	}
	//	选择一个cell对象
	public void SelectTableViewCell(WGTableViewCell aCell,int index){
		Debug.Log("selectScrollViewCellAndIndex=="+index);
	}
	//	长按一个cell对象
	public void HoldTableViewCell(WGTableViewCell aCell,int index){
		Debug.Log("holdScrollViewCellAndIndex=="+index);
	}
	//	刷新当前在视图内能展示的Cell 需要调用SQYScrollView.UpdataCells()函数
	public void UpdateShowTableViewCells(WGTableViewCell aCell,int index){
	}


	GameObject createCell()
	{
		GameObject go = Instantiate(goTestCell) as GameObject;
		return go;
	}
}
	