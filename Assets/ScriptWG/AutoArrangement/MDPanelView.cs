using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MDPanelView : MonoBehaviour {

	public float panelWidth = 1f;
	public float panelHeight = 2f;


	public enum ALIGNMENT_HOR {
		left,
		right,
		center,
	}
	public enum ALIGNMENT_VER{
		up,
		center,
		down,
	}
	public ALIGNMENT_VER vertical = ALIGNMENT_VER.up;//竖
	public ALIGNMENT_HOR horizontal = ALIGNMENT_HOR.left;//横

	public List<MDPanelViewCell> list_vertical;
	public List<MDPanelViewCell> list_horizontal;
	public List<MDPanelViewCellInterFace> list_vertical_IF;
	public List<MDPanelViewCellInterFace> list_horizontal_IF;
	
	void Start () {
		InitPanelView();
	}
	private bool bInitView = false;
	public void InitPanelView()
	{
		if(bInitView)return;
		bInitView = true;
		if(list_horizontal==null)
		{
			list_horizontal= new List<MDPanelViewCell>();
		}
		if(list_vertical == null)
		{
			list_vertical = new List<MDPanelViewCell>();
		}
		if(list_horizontal_IF==null)
		{
			list_horizontal_IF = new List<MDPanelViewCellInterFace>();
		}
		if(list_vertical_IF == null)
		{
			list_vertical_IF = new List<MDPanelViewCellInterFace>();
		}

		updateUIWithHorizontal();
		updateUIWithVertical();
	}

	public void addHorizontalCell(MDPanelViewCell aCell)
	{
		list_horizontal.Add(aCell);
		aCell.transform.parent = this.transform;
	}

	public void removeHorizontalCell(MDPanelViewCell aCell)
	{
		list_horizontal.Remove(aCell);
	}

	public void addVerticalCell(MDPanelViewCell aCell)
	{
		list_vertical.Add(aCell);
		aCell.transform.parent = this.transform;
	}

	public void removeVerticalCell(MDPanelViewCell aCell)
	{
		list_vertical.Remove(aCell);
	}

	public void removeHorizontalCell(MDPanelViewCellInterFace aCell)
	{
		list_horizontal_IF.Remove(aCell);
	}
	public void clearHorizontalCell()
	{
		list_horizontal.Clear();
		list_horizontal_IF.Clear();
	}
	public void clearVerticalCell()
	{
		list_vertical.Clear();
		list_vertical_IF.Clear();
	}

	public void addHorizontalCell(MDPanelViewCellInterFace aCell)
	{
		list_horizontal_IF.Add(aCell);
		aCell.getTransform().parent = this.transform;
	}
	public void addVerticalCell(MDPanelViewCellInterFace aCell)
	{
		list_vertical_IF.Add(aCell);
		aCell.getTransform().parent = this.transform;
	}
	public void removeVerticalCell(MDPanelViewCellInterFace aCell)
	{
		list_vertical_IF.Remove(aCell);
	}
	public void updateUIWithHorizontal()
	{
		updateUIWithHorizontal(false,0);
	}
	public void updateUIWithHorizontal(float y)
	{
		updateUIWithHorizontal(true,y);
	}
	private void updateUIWithHorizontal(bool changeYpos,float y)
	{
		if(list_horizontal.Count>0)
		{
			float totalWidth = 0;
			foreach(MDPanelViewCell cell in list_horizontal)
			{
				totalWidth +=cell.getCellWidth();
			}
			float beginX = 0 ;
			if(horizontal == ALIGNMENT_HOR.left){
				beginX = -panelWidth/2;
			}
			else if( horizontal == ALIGNMENT_HOR.center)
			{
				beginX = -totalWidth/2;
			}
			else if(horizontal == ALIGNMENT_HOR.right)
			{
				beginX = panelWidth/2-totalWidth;
			}

			float xpos = beginX;
			for(int i=0;i<list_horizontal.Count;i++)
			{
				Vector3 v3 = list_horizontal[i].transform.localPosition;
				v3.x = xpos+list_horizontal[i].getCellWidth()/2;
				if(changeYpos)
				{
					v3.y = y;
				}
				list_horizontal[i].transform.localPosition = v3;
				xpos += list_horizontal[i].getCellWidth();
			}
		}
		if(list_horizontal_IF.Count>0)
		{
			float totalWidth = 0;
			foreach(MDPanelViewCellInterFace cell in list_horizontal_IF)
			{
				totalWidth +=cell.getCellWidth();
			}
			float beginX = 0 ;
			if(horizontal == ALIGNMENT_HOR.left){
				beginX = -panelWidth/2;
			}
			else if( horizontal == ALIGNMENT_HOR.center)
			{
				beginX = -totalWidth/2;
			}
			else if(horizontal == ALIGNMENT_HOR.right)
			{
				beginX = panelWidth/2-totalWidth;
			}
			
			float xpos = beginX;
			for(int i=0;i<list_horizontal_IF.Count;i++)
			{
				Vector3 v3 = list_horizontal_IF[i].getTransform().localPosition;
				v3.x = xpos+list_horizontal_IF[i].getCellWidth()/2;
				if(changeYpos)
				{
					v3.y = y;
				}
				list_horizontal_IF[i].getTransform().localPosition = v3;
				xpos += list_horizontal_IF[i].getCellWidth();
			}
		}
	}
	public void updateUIWithVertical()
	{
		updateUIWithVertical(false,0);
	}
	public void updateUIWithVertical(float x)
	{
		updateUIWithVertical(true,x);
	}
	private void updateUIWithVertical(bool changeXpos,float x)
	{
		if(list_vertical.Count>0)
		{
			float totalHeight = 0;
			foreach(MDPanelViewCell cell in list_vertical)
			{
				totalHeight +=cell.getCellHeight();
			}
			float beginY = 0 ;
			if(vertical == ALIGNMENT_VER.up){
				beginY = panelHeight/2;
			}
			else if( vertical == ALIGNMENT_VER.center)
			{
				beginY = totalHeight/2;
			}
			else if(vertical == ALIGNMENT_VER.down)
			{
				beginY = -panelHeight/2+totalHeight;
			}
			float ypos = beginY;
			for(int i =0;i<list_vertical.Count;i++)
			{
				Vector3 v3 = list_vertical[i].transform.localPosition;
				v3.y = ypos-list_vertical[i].getCellHeight()/2;
				if(changeXpos)
				{
					v3.x = x;
				}
				list_vertical[i].transform.localPosition = v3;

				ypos -=list_vertical[i].getCellHeight();
			}
		}
		if(list_vertical_IF.Count>0)
		{
			float totalHeight = 0;
			foreach(MDPanelViewCellInterFace cell in list_vertical_IF)
			{
				totalHeight +=cell.getCellHeight();
			}
			float beginY = 0 ;
			if(vertical == ALIGNMENT_VER.up){
				beginY = panelHeight/2;
			}
			else if( vertical == ALIGNMENT_VER.center)
			{
				beginY = totalHeight/2;
			}
			else if(vertical == ALIGNMENT_VER.down)
			{
				beginY = -panelHeight/2+totalHeight;
			}
			float ypos = beginY;
			for(int i =0;i<list_vertical_IF.Count;i++)
			{
				Vector3 v3 = list_vertical_IF[i].getTransform().localPosition;
				v3.y = ypos - list_vertical_IF[i].getCellHeight()/2;
				if(changeXpos)
				{
					v3.x = x;
				}
				list_vertical_IF[i].getTransform().localPosition = v3;
				ypos -=list_vertical_IF[i].getCellHeight();
			}
		}
	}



	public Color boundsColor = Color.green;
	public bool b_OnDrawGizmos = true;
	public bool b_OnDrawCenterLine = true;
//	public int verticalNum = 2;
//	public int horizontalNum = 2;
	void OnDrawGizmos() {
		if(b_OnDrawGizmos)
		{
			Gizmos.color = boundsColor;
			Gizmos.DrawLine(Pos(-panelWidth/2,-panelHeight/2), Pos(panelWidth/2,-panelHeight/2));
			Gizmos.DrawLine(Pos(-panelWidth/2,panelHeight/2), Pos(panelWidth/2,panelHeight/2));
			Gizmos.DrawLine(Pos(-panelWidth/2,-panelHeight/2), Pos(-panelWidth/2,panelHeight/2));
			Gizmos.DrawLine(Pos(panelWidth/2,-panelHeight/2), Pos(panelWidth/2,panelHeight/2));
		}
		if(b_OnDrawCenterLine)
		{
			Gizmos.color = boundsColor;
			Gizmos.DrawLine(Pos(-panelWidth/2,0),Pos(panelWidth/2,0));
			Gizmos.DrawLine(Pos(0,-panelHeight/2),Pos(0,panelHeight/2));
		}
	}
	Vector3 Pos(float x,float y){
		return this.transform.position + x * Vector3.right + y * Vector3.up;
	}

}
