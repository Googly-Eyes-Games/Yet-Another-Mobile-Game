using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using Task = System.Threading.Tasks.Task;

public class SaveManager : ScriptableObject
{
	[SerializeField]
	private SOEvent onSaveDataChanged;
	
	private static string savePath;
	
    private static SaveManager instance;

    private static object saveGameCS = new ();
    
    public static SaveManager Instance {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<SaveManager>("SO_MoneyManager");
                instance.LoadGame();
            }

            return instance;
        }
    }

    private GameSave save;
    public GameSave Save {
	    get
	    {
		    GameSave saveCopy;
		    lock (saveGameCS)
		    {
				saveCopy = save;
		    }

		    return saveCopy;
	    }
	    private set
	    {
		    lock (saveGameCS)
		    {
			    save = value;
		    }
	    }
    }
    
    public void ResetSave()
    {
	    GameSave initialGameSave = InitializeGameSettings();
	    SaveGameAsync(initialGameSave);
    }
    
    public void SaveGameAsync(GameSave newSave)
    {
	    Save = newSave;
        
	    Task.Run(() =>
	    {
			    lock (saveGameCS)
			    {
					try
					{
						BinaryFormatter binaryFormatter = new BinaryFormatter();
						MemoryStream memoryStream = new MemoryStream();
						binaryFormatter.Serialize(memoryStream, Save);
						byte[] data = memoryStream.ToArray();

						File.WriteAllBytes(savePath, data);
						Debug.Log($"Game saved successfully to {savePath}");
					}
					catch (Exception ex)
					{
						Debug.LogError($"Failed to save game: {ex.Message}");
					}
			    }
	    });

	    onSaveDataChanged?.Invoke();
    }

    private void OnEnable()
    {
	    lock (saveGameCS)
	    {
		    savePath = Path.Combine(Application.persistentDataPath, "save.sav");
	    }
    }

    private void LoadGame()
    {
	    lock (saveGameCS)
	    {
			if (!File.Exists(savePath))
			{
				GameSave initialGameSave = InitializeGameSettings();
				
				SaveGameAsync(initialGameSave);
				return;
			}
			
			byte[] saveBytes = File.ReadAllBytes(savePath);
			
			MemoryStream memoryStream = new MemoryStream();
			memoryStream.Write(saveBytes, 0, saveBytes.Length);
			memoryStream.Seek(0, SeekOrigin.Begin);
			
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			Save = (GameSave) binaryFormatter.Deserialize(memoryStream);
	    }
    }
    
    private GameSave InitializeGameSettings()
    {
	    GameSave initialSave = new GameSave
	    {
		    ShipSpeedLevel = 0,
		    RopeLengthLevel = 0,
		    BoatItemInUse = "",
		    LineItem = "",
		    BoughtItems = new()
	    };
	    
	    return initialSave;
    }

    [Button]
    private void AddMoney()
    {
	    GameSave newSave = save;
	    newSave.MoneyAmount += 10;
	    
	    SaveGameAsync(newSave);
    }
    
    [Button]
    private void Reset()
    {
		ResetSave();
    }
}

[Serializable]
public struct GameSave
{
	public int MoneyAmount { get; set; }
	public float ShipSpeedLevel { get; set; }
	public int RopeLengthLevel { get; set; }
	
	public string BoatItemInUse { get; set; }
	public string LineItem { get; set; }

	public HashSet<string> BoughtItems { get; set; }
}