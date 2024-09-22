using System.Threading.Tasks;
using GAS.General;
using GAS.Runtime;
using UnityEngine;

public class CueBombWarningSpec : GameplayCueDurationalSpec<CueBombWarning>
{
    private AreaVisualization m_visualization;
    private int m_startFrame;
    private int m_durationFrame;

    public CueBombWarningSpec(CueBombWarning cue, GameplayCueParameters parameters) : base(cue, parameters)
    {
        m_durationFrame = (int)(cue.Duration * GASTimer.FrameRate);
    }

    public override void OnAdd()
    {
        m_startFrame = GASTimer.CurrentFrameCount;

        var vfx = Object.Instantiate(cue.Visualization);
        vfx.transform.position = Owner.transform.position;
        m_visualization = vfx.GetComponent<AreaVisualization>();
        m_visualization.SetAreaSize(cue.Radius * 2);
    }

    public override void OnGameplayEffectActivate()
    {

    }

    public override void OnGameplayEffectDeactivate()
    {
    }

    public override void OnRemove()
    {
        if(m_visualization.gameObject) 
            Object.Destroy(m_visualization.gameObject);
    }

    public override void OnTick()
    {
        m_visualization.SetProgress((float)(GASTimer.CurrentFrameCount - m_startFrame) / m_durationFrame);
    }
}