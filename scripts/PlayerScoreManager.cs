using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    public int playerScore;
    private UIManager _uiManager;
    [SerializeField] private PostProcessingManager _postProcessingManager;

    private float _flashAmount = 20f;
    private float _abberationAmount = 0.78f;

    void Start()
    {
        _uiManager = FindObjectOfType<UIManager>();
    }
    public void CalculateKills(int enemiesHit)
    {
        
        if (enemiesHit >= 0 && enemiesHit <= 1)
        {
            _flashAmount = 20f;
            CameraShaker.Instance.ShakeOnce(0.7f, 0.1f, 0.1f, 0.3f);
        }
        else if (enemiesHit >= 2 && enemiesHit <= 4)
        {
            _flashAmount = 22f;
            CameraShaker.Instance.ShakeOnce(0.7f, 0.3f, 0.1f, 0.5f);
        }
        else if (enemiesHit >= 5 && enemiesHit <= 6)
        {
            _flashAmount = 25f;
            _postProcessingManager.IncreaseChromaticAbberation(_abberationAmount);
            CameraShaker.Instance.ShakeOnce(0.9f, 0.6f, 0.1f, 0.5f);
        }
        else if (enemiesHit >= 7 && enemiesHit <= 8)
        {
            _flashAmount = 27f;
            _postProcessingManager.IncreaseChromaticAbberation(_abberationAmount);
            CameraShaker.Instance.ShakeOnce(1.1f, 0.8f, 0.1f, 0.5f);
        }
        else if (enemiesHit >= 9 && enemiesHit <= 10)
        {
            _flashAmount = 30f;
            _postProcessingManager.IncreaseChromaticAbberation(_abberationAmount);
            CameraShaker.Instance.ShakeOnce(1.5f, 1f, 0.1f, 0.5f);
        }
        else
        {
            _flashAmount = 35f;
            _postProcessingManager.IncreaseChromaticAbberation(_abberationAmount);
            CameraShaker.Instance.ShakeOnce(2.2f, 1.2f, 0.1f, 0.5f);
        }
        
        for (int i = 1; i <= enemiesHit; i++)
        {
            playerScore += i;
        }
        
        _postProcessingManager.IncreaseBloom(_flashAmount);

        if (enemiesHit > 0)
        {
            _uiManager.UpdateComboText(enemiesHit);     
        }
       
        _uiManager.UpdateScoreText(playerScore);
    }

    public void ResetPlayerScore()
    {
        playerScore = 0;
    }
}
