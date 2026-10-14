using SanyaBeerExtension;
using System;
using UnityEngine;

[Serializable]
public class PlatformSize
{
    [SerializeField] private float _numerator = 90;
    [SerializeField] private float _denominator = 100;
    [SerializeField] private float _increment = 1;

    public void Resize(Platform platform, Vector3 direction, int count)
    {
        Vector3 localSize = platform.SlicePattern.transform.localScale;

        Action<float> method = direction == Vector3.forward ?
            (ratio) => platform.SlicePattern.transform.localScale = localSize.SetZ(localSize.z * ratio) :
            (ratio) => platform.SlicePattern.transform.localScale = localSize.SetX(localSize.x * ratio);

        float ratio = (_numerator - _increment * (float)count) / (_denominator - _increment * (float)count);
        ratio = Mathf.Abs(ratio);

        method.Invoke(ratio);
    }
}