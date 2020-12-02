using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    public TimescaleManager timescaleManager;
    public PlayerScoreManager scoreManager;
    public UIManager uiManager;
    public DangerIndicator dangerIndicator;
    public enum PlayerState
    {
        Playing,
        Dead,
    }

    public PlayerState currentPlayerState;

    public void SwitchState(PlayerState newState)
    {
        switch (newState)
        {
            case PlayerState.Dead:
                timescaleManager.SlowdownWhenDead();
                dangerIndicator.HideDangerIndicator();
                uiManager.UpdateFinalScoreText(scoreManager.playerScore);
                uiManager.DisplayGameplayScreen(false);
                uiManager.DisplayGameOverScreen(true);
                break;
            case PlayerState.Playing:
                timescaleManager.ResetTimescale();
                uiManager.DisplayGameplayScreen(true);
                break;
        }
        
        currentPlayerState = newState;
    }
}
