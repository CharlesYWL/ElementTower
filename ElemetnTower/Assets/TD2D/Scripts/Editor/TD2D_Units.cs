using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

/// <summary>
/// Tower defense units editor window
/// </summary>
public class TD2D_Units : EditorWindow
{
	// Possible states
	private enum MyState
	{
		Disabled,
		Enemy,
		Defender,
		Tower,
		Barracks
	}
	// Selected unit descriptor
	private class UnitData
	{
		public GameObject gameObject;
		public Price price;
		public Tower tower;
		public DefendersSpawner spawner;
		public TowerActionsInspector towerActions;
		public NavAgent navAgent;
		public DamageTaker damageTaker;
		public FeaturesInspector aiFeature;
		public Attack attack;
		public GameObject range;
		public bool flying;
		public int attackType;
		public bool targetCommon;
		public bool targetFlying;

		public void Clear()
		{
			gameObject = null;
			price = null;
			tower = null;
			spawner = null;
			towerActions = null;
			navAgent = null;
			damageTaker = null;
			aiFeature = null;
			attack = null;
			range = null;
			flying = false;
			attackType = 0;
			targetCommon = false;
			targetFlying = false;
		}
	}
	// Label helps to organize prefabs searching
	private class Lables
	{
		public const string bulletAlly = "l:BulletAlly";
		public const string bulletEnemy = "l:BulletEnemy";
		public const string towerAction = "l:TAction";
		public const string defender = "l:Defender";
		public const string tower = "l:Tower";
		public const string icon = "";
		public const string feature = "l:Feature";
	}
	// Picker window states
	private enum PickerState
	{
		None,
		BulletAlly,
		BulletEnemy,
		TowerActions,
		Defenders,
		Towers,
		Icons,
		Features
	}
	// Visual content for editor parts
	private class Contents
	{
		public GUIContent newEnemyButton;
		public GUIContent newDefenderButton;
		public GUIContent newTowerButton;
		public GUIContent newBarracksButton;
		public GUIContent chooseBulletButton;
		public GUIContent focusOnFirePointButton;
		public GUIContent chooseDefenderButton;
		public GUIContent applyChangesButton;
		public GUIContent removeFromSceneButton;
		public GUIContent add;
		public GUIContent remove;
		public GUIContent next;
		public GUIContent prev;
	}

	// My state
	private MyState myState = MyState.Disabled;
	// Unit descriptor
	private UnitData unitData = new UnitData();
	// Just some free space in editor window
	private float guiSpace = 15f;
	// State for picker window
	private PickerState pickerState = PickerState.None;
	// ID for picker window
	private int currentPickerWindow;
	// Object is selected in picker window
	private Object pickedObject;
	// Editor visual theme
	private GUISkin editorGuiSkin;
	// Editor visual contents
	private Contents contents = new Contents();

	[MenuItem("Window/TD2D/Units")]

