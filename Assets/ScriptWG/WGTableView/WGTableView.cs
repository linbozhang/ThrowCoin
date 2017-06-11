using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ForMiniItween;

public class WGTableView : MonoBehaviour {

	Transform mTrans;

	public float fAddOffset = 0;

	public Camera _mainCamera = null;
	Camera mainCamera{
		get{
			if(_mainCamera == null){
				if(UICamera.mainCamera !=null )
				{
					_mainCamera = UICamera.mainCamera;
				}
				else if(Camera.main != null)
				{
					_mainCamera = Camera.main;
				}
			}
			return _mainCamera;
		}
	}
	 
	public WGTableViewDelegate csDelegate;

	//行数
	public int I_Hang = 6;
	
	//单元格的高度
	public float f_TileHeight = 1;

	//分别表示有一部分相连，完全在里面，完全不在
	public enum BoundsWithRect { Part, In, Out };

	//全部的行数
	public int I_HangAll {
		get {

			int num = csDelegate.NumberOfTableViewCells();
			return num;
		}
	}


	//需要滚动的小物件	
	private List<GameObject> list_ScrollItem = new List<GameObject>();

	private List<WGTableViewCell> list_CacheCells = new List<WGTableViewCell>();

	private Dictionary<int,WGTableViewCell> dic_ScrollCells = new Dictionary<int, WGTableViewCell>();


	//滚动物件的父物体
	private GameObject go_ScrollParent;
	
	//在屏幕上的高度
	private float f_HeightInScreen;
	
	//在世界坐标的高度
	private float f_HeightInWorld;

	//偏移量
	private float f_Offset = 0;
	
	//最小偏移量
	private float f_MinOffset = 0;
	
	//最大偏移量
	private float f_MaxOffset = 900;
	
	//鼠标起始坐标
	private Vector3 v3_StartMousePos;
	
	//转化比例
	public float f_WorldInScreenRate;
	

	//状态 （拖动，正常，回弹，惯性）
	public enum State { Drag, Normal, Bounds, Inertia };
	
	private State myState = State.Normal;
	
	//摩擦力系数
	private float f_Friction = 1.2f;
	
	//摩擦力衰减迭代次数
	private float i_Friction = 1f;
	
	//基础摩擦力
	private float f_FrictionBase = 10f;
	
	//速度取样个数（即最后几帧的速度）
	private int i_TakeSampleCount = 5;
	
	//速度
	private float f_Speed;
	
	//滚动记录器
	private List<WGScrollPosRecorder> list_ScrollPosRecorder = new List<WGScrollPosRecorder>();
	
	//边界回弹速度
	private float f_BoundsReturnSpeed = 10;
	
	//回弹的目标位置
	private float f_BoundsTarget = 0;

	public float deltaTime{
		get{
			return RealTime.deltaTime;
		}
	}

	public WGTableViewCell getCacheCellsWithIdentifier(int identifier)
	{
		int Count = list_CacheCells.Count;
		for(int i=0;i<Count;i++)
		{
			WGTableViewCell aCell = list_CacheCells[i];
			if(aCell.identifier == identifier)
			{
				list_CacheCells.RemoveAt(i);
				return aCell;
			}
		}
		return null;
	}
	public void reloadData()
	{
		for(int i = 0;i<I_Hang;i++)
		{

			if((i)<csDelegate.NumberOfTableViewCells())
			{
				WGTableViewCell aCell = csDelegate.WGTableViewCellWithIndex(this,i);
				dic_ScrollCells.Add(i,aCell);
				aCell.transform.parent = go_ScrollParent.transform;
				aCell.transform.localScale = Vector3.one;
				aCell.transform.localPosition = GetPos(i);

			}

		}
		UpdateHeight(false);
	}
	int GetBeginIndex(){

		float value = GetOffset();

		return (int)(value/f_TileHeight);

	}
	public void UpdataCells()
	{
		int beginIndex = GetBeginIndex();
		int endIndex = beginIndex+(I_Hang+1);
		if(beginIndex<0) beginIndex = 0;
		if(endIndex>csDelegate.NumberOfTableViewCells())
		{
			endIndex = csDelegate.NumberOfTableViewCells();
		}
		for(int i=beginIndex;i<endIndex;i++)
		{
			WGTableViewCell aCell ;
			if(dic_ScrollCells.TryGetValue(i,out aCell))
			{
				csDelegate.UpdateShowTableViewCells(aCell,i);
			}
		}
	}
	public void UpdataParentPosition(Vector3 v3)
	{

		go_ScrollParent.transform.localPosition = v3;
		int beginIndex = GetBeginIndex();

		int endIndex = beginIndex+(I_Hang+1);

		if(beginIndex<0) beginIndex = 0;

		if(endIndex>csDelegate.NumberOfTableViewCells())
		{
			endIndex = csDelegate.NumberOfTableViewCells();
		}
		List<int> keys = new List<int>();
		foreach(int key in dic_ScrollCells.Keys)
		{
			if(key>=beginIndex&&key<endIndex)
			{

			}
			else{
				WGTableViewCell aCell;
				if(dic_ScrollCells.TryGetValue(key,out aCell))
				{
					keys.Add(key);

					list_CacheCells.Add(aCell);

					aCell.transform.position = new Vector3(1000,0,0);
				}
				else{
				}
			}
		}
		foreach(int key in keys){
			dic_ScrollCells.Remove(key);
		}

		for(int i=beginIndex;i<endIndex;i++)
		{
			WGTableViewCell aCell ;
			if(dic_ScrollCells.TryGetValue(i,out aCell))
			{

			}
			else{
				aCell = csDelegate.WGTableViewCellWithIndex(this,i);
				aCell.transform.parent = go_ScrollParent.transform;
				aCell.transform.localPosition = getPosWithIndex(i);

				aCell.transform.localScale = Vector3.one;
				aCell.index = i;
				dic_ScrollCells.Add(i,aCell);
			}
		}

	}

