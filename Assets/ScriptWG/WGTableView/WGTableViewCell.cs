using UnityEngine;
using System.Collections;

public class WGTableViewCell : MonoBehaviour {
	[HideInInspector]
	public int identifier = 0;
	[HideInInspector]
	public int index = 0;
}
public interface WGTableViewDelegate
{
//	根据index获得一个cell,在实现的时候,需要通过 scrView.getCacheCellsWithIdentifier(identifier)获取缓存里面的Cell如果没有,在生成一个新的.
	WGTableViewCell WGTableViewCellWithIndex(WGTableView scrView, int index);
//	获取cell的数目,应该是总行数*总列数
	int NumberOfTableViewCells();
//	选择一个cell对象
	void SelectTableViewCell(WGTableViewCell aCell,int index);
//	长按一个cell对象
	void HoldTableViewCell(WGTableViewCell aCell,int index);
	//	刷新当前在视图内能展示的Cell 需要调用WGTableView.UpdataCells()函数
	void UpdateShowTableViewCells(WGTableViewCell aCell,int index);
}



public class WGScrollPosRecorder{
	public float f_Offset;
	public float f_DeltaTime;
	public WGScrollPosRecorder(float offset,float deltaTime){
		f_Offset = offset;
		f_DeltaTime = deltaTime;
	}
}