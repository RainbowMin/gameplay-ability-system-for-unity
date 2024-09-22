using GAS.Runtime;

public class CueReadyBoom : GameplayCueDurational
{
    public float period = 0.5f;
    public float zoomScale = 0.5f;

    public override GameplayCueDurationalSpec CreateSpec(GameplayCueParameters parameters)
    {
        return new CueReadyBoomSpec(this, parameters);
    }
}