	Vector3 getPosWithIndex(int idx)
	{
		return new Vector3(0 , - f_TileHeight / 2 - (f_TileHeight) * idx, 0);
	}

	void createScrollParent()
	{
		go_ScrollParent = new GameObject("ScrollParent");
		go_ScrollParent.transform.parent = this.transform;
		go_ScrollParent.transform.localPosition = Vector3.zero;
		go_ScrollParent.transform.localScale =Vector3.one;
	}

	//一切都重置
	public void AllReset() {
		f_Offset = 0;
		myState = State.Normal;
		foreach(int key in dic_ScrollCells.Keys)
		{
			WGTableViewCell aCell = dic_ScrollCells[key];
			list_CacheCells.Add(aCell);
			aCell.transform.parent = this.transform;
			aCell.transform.localPosition = new Vector3(1000f,0,0);

		}
		dic_ScrollCells.Clear();

		if(go_ScrollParent != null){
			Destroy(go_ScrollParent);
			go_ScrollParent = null;
		}
		createScrollParent();
		f_Offset = 0;
		list_ScrollItem.Clear();
		list_ScrollPosRecorder.Clear();
	}

	public Vector3 GetPos(int index) {

		return  new Vector3(0 , - f_TileHeight / 2 - (f_TileHeight) * index, 0);
	}

	void Awake() {

		mTrans = transform;

//		createScrollParent();

	}
	
	void Update() {
		switch (myState) {
		case State.Normal:
		case State.Drag:
			break;
		case State.Inertia: 
			{

				if (f_Speed == 0) 
				{
					if (f_Offset < f_MinOffset) {
						f_BoundsTarget = f_MinOffset;
						myState = State.Bounds;
					} else if (f_Offset > f_MaxOffset) {
						f_BoundsTarget = f_MaxOffset;
						myState = State.Bounds;
					} else {
						myState = State.Normal;
					}
					return;
				}
				//计算摩擦力
				float oldSpeed = f_Speed;
				float firctionAbs = Mathf.Abs(f_Speed);
				float frictionValue = Mathf.Pow(firctionAbs, i_Friction) * f_Friction;
				float frictionDir = f_Speed / firctionAbs;
				f_Speed = f_Speed - frictionDir * deltaTime * (frictionValue + f_FrictionBase);
				//结算位置
	//			go_ScrollParent.transform.position += (new Vector3(0, f_Speed * Time.deltaTime, 0));
					UpdataParentPosition(go_ScrollParent.transform.localPosition+new Vector3(0, f_Speed * deltaTime, 0));
//




//				float a = 5000;
//				float d = f_Speed>0?1:-1;
//				float t = Time.deltaTime;
//				float v = f_Speed;
//				float s = 0.5f*t*t*a*d+t*v;
//				float v1 = v-a*t*d;
//				f_Speed = v1;
//				Debug.Log("==t="+t+";=v=="+v1+";==s="+s);
//				UpdataParentPosition(go_ScrollParent.transform.localPosition+new Vector3(0,s,0));
//


				f_Offset = GetOffset();
				
				
				if (f_Offset < f_MinOffset) {
					f_BoundsTarget = f_MinOffset;
					myState = State.Bounds;
				} else if (f_Offset > f_MaxOffset) {
					f_BoundsTarget = f_MaxOffset;
					myState = State.Bounds;
				} else {
					if (f_Speed * oldSpeed <= 0) {
						myState = State.Normal;
					}
				}
			}
			break;
		case State.Bounds: {
			
			f_Offset = Mathf.Lerp(f_Offset, f_BoundsTarget, deltaTime * f_BoundsReturnSpeed);


			if (Mathf.Abs(f_Offset - f_BoundsTarget) < 0.001f) {
				f_Offset = f_BoundsTarget;
				myState = State.Normal;
			}
				UpdataParentPosition(new Vector3(0, f_Offset, 0));
			
		}
			break;
		}
		
	}
	
