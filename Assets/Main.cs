using UnityEngine;
using System.Collections;
using System.IO;
//using System.Windows.Forms;

public class CustomPlane {
	public Renderer planeRenderer;
	private Vector3 position;
	private Texture texture;
	private Vector3 size;
	private string fileName;
	public string getSerialized() {
		string result = "";
		result += "{{p:" + this.position.x.ToString() + ";" + this.position.y.ToString() + ";" +this.position.z.ToString() + "}";
		result += "{s:" + this.size.x.ToString() + ";" + this.position.y.ToString() + ";" + this.size.z.ToString() + "}";
		result += "{t:" + this.fileName + "}";
		result += "}";
		return result;
	}
	public void setSerialized(string data) {
	}
	public CustomPlane(Renderer source, int index) {
		this.planeRenderer = source;
		this.position = new Vector3(0, 0, (float) -index);
		this.size = new Vector3(1, 0, 1);
		if (index > 0) {
			this.planeRenderer.material = new Material(Shader.Find("FX/Flare"));
			this.planeRenderer.transform.localPosition = this.position;
			this.planeRenderer.transform.localScale = this.size;
			//this.planeRenderer.material.shader = Shader.Find("FX/Flare");
		}
	}
	public void move(Vector3 newPosition) {
		this.position = newPosition;
		this.planeRenderer.transform.localPosition = this.position;
	}
	public void move(float diffX, float diffY) {
		this.position.x -= diffX;
		this.position.y -= diffY;
		this.planeRenderer.transform.localPosition = this.position;
	}
	public void resize(Vector3 newSize) {
		this.size = newSize;
		this.planeRenderer.transform.localScale = this.size;
	}
	public void resize(float width, float height) {
		this.size.x = width;
		this.size.z = height;
		this.planeRenderer.transform.localScale = this.size;
	}
	public void updateTexture(string fileName) {
		//file
		this.fileName = fileName;
		FileInfo file = new FileInfo (fileName);
		FileStream rd = file.OpenRead();
		byte[] data = new byte[file.Length];
		rd.Read(data, 0, (int) file.Length);
		Texture2D texture = new Texture2D(4, 4);
		texture.LoadImage(data);
		this.updateTexture(texture);
	}
	private void updateTexture(Texture newTex) {
		this.planeRenderer.material.mainTexture = newTex;
	}
	public float getWidth() {
		return this.size.x;
	}
	public float getHeight() {
		return this.size.z;
	}
}

public class Main : MonoBehaviour {
	private string folderPath = @"./RuntimeAssets/Shapes";
	//public UnityEngine.Plane newPlane;
	public Renderer planeRenderer;
	
	private CustomPlane[] planes;
	private FileInfo[] textureFiles;
	private Renderer newPlane;
	private string shaderSearchString;
	private Shader usedShader;
	private Texture usedTexture;
	private float mouseX;
	private float mouseY;
	private float mouseZ;
	private bool mouseLeftButton = false;
	private Vector3 mouseRay;
	private int selectedPlane; 
	private Vector2 planesListScrollPosition;
	private Vector2 textureListScrollPosition;
	//private bool mouseLeftButton;

