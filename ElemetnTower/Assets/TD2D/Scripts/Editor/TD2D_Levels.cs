using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.IO;

/// <summary>
/// Tower defense levels editor window.
/// </summary>
public class TD2D_Levels : EditorWindow
{
	// Main states
	private enum MyState
	{
		Disabled,
		LevelDescription,
		LevelMap
	}
	// Substates
	private enum MySubState
	{
		MapFolder,
		BuildingPlace,
		TowersFolder,
		SpawnPoint,
		Pathway,
		PathwaysFolder,
		EnemiesFolder,
		LevelChooserFolder,
		LevelDescription
	}
	// Interlayers between editor and gameplay scripts
	private class Inspectors
	{
		public MapFolderInspector mapFolder;
		public TowersFolderInspector towersFolder;
		public BuildingPlaceInspector buildingPlace;
		public PathwaysFolderInspector pathwaysFolder;
		public PathwayInspector pathway;
		public SpawnPointInspector spawnPoint;
		public LevelManagerInspector levelManager;
		public WavesInfoInspector wavesInfo;
		public LevelChooseInspector levelChooser;
		public LevelDescriptionInspector levelDescription;
		public DataManagerInspector dataManager;

		/// <summary>
		/// Clears the temporary inspectors.
		/// </summary>
		public void ClearTemporary()
		{
			buildingPlace = null;
			pathway = null;
			spawnPoint = null;
			levelDescription = null;
		}
	}
	// Visual content for editor parts
	private class Contents
	{
		public GUIContent levelsFolder;
		public GUIContent mapFolder;
		public GUIContent towersFolder;
		public GUIContent pathwaysFolder;
		public GUIContent conditionsFolder;
		public GUIContent newLevel;
		public GUIContent chooseMap;
		public GUIContent spawnIcon;
		public GUIContent captureIcon;
		public GUIContent addTower;
		public GUIContent buildingPlace;
		public GUIContent defendPoint;
		public GUIContent tower;
		public GUIContent addPathway;
		public GUIContent capturePoint;
		public GUIContent spawnPoint;
		public GUIContent waypoint;
		public GUIContent levelIcon;
		public GUIContent next;
		public GUIContent prev;
		public GUIContent add;
		public GUIContent remove;
		public GUIContent resetProgress;
		public GUIContent openAllLevels;
	}
	// Picker window states
	private enum PickerState
	{
		None,
		Map,
		SpawnIcon,
		CaptureIcon,
		Tower,
		Enemy,
		LevelIcon
	}
	// Label helps to organize prefabs searching
	private class Lables
	{
		public const string map = "l:Map";
		public const string spawnIcon = "l:SpawnIcon";
		public const string captureIcon = "l:CaptureIcon";
		public const string tower = "l:Tower";
		public const string levelIcon = "l:LevelIcon";
	}
	// Editor visual theme
	private GUISkin editorGuiSkin;
	// Main state
	private MyState myState = MyState.Disabled;
	// Substates list
	private List<MySubState> mySubState = new List<MySubState>();
	// Inspectors
	private Inspectors inspectors = new Inspectors();
	// Visual content
	private Contents contents = new Contents();
	// State for picker window
	private PickerState pickerState = PickerState.None;
	// ID for picker window
	private int currentPickerWindow;
	// Object is selected in picker window
	private GameObject pickedGameObject;
	// Name of active level
	private string currentLevelName;
	// Just some free space in editor window
	private float guiSpace = 15f;
	// Enemies allowed for this level
	private List<bool> enemiesList = new List<bool>();
	// Tower upgrades allowed for this level
	private List<bool> towersList = new List<bool>();
	// Spells allowed for this level
	private List<bool> spellsList = new List<bool>();
	// Scroll position for enemies list
	private Vector2 enemiesScrollPos;
	// Scroll position for tower upgrades list
	private Vector2 towersScrollPos;
	// Scroll position for spells list
	private Vector2 spellsScrollPos;
	// Scroll position for waves timeouts list
	private Vector2 wavesScrollPos;
	// Normal height of button
	private int buttonHeight = 30;
	// Normal width of button
	private int buttonWidth = 30;

	[MenuItem("Window/TD2D/Levels")]

