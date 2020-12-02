using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    public GameObject transitionSprite;

    public PlayerController player;
    public GameObject enemyContainer;
    public Transform bloodContainer;
    
    public PlayerScoreManager scoreManager;
    public SpawnManager spawnManager;
    public StateManager stateManager;
    public CountdownManager countdownManager;
    public CameraZoom cameraZoom;
    public UIManager uiManager;
    public PostProcessingManager postProcessingManager;
    public TimescaleManager timescaleManager;

    [SerializeField] private float _fadeTime;
    private bool _isRestarting;
    
    void Start()
    {
        StartCoroutine(TransitionAtStart());
    }

    //used by button
    public void RestartSameLevel()
    {
        if (_isRestarting)
        {
            return;
        }
        
        Debug.Log("Restarting!");
        
        //hide GameOverScreen
        LeanTween.moveLocalX(transitionSprite, 0f, _fadeTime * Time.timeScale);
        uiManager.DisplayGameOverScreen(false);
        StartCoroutine(ResetPlayingField());
    }

    public void GoBackToMainMenu()
    {
        uiManager.DisplayGameOverScreen(false);
        LeanTween.moveLocalX(transitionSprite, 0f, _fadeTime * Time.timeScale);
        StartCoroutine(LoadMainMenu());
    }

    private IEnumerator TransitionAtStart()
    {
        LeanTween.moveLocalX(transitionSprite, -90f, _fadeTime);
        yield return new WaitForSeconds(_fadeTime);
        transitionSprite.transform.position = new Vector3(90f, transitionSprite.transform.position.y, transitionSprite.transform.position.z);
    }

    private IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(_fadeTime * Time.timeScale);
        timescaleManager.ResetTimescale();
        SceneManager.LoadScene("MainMenu");
    }
    private IEnumerator ResetPlayingField()
    {
        _isRestarting = true;
        
        yield return new WaitForSeconds(_fadeTime * Time.timeScale);
        
        player.transform.position = new Vector3(0f,0f,0f);
        
        //rensa och fyll på enemyPool
        spawnManager.enemyPool.Clear();
        foreach (Enemy child in enemyContainer.transform.GetComponentsInChildren<Enemy>())
        {
            spawnManager.enemyPool.Add(child);
            child.isActive = false;
            child.ToggleCollider(false);
            child.transform.position = spawnManager.outOfPlacePosition.position;
        }
        
        //återställ bloodContainer
        foreach (Transform child in bloodContainer)
        {
            child.transform.position = spawnManager.outOfPlacePosition.position;
        }

        scoreManager.ResetPlayerScore();
        spawnManager.ResetSpawnTimer();
        
        postProcessingManager.ResetAllEffects();
        cameraZoom.ResetZoom();

        player.GetComponent<TrailRenderer>().enabled = false;
        player.CancelShake();
        player.ResetVignette();
        player.isDragging = false;

        LeanTween.moveLocalX(transitionSprite, -90f, _fadeTime * Time.timeScale);
        
        yield return new WaitForSeconds(_fadeTime * Time.timeScale);

        countdownManager.ResetCountdownTimer();
        stateManager.SwitchState(StateManager.PlayerState.Playing);
        
        LeanTween.cancelAll();
        transitionSprite.transform.position = new Vector3(90f, transitionSprite.transform.position.y, transitionSprite.transform.position.z);

        _isRestarting = false;

    }
}
