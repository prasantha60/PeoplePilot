using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestScript : MonoBehaviour {
	

	Object[] textures ; //To Store the shape texture from foldure
	List<Layers> layersList = new List<Layers>(); //its the Class, to Keep Track of All the Layers in the sceane
	List<GameObject> SelectObjectList = new List<GameObject>(); // To store the Slected Shapes for edting (pane,scale,rotate)

	
	GameObject SelectOBject; //===== for testing ===
	
	//====== Contril Slection Choice ======
	enum EnuSelectContolType{move,TopLeft,top,TopRight,Right,BottomRight,Bottom,BottomLeft,Left}
	
	
	Bounds bound = new Bounds(Vector3.zero,Vector3.zero);
	EnuSelectContolType SelectControlType;//====== Which Control Currently Slected
	Vector3 ControlSelPos = Vector3.zero;//======= When the Drag Start to Store the Start position
	GameObject SelectContol;//====== Which Control is selected 
	bool ControlSelected;//===== Is any Control is selected 
	
	// Use this for initialization
	void Start () {	
		//====== Load All the Shape texture ======
		textures = Resources.LoadAll("Shapes", typeof(Texture2D));
	}
	
		
	void OnGUI()
	{
		LoadControlLIst(); //==== Display the Shape List ====
		LoadLayersName(); //===== Display Whic shapes add to the sceane ====
	}
	
	//======= Load the Shap List========
	void LoadControlLIst()
	{
		float Xposition = Screen.width - 120;
		GUI.Label(new Rect(Xposition,30,100,30),"Select Shapes");
		int YoffSet = 50;
		for (int i=0; i < textures.Length; i++)
		{
			if (GUI.Button(new Rect(Xposition,YoffSet,100,30),textures[i].name))
			{
				InsertLayer(i); //======= Add Shape to Scean =====
			}
			YoffSet = YoffSet + 35;
		}
	}
	
	//====== Load the Layers Name ======
	void LoadLayersName()
	{
		GUI.Label(new Rect(20,30,100,30),"Layers");
		int YoffSet = 50;
		foreach (Layers l in  layersList)
		{
			if (GUI.Button(new Rect(20,YoffSet,100,30),l.ShapeName)){
				SelectOBject = l.LayerPlane;
				SelectObjectList.Add(l.LayerPlane);
			}
			
			if (GUI.Button(new Rect(130,YoffSet,20,30),"-")){DeleteLayer(l);}  //====== Delete the Shape from the Sceane ===
			YoffSet = YoffSet + 35;
		}
	}
	
	//====== Insert Layer ========
	void InsertLayer(int i)
	{
		Texture2D Tex = textures[i] as Texture2D;
		GameObject GO = GameObject.CreatePrimitive(PrimitiveType.Plane);
		GO.renderer.material.mainTexture = Tex;
		GO.transform.position = new Vector3(0,layersList.Count +2,0);
		GO.transform.localScale = new Vector3(5,5,5);// This value might be modified in the feature
		GO.name= textures[i].name;
		GO.tag = "Shapes";
		Layers l  = new Layers();
		l.LayerID = layersList.Count + 1;
		l.ShapeName = textures[i].name;
		l.LayerPlane = GO;
		layersList.Add(l);
	}
	
	//====== Delete the Layer =======
	void DeleteLayer(Layers L)
	{
		GameObject.Destroy(L.LayerPlane);
		layersList.Remove(L);
	}
	
	//====== Calculae Bonds =========
	void CalcualteBonds()
	{
		
		//======== Remove All the Controls and Recreate againe =======
		GameObject[] Obs = GameObject.FindGameObjectsWithTag("MinipulationControls");
		foreach (GameObject G in Obs){GameObject.Destroy(G);}
		
		Vector3 center = Vector3.zero;
		
		if (SelectObjectList.Count == 0){return;}
		
		foreach (GameObject g in SelectObjectList){center += g.renderer.bounds.center;}
		bound = new Bounds(center,Vector3.zero);
		foreach (GameObject g in SelectObjectList){bound.Encapsulate(g.renderer.bounds);}
		//bound.Encapsulate(SelectOBject.renderer.bounds);
		
		//===== center =======
		GameObject c = GameObject.CreatePrimitive(PrimitiveType.Plane);
		c.transform.position = new Vector3(bound.center.x,10,bound.center.z);
		c.transform.localScale = new Vector3(0.5f,0.5f,0.5f); 
		c.tag =  "MinipulationControls";
		c.name = "Move";
		c.renderer.material.color = Color.red;
		
		//====== Top Left C  =====
		GameObject TopL = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		float x =bound.center.x - bound.extents.x;
		float y = 10;
		float z = bound.center.z + bound.extents.z;
		TopL.transform.position = new Vector3(x,y,z);
		TopL.transform.localScale= new Vector3(5,5,5);	
		TopL.tag = "MinipulationControls";
		TopL.name="TopL";
		
		//====== Top Right C ======
		GameObject TopR = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		x = bound.center.x + bound.extents.x;
		TopR.transform.position = new Vector3(x,y,z);
		TopR.transform.localScale= new Vector3(5,5,5);	
		TopR.tag = "MinipulationControls";
		TopR.name = "TopR";
		
		//===== bottom Right C ======
		GameObject BottomR = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		z = bound.center.z - bound.extents.z;
		BottomR.transform.position = new Vector3(x,y,z);
		BottomR.transform.localScale= new Vector3(5,5,5);
		BottomR.tag = "MinipulationControls";
		BottomR.name = "BottomR";
		
		//===== bottom Left C ======
		GameObject BottomL = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		x = x =bound.center.x - bound.extents.x;
		BottomL.transform.position = new Vector3(x,y,z);
		BottomL.transform.localScale= new Vector3(5,5,5);
		BottomL.tag  = "MinipulationControls";
		BottomL.name = "BottomL";
		
		//==== Top =====
		x = bound.center.x ;
		z = bound.center.z + bound.extents.z;
		GameObject Top = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		Top.transform.position = new Vector3(x,y,z);
		Top.transform.localScale= new Vector3(5,5,5);
		Top.tag = "MinipulationControls";
		Top.name = "Top";
		
		//=== Right ====
		x = bound.center.x + bound.extents.x;
		z = bound.center.z ;
		GameObject Right = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		Right.transform.position = new Vector3(x,y,z);
		Right.transform.localScale= new Vector3(5,5,5);
		Right.tag = "MinipulationControls";
		Right.name = "Right";
		
		//=== bottom ====
		x = bound.center.x ;
		z = bound.center.z - bound.extents.z;
		GameObject bottom = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		bottom.transform.position = new Vector3(x,y,z);
		bottom.transform.localScale= new Vector3(5,5,5);
		bottom.tag  = "MinipulationControls";
		bottom.name = "bottom";
		
		//=== Left ====
		x = bound.center.x - bound.extents.x;
		z = bound.center.z ;
		GameObject left = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		left.transform.position = new Vector3(x,y,z);
		left.transform.localScale= new Vector3(5,5,5);
		left.tag  = "MinipulationControls";
		left.name = "left";
	}
	
	//======== Select the Shape ========
	//See if you clike the shape , if click the shape
	//then add to selected Object List and Calculate the bounds
	void SelectShape(Vector3 pos)
	{
				
		Ray ray = Camera.main.ScreenPointToRay(pos);
 		Debug.DrawRay(ray.origin, ray.direction * 150, Color.yellow);
		
		RaycastHit Hit;
		
		if (Physics.Raycast(ray,out Hit))
		{
			if(Hit.collider.tag == "Shapes")
			{
				GameObject Go  = Hit.collider.gameObject;
				print (Go.name);
				if(!SelectObjectList.Exists(E => E == Go))
				{
					SelectObjectList.Add(Go);
					CalcualteBonds();
				}
				
			}
		}
	}
	
	
	//======== Deselect the Shape =====
	//If you Double Click the Shape and its Slected Shape 
	//then remove from Selected Object List and reCalcuate the bounds
	void DeselectShape(Vector3 pos)
	{
		Ray ray = Camera.main.ScreenPointToRay(pos);
 		Debug.DrawRay(ray.origin, ray.direction * 150, Color.yellow);
		
		RaycastHit Hit;
		
		if (Physics.Raycast(ray,out Hit))
		{
			GameObject Go  = Hit.collider.gameObject;
			if(SelectObjectList.Exists(E => E == Go))
			{
					SelectObjectList.Remove(Go);
					CalcualteBonds();
			}
		}
	}
	
	//======= Check if Any control is selected ========
	void CheckSelectedControlType(Vector3 pos)
	{
		Ray ray = Camera.main.ScreenPointToRay(pos);
 		Debug.DrawRay(ray.origin, ray.direction * 150, Color.yellow);
		RaycastHit Hit;
		
		if (Physics.Raycast(ray,out Hit))
		{
			if (Hit.collider.tag == "MinipulationControls")
			{
			SelectContol = Hit.collider.gameObject;
			ControlSelected = true;
			ControlSelPos =Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, pos.z));;
			string ConName = Hit.collider.name;
			
			GameObject[] obj = GameObject.FindGameObjectsWithTag("MinipulationControls");
			foreach(GameObject g in obj){if(g.name != ConName){GameObject.Destroy(g);}}
			print(ConName);	
			switch (ConName)
			{
				case "Move":{SelectControlType = EnuSelectContolType.move; return;}
				case "TopL":{SelectControlType = EnuSelectContolType.TopLeft;return;}
				case "TopR":{SelectControlType = EnuSelectContolType.TopRight;return;}
				case "BottomR":{SelectControlType = EnuSelectContolType.BottomRight;return;}
				case "BottomL":{SelectControlType = EnuSelectContolType.BottomLeft;return;}
				case "Top":{SelectControlType = EnuSelectContolType.top;return;}
				case "Right":{SelectControlType = EnuSelectContolType.Right;return;}
				case "bottom":{SelectControlType = EnuSelectContolType.Bottom;return;}
				case "left":{SelectControlType = EnuSelectContolType.Left;return;}
			}
			}
			
			
		}
	}
	
	//======= Pan and Scale select the Objects ====
	void PanAndScaleSelectObjects(Vector3 pos)
	{
		Vector3 p=Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, pos.z));
		
		switch (SelectControlType)
		{
			case EnuSelectContolType.move:{PanSelectedObject(pos);break;}
			case EnuSelectContolType.Right:{ScaleRight(pos);break;}
			case EnuSelectContolType.Left:{ScaleLeft(pos);break;}
			case EnuSelectContolType.top:{ScaleTop(pos);break;}
			case EnuSelectContolType.Bottom:{SelectBottom(pos);break;}
			case EnuSelectContolType.TopRight:{RoateSelectedShape(pos);break;}
		}
		
	}
	
	//==== See which control is selected and 
	//send to associated action
	void PanSelectedObject(Vector3 pos)
	{
		Vector3 p=Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, pos.z));
		SelectContol.transform.position = new Vector3(p.x,SelectContol.transform.position.y,p.z);
		Vector3 DisVecter = ControlSelPos - new Vector3(p.x,0,p.z);
		print("Control StartUP =" + ControlSelPos.x.ToString() + " p =" + p.x.ToString() + "  Disx =" + DisVecter.x);
		foreach (GameObject g in SelectObjectList)
		{
			g.transform.position -= new Vector3(DisVecter.x,0,DisVecter.z);
			ControlSelPos = p;
		
		}				
	}
	
	//======= Scale Right Selected Objects =====
	void ScaleRight(Vector3 pos)
	{
		Vector3 p=Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, pos.z));
		SelectContol.transform.position = new Vector3(p.x,SelectContol.transform.position.y,SelectContol.transform.position.z);
		float Distance =  ControlSelPos.x - p.x;
		print (Distance);
		if (Distance < 0)
		{
			foreach (GameObject g in SelectObjectList)
			{
				g.transform.localScale += new Vector3(0.1f,0,0);
				ControlSelPos = p;
			}
		}
		else
		{
			foreach (GameObject g in SelectObjectList)
			{
				if (g.transform.localScale.x < 0.5){g.transform.localScale = new Vector3(0.5f,g.transform.localScale.y,g.transform.localScale.z) ; return;}
				g.transform.localScale -= new Vector3(0.1f,0,0);
				ControlSelPos = p;
			}
		}
		ControlSelPos = new Vector3(p.x,0,0);
	}
	
	//======= Scale Left Selected Objects =======
	void ScaleLeft(Vector3 pos)
	{
		Vector3 p=Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, pos.z));
		SelectContol.transform.position = new Vector3(p.x,SelectContol.transform.position.y,SelectContol.transform.position.z);
		float Distance =  ControlSelPos.x - p.x;
		print (Distance);
		if (Distance < 0)
		{
			foreach (GameObject g in SelectObjectList)
			{
				if (g.transform.localScale.x < 0.5){g.transform.localScale = new Vector3(0.5f,g.transform.localScale.y,g.transform.localScale.z) ; return;}
				g.transform.localScale -= new Vector3(0.1f,0,0);
				ControlSelPos = p;
			}
		}
		else
		{
			foreach (GameObject g in SelectObjectList)
			{
				g.transform.localScale += new Vector3(0.1f,0,0);
				ControlSelPos = p;
			}
		}
		ControlSelPos = new Vector3(p.x,0,0);
	}
	
	//======= Scale Top Selected Objects ======
	void ScaleTop(Vector3 pos)
	{
		Vector3 p=Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, pos.z));
		SelectContol.transform.position = new Vector3(SelectContol.transform.position.x,SelectContol.transform.position.y,p.z);
		float Distance =  ControlSelPos.z - p.z;
		print (Distance);
		if (Distance < 0)
		{
			foreach (GameObject g in SelectObjectList)
			{
				g.transform.localScale += new Vector3(0,0,0.1f);
				ControlSelPos = p;
			}
		}
		else
		{
			foreach (GameObject g in SelectObjectList)
			{
				if (g.transform.localScale.z < 0.5){g.transform.localScale = new Vector3(g.transform.localScale.x,g.transform.localScale.y,0.5f) ; return;}
				g.transform.localScale -= new Vector3(0,0,0.1f);
				ControlSelPos = p;
			}
		}
		ControlSelPos = new Vector3(0,0,p.z);
	}
	
	//======= Scale Bottom Selected Object =======
	void SelectBottom(Vector3 pos)
	{
		Vector3 p=Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, pos.z));
		SelectContol.transform.position = new Vector3(SelectContol.transform.position.x,SelectContol.transform.position.y,p.z);
		float Distance =  ControlSelPos.z - p.z;
		print (Distance);
		if (Distance < 0)
		{
			foreach (GameObject g in SelectObjectList)
			{
				if (g.transform.localScale.z < 0.5){g.transform.localScale = new Vector3(g.transform.localScale.x,g.transform.localScale.y,0.5f) ; return;}
				g.transform.localScale -= new Vector3(0,0,0.1f);
				ControlSelPos = p;
			}
		}
		else
		{
			foreach (GameObject g in SelectObjectList)
			{
				g.transform.localScale += new Vector3(0,0,0.1f);
				ControlSelPos = p;
			}
		}
		ControlSelPos = new Vector3(0,0,p.z);
	}
	
	//======= Roate Selected Shape =========
	void RoateSelectedShape(Vector3 pos)
	{
		print("RoateStart");
		Ray cameraToMouseRay = Camera.main.ScreenPointToRay(pos);
       // backgroundPlane.Raycast(cameraToMouseRay, out fDistanceFromCamera);
        Vector3 vNewMouseHitPoint = Camera.main.ScreenToWorldPoint(new Vector3(pos.x, pos.y, pos.z));
        Vector3 vSelectionCenter = bound.center;
		
		Vector3 vToOriginalMouseHitPoint = ControlSelPos - vSelectionCenter;
        Vector3 vToNewMouseHitPoint = vNewMouseHitPoint - vSelectionCenter;
        float fAngle = Vector3.Angle(vToOriginalMouseHitPoint, vToNewMouseHitPoint);
        Quaternion qRotation = Quaternion.AngleAxis(fAngle , Vector3.left);
		
		print(fAngle);
		foreach(GameObject g in SelectObjectList)
		{
			Quaternion OleQut = g.transform.rotation;
			  Quaternion targetRotation =  Quaternion.Euler(0,g.transform.rotation.y + fAngle,0);
			
			g.transform.rotation = qRotation;//Quaternion.Slerp(g.transform.rotation,targetRotation,Time.deltaTime);
			//g.transform.rotation = Quaternion.AngleAxis(fAngle,Vector3.up); //new Vector3(0,fAngle,0) ;
		}
		
		ControlSelPos = vNewMouseHitPoint;
	}
	
	//========= Mouse and Touch Simulation =========
	void OnEnable(){
		Gesture.onMultiTapE += OnTap;
		Gesture.onDraggingE += OnDragging;
		Gesture.onMultiTapE += OnMultiTap;
		Gesture.onDraggingStartE += OnDraggingStart;
		Gesture.onDraggingEndE += OnDraggingEnd;
		
		Gesture.onPinchE += OnonPinch;
		
	}
	
	void OnDisable(){
		Gesture.onMultiTapE -= OnTap;
		Gesture.onDraggingE -= OnDragging;
		Gesture.onMultiTapE -= OnMultiTap;
		Gesture.onDraggingStartE -= OnDraggingStart;
		Gesture.onDraggingEndE -= OnDraggingEnd;
		Gesture.onPinchE += OnonPinch;
	}
	
	void OnTap(Tap tap){
		//RoatePlane(tap.pos);
		SelectShape(tap.pos);
			
	}
	
	void OnMultiTap(Tap tap)
	{
		if (tap.count == 2){DeselectShape(tap.pos);}
	}
	
	Vector3 Touch1 = Vector3.zero;
	Vector3 touch2 = Vector3.zero;
	void OnDraggingStart(DragInfo dragInfo)
	{
		CheckSelectedControlType(dragInfo.pos);
	}
	
	void OnDragging(DragInfo dragInfo){
		
		//PanSelectObjects(dragInfo.pos);
		if (ControlSelected == true){PanAndScaleSelectObjects(dragInfo.pos);}
		
	}
	
	void OnDraggingEnd(DragInfo dragInfo)
	{
		if(ControlSelected == true)
		{
			CalcualteBonds();
			ControlSelected = false;
		}
		
	}
	
	void OnonPinch(float val)
	{
		Touch touch1 = Input.GetTouch(0);
   		Touch touch2 = Input.GetTouch( 1 );
    	// Find out how the touches have moved relative to eachother:

       Vector2 curDist = touch1.position - touch2.position;
   	   Vector2 prevDist = (touch1.position - touch1.deltaPosition) - (touch2.position - touch2.deltaPosition);

       float touchDelta = curDist.magnitude - prevDist.magnitude;
	   foreach(GameObject g in SelectObjectList)
		{
			if (g.transform.localScale.x < 0.5f){g.transform.localScale = new Vector3(0.5f,g.transform.localScale.y,g.transform.localScale.z);}
			if (g.transform.localScale.z < 0.5f){g.transform.localScale = new Vector3(g.transform.localScale.x,g.transform.localScale.y,0.5f);}
			
			if ( touch1.position.x < touch2.position.x ){g.transform.localScale -= new Vector3(touchDelta*0.5f,0,touchDelta*0.5f);}
      		if ( touch1.position.x > touch2.position.x ){g.transform.localScale += new Vector3(touchDelta*0.5f,0,touchDelta*0.5f);}
		}
		
	}
	
	//====== Rotate the Plane Testing =======
	void RoatePlane(Vector3 pos)
	{
		// Generate a plane that intersects the transform's position with an upwards normal.
    		Plane playerPlane = new Plane(Vector3.up , SelectOBject.transform.position);
		
		// Generate a ray from the cursor position
    		Ray ray = Camera.main.ScreenPointToRay(pos);
 			Debug.DrawRay(ray.origin, ray.direction * 150, Color.yellow);
		
		// Determine the point where the cursor ray intersects the plane.
    	// This will be the point that the object must look towards to be looking at the mouse.
    	// Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
    	// then find the point along that ray that meets that distance.  This will be the point
    	// to look at.
    		float hitdist = 0.0f;
    		// If the ray is parallel to the plane, Raycast will return false.
    		if (playerPlane.Raycast (ray , out hitdist)) 
			{
        		// Get the point along the ray that hits the calculated distance.
        		Vector3 targetPoint = ray.GetPoint(hitdist);
        		// Determine the target rotation.  This is the rotation if the transform looks at the target point.
        		Quaternion targetRotation = Quaternion.LookRotation(targetPoint - SelectOBject.transform.position);
        		// Smoothly rotate towards the target point.
        		SelectOBject.transform.rotation = Quaternion.Slerp(SelectOBject.transform.rotation, targetRotation, 2 * Time.time);
			}
	}
	 
	
	
	
	
}
