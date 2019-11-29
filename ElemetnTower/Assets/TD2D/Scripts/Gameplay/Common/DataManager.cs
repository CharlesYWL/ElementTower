using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

/// <summary>
/// Version of saved data format. Use it to check if stored data format is equal to actual data format
/// </summary>
[Serializable]
public class DataVersion
{
    public int major = 1;
    public int minor = 0;
}

/// <summary>
/// Format of stored game progress data.
/// </summary>
[Serializable]
public class GameProgressData
{
	public System.DateTime saveTime = DateTime.MinValue;	// Saving time
    public string lastCompetedLevel = "";					// Name of level was last completed
	public List<string> openedLevels = new List<string>();	// List with levels available to play
}

/// <summary>
/// Format of stored game configurations.
/// </summary>
[Serializable]
public class GameConfigurations
{
	public float soundVolume = 0.5f;
	public float musicVolume = 0.5f;
}

/// <summary>
/// Saving and load data from file.
/// </summary>
public class DataManager : MonoBehaviour
{
	// Singleton
	public static DataManager instance;

	// Game progress data container
    public GameProgressData progress = new GameProgressData();
	// Game configurations container
	public GameConfigurations configs = new GameConfigurations();

	// Data version container
	private DataVersion dataVersion = new DataVersion();
	// Name of file with data version
    private string dataVersionFile = "/DataVersion.dat";
	// Name of file with game progress data
    private string gameProgressFile = "/GameProgress.dat";
	// Name of file with game configurations
	private string gameConfigsFile = "/GameConfigs.dat";

	/// <summary>
	/// Awake this instance.
	/// </summary>
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            UpdateDataVersion();
            LoadGameProgress();
			LoadGameConfigs();
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

	/// <summary>
	/// Raises the destroy event.
	/// </summary>
	void OnDestroy()
	{
		if (instance == this)
		{
			instance = null;
		}
	}

	/// <summary>
	/// Updates the version of data format.
	/// </summary>
    private void UpdateDataVersion()
    {
        if (File.Exists(Application.persistentDataPath + dataVersionFile) == true)
        {
            BinaryFormatter bfOpen = new BinaryFormatter();
            FileStream fileToOpen = File.Open(Application.persistentDataPath + dataVersionFile, FileMode.Open);
            DataVersion version = (DataVersion)bfOpen.Deserialize(fileToOpen);
            fileToOpen.Close();

            switch (version.major)
            {
                case 1:
					// Stored data has version 1.x
					// Some handler to convert data if it is needed ...
                    break;
            }
        }
        BinaryFormatter bfCreate = new BinaryFormatter();
        FileStream fileToCreate = File.Create(Application.persistentDataPath + dataVersionFile);
        bfCreate.Serialize(fileToCreate, dataVersion);
        fileToCreate.Close();
    }

	/// <summary>
	/// Delete file with saved game data. For debug only
	/// </summary>
	public void DeleteGameProgress()
	{
		File.Delete(Application.persistentDataPath + gameProgressFile);
		progress = new GameProgressData();
		Debug.Log("Saved game progress deleted");
	}

	/// <summary>
	/// Saves the game progress to file.
	/// </summary>
    public void SaveGameProgress()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + gameProgressFile);
        progress.saveTime = DateTime.Now;
        bf.Serialize(file, progress);
        file.Close();
    }

	/// <summary>
	/// Loads the game progress from file.
	/// </summary>
    public void LoadGameProgress()
    {
        if (File.Exists(Application.persistentDataPath + gameProgressFile) == true)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + gameProgressFile, FileMode.Open);
            progress = (GameProgressData)bf.Deserialize(file);
            file.Close();
        }
    }

	/// <summary>
	/// Saves the game configurations to file.
	/// </summary>
	public void SaveGameConfigs()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + gameConfigsFile);
		bf.Serialize(file, configs);
		file.Close();
	}

	/// <summary>
	/// Loads the game configurations from file.
	/// </summary>
	public void LoadGameConfigs()
	{
		if (File.Exists(Application.persistentDataPath + gameConfigsFile) == true)
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + gameConfigsFile, FileMode.Open);
			configs = (GameConfigurations)bf.Deserialize(file);
			file.Close();
		}
	}
}
