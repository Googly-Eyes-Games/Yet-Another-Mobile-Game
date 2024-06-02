using erulathra;
using UnityEngine;

public class ScoreSubsystem : SceneSubsystem
{
    [field: SerializeField]
    private IntSOEvent scoreChangedEvent;
    
    private static readonly int BaseSpillScore = 25;
    
    public int Score { get; private set; }
    
    public override void Initialize()
    {
    }

    public void AddBaseScore()
    {
        Score += BaseSpillScore;
        scoreChangedEvent?.Invoke(Score);
    }

    public void AddWholeClearedSpillScore(float selectedArea, float spillArea)
    {
       Score += BaseSpillScore + Mathf.RoundToInt((2.5f * Mathf.Pow(Mathf.Clamp01(spillArea / selectedArea), 2f)) * spillArea);
       scoreChangedEvent?.Invoke(Score);
    }
}