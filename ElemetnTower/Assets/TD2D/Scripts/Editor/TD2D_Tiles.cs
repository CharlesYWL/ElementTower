using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

public class TD2D_Tiles : EditorWindow
{
	private enum MyStates
	{
		Pointer,
		TilePlacement,
		EnvironmentPlacement
	}
	// Visual content for editor parts
	private class Contents
	{
		public GUIContent rotateLeft;
		public GUIContent rotateRight;
		public GUIContent pointer;
	}
	// Picker window states
	private enum PickerState
	{
		None,
		Map
	}

	private MyStates myState = MyStates.Pointer;
	private float snapStep = 200f;
	private Contents contents = new Contents();
	private GUISkin tilesGuiSkin;
	private string mapLayer = "Map";
	private string tilesLayer = "Tiles";
	private bool inGameSorting;
	private string mapName = "TileMap 1";
	// State for picker window
	private PickerState pickerState = PickerState.None;
	// ID for picker window
	private int currentPickerWindow;
	// Object is selected in picker window
	private Object pickedObject;
	private MapFolderInspector mapFolderInspector;
	private string mapSpriteLabel = "l:MapSprite";

	private int tileIdx = -1;
	private List<GameObject> tilePrefabs = new List<GameObject>();
	private Texture[] tileTextures;
	private Vector2 tilesScrollPos;
	private GameObject tile;

	private int environmentIdx = -1;
	private List<GameObject> environmentPrefabs = new List<GameObject>();
	private Texture[] environmentTextures;
	private Vector2 environmentScrollPos;
	private GameObject environment;

	[MenuItem("Window/TD2D/Tiles")]

