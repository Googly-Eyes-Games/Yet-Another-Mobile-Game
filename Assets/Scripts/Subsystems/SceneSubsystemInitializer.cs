using UnityEngine;
using erulathra;

public class SceneSubsystemInitializer : MonoBehaviour
{
    private void InstantiateSubsystems(SceneSubsystemManager manager)
    {
        manager.FindOrAddSubsystem<ScoreSubsystem>();
        manager.FindOrAddSubsystem<OilSpillsSubsystem>();
    }
	
    public void Awake()
    {
        SceneSubsystemManager.Initialize(gameObject, InstantiateSubsystems);
    }
    
}