	public float GetOffset() {
		if(go_ScrollParent==null)return 0;
		return go_ScrollParent.transform.localPosition.y;
//		return go_ScrollParent.transform.localPosition.y*f_WorldInScreenRate; 
	}

	/// <summary>
	/// 更新高度信息
	/// </summary>
	public void UpdateHeight(bool b_CalculateMaxOffsetDirectly) {

		f_HeightInWorld =  I_Hang * f_TileHeight;

		Transform t = this.transform.parent;
		float temp = this.transform.localScale.y;
		while(t != null)
		{
			temp *=t.localScale.y;
			t = t.parent;
		}

//		f_WorldInScreenRate = temp;

		CalculateMaxOffset1();
	}

	void CalculateMaxOffset1(){
		f_MaxOffset =  I_HangAll * f_TileHeight - f_HeightInWorld+fAddOffset;

//		f_MaxOffset *=f_WorldInScreenRate;

		if(f_MaxOffset < 0){
			f_MaxOffset = 0;
		}
	}

	//鼠标按下
	public void OnRedMouseDown(Vector3 mousePosition) {

		myState = State.Drag;
		v3_StartMousePos = mousePosition;
		list_ScrollPosRecorder.Clear();
		list_ScrollPosRecorder.Add(new WGScrollPosRecorder(f_Offset, deltaTime));

	}
	
	//鼠标抬起
	public  void OnRedMouseUp(Vector3 mousePosition) {

		if(myState == State.Drag){
			float curOffset = (Input.mousePosition.y - v3_StartMousePos.y);

			f_Offset +=curOffset;

			//松手的时候就得判断接下来是惯性还是回弹
			if (f_Offset < f_MinOffset) {
				f_BoundsTarget = f_MinOffset;
				myState = State.Bounds;
			} else if (f_Offset > f_MaxOffset) {
				f_BoundsTarget = f_MaxOffset;
				myState = State.Bounds;
			} else {
				myState = State.Inertia;
			}
			UpdataParentPosition(new Vector3(0, f_Offset, 0));

			//惯性取样
			if (list_ScrollPosRecorder.Count >= i_TakeSampleCount) {
				list_ScrollPosRecorder.RemoveAt(0);
			}
			list_ScrollPosRecorder.Add(new WGScrollPosRecorder(f_Offset,deltaTime));
			//惯性的结算
			float time = 0;
			foreach(WGScrollPosRecorder aScrollPosRecorder in list_ScrollPosRecorder){
				time += aScrollPosRecorder.f_DeltaTime;
			}

			f_Speed = (list_ScrollPosRecorder[list_ScrollPosRecorder.Count - 1].f_Offset - list_ScrollPosRecorder[0].f_Offset) / time;
			if(time>1)
			{
				f_Speed = 0;
			}
		}
		
	}
	
	//鼠标移动
	public void OnRedMouseMove(Vector3 mousePosition) {

		if (myState == State.Drag) {
			float curOffset =  (Input.mousePosition.y - v3_StartMousePos.y) ;//* f_WorldInScreenRate;
			float resultOffset = f_Offset + curOffset;//偏移量结果

			UpdataParentPosition(new Vector3(0, resultOffset, 0));

			//惯性取样
			if (list_ScrollPosRecorder.Count >= i_TakeSampleCount) {
				list_ScrollPosRecorder.RemoveAt(0);
			} 
			list_ScrollPosRecorder.Add(new WGScrollPosRecorder(resultOffset, deltaTime));
			
		}
	}
		
}