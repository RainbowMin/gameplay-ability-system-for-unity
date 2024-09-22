using GAS.Runtime;
using UnityEngine;
public class CueReadyBoomSpec : GameplayCueDurationalSpec<CueReadyBoom>
{
    Transform m_ownerTransform;
    private float m_timer;

    public CueReadyBoomSpec(CueReadyBoom cue, GameplayCueParameters parameters) : base(cue, parameters)
    {
        m_ownerTransform = Owner.transform;
    }

    public override void OnAdd()
    {
    }

    public override void OnGameplayEffectActivate()
    {
    }

    public override void OnGameplayEffectDeactivate()
    {
    }

    public override void OnRemove()
    {
    }

    public override void OnTick()
    {
        var p = (Mathf.Sin(m_timer * (2 * Mathf.PI) / cue.period) + 1) * 0.5f;
        var scale = Mathf.Lerp(cue.zoomScale, 1, p);
        m_ownerTransform.localScale = Vector3.one * scale;
        
        m_timer += Time.deltaTime;
    }
}