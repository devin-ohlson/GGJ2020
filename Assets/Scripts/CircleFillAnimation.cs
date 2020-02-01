using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CircleFillAnimation : MonoBehaviour
{
    [SerializeField] private float startFill = 0.01f;
    [SerializeField] private float endFill = 1f;
    [SerializeField] private float duration = 1f;

    [SerializeField] private float decayMod = 2.5f;
    [Range(0,1)]
    [SerializeField] private float amplitudeMod = 0.1f;
    [SerializeField] private float frequency = 60f; // radians per second
    private bool isFilling = false;
    private float currentTime = 0f;

    private float baseScale;

    [SerializeField] private SpriteRenderer child = null;
    private Func<bool> listener;
    
    void Start()
    {
        if (null == child)
            throw new ArgumentException(this.name + " has no child sprite for animation.");

        if (endFill < startFill)
        {
            float tmp = startFill;
            startFill = endFill;
            endFill = tmp;
        }

        baseScale = transform.localScale.x;
    }
    
    void Update()
    {
        UpdateTime(Time.deltaTime);

        float scale = startFill + (endFill - startFill) * (ModifiedTime(currentTime) / duration);

        child.transform.localScale = Vector3.one * scale;

        transform.localScale = Vector3.one * (baseScale + ModifiedScale(currentTime));

        if (currentTime >= duration)
        {
            listener.Invoke();
        }
    }

    float ModifiedTime(float time)
    {
        if (isFilling)
            return time + amplitudeMod * (currentTime / duration) * (endFill - startFill) * Mathf.Sin(frequency * time);
        else
            return time;
    }

    float ModifiedScale(float time)
    {
        float timeMod = currentTime / duration;
        if (isFilling)
            return Mathf.Pow(amplitudeMod, 2) * timeMod * Mathf.Cos(frequency * time);
        else
            return Mathf.Pow(amplitudeMod, 2) * timeMod;
    }

    void UpdateTime(float dt)
    {
        currentTime = Mathf.Clamp(currentTime + (isFilling ? 1 : -decayMod) * dt, 0, duration + 0.01f);
    }

    public void Activate(bool toActivate)
    {
        isFilling = toActivate;
    }

    public void SetDuration(float duration)
    {
        this.duration = duration;
    }

    public void Reset()
    {
        currentTime = 0f;
        listener = null;
    }

    public void Listen(Func<bool> apply)
    {
        listener = apply;
    }
}
