using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessingManager : MonoBehaviour
{

    public PostProcessVolume volume;
    
    private MotionBlur motionBlur = null;
    private Bloom bloom = null;
    private ChromaticAberration _chromaticAberration = null;

    private bool _flashBloom;
    private bool _flashAbberation;
    [SerializeField] private float _fadeOutSpeed;
    [SerializeField] private float _chromaticAbberationFadeOutSpeed;
    
    void Start()
    {
        volume.profile.TryGetSettings(out motionBlur);
        volume.profile.TryGetSettings(out bloom);
        volume.profile.TryGetSettings(out _chromaticAberration);
    }
 
    public void EnableMotionBlur(bool enabled)
    {
        motionBlur.enabled.value = enabled;
    }
    
    public void DisableMotionBlur(bool disabled)
    {
        motionBlur.enabled.value = disabled;
    }

    public void IncreaseBloom(float flashValue)
    {
        bloom.intensity.value = flashValue;
        _flashBloom = true;
    }

    public void IncreaseChromaticAbberation(float abberationValue)
    {
        _chromaticAberration.intensity.value = abberationValue;
        _flashAbberation = true;
    }

    public void ResetAllEffects()
    {
        _flashAbberation = false;
        _flashBloom = false;

        bloom.intensity.value = 5f;
        _chromaticAberration.intensity.value = 0f;
    }

    void Update()
    {
        if (_flashBloom)
        {
            bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, 5f, Time.deltaTime * _fadeOutSpeed);

            if (bloom.intensity.value <= 5f)
            {
                _flashBloom = false;
            }
        }

        if (_flashAbberation)
        {
            _chromaticAberration.intensity.value = Mathf.Lerp(_chromaticAberration.intensity.value, 0f, Time.deltaTime * _chromaticAbberationFadeOutSpeed);

            if (_chromaticAberration.intensity.value <= 0f)
            {
                _flashAbberation = false;
            }
        }
    }
}
