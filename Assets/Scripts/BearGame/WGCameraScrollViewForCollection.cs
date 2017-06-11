using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class WGCameraScrollViewForCollection : MonoBehaviour {

	public UIDraggableCamera dragCamera;

	List<CItemCollectionCell> szCollectionCell = new List<CItemCollectionCell>();

	public Color[] szColors;

	WGDataController _dataCtrl;

	public static WGCameraScrollViewForCollection Instance;
	void Awake()
	{
		Instance = this;
	}



	bool bInit= false;

	public void InitScrollView()
	{

		if(bInit)return;
		bInit = true;
		_dataCtrl = WGDataController.Instance;


		Dictionary<int,List<int>> dicCollections= new Dictionary<int, List<int>>();

		for(int i=0;i<_dataCtrl.szCollectionObj.Count;i++)
		{

			BCObj obj = _dataCtrl.szCollectionObj[i];
			BCCollectionInfo col = _dataCtrl.GetCollectionInfo(obj.ID);
			List<int> szTemp;
			if(dicCollections.TryGetValue(col.groupID,out szTemp))
			{
				szTemp.Add(obj.ID);
			}
			else
			{
				szTemp = new List<int>();
				szTemp.Add(obj.ID);
				dicCollections.Add(col.groupID,szTemp);
			}
		}
		int index =0;

		foreach(KeyValuePair<int,List<int>> kvp in dicCollections)
		{
			CItemCollectionCell cell = CItemCollectionCell.CreateCollectionCell();
			cell.transform.parent = this.transform;
			cell.transform.localScale = Vector3.one;
			cell.transform.localPosition = new Vector3(0,200-index*236,0);
		
			szCollectionCell.Add(cell);

			Color  color = Color.gray;
			if(index<=szColors.Length)
			{
				color = szColors[index];
			}
			cell.freshWithCollections(kvp.Value.ToArray(),szColors[index]);
			cell.SetDragCamera(dragCamera);
			index++;
		}
	}

	public void freshScrollView()
	{
		for(int i=0,max=szCollectionCell.Count;i<max;i++)
		{
			szCollectionCell[i].FreshUI();
		}
	}
}