	/// <summary>
	/// Shows the window.
	/// </summary>
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(TD2D_Units));
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
		myState = MyState.Disabled;
		unitData.Clear();
		// Check for selected gameobject and fill unit descriptor
		if (Selection.GetFiltered<GameObject>(SelectionMode.ExcludePrefab).Length == 1)
		{
			unitData.tower = (Selection.activeObject as GameObject).GetComponentInParent<Tower>();
			if (unitData.tower != null) // Tower
			{
				unitData.spawner = (Selection.activeObject as GameObject).GetComponentInParent<DefendersSpawner>();
				if (unitData.spawner != null) // Barracks
				{
					myState = MyState.Barracks;
					unitData.gameObject = unitData.tower.gameObject;
					unitData.price = unitData.spawner.GetComponent<Price>();
					unitData.tower = unitData.spawner.GetComponent<Tower>();
					if (unitData.tower != null)
					{
						unitData.range = unitData.tower.range;
						if (unitData.tower.actions != null)
						{
							unitData.towerActions = unitData.tower.actions.GetComponent<TowerActionsInspector>();
						}
					}
					if (unitData.spawner.prefab != null)
					{
						contents.chooseDefenderButton.image = AssetPreview.GetAssetPreview(unitData.spawner.prefab);
					}
					else
					{
						contents.chooseDefenderButton.image = null;
					}
				}
				else // Ranged tower
				{
					myState = MyState.Tower;
					unitData.gameObject = unitData.tower.gameObject;
					unitData.price = unitData.gameObject.GetComponent<Price>();
					unitData.range = unitData.tower.range;
					if (unitData.tower.actions != null)
					{
						unitData.towerActions = unitData.tower.actions.GetComponent<TowerActionsInspector>();
					}
					unitData.attack = unitData.gameObject.GetComponentInChildren<Attack>();
					if (unitData.attack != null)
					{
						AiTriggerCollider trigger = unitData.attack.GetComponent<AiTriggerCollider>();
						if (trigger != null)
						{
							unitData.targetCommon = trigger.tags.Contains("Enemy") ? true : false;
							unitData.targetFlying = trigger.tags.Contains("FlyingEnemy") ? true : false;
						}
						if ((unitData.attack is AttackRanged) && (unitData.attack as AttackRanged).arrowPrefab != null)
						{
							contents.chooseBulletButton.image = AssetPreview.GetAssetPreview((unitData.attack as AttackRanged).arrowPrefab);
						}
						else
						{
							contents.chooseBulletButton.image = null;
						}
					}
				}
			}
			else // Unit
			{
				AiBehavior aiBehavior = (Selection.activeObject as GameObject).GetComponentInParent<AiBehavior>();
				if (aiBehavior != null)
				{
					unitData.gameObject = aiBehavior.gameObject;
					switch (unitData.gameObject.tag)
					{
					case "Enemy": goto case "FlyingEnemy";
					case "FlyingEnemy":
						unitData.price = unitData.gameObject.GetComponent<Price>();
						unitData.flying = unitData.gameObject.CompareTag("FlyingEnemy") ? true : false;
						unitData.aiFeature = unitData.gameObject.GetComponentInChildren<FeaturesInspector>();
						goto case "Defender";
					case "Defender":
						myState = unitData.gameObject.CompareTag("Defender") ? MyState.Defender : MyState.Enemy;
						unitData.navAgent = unitData.gameObject.GetComponent<NavAgent>();
						unitData.damageTaker = unitData.gameObject.GetComponent<DamageTaker>();
						unitData.attackType = 0;
						unitData.attack = unitData.gameObject.GetComponentInChildren<Attack>();
						if (unitData.attack != null && (unitData.attack is AttackMelee))
						{
							unitData.attackType = 1;
							AiTriggerCollider trigger = unitData.attack.GetComponent<AiTriggerCollider>();
							if (trigger != null)
							{
								unitData.targetCommon = trigger.tags.Contains("Enemy") ? true : false;
								unitData.targetFlying = trigger.tags.Contains("FlyingEnemy") ? true : false;
							}
						}
						else if (unitData.attack != null && (unitData.attack is AttackRanged))
						{
							unitData.attackType = 2;
							AiTriggerCollider trigger = unitData.attack.GetComponent<AiTriggerCollider>();
							if (trigger != null)
							{
								unitData.targetCommon = trigger.tags.Contains("Enemy") ? true : false;
								unitData.targetFlying = trigger.tags.Contains("FlyingEnemy") ? true : false;
							}
							if ((unitData.attack as AttackRanged).arrowPrefab != null)
							{
								contents.chooseBulletButton.image = AssetPreview.GetAssetPreview((unitData.attack as AttackRanged).arrowPrefab);
							}
							else
							{
								contents.chooseBulletButton.image = null;
							}
						}
						break;
					}
				}
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
		prefabs = AssetDatabase.FindAssets("", new string[] {"Assets/TD2D/Prefabs/Bullets/Ally"});
		foreach (string str in prefabs)
		{
			Object obj = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(str));
			AssetDatabase.SetLabels(obj, new string[] {Lables.bulletAlly});
		}
		prefabs = AssetDatabase.FindAssets("", new string[] {"Assets/TD2D/Prefabs/Bullets/Enemy"});
		foreach (string str in prefabs)
		{
			Object obj = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(str));
			AssetDatabase.SetLabels(obj, new string[] {Lables.bulletEnemy});
		}
		prefabs = AssetDatabase.FindAssets("", new string[] {"Assets/TD2D/Prefabs/Units/Towers/Actions/Actions"});
		foreach (string str in prefabs)
		{
			Object obj = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(str));
			AssetDatabase.SetLabels(obj, new string[] {Lables.towerAction});
		}
		prefabs = AssetDatabase.FindAssets("", new string[] {"Assets/TD2D/Prefabs/Units/Defenders"});
		foreach (string str in prefabs)
		{
			Object obj = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(str));
			AssetDatabase.SetLabels(obj, new string[] {Lables.defender});
		}
		prefabs = AssetDatabase.FindAssets("", new string[] {"Assets/TD2D/Prefabs/Units/Towers/Towers"});
		foreach (string str in prefabs)
		{
			Object obj = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(str));
			AssetDatabase.SetLabels(obj, new string[] {Lables.tower});
		}
		prefabs = AssetDatabase.FindAssets("", new string[] {"Assets/TD2D/Prefabs/Units/Enemies/AiFeatures"});
		foreach (string str in prefabs)
		{
			Object obj = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(str));
			AssetDatabase.SetLabels(obj, new string[] {Lables.feature});
		}

		// Set visual content
		contents.newEnemyButton = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/AddEnemy.png", typeof(Texture)), "Create new enemy");
		contents.newDefenderButton = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/AddDefender.png", typeof(Texture)), "Create new defender");
		contents.newTowerButton = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/AddRangeTower.png", typeof(Texture)), "Create new ranged tower");
		contents.newBarracksButton = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/AddBarracks.png", typeof(Texture)), "Create new barracks");
		contents.chooseBulletButton = new GUIContent((Texture)null, "Choose bulet prefab for ranged attack");
		contents.focusOnFirePointButton = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/FirePoint.png", typeof(Texture)), "Move firepoint");
		contents.chooseDefenderButton = new GUIContent((Texture)null, "Choose defender prefab for this barracks");
		contents.applyChangesButton = new GUIContent("Apply changes", "Apply changes and connect to prefab");
		contents.removeFromSceneButton = new GUIContent("Remove from scene", "Remove this unit from scene");
		contents.add = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/Add.png", typeof(Texture)));
		contents.remove = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/Remove.png", typeof(Texture)));
		contents.next = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/Next.png", typeof(Texture)));
		contents.prev = new GUIContent((Texture)AssetDatabase.LoadAssetAtPath("Assets/TD2D/Sprites/Editor/Prev.png", typeof(Texture)));

		OnSelectionChanged();
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

	/// <summary>
	/// Updates unit's layers and tags.
	/// </summary>
	private void UpdateLayersAndTags()
	{
		if (unitData.gameObject != null)
		{
			switch (unitData.gameObject.gameObject.tag)
			{
			case "Tower":
			case "Defender":
				if (unitData.attack != null)
				{
					unitData.attack.gameObject.layer = LayerMask.NameToLayer("AttackAlly");
					AiTriggerCollider trigger = unitData.attack.GetComponent<AiTriggerCollider>();
					if (trigger != null)
					{
						trigger.tags.Clear();
						trigger.tags.Add("Enemy");
						trigger.tags.Add("FlyingEnemy");
					}
				}
				if (unitData.attack != null)
				{
					unitData.attack.gameObject.layer = LayerMask.NameToLayer("AttackAlly");
					AiTriggerCollider trigger = unitData.attack.GetComponent<AiTriggerCollider>();
					if (trigger != null)
					{
						trigger.tags.Clear();
						trigger.tags.Add("Enemy");
						trigger.tags.Add("FlyingEnemy");
					}
				}
				break;
			case "Enemy":
			case "FlyingEnemy":
				if (unitData.attack != null)
				{
					unitData.attack.gameObject.layer = LayerMask.NameToLayer("AttackEnemy");
					AiTriggerCollider trigger = unitData.attack.GetComponent<AiTriggerCollider>();
					if (trigger != null)
					{
						trigger.tags.Clear();
						trigger.tags.Add("Defender");
					}
				}
				if (unitData.attack != null)
				{
					unitData.attack.gameObject.layer = LayerMask.NameToLayer("AttackEnemy");
					AiTriggerCollider trigger = unitData.attack.GetComponent<AiTriggerCollider>();
					if (trigger != null)
					{
						trigger.tags.Clear();
						trigger.tags.Add("Defender");
					}
				}
				break;
			}
		}
	}

	/// <summary>
	/// Creates new unit from prefab and place on scene.
	/// </summary>
	/// <param name="prefabPath">Prefab path.</param>
	/// <param name="folderPath">Folder path.</param>
	private void CreateNewUnit(string prefabPath, string folderPath)
	{
		string[] templates;
		templates = AssetDatabase.FindAssets("", new string[] {prefabPath});
		if (templates.Length > 0)
		{
			string newUnitFolder = "";
			for (short i = 1; i < short.MaxValue; ++i)
			{
				if (AssetDatabase.ValidateMoveAsset(prefabPath, folderPath + i) == "")
				{
					newUnitFolder = folderPath + i;
					AssetDatabase.CopyAsset(prefabPath, newUnitFolder);
					break;
				}
			}
			AssetDatabase.Refresh();
			if (newUnitFolder != "")
			{
				string[] prefabs;
				prefabs = AssetDatabase.FindAssets("t:prefab", new string[] {newUnitFolder});
				if (prefabs.Length > 0)
				{
					Object unitPrefab = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(prefabs[0]));
					Selection.activeObject = unitPrefab;
					EditorUtility.FocusProjectWindow();
					GameObject newUnit = PrefabUtility.InstantiatePrefab(unitPrefab) as GameObject;
					Selection.activeObject = newUnit;
					newUnit.transform.SetAsLastSibling();
				}
			}
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
			switch (myState)
			{
			case MyState.Disabled:
				
				EditorGUILayout.HelpBox("Create your own units and towers", MessageType.Info);

				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				// Add new enemy button
				if (GUILayout.Button(contents.newEnemyButton, GUILayout.MaxWidth(40f), GUILayout.MaxHeight(40f)) == true)
				{
					CreateNewUnit("Assets/TD2D/Prefabs/Stuff/Templates/Enemy", "Assets/TD2D/Prefabs/Units/Enemies/Units/Enemy");
				}
				// Add new defender button
				if (GUILayout.Button(contents.newDefenderButton, GUILayout.MaxWidth(40f), GUILayout.MaxHeight(40f)) == true)
				{
					CreateNewUnit("Assets/TD2D/Prefabs/Stuff/Templates/Defender", "Assets/TD2D/Prefabs/Units/Defenders/Defender");
				}
				// Add new ranged tower button
				if (GUILayout.Button(contents.newTowerButton, GUILayout.MaxWidth(40f), GUILayout.MaxHeight(40f)) == true)
				{
					CreateNewUnit("Assets/TD2D/Prefabs/Stuff/Templates/Tower", "Assets/TD2D/Prefabs/Units/Towers/Towers/MyTowers/Tower");
				}
				// Add new barracks button
				if (GUILayout.Button(contents.newBarracksButton, GUILayout.MaxWidth(40f), GUILayout.MaxHeight(40f)) == true)
				{
					CreateNewUnit("Assets/TD2D/Prefabs/Stuff/Templates/Barracks", "Assets/TD2D/Prefabs/Units/Towers/Towers/MyTowers/Barracks");
				}
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				break;

			case MyState.Enemy:
			case MyState.Defender:
			case MyState.Tower:
			case MyState.Barracks:

				GUILayout.Space(guiSpace);

				EditorGUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.Label(unitData.gameObject.name);
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();

				if (myState == MyState.Enemy || myState == MyState.Tower || myState == MyState.Barracks)
				{
					// Price
					if (unitData.price != null)
					{
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PrefixLabel("Price");
						unitData.price.price = EditorGUILayout.IntField(unitData.price.price);
						EditorGUILayout.EndHorizontal();

						GUILayout.Space(guiSpace);
					}
				}

				if (myState == MyState.Enemy || myState == MyState.Defender)
				{
					// Speed
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Speed");
					unitData.navAgent.speed = EditorGUILayout.FloatField(unitData.navAgent.speed);
					EditorGUILayout.EndHorizontal();

					// Hitpoints
					EditorGUILayout.BeginHorizontal();
					EditorGUILayout.PrefixLabel("Hitpoints");
					unitData.damageTaker.hitpoints = EditorGUILayout.IntField(unitData.damageTaker.hitpoints);
					EditorGUILayout.EndHorizontal();
				}

				if (myState == MyState.Enemy)
				{
					// Flying flag
					bool flyingEnemy = EditorGUILayout.Toggle("Flying", unitData.flying);
					if (flyingEnemy != unitData.flying && unitData.gameObject != null)
					{
						unitData.gameObject.tag = flyingEnemy == true ? "FlyingEnemy" : "Enemy";
						unitData.flying = flyingEnemy;
					}
				}

				if (myState == MyState.Enemy || myState == MyState.Defender || myState == MyState.Tower)
				{
					GUILayout.Space(guiSpace);

					EditorGUILayout.BeginHorizontal();
					GUILayout.FlexibleSpace();
					GUILayout.Label("Attack");
					GUILayout.FlexibleSpace();
					EditorGUILayout.EndHorizontal();
				}

				if (myState == MyState.Enemy || myState == MyState.Defender)
				{
					// Attack type
					string[] attackTypes = new string[] {"None", "Melee", "Ranged"};
					int attackType = GUILayout.SelectionGrid(unitData.attackType, attackTypes, 1, EditorStyles.radioButton);
					if (attackType != unitData.attackType)
					{
						if (unitData.attack != null)
						{
							DestroyImmediate(unitData.attack.gameObject);
							unitData.attack = null;
						}
						switch (attackType)
						{
						case 1:
							Object meleeAttackPrefab = AssetDatabase.LoadAssetAtPath<Object>("Assets/TD2D/Prefabs/Stuff/AttackTypes/MeleeAttack.prefab");
							if (meleeAttackPrefab != null)
							{
								GameObject meleeAttack = Instantiate(meleeAttackPrefab, unitData.gameObject.transform) as GameObject;
								meleeAttack.name = meleeAttackPrefab.name;
								unitData.attack = meleeAttack.GetComponent<Attack>();
							}
							break;
						case 2:
							Object rangedAttackPrefab = AssetDatabase.LoadAssetAtPath<Object>("Assets/TD2D/Prefabs/Stuff/AttackTypes/RangedAttack.prefab");
							if (rangedAttackPrefab != null)
							{
								GameObject rangedAttack = Instantiate(rangedAttackPrefab, unitData.gameObject.transform) as GameObject;
								rangedAttack.name = rangedAttackPrefab.name;
								unitData.attack = rangedAttack.GetComponent<Attack>();
							}
							break;
						}
						unitData.attackType = attackType;
						Selection.activeObject = unitData.gameObject.gameObject;
						UpdateLayersAndTags();
					}
				}

				if (myState == MyState.Enemy || myState == MyState.Defender || myState == MyState.Tower)
				{
					if (unitData.attack != null)
					{
						// Damage
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PrefixLabel("Damage");
						unitData.attack.damage = EditorGUILayout.IntField(unitData.attack.damage);
						EditorGUILayout.EndHorizontal();
						// Cooldown
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PrefixLabel("Cooldown");
						unitData.attack.cooldown = EditorGUILayout.FloatField(unitData.attack.cooldown);
						EditorGUILayout.EndHorizontal();
						// Range
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PrefixLabel("Range");
						unitData.attack.transform.localScale = EditorGUILayout.FloatField(unitData.attack.transform.localScale.x) * Vector3.one;
						EditorGUILayout.EndHorizontal();
					}

					if (unitData.attack is AttackRanged)
					{
						// Choose bullet button
						EditorGUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
						if (GUILayout.Button(contents.chooseBulletButton, GUILayout.MaxWidth(40f), GUILayout.MaxHeight(40f)) == true)
						{
							switch (unitData.gameObject.tag)
							{
							case "Defender":
							case "Tower":
								ShowPicker<GameObject>(PickerState.BulletAlly, Lables.bulletAlly);
								break;
							case "Enemy":
							case "FlyingEnemy":
								ShowPicker<GameObject>(PickerState.BulletEnemy, Lables.bulletEnemy);
								break;
							}
						}
						// Focus on firepoint button
						if (GUILayout.Button(contents.focusOnFirePointButton, GUILayout.MaxWidth(40f), GUILayout.MaxHeight(40f)) == true)
						{
							if ((unitData.attack as AttackRanged).firePoint != null)
							{
								Selection.activeGameObject = (unitData.attack as AttackRanged).firePoint.gameObject;
							}
						}
						GUILayout.FlexibleSpace();
						EditorGUILayout.EndHorizontal();
					}
				}

				if (myState == MyState.Enemy)
				{
					if (unitData.aiFeature != null)
					{
						EditorGUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
						GUILayout.Label("Features");
						GUILayout.FlexibleSpace();
						EditorGUILayout.EndHorizontal();

						AiFeature feature = (Selection.activeObject as GameObject).GetComponent<AiFeature>();

						// Features buttons
						EditorGUILayout.BeginHorizontal();
						if (GUILayout.Button(contents.add, GUILayout.MaxWidth(30f), GUILayout.MaxHeight(30f)) == true)
						{
							ShowPicker<GameObject>(PickerState.Features, Lables.feature);
						}
						GUILayout.FlexibleSpace();
						if (GUILayout.Button(contents.prev, GUILayout.MaxWidth(30f), GUILayout.MaxHeight(30f)) == true)
						{
							Selection.activeObject = unitData.aiFeature.GetPreviousFeature(Selection.activeObject as GameObject);
						}
						if (GUILayout.Button(contents.next, GUILayout.MaxWidth(30f), GUILayout.MaxHeight(30f)) == true)
						{
							Selection.activeObject = unitData.aiFeature.GetNextFeature(Selection.activeObject as GameObject);
						}
						GUILayout.FlexibleSpace();
						if (GUILayout.Button(contents.remove, GUILayout.MaxWidth(30f), GUILayout.MaxHeight(30f)) == true)
						{
							if (feature != null)
							{
								DestroyImmediate(feature.gameObject);
								Selection.activeObject = unitData.gameObject;
							}
						}
						EditorGUILayout.EndHorizontal();

						if (feature != null)
						{
							EditorGUILayout.BeginHorizontal();
							GUILayout.FlexibleSpace();
							GUILayout.Label(feature.name);
							GUILayout.FlexibleSpace();
							EditorGUILayout.EndHorizontal();

							AiFeaturesGUI(feature);
						}
					}
				}

				if (myState == MyState.Defender || myState == MyState.Tower)
				{
					if (unitData.attack != null)
					{
						// Targets
						EditorGUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
						GUILayout.Label("Targets");
						GUILayout.FlexibleSpace();
						EditorGUILayout.EndHorizontal();

						bool land = EditorGUILayout.Toggle("Land", unitData.targetCommon);
						bool flying = EditorGUILayout.Toggle("Flying", unitData.targetFlying);
						if (land != unitData.targetCommon || flying != unitData.targetFlying)
						{
							AiTriggerCollider trigger = unitData.attack.GetComponent<AiTriggerCollider>();
							if (trigger != null)
							{
								trigger.tags.Clear();
								if (land == true)
								{
									trigger.tags.Add("Enemy");
								}
								if (flying == true)
								{
									trigger.tags.Add("FlyingEnemy");
								}
							}
							unitData.targetCommon = land;
							unitData.targetFlying = flying;
						}
					}
				}

				if (myState == MyState.Barracks)
				{
					if (unitData.spawner != null)
					{
						// Defenders number
						EditorGUILayout.LabelField("Defenders number");
						unitData.spawner.maxNum = EditorGUILayout.IntSlider(unitData.spawner.maxNum, 1, 3);

						// Cooldown between defenders spawning
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PrefixLabel("Cooldown");
						unitData.spawner.cooldown = EditorGUILayout.FloatField(unitData.spawner.cooldown);
						EditorGUILayout.EndHorizontal();
					}

					if (unitData.range != null)
					{
						// Range
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.PrefixLabel("Range");
						unitData.range.transform.localScale = EditorGUILayout.FloatField(unitData.range.transform.localScale.x) * Vector3.one;
						EditorGUILayout.EndHorizontal();
					}

					if (unitData.spawner != null)
					{
						EditorGUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
						GUILayout.Label("Defender");
						GUILayout.FlexibleSpace();
						EditorGUILayout.EndHorizontal();

						// Defender prefab
						EditorGUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
						if (GUILayout.Button(contents.chooseDefenderButton, GUILayout.MaxWidth(60f), GUILayout.MaxHeight(60f)) == true)
						{
							ShowPicker<GameObject>(PickerState.Defenders, Lables.defender);
						}
						GUILayout.FlexibleSpace();
						EditorGUILayout.EndHorizontal();
					}
				}

				if (myState == MyState.Tower || myState == MyState.Barracks)
				{
					if (unitData.range != null)
					{
						EditorGUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
						GUILayout.Label("Range");
						GUILayout.FlexibleSpace();
						EditorGUILayout.EndHorizontal();

						// Show range
						unitData.range.SetActive(EditorGUILayout.Toggle("Show range", unitData.range.activeSelf));
					}

					if (unitData.towerActions != null)
					{
						EditorGUILayout.BeginHorizontal();
						GUILayout.FlexibleSpace();
						GUILayout.Label("Actions");
						GUILayout.FlexibleSpace();
						EditorGUILayout.EndHorizontal();

						unitData.towerActions.gameObject.SetActive(EditorGUILayout.Toggle("Show actions", unitData.towerActions.gameObject.activeSelf));

						// Actions buttons
						if (unitData.towerActions.gameObject.activeSelf == true)
						{
							TowerAction action = (Selection.activeObject as GameObject).GetComponent<TowerAction>();

							EditorGUILayout.BeginHorizontal();
							if (GUILayout.Button(contents.add, GUILayout.MaxWidth(30f), GUILayout.MaxHeight(30f)) == true)
							{
								ShowPicker<GameObject>(PickerState.TowerActions, Lables.towerAction);
							}
							GUILayout.FlexibleSpace();
							if (GUILayout.Button(contents.prev, GUILayout.MaxWidth(30f), GUILayout.MaxHeight(30f)) == true)
							{
								Selection.activeObject = unitData.towerActions.GetPrevioustAction(Selection.activeObject as GameObject);
							}
							if (GUILayout.Button(contents.next, GUILayout.MaxWidth(30f), GUILayout.MaxHeight(30f)) == true)
							{
								Selection.activeObject = unitData.towerActions.GetNextAction(Selection.activeObject as GameObject);
							}
							GUILayout.FlexibleSpace();
							if (GUILayout.Button(contents.remove, GUILayout.MaxWidth(30f), GUILayout.MaxHeight(30f)) == true)
							{
								if (action != null)
								{
									DestroyImmediate(action.gameObject);
									Selection.activeObject = unitData.gameObject;
								}
							}
							EditorGUILayout.EndHorizontal();

							if (action != null)
							{
								EditorGUILayout.BeginHorizontal();
								GUILayout.FlexibleSpace();
								GUILayout.Label(action.name);
								GUILayout.FlexibleSpace();
								EditorGUILayout.EndHorizontal();


								TowerActionsGUI(action);
							}
						}
					}
				}

				// New object selected in picker window
				if (Event.current.commandName == "ObjectSelectorUpdated" && EditorGUIUtility.GetObjectPickerControlID() == currentPickerWindow)
				{
					pickedObject = EditorGUIUtility.GetObjectPickerObject();
					if (pickedObject != null)
					{
						switch (pickerState)
						{
						case PickerState.BulletAlly:
						case PickerState.BulletEnemy:
							if (unitData.attack != null && unitData.attack is AttackRanged)
							{
								(unitData.attack as AttackRanged).arrowPrefab = pickedObject as GameObject;
								contents.chooseBulletButton.image = AssetPreview.GetAssetPreview((unitData.attack as AttackRanged).arrowPrefab);
							}
							break;
						case PickerState.Defenders:
							if (unitData.spawner != null)
							{
								unitData.spawner.prefab = pickedObject as GameObject;
								contents.chooseDefenderButton.image = AssetPreview.GetAssetPreview(unitData.spawner.prefab);
							}
							break;
						case PickerState.Towers:
							{
								TowerActionBuild towerActionBuild = (Selection.activeObject as GameObject).GetComponent<TowerActionBuild>();
								if (towerActionBuild != null)
								{
									towerActionBuild.towerPrefab = pickedObject as GameObject;
									EditorUtility.SetDirty(towerActionBuild.gameObject);
								}
							}
							break;
						case PickerState.Icons:
							{
								TowerAction towerAction = (Selection.activeObject as GameObject).GetComponent<TowerAction>();
								if (towerAction != null)
								{
									Image image = towerAction.enabledIcon.GetComponent<Image>();
									if (image != null)
									{
										image.sprite = pickedObject as Sprite;
										EditorUtility.SetDirty(towerAction.gameObject);
									}
								}
							}
							break;
						}
					}
				}
				// Picker window closed
				if (Event.current.commandName == "ObjectSelectorClosed" && EditorGUIUtility.GetObjectPickerControlID() == currentPickerWindow)
				{
					if (pickedObject != null)
					{
						switch (pickerState)
						{
						case PickerState.TowerActions:
							if (unitData.towerActions != null)
							{
								unitData.towerActions.AddAction(pickedObject as GameObject);
							}
							break;
						case PickerState.Features:
							if (unitData.aiFeature != null)
							{
								unitData.aiFeature.AddFeature(pickedObject as GameObject);
							}
							break;
						}
					}
					pickedObject = null;
					pickerState = PickerState.None;
				}
				break;
			}

			if (myState != MyState.Disabled)
			{
				GUILayout.Space(guiSpace);

				// Apply changes button
				if (unitData.gameObject != null && PrefabUtility.GetPrefabType(unitData.gameObject) != PrefabType.None)
				{
					if (GUILayout.Button(contents.applyChangesButton) == true)
					{
						if (unitData.towerActions != null)
						{
							unitData.towerActions.gameObject.SetActive(false);
						}
						if (unitData.range != null)
						{
							unitData.range.SetActive(false);
						}
						PrefabUtility.ReplacePrefab(unitData.gameObject.gameObject, PrefabUtility.GetCorrespondingObjectFromSource(unitData.gameObject.gameObject), ReplacePrefabOptions.ConnectToPrefab);
					}

					// Remove from scene button
					if (GUILayout.Button(contents.removeFromSceneButton) == true)
					{
						DestroyImmediate(unitData.gameObject.gameObject);
						Selection.activeObject = null;
					}
				}
			}
		}
		else
		{
			EditorGUILayout.HelpBox("Editor disabled in play mode", MessageType.Info);
		}
	}

	/// <summary>
	/// GUI for tower's action tree
	/// </summary>
	/// <param name="action">Action.</param>
	private void TowerActionsGUI(TowerAction action)
	{
		if (action is TowerActionCooldown)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Cooldown");
			(action as TowerActionCooldown).cooldown = EditorGUILayout.FloatField((action as TowerActionCooldown).cooldown);
			EditorGUILayout.EndHorizontal();
		}

		if (action is TowerActionBuild)
		{
			if (GUILayout.Button("Choose tower prefab") == true)
			{
				ShowPicker<GameObject>(PickerState.Towers, Lables.tower);
			}
			if (GUILayout.Button("Choose icon") == true)
			{
				ShowPicker<Sprite>(PickerState.Icons, Lables.icon);
			}
		}
		else if (action is TowerActionSkill)
		{
			if (GUILayout.Button("Choose icon") == true)
			{
				ShowPicker<Sprite>(PickerState.Icons, Lables.icon);
			}
		}
	}

	/// <summary>
	/// GUI for unit's features
	/// </summary>
	/// <param name="feature">Feature.</param>
	private void AiFeaturesGUI(AiFeature feature)
	{
		Heal heal = feature.GetComponent<Heal>();
		if (heal != null)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Range");
			heal.transform.localScale = EditorGUILayout.FloatField(heal.transform.localScale.x) * Vector3.one;
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Heal amount");
			heal.healAmount = EditorGUILayout.IntField(heal.healAmount);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Cooldown");
			heal.cooldown = EditorGUILayout.FloatField(heal.cooldown);
			EditorGUILayout.EndHorizontal();

			return;
		}
		AoeHeal aoeHeal = feature.GetComponent<AoeHeal>();
		if (aoeHeal != null)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Range");
			aoeHeal.transform.localScale = EditorGUILayout.FloatField(aoeHeal.transform.localScale.x) * Vector3.one;
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Heal amount");
			aoeHeal.healAmount = EditorGUILayout.IntField(aoeHeal.healAmount);
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Cooldown");
			aoeHeal.cooldown = EditorGUILayout.FloatField(aoeHeal.cooldown);
			EditorGUILayout.EndHorizontal();

			return;
		}
		AloneSpeedUp aloneSpeedUp = feature.GetComponent<AloneSpeedUp>();
		if (aloneSpeedUp != null)
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.PrefixLabel("Speed up amount");
			aloneSpeedUp.speedUpAmount = EditorGUILayout.FloatField(aloneSpeedUp.speedUpAmount);
			EditorGUILayout.EndHorizontal();

			return;
		}
	}
}
