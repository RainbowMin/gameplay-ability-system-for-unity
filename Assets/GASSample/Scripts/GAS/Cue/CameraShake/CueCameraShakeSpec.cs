using System.Threading.Tasks;
using GAS.Runtime;
using UnityEngine;

public class CueCameraShakeSpec : GameplayCueInstantSpec<CueCameraShake>
{
    private readonly Vector3 _originalPosition = new Vector3(0, 0, -10);

    public CueCameraShakeSpec(CueCameraShake cue, GameplayCueParameters parameters) : base(cue, parameters)
    {
    }

    public override void Trigger()
    {
        CameraShake(cue.shakePower, cue.shakeDuration);
    }

    private async void CameraShake(float magnitude, float duration)
    {
        if (Camera.main == null) return;
        var transform = Camera.main.transform;
        var elapsed = 0f;
        while (elapsed < duration)
        {
            var offset = Random.insideUnitSphere * magnitude;
            transform.localPosition = _originalPosition + offset;
            await Task.Yield();
            elapsed += Time.deltaTime;
        }        
        transform.localPosition = _originalPosition;
    }
}