using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
		    try
		    {
			    lock (saveGameCS)
			    {
				    BinaryFormatter binaryFormatter = new BinaryFormatter();
				    MemoryStream memoryStream = new MemoryStream();
				    binaryFormatter.Serialize(memoryStream, Save);
				    byte[] data = memoryStream.ToArray();

				    File.WriteAllBytes(savePath, data);
				    Debug.Log($"Game saved successfully to {savePath}");
			    }
		    }
		    catch (Exception ex)
		    {
			    Debug.LogError($"Failed to save game: {ex.Message}");
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
	    BaseSettingSO baseSettingSo = Resources.Load<BaseSettingSO>("SO_BaseSettings");
	    
	    if (baseSettingSo == null)
	    {
		    Debug.LogError("Failed to load SO_BaseSettings!");
		    return new GameSave();
	    }
	    
	    GameSave initialSave = new GameSave
	    {
		    ShipSpeedLevel = baseSettingSo.ShipSpeedLevel,
		    LineLengthLevel = baseSettingSo.LineLengthLevel,
		    SpriteName = baseSettingSo.Sprite.name,
		    LineColor = baseSettingSo.LineColor
	    };
	    
	    return initialSave;
    }

}

[Serializable]
public struct GameSave
{
	public int MoneyAmount { get; set; }
	public float ShipSpeedLevel { get; set; }
	public float LineLengthLevel { get; set; }
	
	public string SpriteName { get; set; }
	
	public SerializableColor LineColor { get; set; }
}


[Serializable]
public class SerializableColor{
	
	public	float[]	colorStore = new float[4]{1F,1F,1F,1F};
	public	Color	Color {
		get{ return new Color( colorStore[0], colorStore[1], colorStore[2], colorStore[3] );}
		set{ colorStore = new float[4]{ value.r, value.g, value.b, value.a  };				}
	}
	
	public static implicit operator Color( SerializableColor instance ){
		return instance.Color;
	}
	
	public static implicit operator SerializableColor( Color color ){
		return new SerializableColor{ Color = color};
	}
}