	/// <summary>
	/// Shows the window.
	/// </summary>
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(TD2D_Levels));
	}

	/// <summary>
	/// Raises the enable event.
	/// </summary>
	void OnEnable()
	{
		EditorSceneManager.sceneOpened += OnSceneOpened;
		Selection.selectionChanged += OnSelectionChanged;
		EditorApplication.playModeStateChanged += OnPlaymodeStateChanged;
		UpdateStuff();
	}

	/// <summary>
	/// Raises the disable event.
	/// </summary>
	void OnDisable()
	{
		EditorSceneManager.sceneOpened -= OnSceneOpened;
		Selection.selectionChanged -= OnSelectionChanged;
		EditorApplication.playModeStateChanged -= OnPlaymodeStateChanged;
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
	private void OnPlaymodeStateChanged(UnityEditor.PlayModeStateChange playModeStateChange)
	{
		if (playModeStateChange == PlayModeStateChange.EnteredEditMode || playModeStateChange == PlayModeStateChange.ExitingPlayMode)
		{
			UpdateStuff();
		}
	}

	/// <summary>
	/// Raises the selection changed event.
	/// </summary>
	private void OnSelectionChanged()
	{
		inspectors.ClearTemporary();
		mySubState.Clear();
		// Check for selected gameobjects
		if (Selection.GetFiltered<GameObject>(SelectionMode.ExcludePrefab).Length == 1)
		{
			// Update inspectors list depending on selected gameobject
			switch (myState)
			{
			case MyState.LevelMap:
				if ((Selection.activeObject as GameObject).GetComponentInParent<MapFolderInspector>() != null)
				{
					mySubState.Add(MySubState.MapFolder);
				}
				if ((Selection.activeObject as GameObject).GetComponentInParent<BuildingPlaceInspector>() != null)
				{
					mySubState.Add(MySubState.BuildingPlace);
					inspectors.buildingPlace = (Selection.activeObject as GameObject).GetComponentInParent<BuildingPlaceInspector>();
				}
				if ((Selection.activeObject as GameObject).GetComponentInParent<TowersFolderInspector>() != null)
				{
					mySubState.Add(MySubState.TowersFolder);
				}
				if ((Selection.activeObject as GameObject).GetComponentInParent<SpawnPointInspector>() != null)
				{
					mySubState.Add(MySubState.SpawnPoint);
					inspectors.spawnPoint = (Selection.activeObject as GameObject).GetComponentInParent<SpawnPointInspector>();
				}
				if ((Selection.activeObject as GameObject).GetComponentInParent<PathwayInspector>() != null)
				{
					mySubState.Add(MySubState.Pathway);
					inspectors.pathway = (Selection.activeObject as GameObject).GetComponentInParent<PathwayInspector>();
				}
				if ((Selection.activeObject as GameObject).GetComponentInParent<PathwaysFolderInspector>() != null)
				{
					mySubState.Add(MySubState.PathwaysFolder);
				}
				if ((Selection.activeObject as GameObject).GetComponentInParent<LevelManagerInspector>() != null)
				{
					mySubState.Add(MySubState.EnemiesFolder);
				}
				break;
			case MyState.LevelDescription:
				if ((Selection.activeObject as GameObject).GetComponentInParent<LevelDescriptionInspector>() != null)
				{
					mySubState.Add(MySubState.LevelDescription);
					inspectors.levelDescription = (Selection.activeObject as GameObject).GetComponentInParent<LevelDescriptionInspector>();
					contents.levelIcon = new GUIContent(inspectors.levelDescription.icon.sprite.texture, "Choose icon for this level");
				}
				if ((Selection.activeObject as GameObject).GetComponentInParent<LevelChooseInspector>() != null)
				{
					mySubState.Add(MySubState.LevelChooserFolder);
				}
				break;
			}
		}
	}

	/// <summary>
	/// Updates the stuff data.
	/// </summary>
	private void UpdateStuff()
	{
		// Set editor visual theme
		editorGuiSkin = (GUISkin)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/EditorGuiSkin.guiskin", typeof(GUISkin));
		// Set labels for prebas. Label will use in picker window
		string[] prefabs;
		prefabs = AssetDatabase.FindAssets("", new string[] {"Assets/TD2D/Prefabs/Map/LevelMaps"});
		foreach (string str in prefabs)
		{
			Object obj = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(str));
			AssetDatabase.SetLabels(obj, new string[] {Lables.map});
		}
		prefabs = AssetDatabase.FindAssets("", new string[] {"Assets/TD2D/Prefabs/Map/SpawnIcons"});
		foreach (string str in prefabs)
		{
			Object obj = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(str));
			AssetDatabase.SetLabels(obj, new string[] {Lables.spawnIcon});
		}
		prefabs = AssetDatabase.FindAssets("", new string[] {"Assets/TD2D/Prefabs/Map/CaptureIcons"});
		foreach (string str in prefabs)
		{
			Object obj = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(str));
			AssetDatabase.SetLabels(obj, new string[] {Lables.captureIcon});
		}
		prefabs = AssetDatabase.FindAssets("", new string[] {"Assets/TD2D/Prefabs/Units/Towers/Towers"});
		foreach (string str in prefabs)
		{
			Object obj = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(str));
			AssetDatabase.SetLabels(obj, new string[] {Lables.tower});
		}
		prefabs = AssetDatabase.FindAssets("", new string[] {"Assets/TD2D/Prefabs/Levels/Icons"});
		foreach (string str in prefabs)
		{
			Object obj = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(str));
			AssetDatabase.SetLabels(obj, new string[] {Lables.levelIcon});
		}

		// Search for basic inspectors
		inspectors.mapFolder = GameObject.FindObjectOfType<MapFolderInspector>();
		inspectors.towersFolder = GameObject.FindObjectOfType<TowersFolderInspector>();
		inspectors.pathwaysFolder = GameObject.FindObjectOfType<PathwaysFolderInspector>();
		inspectors.wavesInfo = GameObject.FindObjectOfType<WavesInfoInspector>();
		inspectors.levelManager = GameObject.FindObjectOfType<LevelManagerInspector>();
		inspectors.levelChooser = GameObject.FindObjectOfType<LevelChooseInspector>();
		inspectors.dataManager = GameObject.FindObjectOfType<DataManagerInspector>();

		// Set visual content
		contents.levelsFolder = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/Levels.png", typeof(Texture)), "Create new levels");
		contents.mapFolder = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/Map.png", typeof(Texture)), "Game map visual settings");
		contents.towersFolder = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/BuildingPlace.png", typeof(Texture)), "Towers placement and settings");
		contents.pathwaysFolder = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/Pathway.png", typeof(Texture)), "Enemies pathways creating");
		contents.conditionsFolder = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/Conditions.png", typeof(Texture)), "Level conditions settings");
		contents.newLevel = new GUIContent("Create new level", (Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/CreateNewLevel.png", typeof(Texture)), "Create new scene with tower defense map and open it");
		contents.chooseMap = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/ChooseMap.png", typeof(Texture)), "Load game map");
		contents.spawnIcon = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/AddSpawnIcon.png", typeof(Texture)), "Add spawn place icon to level map");
		contents.captureIcon = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/AddCaptureIcon.png", typeof(Texture)), "Add capture place icon to level map");
		contents.addTower = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/AddTower.png", typeof(Texture)), "Add tower building place. The tower type may be choosen after that");
		contents.buildingPlace = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/BuildingPlace.png", typeof(Texture)), "Focus on building place of this tower");
		contents.defendPoint = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/DefendPoint.png", typeof(Texture)), "Focus on defend point of this tower");
		contents.tower = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/Tower.png", typeof(Texture)), "Choose tower type");
		contents.addPathway = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/AddPathway.png", typeof(Texture)), "Add pathway for enemy waves. It consist of spawn point and waypoints. Dublicate waypoints to create a path");
		contents.capturePoint = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/AddCapturePoint.png", typeof(Texture)), "Add capture point. The pathways must end inside capture point area");
		contents.spawnPoint = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/SpawnPoint.png", typeof(Texture)), "Focus on spawn point of this pathway");
		contents.waypoint = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/Waypoint.png", typeof(Texture)), "Add waypoint to this pathway");
		contents.next = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/Next.png", typeof(Texture)));
		contents.prev = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/Prev.png", typeof(Texture)));
		contents.add = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/Add.png", typeof(Texture)));
		contents.remove = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/Remove.png", typeof(Texture)));
		contents.resetProgress = new GUIContent("Reset game progress", "Delete saved file with \"in game\" completed levels data");
		contents.openAllLevels = new GUIContent("Permit all levels", "Make all game levels opened while in level chooser menu");

		// Check what kind of scene opened now
		if (inspectors.mapFolder != null && inspectors.towersFolder != null && inspectors.pathwaysFolder != null && inspectors.levelManager != null && inspectors.wavesInfo != null)
		{
			// This is level map scene

			myState = MyState.LevelMap;
			string sceneName = SceneManager.GetActiveScene().name;
			currentLevelName = sceneName;
			// Focus camera on level map
			GameObject levelMap = GameObject.Find("LevelMap");
			if (levelMap != null)
			{
				CameraFocus(levelMap);
			}
				
			// Update level manager lists from specified folders
			inspectors.levelManager.enemiesList.Clear();
			prefabs = AssetDatabase.FindAssets("t:prefab", new string[] {"Assets/TD2D/Prefabs/Units/Enemies/Units"});
			foreach (string prefab in prefabs)
			{
				inspectors.levelManager.enemiesList.Add((GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(prefab), typeof(GameObject)));
			}
			inspectors.levelManager.towersList.Clear();
			prefabs = AssetDatabase.FindAssets("t:prefab", new string[] {"Assets/TD2D/Prefabs/Units/Towers/Towers"});
			foreach (string prefab in prefabs)
			{
				inspectors.levelManager.towersList.Add((GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(prefab), typeof(GameObject)));
			}
			inspectors.levelManager.spellsList.Clear();
			prefabs = AssetDatabase.FindAssets("t:prefab", new string[] {"Assets/TD2D/Prefabs/Spells/Spells"});
			foreach (string prefab in prefabs)
			{
				inspectors.levelManager.spellsList.Add((GameObject)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(prefab), typeof(GameObject)));
			}

			// Get random enemies list
			enemiesList.Clear();
			for (int i = 0; i < inspectors.levelManager.enemiesList.Count; ++i)
			{
				if (inspectors.levelManager.enemies.Contains(inspectors.levelManager.enemiesList[i]) == true)
				{
					enemiesList.Add(true);
				}
				else
				{
					enemiesList.Add(false);
				}
			}
			// Get tower upgrades list
			towersList.Clear();
			for (int i = 0; i < inspectors.levelManager.towersList.Count; ++i)
			{
				if (inspectors.levelManager.towers.Contains(inspectors.levelManager.towersList[i]) == true)
				{
					towersList.Add(true);
				}
				else
				{
					towersList.Add(false);
				}
			}
			// Get spells list
			spellsList.Clear();
			for (int i = 0; i < inspectors.levelManager.spellsList.Count; ++i)
			{
				if (inspectors.levelManager.spells.Contains(inspectors.levelManager.spellsList[i]) == true)
				{
					spellsList.Add(true);
				}
				else
				{
					spellsList.Add(false);
				}
			}
		}
		else
		{
			if (inspectors.levelChooser != null)
			{
				// This is a level description scene

				myState = MyState.LevelDescription;
				// Display active level description
				GameObject levelDescriptionPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/TD2D/Prefabs/Levels/" + currentLevelName +".prefab");
				if (levelDescriptionPrefab != null)
				{
					inspectors.levelChooser.AddLevel(levelDescriptionPrefab);
					GameObject levelDescription = PrefabUtility.InstantiatePrefab(levelDescriptionPrefab) as GameObject;
					inspectors.levelChooser.SetActiveLevel(levelDescription);
				}
				LevelDescriptionInspector levelDescriptionInspector = GameObject.FindObjectOfType<LevelDescriptionInspector>();
				if (levelDescriptionInspector != null)
				{
					// Focus on level description
					Selection.activeObject = levelDescriptionInspector.gameObject;
					CameraFocus(levelDescriptionInspector.gameObject);
				}
				EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			}
			else
			{
				// This is not a Tower Defense level scene

				myState = MyState.Disabled;
				currentLevelName = "";
			}
		}

		OnSelectionChanged();
	}

	/// <summary>
	/// Shows the picker window.
	/// </summary>
	/// <param name="state">State.</param>
	/// <param name="filter">Filter.</param>
	private void ShowPicker(PickerState state, string filter)
	{
		pickerState = state;
		// Create a window picker control ID
		currentPickerWindow = EditorGUIUtility.GetControlID(FocusType.Passive);
		// Use the ID you just created
		EditorGUIUtility.ShowObjectPicker<GameObject>(null, false, filter, currentPickerWindow);
	}

	/// <summary>
	/// Focus camera on gameobject.
	/// </summary>
	/// <param name="focus">Focus.</param>
	private void CameraFocus(GameObject focus)
	{
		SceneView sceneView = SceneView.lastActiveSceneView;
		if (sceneView != null)
		{
			Object selected = Selection.activeObject;
			Selection.activeObject = focus;
			sceneView.FrameSelected();
			Selection.activeObject = selected;
		}
	}

	/// <summary>
	/// Raises the inspector update event.
	/// </summary>
	void Update()
	{
		Repaint();
	}

	/// <summary>
	/// Raises the GU event.
	/// </summary>
	void OnGUI()
	{
		// Set visual theme
		GUI.skin = editorGuiSkin;

		if (EditorApplication.isPlaying == false)
		{
			// Display helpbox if choosen level template scene
			if (myState == MyState.LevelMap)
			{
				if (currentLevelName == "Level0")
				{
					EditorGUILayout.HelpBox("Active scene is a level map template", MessageType.Info);
				}
			}

			// Display GUI depending on active object
			EditorGUI();
			LevelsGUI();
			MapGUI();
			TowersFolderGUI();
			BuildingPlaceGUI();
			PathwaysFolderGUI();
			PathwayGUI();
			SpawnPointGUI();
			LevelFolderGUI();
			LevelDescriptionGUI();

			// Display helpbox if scene unknown
			if (myState == MyState.Disabled)
			{
				EditorGUILayout.HelpBox("Active scene is not a Tower Defense 2D scene", MessageType.Info);
			}
		}
		else
		{
			EditorGUILayout.HelpBox("Editor disabled in play mode", MessageType.Info);
		}
	}

	/// <summary>
	/// Display editor's main tabs.
	/// </summary>
	private void EditorGUI()
	{
		EditorGUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		// Levels tab
		if (GUILayout.Button(contents.levelsFolder, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
		{
			Selection.activeObject = null;
		}

		switch (myState)
		{
		case MyState.LevelMap:
			// Map tab
			if (GUILayout.Button(contents.mapFolder, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				UpdateStuff();
				Selection.activeObject = inspectors.mapFolder.gameObject;
			}
			// Towers tab
			if (GUILayout.Button(contents.towersFolder, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				UpdateStuff();
				Selection.activeObject = inspectors.towersFolder.gameObject;
			}
			// Pathways tab
			if (GUILayout.Button(contents.pathwaysFolder, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				UpdateStuff();
				Selection.activeObject = inspectors.pathwaysFolder.gameObject;
			}
			// Level conditions tab
			if (GUILayout.Button(contents.conditionsFolder, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				UpdateStuff();
				Selection.activeObject = inspectors.levelManager.gameObject;
			}
			break;
		}
		GUILayout.FlexibleSpace();
		EditorGUILayout.EndHorizontal();
	}

	/// <summary>
	/// Display levels GUI.
	/// </summary>
	private void LevelsGUI()
	{
		if (mySubState.Count == 0)
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Levels");
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			// Create new level button
			if (GUILayout.Button(contents.newLevel) == true)
			{
				string[] filePaths = Directory.GetFiles(Application.dataPath + "/TD2D/Scenes/Levels", "*.unity");
				int counter = 1;
				// Search for levels scenes
				for (;;)
				{
					string levelName = "Level" + counter.ToString();
					bool hitted = false;
					foreach (string file in filePaths)
					{
						if (Path.GetFileNameWithoutExtension(file) == levelName)
						{
							hitted = true;
						}
					}
					// No such level scene
					if (hitted == false)
					{
						// Create new level from template
						if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() == true)
						{
							// Scene
							if (AssetDatabase.ValidateMoveAsset("Assets/TD2D/Scenes/Levels/Template/Level0.unity", "Assets/TD2D/Scenes/Levels/" + levelName + ".unity") == "")
							{
								AssetDatabase.CopyAsset("Assets/TD2D/Scenes/Levels/Template/Level0.unity", "Assets/TD2D/Scenes/Levels/" + levelName + ".unity");
							}
							// Level description prefab
							if (AssetDatabase.ValidateMoveAsset("Assets/TD2D/Prefabs/Levels/Template/Level0.prefab", "Assets/TD2D/Prefabs/Levels/" + levelName + ".prefab") == "")
							{
								AssetDatabase.CopyAsset("Assets/TD2D/Prefabs/Levels/Template/Level0.prefab", "Assets/TD2D/Prefabs/Levels/" + levelName + ".prefab");
							}
							AssetDatabase.Refresh();
							// Open created scene
							EditorSceneManager.OpenScene("Assets/TD2D/Scenes/Levels/" + levelName +".unity");
						}
						break;
					}
					counter++;
				}
			}

			switch (myState)
			{
			case MyState.LevelMap:
				// Switch to level description button
				if (currentLevelName != "" && currentLevelName != "Level0")
				{
					if (GUILayout.Button("Edit description") == true)
					{
						if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() == true)
						{
							EditorSceneManager.OpenScene("Assets/TD2D/Scenes/LevelChoose.unity");
						}
					}
				}
				break;
			case MyState.LevelDescription:
				// Switch to map editing button
				if (currentLevelName != "")
				{
					if (GUILayout.Button("Edit map") == true)
					{
						if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() == true)
						{
							EditorSceneManager.OpenScene("Assets/TD2D/Scenes/Levels/" + currentLevelName +".unity");
						}
					}
				}
				break;
			}

			GUILayout.Space(guiSpace);

			// Reset game progress button
			if (inspectors.dataManager != null)
			{
				if (GUILayout.Button(contents.resetProgress) == true)
				{
					inspectors.dataManager.ResetGameProgress();
				}
			}

			GUILayout.Space(guiSpace);

			// Open all game levels button
			if (inspectors.dataManager != null && inspectors.levelChooser != null)
			{
				if (GUILayout.Button(contents.openAllLevels) == true)
				{
					foreach (GameObject level in inspectors.levelChooser.GetLevelPrefabs())
					{
						inspectors.dataManager.PermitLevel(level.name);
					}
					Debug.Log("All game levels allowed now");
				}
			}
		}
	}

	/// <summary>
	/// Display map GUI.
	/// </summary>
	private void MapGUI()
	{
		if (myState == MyState.LevelMap && mySubState.Contains(MySubState.MapFolder))
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Map");
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			// Choose map image button
			if (GUILayout.Button(contents.chooseMap, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				ShowPicker(PickerState.Map, Lables.map);
			}
			// Create spawn icon button
			if (GUILayout.Button(contents.spawnIcon, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				ShowPicker(PickerState.SpawnIcon, Lables.spawnIcon);
			}
			// Create capture icon button
			if (GUILayout.Button(contents.captureIcon, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				ShowPicker(PickerState.CaptureIcon, Lables.captureIcon);
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			// New object selected in picker window
			if (Event.current.commandName == "ObjectSelectorUpdated" && EditorGUIUtility.GetObjectPickerControlID() == currentPickerWindow)
			{
				pickedGameObject = EditorGUIUtility.GetObjectPickerObject() as GameObject;
			}
			// Picker window closed
			if (Event.current.commandName == "ObjectSelectorClosed" && EditorGUIUtility.GetObjectPickerControlID() == currentPickerWindow)
			{
				switch (pickerState)
				{
				case PickerState.Map:
					if (pickedGameObject != null)
					{
						// Load new map
						inspectors.mapFolder.LoadMap(pickedGameObject);
						if (inspectors.mapFolder.map != null)
						{
							Selection.activeGameObject = inspectors.mapFolder.map.gameObject;
							CameraControl cameraControl = FindObjectOfType<CameraControl>();
							if (cameraControl != null)
							{
								cameraControl.focusObjectRenderer = inspectors.mapFolder.map;
							}
						}
						EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
					}
					break;
				case PickerState.SpawnIcon:
					if (pickedGameObject != null)
					{
						// Add spawn icon to map
						Selection.activeObject = inspectors.mapFolder.AddSpawnIcon(pickedGameObject);
					}
					break;
				case PickerState.CaptureIcon:
					if (pickedGameObject != null)
					{
						// Add capture icon to map
						Selection.activeObject = inspectors.mapFolder.AddCaptureIcon(pickedGameObject);
					}
					break;
				}

				pickedGameObject = null;
				pickerState = PickerState.None;
			}
		}
	}

	/// <summary>
	/// Display towers GUI.
	/// </summary>
	private void TowersFolderGUI()
	{
		if (myState == MyState.LevelMap && mySubState.Contains(MySubState.TowersFolder))
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Towers");
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			// Choose previous tower button
			if (GUILayout.Button(contents.prev, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				Selection.activeObject = inspectors.towersFolder.GetPrevioustBuildingPlace(Selection.activeObject as GameObject);
			}
			// Add tower button
			if (GUILayout.Button(contents.addTower, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				GameObject tower = inspectors.towersFolder.AddTower();
				if (tower != null)
				{
					Selection.activeObject = tower;
				}
			}
			// Choose next tower button
			if (GUILayout.Button(contents.next, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				Selection.activeObject = inspectors.towersFolder.GetNextBuildingPlace(Selection.activeObject as GameObject);
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}
	}

	/// <summary>
	/// Display building place GUI.
	/// </summary>
	private void BuildingPlaceGUI()
	{
		if (myState == MyState.LevelMap && mySubState.Contains(MySubState.BuildingPlace))
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Tower");
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			// Focus on building place button
			if (GUILayout.Button(contents.buildingPlace, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				Selection.activeObject = inspectors.buildingPlace.gameObject;
			}
			// Focus on defend point button
			if (GUILayout.Button(contents.defendPoint, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				Selection.activeObject = inspectors.buildingPlace.GetDefendPoint();
			}
			// Change tower button
			if (GUILayout.Button(contents.tower, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				ShowPicker(PickerState.Tower, Lables.tower);
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			// New object selected in picker window
			if (Event.current.commandName == "ObjectSelectorUpdated" && EditorGUIUtility.GetObjectPickerControlID() == currentPickerWindow)
			{
				pickedGameObject = EditorGUIUtility.GetObjectPickerObject() as GameObject;
				if (pickedGameObject != null)
				{
					// Set new tower for this building place
					Selection.activeObject = inspectors.buildingPlace.ChooseTower(pickedGameObject);
				}
			}
			// Picker window closed
			if (Event.current.commandName == "ObjectSelectorClosed" && EditorGUIUtility.GetObjectPickerControlID() == currentPickerWindow)
			{
				pickedGameObject = null;
				pickerState = PickerState.None;
			}
		}
	}

	/// <summary>
	/// Display pathways GUI.
	/// </summary>
	private void PathwaysFolderGUI()
	{
		if (myState == MyState.LevelMap && mySubState.Contains(MySubState.PathwaysFolder))
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Pathways");
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			// Choose previous capture point button
			if (GUILayout.Button(contents.prev, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				Selection.activeObject = inspectors.pathwaysFolder.GetPrevioustCapturePoint(Selection.activeObject as GameObject);
			}
			// Add capture point button
			if (GUILayout.Button(contents.capturePoint, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				Selection.activeObject = inspectors.pathwaysFolder.AddCapturePoint();
			}
			// Choose next capture point button
			if (GUILayout.Button(contents.next, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				Selection.activeObject = inspectors.pathwaysFolder.GetNextCapturePoint(Selection.activeObject as GameObject);
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			// Choose previous pathway button
			if (GUILayout.Button(contents.prev, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				Selection.activeObject = inspectors.pathwaysFolder.GetPrevioustPathway(Selection.activeObject as GameObject);
			}
			// Add pathway button
			if (GUILayout.Button(contents.addPathway, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				Selection.activeObject = inspectors.pathwaysFolder.AddPathway();
			}
			// Choose next pathway button
			if (GUILayout.Button(contents.next, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				Selection.activeObject = inspectors.pathwaysFolder.GetNextPathway(Selection.activeObject as GameObject);
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}
	}

	/// <summary>
	/// Display pathway GUI.
	/// </summary>
	private void PathwayGUI()
	{
		if (myState == MyState.LevelMap && mySubState.Contains(MySubState.Pathway))
		{
			GUILayout.Space(guiSpace);

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Pathway");
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			// Focus on spawn point button
			if (GUILayout.Button(contents.spawnPoint, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				Selection.activeObject = inspectors.pathway.GetSpawnPoint();
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();


			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			// Choose previous waypoint button
			if (GUILayout.Button(contents.prev, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				Selection.activeObject = inspectors.pathway.GetPrevioustWaypoint(Selection.activeObject as GameObject);
			}
			// Add waypoint button
			if (GUILayout.Button(contents.waypoint, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				Selection.activeObject = inspectors.pathway.AddWaypoint();
			}
			// Choose next waypoint button
			if (GUILayout.Button(contents.next, GUILayout.Width(buttonWidth), GUILayout.Height(buttonHeight)) == true)
			{
				Selection.activeObject = inspectors.pathway.GetNextWaypoint(Selection.activeObject as GameObject);
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		}
	}

	/// <summary>
	/// Display spawn point GUI.
	/// </summary>
	private void SpawnPointGUI()
	{
		if (myState == MyState.LevelMap && mySubState.Contains(MySubState.SpawnPoint))
		{
			GUILayout.Space(guiSpace);

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Enemies");
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			// Display enemies count for each wave in this spawn point
			for (int i = 0; i < inspectors.spawnPoint.enemies.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				GUILayout.Label("Wave " + (i + 1));
				GUILayout.FlexibleSpace();
				inspectors.spawnPoint.enemies[i] = EditorGUILayout.IntField(inspectors.spawnPoint.enemies[i]);
				EditorGUILayout.EndHorizontal();
			}

			EditorGUILayout.BeginHorizontal();
			// Remove wave button
			if (GUILayout.Button(contents.remove, GUILayout.Width(20), GUILayout.Height(20)) == true)
			{
				inspectors.spawnPoint.RemoveWave();
			}
			GUILayout.FlexibleSpace();
			// Add wave button
			if (GUILayout.Button(contents.add, GUILayout.Width(20), GUILayout.Height(20)) == true)
			{
				inspectors.spawnPoint.AddWave();
			}
			EditorGUILayout.EndHorizontal();

			if (GUI.changed == true)
			{
				inspectors.spawnPoint.UpdateWaveList();
				inspectors.wavesInfo.Update();
			}
		}
	}

	/// <summary>
	/// Level config GUI.
	/// </summary>
	private void LevelFolderGUI()
	{
		if (myState == MyState.LevelMap && mySubState.Contains(MySubState.EnemiesFolder))
		{
			GUILayout.Space(guiSpace);

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Gold amount");
			// Gold amount for this level
			inspectors.levelManager.goldAmount = EditorGUILayout.IntField(inspectors.levelManager.goldAmount);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Defeat attempts");
			// Defeat attempts before loose for this level
			inspectors.levelManager.defeatAttempts = EditorGUILayout.IntField(inspectors.levelManager.defeatAttempts);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Random enemies");
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			// Update random enemies list
			while (enemiesList.Count < inspectors.levelManager.enemiesList.Count)
			{
				enemiesList.Add(true);
			}
			while (enemiesList.Count > inspectors.levelManager.enemiesList.Count)
			{
				enemiesList.RemoveAt(enemiesList.Count - 1);
			}
			// Display random enemies list
			enemiesScrollPos = EditorGUILayout.BeginScrollView(enemiesScrollPos, GUILayout.MaxHeight(55));
			for (int i = 0; i < enemiesList.Count; ++i)
			{
				enemiesList[i] = EditorGUILayout.Toggle(inspectors.levelManager.enemiesList[i].name, enemiesList[i]);
			}
			EditorGUILayout.EndScrollView();

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Tower upgrades");
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			// Update tower upgrades list
			while (towersList.Count < inspectors.levelManager.towersList.Count)
			{
				towersList.Add(true);
			}
			while (towersList.Count > inspectors.levelManager.towersList.Count)
			{
				towersList.RemoveAt(towersList.Count - 1);
			}
			// Display tower upgrades list
			towersScrollPos = EditorGUILayout.BeginScrollView(towersScrollPos, GUILayout.MaxHeight(55));
			for (int i = 0; i < towersList.Count; ++i)
			{
				towersList[i] = EditorGUILayout.Toggle(inspectors.levelManager.towersList[i].name, towersList[i]);
			}
			EditorGUILayout.EndScrollView();

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Spells");
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			// Update spells list
			while (spellsList.Count < inspectors.levelManager.spellsList.Count)
			{
				spellsList.Add(true);
			}
			while (spellsList.Count > inspectors.levelManager.spellsList.Count)
			{
				spellsList.RemoveAt(towersList.Count - 1);
			}
			// Display spells list
			spellsScrollPos = EditorGUILayout.BeginScrollView(spellsScrollPos, GUILayout.MaxHeight(55));
			for (int i = 0; i < spellsList.Count; ++i)
			{
				spellsList[i] = EditorGUILayout.Toggle(inspectors.levelManager.spellsList[i].name, spellsList[i]);
			}
			EditorGUILayout.EndScrollView();

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Waves timeouts");
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.HelpBox("Waves number calculated from spawnpoints", MessageType.Info);
			// Display timeouts between waves
			wavesScrollPos = EditorGUILayout.BeginScrollView(wavesScrollPos, GUILayout.MaxHeight(55));
			for (int i = 0; i < inspectors.wavesInfo.timeouts.Count; i++)
			{
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.PrefixLabel("Wave " + (i + 1));
				inspectors.wavesInfo.timeouts[i] = EditorGUILayout.FloatField(inspectors.wavesInfo.timeouts[i]);
				EditorGUILayout.EndHorizontal();
			}
			EditorGUILayout.EndScrollView();

			// Apply changes
			if (GUI.changed == true)
			{
				inspectors.levelManager.enemies.Clear();
				for (int i = 0; i < enemiesList.Count; ++i)
				{
					if (enemiesList[i] == true)
					{
						inspectors.levelManager.enemies.Add(inspectors.levelManager.enemiesList[i]);
					}
				}
				inspectors.levelManager.towers.Clear();
				for (int i = 0; i < towersList.Count; ++i)
				{
					if (towersList[i] == true)
					{
						inspectors.levelManager.towers.Add(inspectors.levelManager.towersList[i]);
					}
				}
				inspectors.levelManager.spells.Clear();
				for (int i = 0; i < spellsList.Count; ++i)
				{
					if (spellsList[i] == true)
					{
						inspectors.levelManager.spells.Add(inspectors.levelManager.spellsList[i]);
					}
				}
				EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			}
		}
	}

	/// <summary>
	/// Display level description GUI.
	/// </summary>
	private void LevelDescriptionGUI()
	{
		if (myState == MyState.LevelDescription && mySubState.Contains(MySubState.LevelDescription))
		{
			GUILayout.Space(guiSpace);

			EditorGUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			// Set level icon button
			if (GUILayout.Button(contents.levelIcon, GUILayout.MaxWidth(100f), GUILayout.MaxHeight(100f)) == true)
			{
				ShowPicker(PickerState.LevelIcon, Lables.levelIcon);
			}
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
			// Header
			GUILayout.Label("Header");
			inspectors.levelDescription.header.text = EditorGUILayout.TextField(inspectors.levelDescription.header.text);
			// Description
			GUILayout.Label("Description");
			inspectors.levelDescription.description.text = EditorGUILayout.TextArea(inspectors.levelDescription.description.text, GUILayout.MaxHeight(80f));
			// Attention
			GUILayout.Label("Attention");
			inspectors.levelDescription.attention.text = EditorGUILayout.TextArea(inspectors.levelDescription.attention.text, GUILayout.MaxHeight(40f));

			GUILayout.Space(guiSpace);
			// Apply changes button
			if (GUILayout.Button("Apply changes") == true)
			{
				PrefabUtility.ReplacePrefab(inspectors.levelDescription.gameObject, PrefabUtility.GetCorrespondingObjectFromSource(inspectors.levelDescription.gameObject), ReplacePrefabOptions.ConnectToPrefab);
			}

			// New object selected in picker window
			if (Event.current.commandName == "ObjectSelectorUpdated" && EditorGUIUtility.GetObjectPickerControlID() == currentPickerWindow)
			{
				pickedGameObject = EditorGUIUtility.GetObjectPickerObject() as GameObject;
				if (pickedGameObject != null)
				{
					SpriteRenderer spriteRenderer = pickedGameObject.GetComponent<SpriteRenderer>();
					if (spriteRenderer != null)
					{
						// Set icon in level description
						inspectors.levelDescription.icon.sprite = spriteRenderer.sprite;
						EditorUtility.SetDirty(inspectors.levelDescription.gameObject);
						contents.levelIcon = new GUIContent(inspectors.levelDescription.icon.sprite.texture, "Choose icon for this level");
					}
				}
			}
			// Picker window closed
			if (Event.current.commandName == "ObjectSelectorClosed" && EditorGUIUtility.GetObjectPickerControlID() == currentPickerWindow)
			{
				pickedGameObject = null;
				pickerState = PickerState.None;
			}

			if (GUI.changed == true)
			{
				EditorUtility.SetDirty(inspectors.levelDescription.gameObject);
			}
		}
	}
}