	/// <summary>
	/// Shows the window.
	/// </summary>
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(TD2D_Tiles));
	}

	void OnEnable()
	{
		SceneView.onSceneGUIDelegate += UpdateView;
		UpdateStuff();
	}

	void OnDisable()
	{
		SceneView.onSceneGUIDelegate -= UpdateView;
		ClearStuff();
	}

	/// <summary>
	/// Raises the scene opened event.
	/// </summary>
	/// <param name="scene">Scene.</param>
	/// <param name="openSceneMode">Open scene mode.</param>
	private void OnSceneOpened(Scene scene, OpenSceneMode openSceneMode)
	{
		UpdateStuff();
	}

	/// <summary>
	/// Raises the playmode state changed event.
	/// </summary>
	private void OnPlaymodeStateChanged()
	{
		if (EditorApplication.isPlaying == false)
		{
			UpdateStuff();
		}
		else
		{
			ClearStuff();
		}
	}

	private void UpdateStuff()
	{
		mapFolderInspector = FindObjectOfType<MapFolderInspector>();

		List<Texture> tilesList = new List<Texture>();
		string[] paths = AssetDatabase.FindAssets("t:Prefab", new string[] {"Assets/TD2D/Prefabs/Tiles"});
		foreach (string str in paths)
		{
			GameObject tilePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(str));
			if (tilePrefab != null)
			{
				SpriteRenderer sprite = tilePrefab.GetComponent<SpriteRenderer>();
				if (sprite != null && sprite.sprite != null)
				{
					tilePrefabs.Add(tilePrefab);
					tilesList.Add(sprite.sprite.texture);
				}
			}
		}
		tileTextures = tilesList.ToArray();

		List<Texture> environmentList = new List<Texture>();
		paths = AssetDatabase.FindAssets("t:Prefab", new string[] {"Assets/TD2D/Prefabs/Environment"});
		foreach (string str in paths)
		{
			GameObject environmentPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(AssetDatabase.GUIDToAssetPath(str));
			if (environmentPrefab != null)
			{
				SpriteRenderer sprite = environmentPrefab.GetComponent<SpriteRenderer>();
				if (sprite != null && sprite.sprite != null)
				{
					environmentPrefabs.Add(environmentPrefab);
					environmentList.Add(sprite.sprite.texture);
				}
			}
		}
		environmentTextures = environmentList.ToArray();

		tileIdx = -1;
		environmentIdx = -1;
		Tools.lockedLayers |= 1 << LayerMask.NameToLayer(mapLayer);
		Tools.lockedLayers &= ~(1 << LayerMask.NameToLayer(tilesLayer));
		myState = MyStates.Pointer;

		tilesGuiSkin = (GUISkin)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/TilesGuiSkin.guiskin", typeof(GUISkin));

		// Set labels for prebas. Label will use in picker window
		string[] prefabs;
		prefabs = AssetDatabase.FindAssets("t:Sprite", new string[] {"Assets/TD2D/Sprites/Maps/Backgrounds"});
		foreach (string str in prefabs)
		{
			Object obj = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(str));
			AssetDatabase.SetLabels(obj, new string[] {mapSpriteLabel});
		}

		contents.rotateLeft = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/RotateLeft.png", typeof(Texture)));
		contents.rotateRight = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/RotateRight.png", typeof(Texture)));
		contents.pointer = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/Pointer.png", typeof(Texture)));
	}

	private void ClearStuff()
	{
		if (tile != null)
		{
			DestroyImmediate(tile.gameObject);
		}
		if (environment != null)
		{
			DestroyImmediate(environment.gameObject);
		}
		Tools.lockedLayers &= ~(1 << LayerMask.NameToLayer(mapLayer));
		Tools.lockedLayers &= ~(1 << LayerMask.NameToLayer(tilesLayer));
	}

	/// <summary>
	/// Shows the picker window.
	/// </summary>
	/// <param name="state">State.</param>
	/// <param name="filter">Filter.</param>
	private void ShowPicker<T>(PickerState state, string filter) where T : UnityEngine.Object
	{
		pickerState = state;
		// Create a window picker control ID
		currentPickerWindow = EditorGUIUtility.GetControlID(FocusType.Passive);
		// Use the ID you just created
		EditorGUIUtility.ShowObjectPicker<T>(null, false, filter, currentPickerWindow);
	}

	private void SetTile(GameObject tilePrefab)
	{
		if (tile != null)
		{
			DestroyImmediate(tile);
		}
		if (tilePrefab != null)
		{
			tile = Instantiate(tilePrefab);
			tile.name = tilePrefab.name;
		}
		else
		{
			tile = null;
		}
	}

	private void RotateTile(float angle)
	{
		if (tile != null)
		{
			tile.transform.Rotate(0f, 0f, angle);
		}
	}

	private void SetEnvironment(GameObject environmentPrefab)
	{
		if (environment != null)
		{
			DestroyImmediate(environment);
		}
		if (environmentPrefab != null)
		{
			environment = Instantiate(environmentPrefab);
			environment.name = environmentPrefab.name;
		}
		else
		{
			environment = null;
		}
	}

	private void UpdateTemp()
	{
		switch (myState)
		{
		case MyStates.Pointer:
			if (environment != null)
			{
				DestroyImmediate(environment);
			}
			if (tile != null)
			{
				DestroyImmediate(tile);
			}
			if (mapFolderInspector == null)
			{
				mapFolderInspector = FindObjectOfType<MapFolderInspector>();
			}
			if (mapFolderInspector != null && mapFolderInspector.map != null)
			{
				Collider2D col = mapFolderInspector.map.GetComponent<Collider2D>();
				if (col != null)
				{
					DestroyImmediate(col);
				}
			}
			Tools.lockedLayers &= ~(1 << LayerMask.NameToLayer(tilesLayer));
			break;
		default:
			if (mapFolderInspector == null)
			{
				mapFolderInspector = FindObjectOfType<MapFolderInspector>();
			}
			if (mapFolderInspector != null && mapFolderInspector.map != null)
			{
				Collider2D col = mapFolderInspector.map.GetComponent<Collider2D>();
				if (col == null)
				{
					mapFolderInspector.map.gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
				}
			}
			Tools.lockedLayers |= 1 << LayerMask.NameToLayer(tilesLayer);
			break;
		}
	}

	private void UpdateView(SceneView sceneView)
	{
		switch (myState)
		{
		case MyStates.TilePlacement:
		case MyStates.EnvironmentPlacement:
			if (SceneView.currentDrawingSceneView != null)
			{
				if (Event.current != null)
				{
					Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
					RaycastHit2D hitInfo = Physics2D.Raycast(ray.origin, ray.direction, float.MaxValue, 1 << LayerMask.NameToLayer(mapLayer));
					if (hitInfo.collider != null)
					{
						if (myState == MyStates.TilePlacement && tile != null)
						{
							Vector3 pos = hitInfo.collider.transform.position;
							float deltaX = pos.x - hitInfo.point.x;
							float snapScale = snapStep / 100f;
							deltaX = Mathf.Ceil(deltaX / snapScale) * snapScale;
							float deltaY = pos.y - hitInfo.point.y;
							deltaY = Mathf.Ceil(deltaY / snapScale) * snapScale;

							SpriteRenderer spriteRenderer = tile.GetComponent<SpriteRenderer>();
							if (spriteRenderer != null)
							{
								deltaX -= spriteRenderer.bounds.extents.x;
								deltaY -= spriteRenderer.bounds.extents.y;
							}

							tile.transform.position = pos - new Vector3(deltaX, deltaY, 0f);

							if(Event.current.type == EventType.MouseDown && Event.current.button == 0)
							{
								Instantiate(tile, hitInfo.collider.gameObject.transform).name = tile.name;
							}
						}
						else if (myState == MyStates.EnvironmentPlacement && environment != null)
						{
							Vector3 pos = hitInfo.point;
							pos.z = hitInfo.collider.transform.position.z;
							environment.transform.position = pos;

							if(Event.current.type == EventType.MouseDown && Event.current.button == 0)
							{
								GameObject newEnvironment = Instantiate(environment, hitInfo.collider.gameObject.transform);
								newEnvironment.name = environment.name;
								if (inGameSorting == true)
								{
									newEnvironment.AddComponent<SpriteSorting>().isStatic = true;
								}
							}
						}
					}
				}
			}
			break;
		}
	}

	/// <summary>
	/// Raises the GU event.
	/// </summary>
	void OnGUI()
	{
		GUI.skin = null;

		if (EditorApplication.isPlaying == false)
		{
			GUILayout.Space(5);

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUI.skin = myState == MyStates.Pointer ? null : tilesGuiSkin;
			if (GUILayout.Button(contents.pointer) == true)
			{
				myState = MyStates.Pointer;
				UpdateTemp();
				tileIdx = -1;
				environmentIdx = -1;
			}
			GUI.skin = null;
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			GUILayout.Space(20);

			if (GUILayout.Button("Choose background") == true)
			{
				ShowPicker<Sprite>(PickerState.Map, mapSpriteLabel);
			}

			GUI.skin = tilesGuiSkin;

			GUILayout.Space(5);
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("------------Environment------------");
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("In game sorting");
			inGameSorting = EditorGUILayout.Toggle(inGameSorting);
			EditorGUILayout.EndHorizontal();

			environmentScrollPos = EditorGUILayout.BeginScrollView(environmentScrollPos, GUILayout.MaxHeight(120));
			int environmentNewIdx = GUILayout.SelectionGrid(environmentIdx, environmentTextures, 4);
			if (environmentNewIdx != environmentIdx)
			{
				environmentIdx = environmentNewIdx;
				if (environmentIdx >= 0)
				{
					myState = MyStates.EnvironmentPlacement;
					UpdateTemp();
					tileIdx = -1;
					SetEnvironment(environmentPrefabs[environmentIdx]);
				}
			}
			EditorGUILayout.EndScrollView();

			GUILayout.Space(5);
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("---------------Tiles---------------");
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Snap step");
			snapStep = EditorGUILayout.FloatField(snapStep);
			EditorGUILayout.EndHorizontal();

			tilesScrollPos = EditorGUILayout.BeginScrollView(tilesScrollPos, GUILayout.MaxHeight(140));
			int tileNewIdx = GUILayout.SelectionGrid(tileIdx, tileTextures, 4);
			if (tileNewIdx != tileIdx)
			{
				tileIdx = tileNewIdx;
				if (tileIdx >= 0)
				{
					myState = MyStates.TilePlacement;
					UpdateTemp();
					environmentIdx = -1;
					SetTile(tilePrefabs[tileIdx]);
				}
			}
			EditorGUILayout.EndScrollView();

			GUI.skin = null;

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			if (GUILayout.Button(contents.rotateLeft) == true)
			{
				RotateTile(90f);
			}
			GUILayout.FlexibleSpace();
			if (GUILayout.Button(contents.rotateRight) == true)
			{
				RotateTile(-90f);
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			if (myState == MyStates.Pointer)
			{
				GUILayout.Space(20);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Map name");
				mapName = EditorGUILayout.TextField(mapName);
				EditorGUILayout.EndHorizontal();

				if (GUILayout.Button("Save map") == true)
				{
					if (mapFolderInspector != null)
					{
						if (mapFolderInspector.map != null)
						{
							mapFolderInspector.map.gameObject.name = mapName;
							GameObject newMapPrefab = PrefabUtility.CreatePrefab("Assets/TD2D/Prefabs/Map/LevelMaps/" + mapName + ".prefab", mapFolderInspector.map.gameObject, ReplacePrefabOptions.ConnectToPrefab);
							AssetDatabase.Refresh();
							Selection.activeObject = newMapPrefab;
							EditorUtility.FocusProjectWindow();
						}
					}
				}
			}

			// New object selected in picker window
			if (Event.current.commandName == "ObjectSelectorUpdated" && EditorGUIUtility.GetObjectPickerControlID() == currentPickerWindow)
			{
				pickedObject = EditorGUIUtility.GetObjectPickerObject();
			}
			// Picker window closed
			if (Event.current.commandName == "ObjectSelectorClosed" && EditorGUIUtility.GetObjectPickerControlID() == currentPickerWindow)
			{
				switch (pickerState)
				{
				case PickerState.Map:
					if (pickedObject != null && pickedObject is Sprite && mapFolderInspector != null)
					{
						mapFolderInspector.ChangeMapSprite(pickedObject as Sprite);
						EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
					}
					break;
				}

				pickedObject = null;
				pickerState = PickerState.None;
			}
		}
		else
		{
			EditorGUILayout.HelpBox("Editor disabled in play mode", MessageType.Info);
		}
	}
}
