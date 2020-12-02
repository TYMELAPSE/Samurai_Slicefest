using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownManager : MonoBehaviour
{
    public float timeRemaining = 30f;
    public bool gameOver;
    [SerializeField] private StateManager _stateManager;
    [SerializeField] private UIManager _uiManager;
    
    void Update()
    {
        if (timeRemaining > 0f)
        {
            timeRemaining -= Time.deltaTime * Time.timeScale;
            _uiManager.UpdateTimeText(timeRemaining.ToString("0.00"));
        }
        

        if (timeRemaining <= 0f && !gameOver)
        {
            gameOver = true;
            _stateManager.SwitchState(StateManager.PlayerState.Dead);
        }
    }

    public void ResetCountdownTimer()
    {
        gameOver = false;
        timeRemaining = 30f;
    }
}
