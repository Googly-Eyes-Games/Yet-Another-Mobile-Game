using System.Collections.Generic;
using erulathra;

public class OilSpillsSubsystem : SceneSubsystem
{
    public HashSet<OilSpill> ActiveOilSpills { get; private set; } = new();

    public override void Initialize()
    {
    }

    public void RegisterSpill(OilSpill spill)
    {
        ActiveOilSpills.Add(spill);
    }

    public void UnregisterSpill(OilSpill spill)
    {
        if (ActiveOilSpills.Contains(spill))
        {
            ActiveOilSpills.Remove(spill);
        }
    }
}