	// Use this for initialization
	void Start () {
		DirectoryInfo dI = new DirectoryInfo(Path.GetFullPath(folderPath));
		this.textureFiles =  dI.GetFiles("*.png");
		//this.newPlane = new Plane(new Vector3(0.1F, 0.1F, 0.1F), new Vector3(1.0F, 1.0F, 1.0F));
		//newPlane = (Renderer) Instantiate(planeRenderer);
		//newPlane.transform.localPosition = new Vector3(0, 0, -1);
		this.shaderSearchString = "FX/Flare";
		this.usedShader = Shader.Find(this.shaderSearchString);
		this.usedTexture = planeRenderer.material.mainTexture;
		this.planes = new CustomPlane[1];
		this.planes[0] = new CustomPlane(planeRenderer, 0);
		this.planesListScrollPosition = new Vector2(0, 0);
		this.textureListScrollPosition = new Vector2(0, 0);
	}
	private void saveStructure() {
		string structInfo = "";
		for (int i = 1; i < this.planes.Length; i++) {
			structInfo += this.planes[i].getSerialized();
		}
		Debug.Log("savedStruct:\"" + structInfo + "\"");
		FileInfo fi = new FileInfo("./SaveFile1.psv");
		StreamWriter wrt = fi.CreateText();
		wrt.Write(structInfo);
		wrt.Close();
	}
	private void resetLayout() {
		for (int i = 1; i < this.planes.Length; i++) {
			Destroy(this.planes[i].planeRenderer);
		}
		CustomPlane[] tmp = new CustomPlane[1];
		tmp[0] = this.planes[0];
		this.planes = tmp;
	}
	private void loadStructure() {
		FileInfo fi = new FileInfo("./SaveFile1.psv");
		StreamReader rdr = fi.OpenText();
		string[] tmpData = rdr.ReadToEnd().Split(new string[]{"}}{{"}, System.StringSplitOptions.RemoveEmptyEntries);
		rdr.Close();
		CustomPlane[] newLayout = new CustomPlane[tmpData.Length + 1];
		for (int i = 0; i< tmpData.Length; i++) {
			Debug.Log("layer: " + tmpData[i]);
			tmpData[i] = tmpData[i].Replace("{", "");
			string[] pars = tmpData[i].Split(new string[]{"}"}, System.StringSplitOptions.RemoveEmptyEntries);
			newLayout[i+1] = new CustomPlane((Renderer) Instantiate(planeRenderer), i+1);
			for (int j = 0; j < pars.Length; j++) {
				Debug.Log("param: " + pars[j]);
				string[] coords;
				Vector3 position;
				switch (pars[j].Substring(0,1)) {
				case "p": 
					//position
					coords = pars[j].Split(new string[]{"p:", ";"}, System.StringSplitOptions.RemoveEmptyEntries);
					position = new Vector3(0, 0, 0);
					position.x = (float) System.Convert.ToDouble(coords[0]);
					position.y = (float) System.Convert.ToDouble(coords[1]);
					position.z = (float) System.Convert.ToDouble(coords[2]);
					newLayout[i+1].move(position);
					break;
				case "s":
					//size
					coords = pars[j].Split(new string[]{"s:", ";"}, System.StringSplitOptions.RemoveEmptyEntries);
					position = new Vector3(0, 0, 0);
					position.x = (float) System.Convert.ToDouble(coords[0]);
					position.y = (float) System.Convert.ToDouble(coords[1]);
					position.z = (float) System.Convert.ToDouble(coords[2]);
					newLayout[i+1].resize(position);
					break;
				case "t":
					//string.
					newLayout[i+1].updateTexture(pars[j].Substring(2));
					//texture
					break;
				default:
					Debug.LogError("Error loading layer: " + pars[j]);
					for (int k = 0; k <=i; k++) {
						Destroy(newLayout[k+1].planeRenderer);
					}
					return;
				}
			}
		}
		newLayout[0] = this.planes[0];
		this.planes = newLayout;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 winSize = GetMainGameViewSize();
		bool mouseLeftPressed = Input.GetMouseButton(0);
		if (mouseLeftPressed && Input.mousePosition.y >55 && Input.mousePosition.x <winSize.x-35)  {// != this.mouseLeftButton) { //Input.GetKeyDown("Fire1")) {
			//just pressed button.
			float diffX = this.mouseRay.x - camera.ScreenToWorldPoint(Input.mousePosition).x;
			float diffY = this.mouseRay.y - camera.ScreenToWorldPoint(Input.mousePosition).y;
			if (this.selectedPlane > 0) {
				this.planes[this.selectedPlane].move(diffX, diffY);
			}
			//newPlane.transform.localPosition = new Vector3(oldPosition.x - diffX, oldPosition.y - diffY, -1);
		}
		this.mouseLeftButton = mouseLeftPressed;
		this.mouseX = Input.mousePosition.x;
		this.mouseY = Input.mousePosition.y;
		this.mouseZ = Input.mousePosition.z;
		this.mouseRay = camera.ScreenToWorldPoint(Input.mousePosition);
	}
	void OnGUI() {
		Vector2 winSize = GetMainGameViewSize();
		GUILayout.BeginVertical("box");
		//planesListScrollPosition = GUI.BeginScrollView(new Rect(0, 0, 110, 100), planesListScrollPosition, new Rect(0, 0, 80, this.planes.Length * 32));
		if (GUILayout.Button("Add Layer", new GUILayoutOption[0])) {
			CustomPlane[] newPlaneList = new CustomPlane[this.planes.Length+1];
			for (int i = 0; i < this.planes.Length;i++) {
				newPlaneList[i] = this.planes[i];
			}
			//Plane tmpPlane = new Plane(new Vector3(1, 1, 0), -this.planes.Length);
			//Renderer tmpRenderer = new Renderer();
			//tmpRenderer.gameObject = tmpPlane;
			//tmpRenderer.
			newPlaneList[this.planes.Length] = new CustomPlane((Renderer) Instantiate(planeRenderer), this.planes.Length);
			this.planes = newPlaneList;
		}
		if (GUILayout.Button("Background", new GUILayoutOption[0])){
			this.selectedPlane = 0;
		}
		for (int i = 1; i < this.planes.Length; i++) {
			if (GUILayout.Button((i == this.selectedPlane? "-->" : "") + "Layer #" + i.ToString(), new GUILayoutOption[0])){
				this.selectedPlane = i;
			}
		}
        //GUI.EndScrollView();
		GUILayout.EndVertical();
		if (this.selectedPlane > 0) {
			//textureListScrollPosition = GUI.BeginScrollView(new Rect(0, 100, 100, 200), textureListScrollPosition, new Rect(0, 0, 80, this.textureFiles.Length * 32));
			if (GUILayout.Button("Delete Layer", new GUILayoutOption[0])) {
				Destroy(this.planes[this.selectedPlane].planeRenderer);
				CustomPlane[] tmpPlanes = new CustomPlane[this.planes.Length-1];
				for (int i = 0; i < this.selectedPlane; i++) {
					tmpPlanes[i] = this.planes[i];
				}
				for (int i = this.selectedPlane+1; i < this.planes.Length; i++) {
					tmpPlanes[i-1] = this.planes[i];
				}
				this.planes = tmpPlanes;
				this.selectedPlane = 0;
				return;
			}
			GUILayout.BeginVertical("box");
			foreach(FileInfo file in this.textureFiles) {
				if (GUILayout.Button(file.Name, new GUILayoutOption[0])) {
					this.planes[this.selectedPlane].updateTexture(file.FullName);
					//newMaterial.mainTexture = texture;
					//newMaterial.SetTextureScale("_MainTex", new Vector2(3F, 3F));
					//this.usedTexture = texture;
				}
			}
	        //GUI.EndScrollView();
			GUILayout.EndVertical();
			float newWidth = GUI.HorizontalScrollbar(new Rect(0, winSize.y-35, winSize.x-35, winSize.y-55), this.planes[this.selectedPlane].getWidth(), 1, 1, 10);
			float newHeight = GUI.VerticalScrollbar(new Rect(winSize.x-15, 0, winSize.x, winSize.y-55), this.planes[this.selectedPlane].getHeight(), 1, 1, 10);
			this.planes[this.selectedPlane].resize(newWidth, newHeight);
			
			
			GUILayout.Label(newWidth.ToString() +"x" + newHeight.ToString(), new GUILayoutOption[0]);
		}
		if (this.selectedPlane == 0) {
			if(GUI.Button(new Rect(winSize.x-100, winSize.y-35, 100, 35), "Load")) {
				this.loadStructure();
			}
			if(GUI.Button(new Rect(winSize.x-100, winSize.y-70, 100, 35), "Save")) {
				this.saveStructure();
			}
		}
		/*
		//input
		Material newMaterial = new Material(usedShader);
		Vector2 winSize = GetMainGameViewSize();
		string newName = GUI.TextField(new Rect(winSize.x-100, winSize.y-20, 100, 20), this.shaderSearchString);
		if (newName != this.shaderSearchString) {
			this.shaderSearchString = newName;
			Shader shd = Shader.Find(this.shaderSearchString);
			if (shd != null) {
				newMaterial = new Material(shd);
				this.usedShader = shd;
			}
		}
		// textures
		newMaterial.mainTexture = this.usedTexture;
		foreach(FileInfo file in this.textureFiles) {
			if (GUILayout.Button(file.Name)) {
				FileStream rd = file.OpenRead();
				byte[] data = new byte[file.Length];
				rd.Read(data, 0, (int) file.Length);
				Texture2D texture = new Texture2D(4, 4);
				texture.LoadImage(data);
				newMaterial.mainTexture = texture;
				//newMaterial.SetTextureScale("_MainTex", new Vector2(3F, 3F));
				this.usedTexture = texture;
			}
		}
		
		//texture scale
		this.textureHorizontalScale = GUI.HorizontalScrollbar(new Rect(0, winSize.y-35, winSize.x-35, winSize.y-55), this.textureHorizontalScale, 1, 1, 10);
		this.textureVerticalScale = GUI.VerticalScrollbar(new Rect(winSize.x-15, 0, winSize.x, winSize.y-55), this.textureVerticalScale, 1, 1, 10);

		//this.textureHorizontalOffset = GUI.HorizontalScrollbar(new Rect(0, winSize.y-55, winSize.x-35, winSize.y-40), this.textureHorizontalOffset, 1, 1, 10);
		//this.textureVerticalOffset = 10 - GUI.VerticalScrollbar(new Rect(winSize.x-35, 0, winSize.x-10, winSize.y-55), 10 - this.textureVerticalOffset, 1, 1, 10);
		
		//newMaterial.SetTextureScale("_MainTex", new Vector2(this.textureHorizontalScale, this.textureVerticalScale));
		newPlane.transform.localScale = new Vector3(this.textureHorizontalScale,1,  this.textureVerticalScale);
		newMaterial.SetTextureOffset("_MainTex", new Vector2(this.textureHorizontalOffset, this.textureVerticalOffset));
		newPlane.material = newMaterial;
		if (this.debug) {
			GUILayout.Label("x:" + this.mouseX.ToString() + "::" + Input.mousePosition.x, new GUILayoutOption[0]);
			GUILayout.Label("y:" + this.mouseY.ToString() + "::" + Input.mousePosition.y, new GUILayoutOption[0]);
			GUILayout.Label("z:" + this.mouseZ.ToString() + "::" + Input.mousePosition.z, new GUILayoutOption[0]);
			GUILayout.Label("l" + (Input.GetMouseButton(0)? "+" : "-")+"r" + (Input.GetMouseButton(1)? "+" : "-")+"m" + (Input.GetMouseButton(2)? "+" : "-"), new GUILayoutOption[0]);
			GUILayout.Label("ray:{" + this.mouseRay.x + "," + this.mouseRay.y + "," + this.mouseRay.z + "}", new GUILayoutOption[0]);
			GUILayout.Label("Scale:{" + this.textureHorizontalScale + "," + this.textureVerticalScale + "}", new GUILayoutOption[0]);
		}
		*/
	}
	public static Vector2 GetMainGameViewSize() {
    	System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
    	System.Reflection.MethodInfo GetSizeOfMainGameView = T.GetMethod("GetSizeOfMainGameView",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
    	System.Object Res = GetSizeOfMainGameView.Invoke(null,null);
    	return (Vector2)Res;
	}
}
